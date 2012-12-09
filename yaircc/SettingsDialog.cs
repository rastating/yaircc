//-----------------------------------------------------------------------
// <copyright file="SettingsDialog.cs" company="intninety">
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
            this.settings = new GlobalSettings();
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
