//-----------------------------------------------------------------------
// <copyright file="IRCMarshal.cs" company="intninety">
//     Copyright 2012 Robert Carr
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//     
//     http://www.apache.org/licenses/LICENSE-2.0
//     
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
// </copyright>
//-----------------------------------------------------------------------

namespace Yaircc.UI
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;
    using Yaircc.Net.IRC;
    using TabControl = System.Windows.Forms.TabControl;

    /// <summary>
    /// Represents a marshal that sits between an IRC connection and the application's UI.
    /// </summary>
    public class IRCMarshal : IDisposable
    {
        #region Fields

        /// <summary>
        /// The connection the marshal operates on.
        /// </summary>
        private Connection connection;

        /// <summary>
        /// The channels that data is being marshalled to.
        /// </summary>
        private List<IRCChannel> channels;

        /// <summary>
        /// The tab page that represents the server connection.
        /// </summary>
        private IRCTabPage serverTab;

        /// <summary>
        /// A value indicating whether or not the server is being disconnected from.
        /// </summary>
        private bool disconnecting;

        /// <summary>
        /// A value indicating whether or not the marshal is being disposed.
        /// </summary>
        private bool disposing;

        /// <summary>
        /// A value indicating whether or not the marshal is attempting to reconnect to the server.
        /// </summary>
        private bool reconnecting;

        /// <summary>
        /// The last used nick name.
        /// </summary>
        private string previousNickName;

        /// <summary>
        /// A value indicating whether or not the initial mode message is pending.
        /// </summary>
        private bool awaitingModeMessage;

        /// <summary>
        /// A timer used to invoke a connection attempt to the server.
        /// </summary>
        private System.Timers.Timer reconnectTimer;

        /// <summary>
        /// Occurs when a channel is created via the marshal.
        /// </summary>
        private ChannelCreatedHandler channelCreated;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="IRCMarshal"/> class.
        /// </summary>
        /// <param name="connection">The connection to marshal data to and from.</param>
        public IRCMarshal(Connection connection)
        {
            this.previousNickName = connection.Nickname;
            this.awaitingModeMessage = true;
            this.connection = connection;
            this.SetupConnectionEventHandlers();
            this.channels = new List<IRCChannel>();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for event <see cref="ChannelCreated"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The channel that was created.</param>
        public delegate void ChannelCreatedHandler(object sender, IRCChannel channel);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when a channel is created via the marshal.
        /// </summary>
        public event ChannelCreatedHandler ChannelCreated
        {
            add { this.channelCreated += value; }
            remove { this.channelCreated -= value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the channels that data is being marshalled to.
        /// </summary>
        public List<IRCChannel> Channels
        {
            get { return this.channels; }
        }

        /// <summary>
        /// Gets or sets the connection the marshal operates on.
        /// </summary>
        public Connection Connection
        {
            get { return this.connection; }
            set { this.connection = value; }
        }

        /// <summary>
        /// Gets or sets the tab page that represents the server connection.
        /// </summary>
        public IRCTabPage ServerTab
        {
            get { return this.serverTab; }
            set { this.serverTab = value; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the connection is still active.
        /// </summary>
        public bool IsConnected
        {
            get { return this.connection.IsConnected; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the server is being disconnected from.
        /// </summary>
        public bool IsDisconnecting
        {
            get { return this.disconnecting; }
            set { this.disconnecting = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Get the <see cref="MessageTypeAttribute"/> associated with <paramref name="reply"/>
        /// </summary>
        /// <param name="reply">The <see cref="Reply"/> whose attributes are returned</param>
        /// <returns><see cref="MessageTypeAttribute"/></returns>
        public static MessageTypeAttribute GetReplyAttributes(Reply reply)
        {
            MessageTypeAttribute retval = new MessageTypeAttribute(MessageType.ServerMessage);
            var type = typeof(Reply);
            var info = type.GetMember(reply.ToString());

            retval.Source = string.Format("[{0}]", Strings_Connection.Unknown);
            if (info.Length > 0)
            {
                var attributes = info[0].GetCustomAttributes(typeof(MessageTypeAttribute), false);
                if (attributes.Length > 0)
                {
                    retval = (MessageTypeAttribute)attributes[0];
                    if (retval.MessageType == MessageType.ErrorMessage)
                    {
                        retval.Source = string.Format("[{0}]", Strings_Connection.Error);
                    }
                    else
                    {
                        retval.Source = Strings_General.ReplySource;
                    }
                }
            }

            return retval;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by the IRCMarshal.
        /// </summary>
        public void Dispose()
        {
            this.disposing = true;

            if (this.Connection.IsConnected)
            {
                this.connection.Close();
            }

            this.Channels.ForEach(i => i.Dispose());
            this.Channels.Clear();

            TabControl tabControl = this.ServerTab.Parent as TabControl;
            tabControl.InvokeAction(() =>
            {
                tabControl.TabPages.Remove(this.ServerTab);
                this.ServerTab.Dispose();
            });
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Gets a channel using the tab it is linked to.
        /// </summary>
        /// <param name="tab">The tab to find the channel by.</param>
        /// <returns>The matching channel.</returns>
        public IRCChannel GetChannelByTab(IRCTabPage tab)
        {
            return this.channels.Find(i => i.TabPage == tab);
        }

        /// <summary>
        /// Sends the message to the associated server.
        /// </summary>
        /// <param name="sender">The source tab page.</param>
        /// <param name="message">The message to send.</param>
        public void Send(IRCTabPage sender, Message message)
        {
            message = MessageFactory.AssimilateMessage(message);
            if (this.Connection.IsConnected)
            {
                if ((message is PrivMsgMessage) || (message is NoticeMessage))
                {
                    IRCTabPage tabPage = null;
                    string source = this.Connection.Nickname;

                    if (message is PrivMsgMessage)
                    {
                        IRCChannel channel = this.Channels.Find(i => i.Name.Equals(message.Target, StringComparison.OrdinalIgnoreCase));
                        if (channel == null)
                        {
                            channel = this.CreateChannel(message.Target, true);
                        }

                        tabPage = channel.TabPage;
                    }
                    else
                    {
                        source = string.Format("to({0})", message.Target);
                        tabPage = sender;
                    }

                    tabPage.AppendMessage(message.Command, source, message.Content, message.Type);
                }
                else if (message is JoinMessage)
                {
                    if (!this.Channels.Exists(i => i.Name.Equals(message.Parameters[0], StringComparison.OrdinalIgnoreCase)))
                    {
                        IRCChannel channel = this.CreateChannel(message.Parameters[0], true);
                    }
                }
                else if (message is NickMessage)
                {
                    this.previousNickName = this.connection.Nickname;
                    this.connection.Nickname = (message as NickMessage).NewNickname;
                }

                if (message is QuitMessage)
                {
                    this.disconnecting = true;
                }

                this.Connection.Send(message.ToString());
            }

            if (message is QuitMessage)
            {
                this.Dispose();
            }
        }

        /// <summary>
        /// Creates a new channel and tab.
        /// </summary>
        /// <param name="displayName">The name of the channel.</param>
        /// <param name="switchTo">A value indicating whether or not to switch to the new channel.</param>
        /// <returns>The new channel.</returns>
        public IRCChannel CreateChannel(string displayName, bool switchTo)
        {
            IRCChannel retval = new IRCChannel(this, this.Connection);
            retval.Disposed += new IRCChannel.DisposedHandler(this.ChannelDisposed);

            TabControl tabControl = this.ServerTab.Parent as TabControl;
            tabControl.InvokeAction(() =>
            {
                IRCTabType type = IRCTabType.Channel;
                Regex regex = new Regex(@"(?<channel>[#&][^\x07\x2C\s]{1,199})", RegexOptions.IgnoreCase);
                if (!regex.Match(displayName).Success)
                {
                    type = IRCTabType.PM;
                }

                string tabName = string.Format("{0}_{1}", this.Connection.ToString(), displayName);
                IRCTabPage tabPage = new IRCTabPage(tabName, displayName, type);
                tabPage.Connection = this.Connection;
                tabPage.ConnectionSpecificName = displayName;

                retval.TabPage = tabPage;
                retval.TabPage.Marshal = this;
                retval.Name = displayName;

                tabControl.TabPages.Add(tabPage);
                if (switchTo)
                {
                    tabControl.SelectedTab = tabPage;
                }

                this.Channels.Add(retval);
            });

            if (this.channelCreated != null)
            {
                this.channelCreated.Invoke(this, retval);
            }

            return retval;
        }

        /// <summary>
        /// Process a message that is categorised as being a numeric reply in the RFC documents.
        /// </summary>
        /// <param name="message">The message to process.</param>
        private void ProcessNumericReply(Message message)
        {
            Reply reply = (Reply)int.Parse(message.Command);
            Action<IRCTabPage> appendMessage = (IRCTabPage tabPage) =>
            {
                MessageTypeAttribute attributes = IRCMarshal.GetReplyAttributes(reply);
                MessageType messageType = attributes.MessageType;
                GlobalSettings settings = new GlobalSettings();
                string source = attributes.Source;

                string content;
                if (attributes.OutputAction == null)
                {
                    content = message.ToString(attributes.OutputFormat, attributes.ParameterDelimiter, attributes.RemoveFirstParameter);
                }
                else
                {
                    content = attributes.OutputAction.Invoke(message);
                }

                if (settings.DebugMode == GlobalSettings.Boolean.Yes)
                {
                    tabPage.AppendMessage(reply.ToString(), "[RAW]", message.ToString(), MessageType.WarningMessage);
                }

                tabPage.AppendMessage(reply.ToString(), source, content, messageType);
            };

            // If we have a names reply or an end of names reply, then we need to check the channel 
            // it is in regards to exists in our channel list, and if it does check to see if it 
            // is awaiting a reply from a names request (i.e. it is wanting to refresh the user list).

            // If this is indeed the case, we need to force it through to that channel rather than
            // following the default procedure of going to the selected tab.
            if ((reply == Reply.RPL_NAMREPLY) || (reply == Reply.RPL_ENDOFNAMES))
            {
                string target = string.Empty;
                if (reply == Reply.RPL_NAMREPLY)
                {
                    target = message.Parameters[2];
                }
                else
                {
                    target = message.Parameters[1];
                }

                IRCChannel channel = this.channels.Find(i => i.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
                if (channel != null && channel.ExpectingNamesMessage)
                {
                    channel.HandleReply(message);
                    return;
                }
            }

            // If the currently selected tab belongs to the channel list for this connection
            // AND we aren't awaiting a mode message (i.e. connecting to the server)
            // then marshal the message to the owning channel, otherwise default to the server tab
            TabControl tabControl = this.ServerTab.Parent as TabControl;
            tabControl.InvokeAction(() =>
            {
                IRCChannel selectedChannel = this.Channels.Find(i => tabControl.SelectedTab.Equals(i.TabPage));
                if ((selectedChannel != null) && (!this.awaitingModeMessage))
                {
                    selectedChannel.HandleReply(message);
                }
                else
                {
                    appendMessage.Invoke(this.ServerTab);
                }
            });

            // If a nick in use message comes through, we need to revert the nick against the connection
            // back to the previously assigned nick (if there is one).
            // If there wasn't one, then we'll append an underscore to the current one and resend the nick message
            // we couldn't possibly get stuck in a loop, right?
            if (reply == Reply.ERR_NICKNAMEINUSE)
            {
                if (this.previousNickName.Equals(this.Connection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    this.previousNickName = string.Format("{0}_", this.previousNickName);
                    this.connection.Nickname = this.previousNickName;
                    this.Send(this.ServerTab, new NickMessage(this.previousNickName));
                }
                else
                {
                    if (!this.awaitingModeMessage)
                    {
                        this.Connection.Nickname = this.previousNickName;
                    }
                    else
                    {
                        this.previousNickName = string.Format("{0}_", this.connection.Nickname);
                        this.connection.Nickname = this.previousNickName;
                        this.Send(this.ServerTab, new NickMessage(this.previousNickName));
                    }
                }
            }
        }

        /// <summary>
        /// Process a message that contains a text based command.
        /// </summary>
        /// <param name="message">The message to process.</param>
        private void ProcessMessage(Message message)
        {
            message = MessageFactory.AssimilateMessage(message);
            string target = message.Target.Equals(this.Connection.Nickname, StringComparison.OrdinalIgnoreCase) ? message.Source : message.Target;

            if (message.MustBeHandledByTarget)
            {
                IRCChannel channel = this.Channels.Find(i => i.Name.Equals(target, StringComparison.OrdinalIgnoreCase));
                if (channel == null)
                {
                    channel = this.CreateChannel(target, false);
                }

                channel.HandleMessage(message);
            }
            else if (message.IsMultiChannelMessage)
            {
                for (int i = 0; i < this.Channels.Count; i++)
                {
                    this.Channels[i].HandleMessage(message);
                }
            }
            else if (message.IsGlobalMessage)
            {
                this.Channels.ForEach(i => i.HandleMessage(message));
                this.ServerTab.AppendMessage(message.Command, message.Source, message.Content, message.Type);
            }
            else
            {
                // If the message can be handled by both the server tab
                // and a channel tab, then we need to check if the currently
                // focused tab is a channel and if so handle it in the channel.
                // Otherwise we can just print it in the server tab
                TabControl tabControl = this.ServerTab.Parent as TabControl;
                tabControl.InvokeAction(() =>
                {
                    if (tabControl.SelectedTab is IRCTabPage)
                    {
                        IRCChannel channel = this.Channels.Find(i => i.TabPage.Equals(tabControl.SelectedTab));
                        if ((channel != null) && (!this.awaitingModeMessage))
                        {
                            channel.HandleMessage(message);
                        }
                        else
                        {
                            this.ServerTab.AppendMessage(message.Command, message.Source, message.Content, message.Type);
                        }
                    }
                });
            }

            // If we are still awaiting the MODE message (i.e. we are awaiting confirmation we have finished connecting)
            // then also re-join any channels that we have in the Channels collection (i.e. reconnect to any channels)
            if (this.awaitingModeMessage && message is ModeMessage)
            {
                GlobalSettings settings = new GlobalSettings();
                this.awaitingModeMessage = false;
                this.Send(this.ServerTab, new ModeMessage(new string[] { this.Connection.Nickname, settings.Mode }));
                this.Channels.ForEach(i => 
                    { 
                        if (i.TabPage.TabType == IRCTabType.Channel) 
                        { 
                            this.Send(this.ServerTab, new JoinMessage(i.Name)); 
                        } 
                    });
            }
        }

        /// <summary>
        /// Reconnects to the server.
        /// </summary>
        private void Reconnect()
        {
            this.reconnecting = true;
            this.awaitingModeMessage = true;
            if (this.reconnectTimer == null)
            {
                this.reconnectTimer = new System.Timers.Timer(15000);
                this.reconnectTimer.Elapsed += new System.Timers.ElapsedEventHandler(this.ReconnectTimer_Elapsed);
                this.reconnectTimer.Enabled = true;
            }
        }

        /// <summary>
        /// Handles the Elapsed event of System.Timers.Timer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ReconnectTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            this.Connection.Connect();
            this.reconnectTimer.Enabled = false;
            this.reconnectTimer.Dispose();
            this.reconnectTimer = null;
        }

        /// <summary>
        /// Handles the BeganConnecting event of Yaircc.Net.IRC.Connection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void BeganConnecting(object sender, EventArgs e)
        {
            this.PerformServerTabAction(() =>
            {
                string message = string.Format(Strings_Connection.AttemptingConnection, this.Connection.Server);
                this.ServerTab.AppendMessage(null, Strings_Connection.Info, message, MessageType.ServerMessage);
            });
        }

        /// <summary>
        /// Handles the ConnectionEstablished event of Yaircc.Net.IRC.Connection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ConnectionEstablished(object sender, EventArgs e)
        {
            this.reconnecting = false;

            this.PerformServerTabAction(() =>
            {
                string message = string.Format(Strings_Connection.EstablishedConnection, this.Connection.Server);
                this.ServerTab.AppendMessage(null, Strings_Connection.Info, message, MessageType.ServerMessage);
                this.Connection.RegisterClient();
            });
        }

        /// <summary>
        /// Handles the ConnectionFailed event of Yaircc.Net.IRC.Connection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ConnectionFailed(object sender, ConnectionFailedEventArgs e)
        {
            this.PerformServerTabAction(() =>
            {
                string message = string.Format(this.reconnecting ? Strings_Connection.ConnectionFailedRetrying : Strings_Connection.ConnectionFailed, this.Connection.Server, e.Exception.Message);
                this.ServerTab.AppendMessage(null, Strings_Connection.Error, message, MessageType.ErrorMessage);
            });

            if (this.reconnecting)
            {
                this.Reconnect();
            }
        }

        /// <summary>
        /// Handles the ConnectionTerminated event of Yaircc.Net.IRC.Connection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ConnectionTerminated(object sender, EventArgs e)
        {
            Action<IRCTabPage> action = (IRCTabPage tabPage) =>
            {
                if (tabPage != null)
                {
                    string message = string.Format(this.IsDisconnecting ? Strings_Connection.ConnectionTerminated : Strings_Connection.ConnectionLostReconnecting, this.Connection.Server);
                    tabPage.AppendMessage(null, Strings_Connection.Info, message, MessageType.ErrorMessage);
                }
            };

            action.Invoke(this.ServerTab);
            this.Channels.ForEach(i => action.Invoke(i.TabPage));

            if (!this.IsDisconnecting)
            {
                this.Reconnect();
            }
        }

        /// <summary>
        /// Handles the DataReceived event of Yaircc.Net.IRC.Connection.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void DataReceived(object sender, DataReceivedEventArgs e)
        {
            Message message = MessageFactory.AssimilateMessage(Message.ParseMessage(e.Data));
            message.AssociatedConnection = this.connection;
            if (!string.IsNullOrEmpty(message.Command))
            {
                if (message.Command.IsNumeric())
                {
                    this.ProcessNumericReply(message);
                }
                else
                {
                    this.ProcessMessage(message);
                }
            }
        }

        /// <summary>
        /// Handles the Disposed event of Yaircc.UI.IRCChannel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        private void ChannelDisposed(object sender)
        {
            if ((!this.disposing) && (sender is IRCChannel))
            {
                IRCChannel channel = sender as IRCChannel;
                this.Channels.Remove(channel);
            }
        }

        /// <summary>
        /// Invoke an action if the server tab has been instantiated.
        /// </summary>
        /// <param name="action">The action to invoke.</param>
        private void PerformServerTabAction(Action action)
        {
            if (this.ServerTab != null)
            {
                action.Invoke();
            }
        }

        /// <summary>
        /// Setup the event handlers required by the connection.
        /// </summary>
        private void SetupConnectionEventHandlers()
        {
            this.Connection.BeganConnecting += new Connection.BeganConnectingHandler(this.BeganConnecting);
            this.Connection.ConnectionEstablished += new Connection.ConnectionEstablishedHandler(this.ConnectionEstablished);
            this.Connection.ConnectionFailed += new Connection.ConnectionFailedHandler(this.ConnectionFailed);
            this.Connection.ConnectionTerminated += new Connection.ConnectionTerminatedHandler(this.ConnectionTerminated);
            this.Connection.DataReceived += new Connection.DataReceivedHandler(this.DataReceived);
        }

        #endregion
    }
}