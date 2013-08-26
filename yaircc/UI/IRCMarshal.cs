//-----------------------------------------------------------------------
// <copyright file="IRCMarshal.cs" company="rastating">
//     yaircc - the free, open-source IRC client for Windows.
//     Copyright (C) 2012-2013 Robert Carr
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see http://www.gnu.org/licenses/.
// </copyright>
//-----------------------------------------------------------------------

namespace Yaircc.UI
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using System.Threading;
    using Yaircc.Localisation;
    using Yaircc.Net.IRC;
    using Yaircc.Settings;
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
        /// The TabControl that hosts the server and channel tabs.
        /// </summary>
        private TabControl tabHost;

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
        /// A value indicating whether or not the initial user host message is pending.
        /// </summary>
        private bool awaitingUserHostMessage;

        /// <summary>
        /// A timer used to invoke a connection attempt to the server.
        /// </summary>
        private System.Timers.Timer reconnectTimer;

        /// <summary>
        /// Occurs when a channel is created via the marshal.
        /// </summary>
        private ChannelCreatedHandler channelCreated;

        /// <summary>
        /// Occurs when a marshal un-sets both the <see cref="AwaitingModeMessage" /> and awaiting <see cref="UserHostMessage" /> flags.
        /// </summary>
        private NetworkRegisteredHandler networkRegistered;

        /// <summary>
        /// A list of commands to execute when the mode message has been received.
        /// </summary>
        private List<string> autoCommands;

        /// <summary>
        /// The AutoResetEvent that signals to the queue processing thread to stop waiting.
        /// </summary>
        private AutoResetEvent queueResetEvent;

        /// <summary>
        /// The thread that processes the queue of pending messages.
        /// </summary>
        private Thread queueProcessingThread;

        /// <summary>
        /// A queue containing messages to be processed by the marshal to the user interface.
        /// </summary>
        private Queue<Message> messageQueue;

        /// <summary>
        /// The channel browser associated with the connection.
        /// </summary>
        private ChannelBrowser channelBrowser;

        /// <summary>
        /// The owning form of the marshal.
        /// </summary>
        private MainForm parent;

        /// <summary>
        /// The full user host that will be seen by other clients.
        /// </summary>
        private string fullUserHost;

        /// <summary>
        /// A value indicating whether or not the auto commands for the network have been executed.
        /// </summary>
        private bool hasExecutedAutoCommands;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="IRCMarshal"/> class.
        /// </summary>
        /// <param name="connection">The connection to marshal data to and from.</param>
        /// <param name="tabHost">The TabControl that hosts the server and channel tabs.</param>
        /// <param name="autoCommands">A queue of commands to automatically execute once a connection is fully established.</param>
        /// <param name="parent">The owning form.</param>
        public IRCMarshal(Connection connection, TabControl tabHost, List<string> autoCommands, MainForm parent)
        {
            this.tabHost = tabHost;
            this.parent = parent;
            this.previousNickName = connection.Nickname;
            this.AwaitingModeMessage = true;
            this.AwaitingUserHostMessage = true;
            this.connection = connection;
            this.SetupConnectionEventHandlers();
            this.channels = new List<IRCChannel>();
            this.messageQueue = new Queue<Message>(15);
            this.channelBrowser = new ChannelBrowser(this);

            if (autoCommands != null)
            {
                this.autoCommands = autoCommands;
            }
            else
            {
                this.autoCommands = new List<string>();
            }

            this.queueResetEvent = new AutoResetEvent(false);
            this.queueProcessingThread = new Thread(this.ProcessQueue);
            this.queueProcessingThread.Start();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for event <see cref="ChannelCreated"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The channel that was created.</param>
        public delegate void ChannelCreatedHandler(object sender, IRCChannel channel);

        /// <summary>
        /// Delegate for event <see cref="NetworkRegistered" />.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void NetworkRegisteredHandler(object sender, EventArgs e);

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

        /// <summary>
        /// Occurs when a marshal un-sets both the <see cref="AwaitingModeMessage" /> and awaiting <see cref="UserHostMessage" /> flags.
        /// </summary>
        public event NetworkRegisteredHandler NetworkRegistered
        {
            add { this.networkRegistered += value; }
            remove { this.networkRegistered -= value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not the initial mode message is pending.
        /// </summary>
        public bool AwaitingModeMessage
        {
            get 
            { 
                return this.awaitingModeMessage; 
            }

            private set
            {
                if (value || this.AwaitingUserHostMessage)
                {
                    this.parent.InvokeAction(() => this.parent.MarshalUnregistered(this));
                }
                else
                {
                    this.parent.InvokeAction(() => this.parent.MarshalRegistered(this));
                }

                this.awaitingModeMessage = value;

                if (this.IsConnectedAndRegistered && this.networkRegistered != null)
                {
                    this.networkRegistered.Invoke(this, EventArgs.Empty);
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the initial user host message is pending.
        /// </summary>
        public bool AwaitingUserHostMessage
        {
            get
            {
                return this.awaitingUserHostMessage;
            }

            private set
            {
                if (value || this.AwaitingModeMessage)
                {
                    this.parent.InvokeAction(() => this.parent.MarshalUnregistered(this));
                }
                else
                {
                    this.parent.InvokeAction(() => this.parent.MarshalRegistered(this));
                }

                this.awaitingUserHostMessage = value;

                if (this.IsConnectedAndRegistered && this.networkRegistered != null)
                {
                    this.networkRegistered.Invoke(this, EventArgs.Empty);
                }
            }
        }

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
        /// Gets the full user host that will be seen by other clients.
        /// </summary>
        public string FullUserHost
        {
            get { return this.fullUserHost; }
        }

        /// <summary>
        /// Gets or sets the TabControl that hosts the server and channel tabs.
        /// </summary>
        public TabControl TabHost
        {
            get { return this.tabHost; }
            set { this.tabHost = value; }
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
            get 
            {
                bool retval = this.connection.IsConnected;

                if (!retval)
                {
                    this.hasExecutedAutoCommands = false;
                }

                return retval; 
            }
        }

        /// <summary>
        /// Gets the channel browser associated with the connection.
        /// </summary>
        public ChannelBrowser ChannelBrowser
        {
            get
            {
                if (this.channelBrowser == null)
                {
                    this.channelBrowser = new ChannelBrowser(this);
                }

                return this.channelBrowser;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not the server is being disconnected from.
        /// </summary>
        public bool IsDisconnecting
        {
            get { return this.disconnecting; }
            set { this.disconnecting = value; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the user has connected and registered on the network.
        /// </summary>
        public bool IsConnectedAndRegistered
        {
            get { return !this.AwaitingModeMessage && !this.AwaitingUserHostMessage; }
        }

        /// <summary>
        /// Gets a list of commands to execute when the mode message has been received.
        /// </summary>
        private List<string> AutoCommands
        {
            get { return this.autoCommands; }
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
            this.queueResetEvent.Set();

            if (this.Connection.IsConnected)
            {
                this.connection.Close();
            }

            if (this.channelBrowser != null)
            {
                this.channelBrowser.Dispose();
            }

            this.Channels.ForEach(i => i.Dispose());
            this.Channels.Clear();
            this.TabHost.InvokeAction(() => this.TabHost.TabPages.Remove(this.ServerTab));
            this.ServerTab.Dispose();
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
                    IRCChannel channel = this.Channels.Find(i => i.Name.Equals(message.Parameters[0], StringComparison.OrdinalIgnoreCase));
                    if (channel == null)
                    {
                        channel = this.CreateChannel(message.Parameters[0], true);
                    }
                    else
                    {
                        // If we're already in the channel, just switch tabs.
                        this.TabHost.InvokeAction(() => this.TabHost.SelectedTab = channel.TabPage);
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

            IRCTabType type = IRCTabType.Channel;
            Regex regex = new Regex(@"(?<channel>[#&][^\x07\x2C\s]{1,199})", RegexOptions.IgnoreCase);
            if (!regex.Match(displayName).Success)
            {
                type = IRCTabType.PM;
            }

            this.TabHost.InvokeAction(() =>
                {
                    string tabName = string.Format("{0}_{1}", this.Connection.ToString(), displayName);
                    IRCTabPage tabPage = new IRCTabPage(this.ServerTab.OwningForm, tabName, displayName, type);
                    tabPage.Connection = this.Connection;
                    tabPage.ConnectionSpecificName = displayName;

                    retval.TabPage = tabPage;
                    retval.TabPage.Marshal = this;
                    retval.Name = displayName;

                    // Iterate through the tab pages to figure out how to place
                    // this channels tab page in a group of other ones on the
                    // same network, in alphabetical order.
                    int index = this.TabHost.TabPages.IndexOf(this.ServerTab) + 1;
                    for (int i = index; i < this.TabHost.TabPages.Count; i++)
                    {
                        IRCTabPage tab = this.TabHost.TabPages[i] as IRCTabPage;
                        if (tab != null && tab.Marshal == this && tab.Text.CompareTo(retval.TabPage.Text) < 0)
                        {
                            index = i + 1;
                        }
                        else if (tab != null && tab.Marshal != this)
                        {
                            break;
                        }
                    }
                    
                    this.TabHost.TabPages.Insert(index, tabPage);
                    if (switchTo)
                    {
                        this.TabHost.SelectedTab = tabPage;
                    }
                });

            this.Channels.Add(retval);

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
                GlobalSettings settings = GlobalSettings.Instance;
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

            // If we are retrieving list replies we need to populate the channel browser
            if (reply == Reply.RPL_LISTSTART)
            {
                this.parent.InvokeAction(() => this.ChannelBrowser.BeginRefresh(true));
                return;
            }
            else if (reply == Reply.RPL_LIST)
            {
                this.ChannelBrowser.AddChannel(message);
                return;
            }
            else if (reply == Reply.RPL_LISTEND)
            {
                this.parent.InvokeAction(() => this.ChannelBrowser.FlushChannels());
                return;
            }

            // If we are still awaiting the UserHost reply (i.e. we are awaiting confirmation of the full userhost 
            // prefix that we will use to determine the max PRIVMSG lengths) then cache it in the marshal for future
            // reference.
            if (this.AwaitingUserHostMessage && reply == Reply.RPL_USERHOST)
            {
                this.fullUserHost = message.TrailingParameter;
                this.AwaitingUserHostMessage = false;

                // Execute any auto commands.
                if (!this.hasExecutedAutoCommands && this.AutoCommands.Count > 0)
                {
                    for (int i = 0; i < this.AutoCommands.Count; i++)
                    {
                        MessageParseResult parseResult = MessageFactory.CreateFromUserInput(this.AutoCommands[i], null);
                        if (parseResult.Success)
                        {
                            this.Send(this.ServerTab, parseResult.IRCMessage);
                        }
                    }

                    this.hasExecutedAutoCommands = true;
                }

                if (this.reconnecting)
                {
                    // Pause the thread for a second to give time for any authentication to 
                    // take place and then rejoin the channels.
                    System.Threading.Thread.Sleep(1000);
                    this.Channels.ForEach(i =>
                    {
                        if (i.TabPage.TabType == IRCTabType.Channel)
                        {
                            this.Send(this.ServerTab, new JoinMessage(i.Name));
                        }
                    });

                    this.reconnecting = false;
                }

                return;
            }

            // If the user has received a new hidden host then we need to re-evaluate
            // their full user host mask that will be seen by other clients.
            if (reply == Reply.RPL_HOSTHIDDEN)
            {
                this.AwaitingUserHostMessage = true;
                this.Send(this.ServerTab, new UserHostMessage(new string[] { this.connection.Nickname }));
            }

            // If we have a names reply or an end of names reply, then we need to check the channel 
            // it is in regards to exists in our channel list, and if it does check to see if it 
            // is awaiting a reply from a names request (i.e. it is wanting to refresh the user list).
            //
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
            this.TabHost.InvokeAction(() =>
                {
                    IRCChannel selectedChannel = this.Channels.Find(i => this.TabHost.SelectedTab.Equals(i.TabPage));
                    if ((selectedChannel != null) && (!this.AwaitingModeMessage))
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
                    if (!this.AwaitingModeMessage)
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
                this.TabHost.InvokeAction(() =>
                    {
                        if (this.TabHost.SelectedTab is IRCTabPage)
                        {
                            IRCChannel channel = this.Channels.Find(i => i.TabPage.Equals(this.TabHost.SelectedTab));
                            if ((channel != null) && (!this.AwaitingModeMessage))
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
            if (this.AwaitingModeMessage && message is ModeMessage)
            {
                this.AwaitingModeMessage = false;
                this.Send(this.ServerTab, new ModeMessage(new string[] { this.Connection.Nickname, this.Connection.Mode }));

                // Send the user host message so we can determine the full user host string.
                this.Send(this.ServerTab, new UserHostMessage(new string[] { this.connection.Nickname }));
            }
        }

        /// <summary>
        /// Reconnects to the server.
        /// </summary>
        private void Reconnect()
        {
            this.reconnecting = true;
            this.AwaitingModeMessage = true;
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

            lock (this.messageQueue)
            {
                this.messageQueue.Enqueue(message);
                this.queueResetEvent.Set();
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
        /// Processes all the current items in the message queue.
        /// </summary>
        private void ProcessQueue()
        {
            while (!this.disposing)
            {
                this.queueResetEvent.WaitOne();

                Message message = null;
                lock (this.messageQueue)
                {
                    message = this.messageQueue.Count > 0 ? this.messageQueue.Dequeue() : null;
                }

                while (message != null)
                {
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

                    lock (this.messageQueue)
                    {
                        message = this.messageQueue.Count > 0 ? this.messageQueue.Dequeue() : null;
                    }
                }
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