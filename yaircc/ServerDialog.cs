//-----------------------------------------------------------------------
// <copyright file="ServerDialog.cs" company="intninety">
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
