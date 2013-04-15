//-----------------------------------------------------------------------
// <copyright file="ServerDialog.cs" company="rastating">
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

namespace Yaircc
{
    using System;
    using System.Windows.Forms;

    /// <summary>
    /// Represents a dialog that accepts a server name.
    /// </summary>
    public partial class ServerDialog : Form
    {
        #region Fields

        /// <summary>
        /// The address entered by the user.
        /// </summary>
        private string address;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ServerDialog"/> class.
        /// </summary>
        public ServerDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the address entered by the user.
        /// </summary>
        public string Address
        {
            get { return this.address; }
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
        /// <param name="e">The event parameters.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event parameters.</param>
        private void ConnectButton_Click(object sender, EventArgs e)
        {
            this.address = this.addressTextBox.Text;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the TextChanged event of System.Windows.Forms.TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event parameters.</param>
        private void AddressTextBox_TextChanged(object sender, EventArgs e)
        {
            this.connectButton.Enabled = this.addressTextBox.Text.Length > 0;
        }

        /// <summary>
        /// Handles the KeyDown event of System.Windows.Forms.TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event parameters.</param>
        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if ((e.KeyCode == Keys.Enter) && (!string.IsNullOrEmpty(this.addressTextBox.Text)))
            {
                this.ConnectButton_Click(sender, e);
            }
        }

        #endregion
    }
}
