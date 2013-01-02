//-----------------------------------------------------------------------
// <copyright file="UpdateInformationDialog.cs" company="intninety">
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
    using System.ComponentModel;
    using System.Diagnostics;
    using System.IO;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Windows.Forms;
    using Yaircc.Localisation;
    using Yaircc.Net;

    /// <summary>
    /// Represents the update information dialog.
    /// </summary>
    public partial class UpdateInformationDialog : Form
    {
        #region Fields

        /// <summary>
        /// The program update that information is being displayed about.
        /// </summary>
        private ProgramUpdate update;

        /// <summary>
        /// The web client used to retrieve the setup file.
        /// </summary>
        private WebClient client;

        /// <summary>
        /// A value indicating whether or not the download is being cancelled.
        /// </summary>
        private bool cancelling;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="UpdateInformationDialog"/> class.
        /// </summary>
        /// <param name="update">The update to show information about.</param>
        public UpdateInformationDialog(ProgramUpdate update)
        {
            this.InitializeComponent();

            this.client = new WebClient();
            this.client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(this.Client_DownloadProgressChanged);
            this.client.DownloadFileCompleted += new AsyncCompletedEventHandler(this.Client_DownloadFileCompleted);

            this.update = update;
            this.summaryTextBox.Text = update.Summary.Replace("\n", Environment.NewLine);

            this.Localise();
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Validates the integrity of an update file.
        /// </summary>
        /// <param name="fileName">The full path of the local file to validate.</param>
        /// <param name="update">The upgrade the file is associated with.</param>
        /// <returns>true if the hashes match, false if not.</returns>
        private bool ValidateUpgradeFile(string fileName, ProgramUpdate update)
        {
            bool retval = false;

            if (File.Exists(fileName))
            {
                SHA1Managed sha1 = new SHA1Managed();
                StringBuilder sb = new StringBuilder(41);
                byte[] data = File.ReadAllBytes(fileName);
                byte[] hash = sha1.ComputeHash(data);
                
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                retval = sb.ToString().Equals(update.Hash, StringComparison.OrdinalIgnoreCase);
            }

            return retval;
        }

        /// <summary>
        /// Localise the form.
        /// </summary>
        private void Localise()
        {
            this.Text = Strings_General.UpdateAvailableTitle;
            this.newVersionLabel.Text = string.Format(Strings_General.NewVersionAvailable, this.update.Version);
            this.yesButton.Text = Strings_General.Yes;
            this.noButton.Text = Strings_General.No;
            this.questionLabel.Text = Strings_General.DoYouWantToDownloadIt;
            this.statusStripLabel.Text = Strings_General.Ready;
        }

        /// <summary>
        /// Handles the DownloadFileCompleted event of System.Net.WebClient.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_DownloadFileCompleted(object sender, AsyncCompletedEventArgs e)
        {
            string fileName = e.UserState.ToString();
            
            this.statusStripLabel.Text = Strings_UpdateInformationDialog.VerifyingUpdate;
            if (this.ValidateUpgradeFile(fileName, this.update))
            {
                if (!e.Cancelled)
                {
                    DialogResult result = MessageBox.Show(Strings_UpdateInformationDialog.DownloadComplete, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Yes)
                    {
                        Process.Start(fileName);
                        Application.Exit();
                    }
                    else
                    {
                        this.DialogResult = DialogResult.OK;
                    }
                }
                else
                {
                    if (File.Exists(e.UserState.ToString()))
                    {
                        File.Delete(e.UserState.ToString());
                    }

                    this.DialogResult = DialogResult.Cancel;
                }
            }
            else
            {
                MessageBox.Show(Strings_UpdateInformationDialog.UpdateCorrupt, Strings_General.Error, MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Handles the DownloadProgressChanged event of System.Net.WebClient.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Client_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (!this.cancelling)
            {
                this.statusStripLabel.Text = string.Format(Strings_UpdateInformationDialog.Downloading, e.ProgressPercentage);
                this.downloadProgressBar.Value = e.ProgressPercentage;
            }
        }

        /// <summary>
        /// Handles the Shown event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UpdateInformationDialog_Shown(object sender, EventArgs e)
        {
            this.yesButton.Focus();
        }
        
        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void NoButton_Click(object sender, EventArgs e)
        {
            if (this.client.IsBusy)
            {
                if (MessageBox.Show(Strings_General.AreYouSureYouWantToCancel, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.client.CancelAsync();
                    this.cancelling = true;
                    this.statusStripLabel.Text = Strings_General.Cancelling;
                }
            }
            else
            {
                this.client.CancelAsync();
                this.DialogResult = DialogResult.Cancel;
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void YesButton_Click(object sender, EventArgs e)
        {
            using (SaveFileDialog dialog = new SaveFileDialog())
            {
                dialog.FileName = Path.GetFileName(this.update.URI.LocalPath);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    this.yesButton.Enabled = false;
                    this.noButton.Text = Strings_General.Cancel;
                    this.downloadProgressBar.Visible = true;
                    this.statusStripLabel.Text = Strings_UpdateInformationDialog.StartingDownload;
                    this.client.DownloadFileAsync(this.update.URI, dialog.FileName, dialog.FileName);
                }
            }
        }

        /// <summary>
        /// Handles the FormClosing event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UpdateInformationDialog_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (this.client.IsBusy)
            {
                if (MessageBox.Show(Strings_General.AreYouSureYouWantToCancel, this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    this.client.CancelAsync();
                    e.Cancel = true;
                }
                else
                {
                    e.Cancel = true;
                }
            }
            else
            {
                this.client.Dispose();
            }
        }

        #endregion
    }
}
