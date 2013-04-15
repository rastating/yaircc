//-----------------------------------------------------------------------
// <copyright file="SettingsDialog.cs" company="rastating">
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
    using Yaircc.Settings;

    /// <summary>
    /// Represents the settings dialog.
    /// </summary>
    public partial class SettingsDialog : Form
    {
        #region Fields

        /// <summary>
        /// The global user settings for the application.
        /// </summary>
        private GlobalSettings settings;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SettingsDialog"/> class.
        /// </summary>
        public SettingsDialog()
        {
            this.InitializeComponent();
            this.settings = GlobalSettings.Instance;
            this.propertyGrid.SelectedObject = this.settings;
            this.SelectFirstGridItem();
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
        /// Highlights the first item in the property grid.
        /// </summary>
        private void SelectFirstGridItem()
        {
            GridItem item = this.propertyGrid.SelectedGridItem;
            GridItem parent = item.Parent.Parent;
            parent.GridItems[0].GridItems[0].Select();
            return;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void OKButton_Click(object sender, EventArgs e)
        {
            this.settings.Save();
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
