//-----------------------------------------------------------------------
// <copyright file="CommandDialog.cs" company="rastating">
//     yaircc - the free, open-source IRC client for Windows.
//     Copyright (C) 2012-2014 Robert Carr
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
