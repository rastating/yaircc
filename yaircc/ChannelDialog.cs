//-----------------------------------------------------------------------
// <copyright file="ChannelDialog.cs" company="intninety">
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
    using System.Windows.Forms;

    /// <summary>
    /// Represents a dialog that accepts a channel name.
    /// </summary>
    public partial class ChannelDialog : Form
    {
        #region Fields

        /// <summary>
        /// The name of the channel entered.
        /// </summary>
        private string channelName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ChannelDialog"/> class.
        /// </summary>
        public ChannelDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the name of the channel entered.
        /// </summary>
        public string ChannelName
        {
            get { return this.channelName; }
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
        /// Handles the TextChanged event of System.Windows.Forms.TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelTextBox_TextChanged(object sender, EventArgs e)
        {
            this.joinButton.Enabled = this.channelTextBox.Text.Length > 0;
        }

        /// <summary>
        /// Handles the KeyDown event of System.Windows.Forms.TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter && this.joinButton.Enabled)
            {
                this.JoinButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void JoinButton_Click(object sender, EventArgs e)
        {
            this.channelName = this.channelTextBox.Text;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CancelButton_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
        }

        #endregion
    }
}
