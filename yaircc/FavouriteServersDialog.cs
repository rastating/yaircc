//-----------------------------------------------------------------------
// <copyright file="FavouriteServersDialog.cs" company="intninety">
//     Copyright 2012-2013 Robert Carr
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
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Yaircc.Localisation;
    using Yaircc.Settings;
    using Connection = Yaircc.Net.IRC.Connection;

    /// <summary>
    /// Represents a dialog that allows for the management of a favourite server list.
    /// </summary>
    public partial class FavouriteServersDialog : Form
    {
        #region Fields

        /// <summary>
        /// A value indicating whether or not changes have been made that are pending saving.
        /// </summary>
        private bool isDirty;

        /// <summary>
        /// A value indicating whether or not the controls are being populated programmatically.
        /// </summary>
        private bool isPopulating;

        /// <summary>
        /// The last validation error to occur.
        /// </summary>
        private string lastValidationError;

        /// <summary>
        /// The control to focus after a validation attempt.
        /// </summary>
        private Control controlToFocus;

        /// <summary>
        /// The server to open once the dialog closes.
        /// </summary>
        private Server serverToOpen;

        /// <summary>
        /// The connections currently open in yaircc.
        /// </summary>
        private List<Connection> openConnections;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FavouriteServersDialog"/> class.
        /// </summary>
        /// <param name="openConnections">The connections currently open in yaircc.</param>
        public FavouriteServersDialog(List<Connection> openConnections)
        {
            this.InitializeComponent();

            this.openConnections = openConnections;
            this.serverTreeView.Nodes[0].Nodes.Clear();
            this.isDirty = false;
            this.isPopulating = false;
            this.saveButton.Enabled = false;
            this.ToggleEnabledStateOfNodeControls(false);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the server to open when the dialog is closed.
        /// </summary>
        public Server ServerToOpen
        {
            get { return this.serverToOpen; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not changes have been made that are pending saving.
        /// </summary>
        private bool IsDirty
        {
            get
            {
                return this.isDirty;
            }

            set
            {
                this.isDirty = value;
                this.saveButton.Enabled = this.isDirty;
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Processes a command key.
        /// </summary>
        /// <param name="msg">A System.Windows.Forms.Message, passed by reference, that represents the Win32 message to process.</param>
        /// <param name="keyData">One of the System.Windows.Forms.Keys values that represents the key to process.</param>
        /// <returns>true if the keystroke was processed and consumed by the control; otherwise, false to allow further processing.</returns>
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.Escape)
            {
                this.DialogResult = DialogResult.Cancel;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void AddCommandButton_Click(object sender, EventArgs e)
        {
            using (CommandDialog dialog = new CommandDialog())
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.commandsListBox.Items.Add(dialog.Command);
                    this.IsDirty = true;
                }
            }
        }

        /// <summary>
        /// Adds a server to the tree view.
        /// </summary>
        /// <param name="server">The server to create a node from.</param>
        /// <param name="select">A value indicating whether or not to select the node after creation.</param>
        private void AddServer(Server server, bool select)
        {
            TreeNode node = new TreeNode();
            node.Text = server.Alias;

            if (this.openConnections.Find(i => i.Server.Equals(server.Address, StringComparison.OrdinalIgnoreCase) && i.Port.Equals(server.Port)) != null)
            {
                node.ImageKey = "bullet_green";
                node.SelectedImageKey = "bullet_green";
            }
            else
            {
                node.ImageKey = "bullet_white";
                node.SelectedImageKey = "bullet_white";
            }

            node.Tag = server;

            this.serverTreeView.Nodes[0].Nodes.Add(node);

            if (select)
            {
                this.serverTreeView.SelectedNode = node;
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void AddServerButton_Click(object sender, EventArgs e)
        {
            Server server = FavouriteServers.Instance.Create();
            this.AddServer(server, true);
            this.Save();
            this.serverTreeView.Focus();
            this.serverTreeView.ExpandAll();
        }

        /// <summary>
        /// Remove the data from the user editable controls.
        /// </summary>
        private void ClearControls()
        {
            this.aliasTextBox.Text = string.Empty;
            this.addressTextBox.Text = string.Empty;
            this.portTextBox.Text = string.Empty;
            this.automaticallyConnectCheckBox.Checked = false;
            this.nickNameTextBox.Text = string.Empty;
            this.userNameTextBox.Text = string.Empty;
            this.realNameTextBox.Text = string.Empty;
            this.modeTextBox.Text = string.Empty;
            this.commandsListBox.Items.Clear();
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of System.Windows.Forms.ListBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CommandsListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            bool enabled = this.commandsListBox.SelectedIndex >= 0;
            this.editCommandButton.Enabled = enabled;
            this.removeCommandButton.Enabled = enabled;
            this.moveUpButton.Enabled = enabled;
            this.moveDownButton.Enabled = enabled;
            this.moveUpButton.Enabled = enabled;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.serverToOpen = this.serverTreeView.SelectedNode.Tag as Server;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void EditCommandButton_Click(object sender, EventArgs e)
        {
            using (CommandDialog dialog = new CommandDialog(this.commandsListBox.SelectedItem.ToString()))
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.commandsListBox.Items[this.commandsListBox.SelectedIndex] = dialog.Command;
                    this.IsDirty = true;
                }
            }
        }

        /// <summary>
        /// Handles the FormClosing event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void FavouriteServersDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.IsDirty)
            {
                Server server = this.serverTreeView.SelectedNode.Tag as Server;
                string question = string.Format(Strings_FavouriteServers.SaveChangesQuestion, server.Alias);
                DialogResult result = MessageBox.Show(question, Strings_FavouriteServers.SaveChangesTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                switch (result)
                {
                    case DialogResult.Yes:
                        this.SaveButton_Click(this, EventArgs.Empty);
                        break;

                    case DialogResult.No:
                        this.IsDirty = false;
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Handles the Shown event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void FavouriteServersDialog_Shown(object sender, EventArgs e)
        {
            FavouriteServers.Instance.Servers.ForEach(i => this.AddServer(i, false));
            this.serverTreeView.ExpandAll();
        }

        /// <summary>
        /// Handle a change to a user editable control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void HandleChange(object sender, EventArgs e)
        {
            if (!this.isPopulating)
            {
                this.IsDirty = true;
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MoveDownButton_Click(object sender, EventArgs e)
        {
            if (this.commandsListBox.SelectedIndex < this.commandsListBox.Items.Count - 1)
            {
                int index = this.commandsListBox.SelectedIndex;
                string command = this.commandsListBox.Items[index].ToString();
                this.commandsListBox.Items.RemoveAt(index);
                this.commandsListBox.Items.Insert(index + 1, command);
                this.commandsListBox.SelectedIndex = index + 1;
                this.commandsListBox.Focus();

                this.IsDirty = true;
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void MoveUpButton_Click(object sender, EventArgs e)
        {
            if (this.commandsListBox.SelectedIndex > 0)
            {
                int index = this.commandsListBox.SelectedIndex;
                string command = this.commandsListBox.Items[index].ToString();
                this.commandsListBox.Items.RemoveAt(index);
                this.commandsListBox.Items.Insert(index - 1, command);
                this.commandsListBox.SelectedIndex = index - 1;
                this.commandsListBox.Focus();

                this.IsDirty = true;
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RemoveCommandButton_Click(object sender, EventArgs e)
        {
            this.commandsListBox.Items.RemoveAt(this.commandsListBox.SelectedIndex);
            this.IsDirty = true;
            this.commandsListBox.Focus();
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RemoveServerButton_Click(object sender, EventArgs e)
        {
            Server server = this.serverTreeView.SelectedNode.Tag as Server;
            string question = string.Format(Strings_FavouriteServers.ConfirmServerRemoval, server.Alias);
            DialogResult result = MessageBox.Show(question, Strings_General.AreYouSureYouWantToDoThis, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2);

            if (result == DialogResult.Yes)
            {
                FavouriteServers.Instance.Servers.Remove(server);
                FavouriteServers.Instance.Save();
                this.IsDirty = false;
                this.serverTreeView.Nodes.Remove(this.serverTreeView.SelectedNode);
            }
        }

        /// <summary>
        /// Save the changes made to the current server.
        /// </summary>
        /// <returns>true if saved.</returns>
        private bool Save()
        {
            bool retval;
            Server server = this.serverTreeView.SelectedNode.Tag as Server;

            if (this.ValidateData(server))
            {
                server.Alias = this.aliasTextBox.Text;
                server.Address = this.addressTextBox.Text;
                server.AutomaticallyConnect = this.automaticallyConnectCheckBox.Checked;
                server.Commands = new List<string>();
                server.Mode = this.modeTextBox.Text;
                server.NickName = this.nickNameTextBox.Text;
                server.Port = int.Parse(this.portTextBox.Text);
                server.RealName = this.realNameTextBox.Text;
                server.UserName = this.userNameTextBox.Text;

                server.Commands.Clear();
                server.Commands = this.commandsListBox.Items.OfType<string>().ToList();

                FavouriteServers.Instance.Save();

                this.IsDirty = false;
                this.serverTreeView.SelectedNode.Text = server.Alias;
                retval = true;
            }
            else
            {
                MessageBox.Show(this.lastValidationError, Strings_General.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                if (this.controlToFocus != null)
                {
                    this.controlToFocus.Focus();
                    if (this.controlToFocus is TextBox)
                    {
                        (this.controlToFocus as TextBox).SelectAll();
                    }
                }

                retval = false;
            }

            return retval;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void SaveButton_Click(object sender, EventArgs e)
        {
            this.Save();
        }

        /// <summary>
        /// Handles the AfterSelect event of System.Windows.Forms.TreeView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ServerTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            this.isPopulating = true;

            if (e.Node.Tag != null)
            {
                this.ToggleEnabledStateOfNodeControls(true);

                Server server = e.Node.Tag as Server;
                this.aliasTextBox.Text = server.Alias;
                this.addressTextBox.Text = server.Address;
                this.portTextBox.Text = server.Port.ToString();
                this.automaticallyConnectCheckBox.Checked = server.AutomaticallyConnect;
                this.nickNameTextBox.Text = server.NickName;
                this.userNameTextBox.Text = server.UserName;
                this.realNameTextBox.Text = server.RealName;
                this.modeTextBox.Text = server.Mode;
                this.commandsListBox.Items.Clear();
                server.Commands.ForEach(i => this.commandsListBox.Items.Add(i));

                this.editCommandButton.Enabled = false;
                this.removeCommandButton.Enabled = false;
            }
            else
            {
                this.ToggleEnabledStateOfNodeControls(false);
                this.ClearControls();
            }

            this.isPopulating = false;
        }

        /// <summary>
        /// Handles the BeforeSelect event of System.Windows.Forms.TreeView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ServerTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            if (this.IsDirty)
            {
                Server server = this.serverTreeView.SelectedNode.Tag as Server;
                string question = string.Format(Strings_FavouriteServers.SaveChangesQuestion, server.Alias);
                DialogResult result = MessageBox.Show(question, Strings_FavouriteServers.SaveChangesTitle, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);

                switch (result)
                {
                    case DialogResult.Yes:
                        if (!this.Save())
                        {
                            e.Cancel = true;
                        }

                        break;

                    case DialogResult.No:
                        this.IsDirty = false;
                        break;

                    case DialogResult.Cancel:
                        e.Cancel = true;
                        break;
                }
            }
        }

        /// <summary>
        /// Toggle the enabled state of the controls that require a valid server node.
        /// </summary>
        /// <param name="enabled">The enabled state to use.</param>
        private void ToggleEnabledStateOfNodeControls(bool enabled)
        {
            this.connectButton.Enabled = enabled;
            this.removeServerButton.Enabled = enabled;
            this.connectButton.Enabled = enabled;
            this.tabControl1.Enabled = enabled;
        }

        /// <summary>
        /// Verifies that the address entered is valid.
        /// </summary>
        /// <param name="address">The address to validate.</param>
        /// <param name="server">The server the address is associated with.</param>
        /// <returns>true if valid.</returns>
        private bool ValidateAddress(string address, Server server)
        {
            bool retval = true;

            if (FavouriteServers.Instance.Servers.Find(i => i.Address.Equals(address, StringComparison.OrdinalIgnoreCase) && i != server) != null)
            {
                retval = false;
                this.lastValidationError = Strings_Validation.AddressInUse;
            }

            return retval;
        }

        /// <summary>
        /// Verifies that the alias entered is valid.
        /// </summary>
        /// <param name="alias">The alias to validate.</param>
        /// <param name="server">The server the alias represents.</param>
        /// <returns>true if valid.</returns>
        private bool ValidateAlias(string alias, Server server)
        {
            return string.IsNullOrEmpty(alias.Trim()) || FavouriteServers.Instance.Servers.Find(i => i.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase) && i != server) == null;
        }

        /// <summary>
        /// Verifies all the data entered into the input controls is valid.
        /// </summary>
        /// <param name="server">The server the data is being entered for.</param>
        /// <returns>true if valid.</returns>
        private bool ValidateData(Server server)
        {
            if (!this.ValidateAlias(this.aliasTextBox.Text, server))
            {
                this.lastValidationError = Strings_Validation.DuplicateAlias;
                this.controlToFocus = this.aliasTextBox;
                return false;
            }

            if (!this.ValidateAddress(this.addressTextBox.Text, server))
            {
                this.controlToFocus = this.addressTextBox;
                return false;
            }

            if (!this.portTextBox.Text.IsNumeric())
            {
                this.lastValidationError = Strings_Validation.NonNumericPort;
                this.controlToFocus = this.portTextBox;
                return false;
            }

            if (!this.ValidateNickName(this.nickNameTextBox.Text))
            {
                this.lastValidationError = Strings_Validation.NickName;
                this.controlToFocus = this.nickNameTextBox;
                return false;
            }

            return true;
        }

        /// <summary>
        /// Verifies that the nick name entered is valid.
        /// </summary>
        /// <param name="nickName">The nick name to validate.</param>
        /// <returns>true if valid.</returns>
        private bool ValidateNickName(string nickName)
        {
            string pattern = @"^[\[\]\\`_\^\{\|\}a-zA-Z]{1}[\[\]\\`_\^\{\|\}a-zA-Z0-9\-]+$";
            return Regex.IsMatch(nickName, pattern);
        }

        #endregion
    }
}