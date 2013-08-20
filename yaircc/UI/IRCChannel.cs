//-----------------------------------------------------------------------
// <copyright file="IRCChannel.cs" company="rastating">
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
    using Yaircc.Net.IRC;
    using Yaircc.Settings;
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

        /// <summary>
        /// The maximum number of characters that can be sent in a PRIVMSG message.
        /// </summary>
        private int maximumMessageSize;

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
        /// Gets the maximum number of characters that can be sent in a PRIVMSG message.
        /// </summary>
        public int MaximumMessageSize
        {
            get 
            {
                // The maximum message size (including headers) for an IRC message is 512 characters
                // and as the headers vary from channel to channel we need to construct an empty
                // PRIVMSG message to calculate how much room we have for the user's message.
                PrivMsgMessage blankMessage = new PrivMsgMessage(this.name, string.Empty);
                this.maximumMessageSize = 508 - blankMessage.ToString().Length - this.marshal.FullUserHost.Length;
                return this.maximumMessageSize;
            }
        }

        /// <summary>
        /// Gets or sets the name of the channel.
        /// </summary>
        public string Name
        {
            get 
            { 
                return this.name; 
            }

            set 
            { 
                this.name = value;
            }
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
            this.marshal.TabHost.InvokeAction(() =>
                {
                    if ((this.marshal.TabHost.SelectedTab == this.TabPage) && (this.marshal.TabHost.SelectedIndex > 0))
                    {
                        this.marshal.TabHost.SelectedIndex--;
                    }

                    this.marshal.TabHost.TabPages.Remove(this.TabPage);
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

            GlobalSettings settings = GlobalSettings.Instance;
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