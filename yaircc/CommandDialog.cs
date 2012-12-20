//-----------------------------------------------------------------------
// <copyright file="CommandDialog.cs" company="intninety">
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
    /// Represents a dialog that accepts an IRC command.
    /// </summary>
    public partial class CommandDialog : Form
    {
        #region Fields

        /// <summary>
        /// The command entered.
        /// </summary>
        private string command;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandDialog"/> class.
        /// </summary>
        /// <param name="command">The command that is to be edited.</param>
        public CommandDialog(string command)
        {
            this.InitializeComponent();
            this.command = command;
            this.commandTextBox.Text = command;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="CommandDialog"/> class.
        /// </summary>
        public CommandDialog()
        {
            this.InitializeComponent();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the command entered.
        /// </summary>
        public string Command
        {
            get { return this.command; }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            this.command = this.commandTextBox.Text;
            this.DialogResult = DialogResult.OK;
        }

        /// <summary>
        /// Handles the TextChanged event of System.Windows.Forms.TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CommandTextBox_TextChanged(object sender, EventArgs e)
        {
            this.okButton.Enabled = !string.IsNullOrEmpty(this.commandTextBox.Text);
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

        /// <summary>
        /// Handles the KeyDown event of System.Windows.Forms.TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CommandTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.OKButton_Click(sender, EventArgs.Empty);
            }
        }

        #endregion
    }
}
