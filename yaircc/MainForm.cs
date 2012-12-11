//-----------------------------------------------------------------------
// <copyright file="MainForm.cs" company="intninety">
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

namespace Yaircc
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Drawing;
    using System.IO;
    using System.Linq;
    using System.Reflection;
    using System.Text;
    using System.Windows.Forms;
    using Intninety;
    using Yaircc.Localisation;
    using Yaircc.Net;
    using Yaircc.Net.IRC;
    using Yaircc.UI;
    using Message = Yaircc.Net.IRC.Message;
    
    /// <summary>
    /// Represents the main form used in the application.
    /// </summary>
    public partial class MainForm : Form
    {
        #region Fields

        /// <summary>
        /// A value indicating whether or not the update check was automatically invoked.
        /// </summary>
        private bool autoCheckingForUpdate;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the MainForm class.
        /// </summary>
        public MainForm()
        {
            this.InitializeComponent();
            Yaircc.Properties.Settings.Default.Upgrade();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not the form is in the foreground.
        /// </summary>
        public bool IsForegroundWindow
        {
            get
            {
                return this.Handle.Equals(GetForegroundWindow());
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Handles the ChannelCreated event of Yaircc.UI.IRCMarshal.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="channel">The newly created channel.</param>
        public void ChannelCreated(object sender, IRCChannel channel)
        {
            this.splitContainer.Panel1.InvokeAction(() =>
            {
                channel.NamesPopulated += new IRCChannel.NamesPopulatedHandler(this.ChannelUsersPopulated);

                TreeView userTreeView = new TreeView();
                userTreeView.Name = string.Format("{0}_TreeView", channel.ToString());
                userTreeView.ImageList = userModeImageList;
                userTreeView.TreeViewNodeSorter = new UserNodeSorter();
                userTreeView.MouseDown += new MouseEventHandler(UserTreeView_MouseDown);

                if (channel.TabPage.TabType == IRCTabType.Channel)
                {
                    userTreeView.Nodes.Add("Fetching user list...");
                }
                else if (channel.TabPage.TabType == IRCTabType.PM)
                {
                    TreeNode node = new TreeNode();
                    node.Text = channel.Name;
                    node.ImageKey = "user";
                    node.SelectedImageKey = node.ImageKey;
                    userTreeView.Nodes.Add(node);
                }

                splitContainer.Panel1.Controls.Add(userTreeView);
                userTreeView.Dock = DockStyle.Fill;

                channel.TabPage.UserTreeView = userTreeView;
                channel.TabPage.NickNameMentioned += new IRCTabPage.NickNameMentionedHandler(this.FlashWindowInTaskBar);

                if (channelsTabControl.SelectedTab == channel.TabPage)
                {
                    userTreeView.BringToFront();
                }

                this.RefreshWindowsMenuItems(null);
            });
        }

        /// <summary>
        /// Handles the NamesPopulated event of Yaircc.UI.IRCChannel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="users">The users that are in the channel.</param>
        public void ChannelUsersPopulated(object sender, List<IRCUser> users)
        {
            IRCChannel channel = sender as IRCChannel;
            channel.TabPage.UserTreeView.InvokeAction(() =>
            {
                string topNodePath = null;
                bool topNodeIsGroup = false;

                if (channel.TabPage.UserTreeView.TopNode != null)
                {
                    if (channel.TabPage.UserTreeView.TopNode.Tag is IRCUser)
                    {
                        topNodePath = channel.TabPage.UserTreeView.TopNode.FullPath;
                    }
                    else if (channel.TabPage.UserTreeView.TopNode.Nodes.Count > 0)
                    {
                        topNodeIsGroup = true;
                        topNodePath = channel.TabPage.UserTreeView.TopNode.Nodes[0].FullPath;
                    }
                    else
                    {
                    }
                }

                channel.TabPage.UserTreeView.Nodes.Clear();
                TreeNode topNode = null;

                for (int i = 0; i < users.Count; i++)
                {
                    TreeNode node = channel.TabPage.AddUserToList(users[i]);
                    if (topNodePath != null && node.FullPath.Equals(topNodePath))
                    {
                        topNode = topNodeIsGroup ? node.Parent : node;
                    }
                }

                channel.TabPage.UserTreeView.Sort();
                channel.TabPage.RefreshTreeNodeCollapseState();

                if (topNode != null)
                {
                    channel.TabPage.UserTreeView.TopNode = topNode;
                }
                else
                {
                    channel.TabPage.UserTreeView.TopNode = channel.TabPage.UserTreeView.Nodes[0];
                }
            });
        }

        /// <summary>
        /// Configures a newly created Yaircc.UI.IRCMarshal.
        /// </summary>
        /// <param name="sender">The source of the invocation.</param>
        /// <param name="marshal">The marshal that was created.</param>
        public void ConfigureNewIRCMarshal(object sender, IRCMarshal marshal)
        {
            TreeView treeView = new TreeView();
            treeView.Name = string.Format("{0}_TreeView", marshal.Connection.ToString());
            treeView.ImageList = this.userModeImageList;

            TreeNode node = new TreeNode();
            node.Text = marshal.Connection.ToString();
            node.ImageKey = "lightning.png";
            node.SelectedImageKey = node.ImageKey;
            treeView.Nodes.Add(node);

            this.splitContainer.Panel1.Controls.Add(treeView);
            treeView.Dock = DockStyle.Fill;

            marshal.ServerTab.UserTreeView = treeView;
            marshal.ServerTab.NickNameMentioned += new IRCTabPage.NickNameMentionedHandler(this.FlashWindowInTaskBar);
            treeView.BringToFront();

            this.RefreshWindowsMenuItems(null);
        }

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern IntPtr GetForegroundWindow();

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void AboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutDialog dialog = new AboutDialog())
            {
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the ControlRemoved event of System.Windows.Forms.TabControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelsTabControl_ControlRemoved(object sender, ControlEventArgs e)
        {
            this.RefreshWindowsMenuItems(e.Control);
        }

        /// <summary>
        /// Handles the MouseDown event of System.Windows.Forms.TabControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelsTabControl_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                IRCTabPage tab = null;
                for (int i = 0; i < this.channelsTabControl.TabPages.Count; i++)
                {
                    if (this.channelsTabControl.GetTabRect(i).Contains(e.Location))
                    {
                        tab = this.channelsTabControl.TabPages[i] as IRCTabPage;
                    }
                }

                if (tab != null)
                {
                    if (tab.TabType == IRCTabType.Channel)
                    {
                        this.leaveChannelTabMenuItem.Visible = true;
                        this.leaveChannelTabMenuItem.Text = string.Format("Leave {0}", tab.Text);
                        this.tabSeparatorItem.Visible = true;
                        this.disconnectTabMenuItem.Visible = true;
                        this.disconnectTabMenuItem.Text = string.Format("Disconnect from {0}", tab.Marshal.Connection.ToString());
                        this.closeChannelTabMenuItem.Visible = false;
                    }
                    else if (tab.TabType == IRCTabType.Server)
                    {
                        this.leaveChannelTabMenuItem.Visible = false;
                        this.tabSeparatorItem.Visible = false;
                        this.disconnectTabMenuItem.Visible = true;
                        this.disconnectTabMenuItem.Text = string.Format("Disconnect from {0}", tab.Marshal.Connection.ToString());
                        this.closeChannelTabMenuItem.Visible = false;
                    }
                    else if (tab.TabType == IRCTabType.PM)
                    {
                        this.closeChannelTabMenuItem.Visible = true;
                        this.leaveChannelTabMenuItem.Visible = false;
                        this.tabSeparatorItem.Visible = true;
                        this.disconnectTabMenuItem.Visible = true;
                        this.disconnectTabMenuItem.Text = string.Format("Disconnect from {0}", tab.Marshal.Connection.ToString());
                    }

                    if (tab.TabType != IRCTabType.Console)
                    {
                        this.channelsTabControl.SelectedTab = tab;
                        this.tabContextMenu.Show(this.channelsTabControl, e.Location, ToolStripDropDownDirection.Default);
                    }
                }
            }
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of System.Windows.Forms.TabControl.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelsTabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.channelsTabControl.SelectedTab is IRCTabPage)
            {
                IRCTabPage tabPage = this.channelsTabControl.SelectedTab as IRCTabPage;
                tabPage.ImageIndex = tabPage.NormalImageIndex;

                if (tabPage.UserTreeView != null)
                {
                    tabPage.UserTreeView.BringToFront();
                }
                else
                {
                    this.userTreeView.BringToFront();
                }

                if (tabPage.TabType == IRCTabType.Server)
                {
                    this.Text = string.Format("{0} - yaircc", tabPage.Marshal.Connection.ToString());
                }
                else if (tabPage.TabType == IRCTabType.Channel)
                {
                    if (tabPage.Marshal != null)
                    {
                        this.Text = string.Format("{0} @ {1} - yaircc", tabPage.Text, tabPage.Marshal.Connection.ToString());
                    }
                }
                else if (tabPage.TabType == IRCTabType.Console)
                {
                    this.Text = "yaircc";
                }
                else if (tabPage.TabType == IRCTabType.PM)
                {
                    this.Text = string.Format("Conversation with {0} on {1} - yaircc", tabPage.Text, tabPage.Marshal.Connection.ToString());
                }

                this.disconnectToolStripMenuItem.Enabled = tabPage.TabType != IRCTabType.Console;
                this.disconnectToolStripButton.Enabled = tabPage.TabType != IRCTabType.Console;
                this.joinChannelToolStripButton.Enabled = tabPage.TabType != IRCTabType.Console;
                this.joinChannelToolStripMenuItem.Enabled = tabPage.TabType != IRCTabType.Console;
                this.leaveChannelToolStripButton.Enabled = tabPage.TabType == IRCTabType.Channel;
                this.leaveChannelToolStripMenuItem.Enabled = tabPage.TabType == IRCTabType.Channel;
            }

            this.inputTextBox.Focus();
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CheckForUpdatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.statusLabel.Text = Strings_General.CheckingForUpdates;
            if (!this.updateCheckBackgroundWorker.IsBusy)
            {
                this.updateCheckBackgroundWorker.RunWorkerAsync();
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ClearLogToolStripButton_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.ClearLog();
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CloseChannelTabMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.Marshal.GetChannelByTab(tab).Dispose();
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CollapseAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.UserTreeView.CollapseAll();
        }

        /// <summary>
        /// Gets a value that indicates whether or not the command issued was /commands.
        /// </summary>
        /// <param name="input">The command to check.</param>
        /// <returns>True if the command started with /commands.</returns>
        private bool CommandIsCommandsList(string input)
        {
            return input.Trim().ToLower().Equals("/commands");
        }

        /// <summary>
        /// Gets a value that indicates whether or not <paramref name="command"/> is a server command.
        /// </summary>
        /// <param name="command">The command to check.</param>
        /// <returns>True if <paramref name="command"/> starts with "/server", otherwise false.</returns>
        private bool CommandIsConnectionRequest(string command)
        {
            return command.ToLower().StartsWith("/server") || command.ToLower().StartsWith("/connect");
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ConnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ServerDialog dialog = new ServerDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.ProcessConnectionRequest(string.Format(@"/server {0}", dialog.Address));
                }
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CopyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.inputTextBox.Focused)
            {
                this.inputTextBox.Copy();
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.inputTextBox.Cut();
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void DeleteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            int index = this.inputTextBox.SelectionStart;
            if (index <= this.inputTextBox.Text.Length - 1)
            {
                if (this.inputTextBox.SelectionLength > 0)
                {
                    this.inputTextBox.Text = this.inputTextBox.Text.Remove(index, this.inputTextBox.SelectionLength);
                }
                else
                {
                    this.inputTextBox.Text = this.inputTextBox.Text.Remove(index, 1);
                }

                this.inputTextBox.Select(index, 0);
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void DisconnectTabMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.Marshal.Send(tab, new QuitMessage());
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void DisconnectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.Marshal.Send(tab, new QuitMessage());
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ExpandAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.UserTreeView.ExpandAll();
        }

        /// <summary>
        /// Handles the NickNameMentioned event of Yaircc.UI.IRCTabPage.
        /// </summary>
        private void FlashWindowInTaskBar()
        {
            FlashWindow.Flash(this);
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void GroupByModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.ToggleGrouping();
            this.groupByModeToolStripMenuItem.Checked = tab.GroupingByMode;
        }

        /// <summary>
        /// Handle the data currently in the text box.
        /// </summary>
        private void HandleInput()
        {
            if (this.CommandIsConnectionRequest(this.inputTextBox.Text))
            {
                this.ProcessConnectionRequest(this.inputTextBox.Text);
            }
            else if (this.CommandIsCommandsList(this.inputTextBox.Text))
            {
                this.ListCommands();
            }
            else
            {
                IRCTabPage currentTab = this.channelsTabControl.SelectedTab as IRCTabPage;
                if (currentTab.TabType != IRCTabType.Console)
                {
                    if (currentTab.Marshal.IsConnected)
                    {
                        string source = string.Empty;
                        if (currentTab.TabType == IRCTabType.Channel || currentTab.TabType == IRCTabType.PM)
                        {
                            source = currentTab.ConnectionSpecificName;
                        }

                        MessageParseResult parseResult = MessageFactory.CreateFromUserInput(this.inputTextBox.Text, source);
                        if (parseResult.Success)
                        {
                            Message message = parseResult.IRCMessage;
                            if ((message is PartMessage) && (currentTab.TabType == IRCTabType.PM))
                            {
                                if ((this.channelsTabControl.SelectedTab == currentTab) && (this.channelsTabControl.SelectedIndex > 0))
                                {
                                    this.channelsTabControl.SelectedIndex--;
                                }

                                currentTab.Marshal.GetChannelByTab(currentTab).Dispose();
                            }
                            else
                            {
                                currentTab.Marshal.Send(currentTab, message);
                            }
                        }
                        else
                        {
                            if (string.IsNullOrEmpty(parseResult.Message))
                            {
                                currentTab.AppendMessage(null, "[ERROR]", "Unrecognised command: " + this.inputTextBox.Text, MessageType.ErrorMessage);
                            }
                            else
                            {
                                currentTab.AppendMessage(null, "[ERROR]", parseResult.Message, MessageType.ErrorMessage);
                            }
                        }
                    }
                    else
                    {
                        currentTab.AppendMessage(null, "[ERROR]", Strings_Connection.NotConnectedToThisServer, MessageType.ErrorMessage);
                    }
                }
                else
                {
                    currentTab.AppendMessage(null, "[ERROR]", Strings_MessageParseResults.Server_InvalidMessageContext, MessageType.ErrorMessage);
                }
            }
        }

        /// <summary>
        /// Handles the KeyDown event of Intninety.TemporalTextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void InputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                IRCTabPage currentTab = this.channelsTabControl.SelectedTab as IRCTabPage;
                this.HandleInput();
                this.inputTextBox.Text = string.Empty;
                currentTab.WebBrowser.Document.Window.ScrollTo(0, currentTab.WebBrowser.Document.Window.Size.Height);
                this.inputTextBox.ClearStack();
                this.InputTextBox_TextChanged(sender, e);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the KeyPress event of Intninety.TemporalTextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void InputTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Return)
            {
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the TextChanged event of Intninety.TemporalTextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event parameters.</param>
        private void InputTextBox_TextChanged(object sender, EventArgs e)
        {
            this.undoToolStripMenuItem.Enabled = this.inputTextBox.CanUndo;
            this.redoToolStripMenuItem.Enabled = this.inputTextBox.CanRedo;
            this.undoToolStripButton.Enabled = this.undoToolStripMenuItem.Enabled;
            this.redoToolStripButton.Enabled = this.redoToolStripMenuItem.Enabled;
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void JoinChannelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (ChannelDialog dialog = new ChannelDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
                    tab.Marshal.Send(tab, new JoinMessage(dialog.ChannelName));
                }
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void LeaveChannelTabMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.Marshal.Send(tab, new PartMessage(tab.Text));
            tab.Marshal.GetChannelByTab(tab).Dispose();
        }

        /// <summary>
        /// Lists the supported commands in the current tab.
        /// </summary>
        private void ListCommands()
        {
            IRCTabPage currentTab = this.channelsTabControl.SelectedTab as IRCTabPage;
            currentTab.AppendMessage(null, "[INFO]", "away, back, ban, connect, dehop, deop, except, hop, invite, j, join, kick, knock, leave, links, list", MessageType.ServerMessage);
            currentTab.AppendMessage(null, "[INFO]", "map, me, mode, motd, msg, names, nick, notice, op, oper, part, ping, quit, stats, time, topic", MessageType.ServerMessage);
            currentTab.AppendMessage(null, "[INFO]", "unban, unexcept, userhost, voice, who, whois, whowas", MessageType.ServerMessage);
        }

        /// <summary>
        /// Handles the Load event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MainForm_Load(object sender, EventArgs e)
        {
            GlobalSettings settings = new GlobalSettings();
            if (settings.CheckForUpdateOnStart == GlobalSettings.Boolean.Yes)
            {
                this.autoCheckingForUpdate = true;
                this.CheckForUpdatesToolStripMenuItem_Click(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the Shown event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MainForm_Shown(object sender, EventArgs e)
        {
            this.channelsTabControl.TabPages.Add(new IRCTabPage(this, "consoleTabPage", "yaircc", IRCTabType.Console));
            this.inputTextBox.Focus();
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.SortByMode(true);
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void NicknameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.SortByMode(false);
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OpenPrivateChatMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            IRCChannel channel = tab.Marshal.Channels.Find(i => i.Name.Equals(this.openPrivateChatMenuItem.Tag.ToString(), StringComparison.OrdinalIgnoreCase));
            if (channel == null)
            {
                channel = tab.Marshal.CreateChannel(this.openPrivateChatMenuItem.Tag.ToString(), true);
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void PasteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.inputTextBox.Paste();
        }

        /// <summary>
        /// Processes a connection request and creates marshals and tabs were needed.
        /// </summary>
        /// <param name="input">The raw connection command.</param>
        private void ProcessConnectionRequest(string input)
        {
            Connection connection = new Connection();
            ParseResult result = connection.Parse(input);
            Action<IRCTabPage> connectAction = (t) =>
            {
                IRCTabPage serverTab;
                if (t == null)
                {
                    GlobalSettings settings = new GlobalSettings();
                    serverTab = new IRCTabPage(this, connection.ToString(), connection.Server, IRCTabType.Server);
                    serverTab.Connection = connection;
                    serverTab.Connection.UserName = settings.UserName;
                    serverTab.Connection.RealName = settings.RealName;
                    serverTab.Connection.Nickname = settings.NickName;
                    IRCMarshal marshal = new IRCMarshal(connection);
                    marshal.ChannelCreated += new IRCMarshal.ChannelCreatedHandler(this.ChannelCreated);
                    serverTab.Marshal = marshal;
                    marshal.ServerTab = serverTab;

                    channelsTabControl.TabPages.Add(serverTab);
                    this.ConfigureNewIRCMarshal(this, marshal);
                }
                else
                {
                    serverTab = t;
                }

                channelsTabControl.SelectedTab = serverTab;
                serverTab.Connection.Connect();
            };

            if (result.Success)
            {
                if (this.channelsTabControl.TabPages.ContainsKey(connection.ToString()))
                {
                    IRCTabPage tabPage = this.channelsTabControl.TabPages[connection.ToString()] as IRCTabPage;
                    if (tabPage.Marshal.IsConnected)
                    {
                        this.channelsTabControl.SelectedTab = tabPage;
                    }
                    else
                    {
                        connectAction.Invoke(tabPage);
                    }
                }
                else
                {
                    connectAction.Invoke(null);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RedoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.inputTextBox.Redo();
        }

        /// <summary>
        /// Refreshes the menu items that appear under the windows menu.
        /// </summary>
        /// <param name="controlToIgnore">A tab to ignore when generating the menu items.</param>
        private void RefreshWindowsMenuItems(Control controlToIgnore)
        {
            Action<IRCTabPage> addTabAsMenuItem = (tab) =>
            {
                if ((controlToIgnore == null) || (controlToIgnore != tab))
                {
                    ToolStripMenuItem item = new ToolStripMenuItem();
                    Image image;
                    if (tab.TabType == IRCTabType.Server)
                    {
                        image = Yaircc.Properties.Resources.application_lightning;
                    }
                    else if (tab.TabType == IRCTabType.Channel)
                    {
                        image = Yaircc.Properties.Resources.application_side_tree;
                    }
                    else
                    {
                        image = Yaircc.Properties.Resources.user_comment;
                    }

                    item.Name = string.Format("{0}_TabWindow", tab.Name);
                    item.Text = tab.Text;
                    item.Image = image;
                    item.Click += new EventHandler(this.WindowsToolStripMenuItem_Click);
                    this.windowsToolStripMenuItem.DropDownItems.Add(item);
                }
            };

            this.windowsToolStripMenuItem.DropDownItems
                                         .Cast<ToolStripItem>()
                                         .Where(i => i != this.yairccToolStripMenuItem)
                                         .ToList()
                                         .ForEach(i => this.windowsToolStripMenuItem.DropDownItems.Remove(i));

            var serverTabs = (from t in this.channelsTabControl.TabPages.Cast<IRCTabPage>()
                              where t.TabType == IRCTabType.Server
                              orderby t.Text descending
                              select t).ToArray();

            for (int i = 0; i < serverTabs.Length; i++)
            {
                IRCTabPage tab = serverTabs[i];
                this.windowsToolStripMenuItem.DropDownItems.Add(new ToolStripSeparator());
                addTabAsMenuItem.Invoke(tab);

                tab.Marshal.Channels.OrderBy(channel => channel.TabPage.Text).ToList()
                                    .ForEach(channel => addTabAsMenuItem.Invoke(channel.TabPage));
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.AddExtension = true;
                dialog.CheckPathExists = true;
                dialog.DefaultExt = "html";
                dialog.FileName = tab.Text;
                dialog.Filter = "HTML file (*.html)|*.html";

                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    File.WriteAllText(dialog.FileName, tab.WebBrowser.Document.Body.Parent.OuterHtml, Encoding.GetEncoding(tab.WebBrowser.Document.Encoding));
                }
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void SelectAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.inputTextBox.Focused)
            {
                this.inputTextBox.SelectAll();
            }
            else
            {
                IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
                if (tab.WebBrowser.Focused)
                {
                    tab.WebBrowser.Document.ExecCommand("SelectAll", false, null);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void SettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SettingsDialog dialog = new SettingsDialog())
            {
                dialog.ShowDialog();
            }
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UndoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.inputTextBox.Undo();
        }

        /// <summary>
        /// Handles the DoWork event of System.ComponentModel.BackgroundWorker.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UpdateCheckBackgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            ProgramUpdate update = new ProgramUpdate();
            if (update.Fetch())
            {
                Version executingVersion = Assembly.GetExecutingAssembly().GetName().Version;
                Version updateVersion = new Version(update.Version);
                if (executingVersion < updateVersion)
                {
                    e.Result = update;
                }
                else
                {
                    e.Result = null;
                }
            }
            else
            {
                e.Result = false;
            }
        }

        /// <summary>
        /// Handles the ProgressChanged event of System.ComponentModel.BackgroundWorker.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UpdateCheckBackgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
        }

        /// <summary>
        /// Handles the RunWorkerCompleted event of System.ComponentModel.BackgroundWorker.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UpdateCheckBackgroundWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (e.Result == null)
            {
                if (!this.autoCheckingForUpdate)
                {
                    MessageBox.Show(Strings_General.UpToDate, "yaircc", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            else if ((e.Result is bool) && (!(bool)e.Result))
            {
                if (!this.autoCheckingForUpdate)
                {
                    DialogResult result = MessageBox.Show(Strings_General.UpdateConnectivityFailure, "Uh oh...", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error);
                    if (result == DialogResult.Retry)
                    {
                        this.CheckForUpdatesToolStripMenuItem_Click(this, EventArgs.Empty);
                    }
                }
            }
            else if (e.Result is ProgramUpdate)
            {
                ProgramUpdate update = e.Result as ProgramUpdate;
                using (UpdateInformationDialog dialog = new UpdateInformationDialog(update))
                {
                    dialog.ShowDialog();
                }
            }

            this.statusLabel.Text = Strings_General.Ready;
            this.autoCheckingForUpdate = false;
        }

        /// <summary>
        /// Handles the MouseDown event of System.Windows.Forms.TreeView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UserTreeView_MouseDown(object sender, MouseEventArgs e)
        {
            TreeView treeView = sender as TreeView;
            TreeViewHitTestInfo info = treeView.HitTest(e.Location);
            treeView.SelectedNode = info.Node;

            if (e.Button == MouseButtons.Right)
            {
                IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
                if (tab.TabType == IRCTabType.Channel)
                {
                    if (info.Node != null && info.Node.Tag is IRCUser)
                    {
                        this.whoIsMenuItem.Text = string.Format("Who is {0}?", info.Node.Text);
                        this.whoIsMenuItem.Tag = info.Node.Text;
                        this.whoIsMenuItem.Visible = true;
                        this.openPrivateChatMenuItem.Tag = info.Node.Text;
                        this.openPrivateChatMenuItem.Visible = true;
                        this.userSeparator.Visible = true;
                    }
                    else
                    {
                        this.whoIsMenuItem.Visible = false;
                        this.openPrivateChatMenuItem.Visible = false;
                        this.userSeparator.Visible = false;
                    }

                    this.groupByModeToolStripMenuItem.Checked = tab.GroupingByMode;
                    this.orderByMenuItem.Enabled = !this.groupByModeToolStripMenuItem.Checked;
                    this.userTreeViewContextMenu.Show(treeView, e.Location);
                }
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WhoIsMenuItem_Click(object sender, EventArgs e)
        {
            IRCTabPage tab = this.channelsTabControl.SelectedTab as IRCTabPage;
            tab.Marshal.Send(tab, new WhoisMessage(this.whoIsMenuItem.Tag.ToString()));
        }

        /// <summary>
        /// Handles the Click event of ToolStripMenuItem.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WindowsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (sender == this.yairccToolStripMenuItem)
            {
                this.channelsTabControl.SelectedIndex = 0;
            }
            else if (sender is ToolStripMenuItem)
            {
                ToolStripMenuItem item = sender as ToolStripMenuItem;
                string tabName = item.Name.Substring(0, item.Name.LastIndexOf("_TabWindow"));
                this.channelsTabControl.SelectedTab = this.channelsTabControl.TabPages[tabName];
            }
        }

        /// <summary>
        /// Handles the Activated event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MainForm_Activated(object sender, EventArgs e)
        {
            if (this.channelsTabControl.SelectedIndex > 0)
            {
                IRCTabPage tabPage = this.channelsTabControl.SelectedTab as IRCTabPage;
                tabPage.ImageIndex = tabPage.NormalImageIndex;
            }
        }

        #endregion
    }
}