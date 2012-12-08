//-----------------------------------------------------------------------
// <copyright file="IRCChannel.cs" company="intninety">
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
    using Yaircc.Net.IRC;
    using TabControl = System.Windows.Forms.TabControl;

    /// <summary>
    /// Represents an IRC channel accessible via an <see cref="IRCTabPage"/>.
    /// </summary>
    public class IRCChannel : IDisposable
    {
        #region Events

        #endregion

        #region Fields

        /// <summary>
        /// Occurs when the channel's user list has been populated.
        /// </summary>
        private NamesPopulatedHandler namesPopulated;

        /// <summary>
        /// Occurs when the channel 
        /// </summary>
        private DisposedHandler disposed;

        /// <summary>
        /// The tab page that provides access to the channel.
        /// </summary>
        private IRCTabPage tabPage;

        /// <summary>
        /// A value indicating whether or not the channel has requested the names list.
        /// </summary>
        private bool expectingNamesMessage;

        /// <summary>
        /// The list of users in the channel.
        /// </summary>
        private List<IRCUser> users;

        /// <summary>
        /// The name of the channel.
        /// </summary>
        private string name;

        /// <summary>
        /// The connection the channel operates on.
        /// </summary>
        private Connection connection;

        /// <summary>
        /// The network marshal for the channel.
        /// </summary>
        private IRCMarshal marshal;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="IRCChannel"/> class.
        /// </summary>
        /// <param name="marshal">The network marshal for the channel.</param>
        /// <param name="connection">The connection the channel operates on.</param>
        public IRCChannel(IRCMarshal marshal, Connection connection)
        {
            this.marshal = marshal;
            this.connection = connection;
            this.users = new List<IRCUser>();
            this.expectingNamesMessage = true;
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for event <see cref="NamesPopulated"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="users">The list of users that have been populated.</param>
        public delegate void NamesPopulatedHandler(object sender, List<IRCUser> users);

        /// <summary>
        /// Delegate for event <see cref="Disposed"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        public delegate void DisposedHandler(object sender);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the channel's user list has been populated.
        /// </summary>
        public event NamesPopulatedHandler NamesPopulated
        {
            add { this.namesPopulated += value; }
            remove { this.namesPopulated -= value; }
        }

        /// <summary>
        /// Occurs when the channel 
        /// </summary>
        public event DisposedHandler Disposed
        {
            add { this.disposed += value; }
            remove { this.disposed -= value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        public string Name
        {
            get { return this.name; }
            set { this.name = value; }
        }

        /// <summary>
        /// Gets or sets the tab page that provides access to the channel.
        /// </summary>
        public IRCTabPage TabPage
        {
            get { return this.tabPage; }
            set { this.tabPage = value; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the channel has requested the names list.
        /// </summary>
        public bool ExpectingNamesMessage
        {
            get { return this.expectingNamesMessage; }
        }

        /// <summary>
        /// Gets the list of users in the channel.
        /// </summary>
        public List<IRCUser> Users
        {
            get { return this.users; }
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Releases all resources used by the IRCChannel.
        /// </summary>
        public void Dispose()
        {
            TabControl tabControl = this.TabPage.Parent as TabControl;
            tabControl.InvokeAction(() =>
            {
                if ((tabControl.SelectedTab == this.TabPage) && (tabControl.SelectedIndex > 0))
                {
                    tabControl.SelectedIndex--;
                }

                tabControl.TabPages.Remove(this.TabPage);
                this.TabPage.Dispose();
                this.users.Clear();
            });

            if (this.disposed != null)
            {
                this.disposed.Invoke(this);
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}_{1}", this.connection.ToString(), this.Name);
        }

        /// <summary>
        /// Do any channel level processing required for the reply.
        /// </summary>
        /// <param name="message">The reply message to process.</param>
        public void HandleReply(Message message)
        {
            Reply reply = (Reply)int.Parse(message.Command);
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

            if (this.expectingNamesMessage && reply == Reply.RPL_NAMREPLY)
            {
                this.PopulateChannelNamesFromMessage(message);
            }
            else if (this.expectingNamesMessage && reply == Reply.RPL_ENDOFNAMES)
            {
                this.expectingNamesMessage = false;
                if (this.namesPopulated != null)
                {
                    this.namesPopulated.Invoke(this, this.users);
                }
            }
            else
            {
                if (settings.DebugMode == GlobalSettings.Boolean.Yes)
                {
                    this.tabPage.AppendMessage(reply.ToString(), "[RAW]", message.ToString(), MessageType.WarningMessage);
                }

                this.tabPage.AppendMessage(reply.ToString(), source, content, messageType);
            }
        }

        /// <summary>
        /// Do any channel level processing required for the message.
        /// </summary>
        /// <param name="message">The message to process.</param>
        public void HandleMessage(Message message)
        {
            Action namesRequest = () =>
            {
                if (!this.expectingNamesMessage)
                {
                    this.users.Clear();
                    this.expectingNamesMessage = true;
                    this.marshal.Send(this.TabPage, new NamesMessage(this.Name));
                }
            };

            if (message is JoinMessage)
            {
                namesRequest.Invoke();
            }
            else if (message is PartMessage)
            {
                PartMessage partMessage = message as PartMessage;
                if (partMessage.NickName.Equals(this.marshal.Connection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    this.Dispose();
                    return;
                }
                else
                {
                    IRCUser user = this.users.Find(i => i.NickName.Equals(partMessage.NickName, StringComparison.OrdinalIgnoreCase));
                    this.TabPage.RemoveUserFromList(user);
                    this.users.Remove(user);
                }
            }
            else if (message is KickMessage)
            {
                KickMessage kickMessage = message as KickMessage;
                IRCUser user = this.users.Find(i => i.NickName.Equals(kickMessage.NickName, StringComparison.OrdinalIgnoreCase));
                this.TabPage.RemoveUserFromList(user);
                this.users.Remove(user);
            }
            else if (message is NickMessage)
            {
                NickMessage nickMessage = message as NickMessage;
                IRCUser user = this.users.Find(i => i.NickName.Equals(nickMessage.OldNickname, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    this.TabPage.RemoveUserFromList(user);
                    user.NickName = nickMessage.NewNickname;
                    this.TabPage.AddUserToList(user, true);
                }
                else
                {
                    return;
                }
            }
            else if (message is QuitMessage)
            {
                QuitMessage quitMessage = message as QuitMessage;
                IRCUser user = this.users.Find(i => i.NickName.Equals(quitMessage.NickName, StringComparison.OrdinalIgnoreCase));
                if (user != null)
                {
                    this.TabPage.RemoveUserFromList(user);
                    this.users.Remove(user);
                }
                else
                {
                    return;
                }
            }
            else if (message is ModeMessage)
            {
                namesRequest.Invoke();
            }

            GlobalSettings settings = new GlobalSettings();
            if (settings.DebugMode == GlobalSettings.Boolean.Yes)
            {
                this.TabPage.AppendMessage(message.Command, "[RAW]", message.ToString(), MessageType.WarningMessage);
            }

            this.TabPage.AppendMessage(message.Command, message.Source, message.Content, message.Type);
        }

        /// <summary>
        /// Populate the user list from a names message.
        /// </summary>
        /// <param name="message">The message to process.</param>
        private void PopulateChannelNamesFromMessage(Message message)
        {
            string[] names = message.TrailingParameter.Split(new char[] { ' ' });
            for (int i = 0; i < names.GetLength(0); i++)
            {
                if (!string.IsNullOrEmpty(names[i]))
                {
                    this.users.Add(IRCUser.Parse(names[i]));
                }
            }
        }

        #endregion
    }
}