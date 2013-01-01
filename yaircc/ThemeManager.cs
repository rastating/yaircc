//-----------------------------------------------------------------------
// <copyright file="ThemeManager.cs" company="intninety">
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
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using Yaircc.Settings;
    using Yaircc.UI;

    /// <summary>
    /// Represents the theme manager dialog.
    /// </summary>
    public partial class ThemeManager : Form
    {
        #region Fields

        /// <summary>
        /// A value indicating whether or not changes have been made to the settings in the dialog.
        /// </summary>
        private bool isDirty;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ThemeManager" /> class.
        /// </summary>
        public ThemeManager()
        {
            this.InitializeComponent();
            this.themesTreeView.Nodes[0].Nodes.Clear();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not changes have been made to the settings in the dialog.
        /// </summary>
        public bool IsDirty
        {
            get { return this.isDirty; }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Adds a node to represent the theme to the tree view.
        /// </summary>
        /// <param name="theme">The theme to create a node from.</param>
        private void AddTheme(Theme theme)
        {
            TreeNode node = new TreeNode();
            TreeNode parent = theme.IsOfficial ? this.themesTreeView.Nodes[0] : this.themesTreeView.Nodes[1];
            string imageKey = "layout";

            if (theme.Path.Equals(GlobalSettings.Instance.ThemeFileName))
            {
                imageKey = "tick";
            }

            node.Text = theme.Name;
            node.SelectedImageKey = imageKey;
            node.ImageKey = imageKey;
            node.Tag = theme;

            parent.Nodes.Add(node);
        }

        /// <summary>
        /// Handles the Load event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ThemeManager_Load(object sender, EventArgs e)
        {
            this.LoadThemes();
        }

        /// <summary>
        /// Populates the tree view with the themes installed on the system.
        /// </summary>
        private void LoadThemes()
        {
            this.themesTreeView.Nodes[0].Nodes.Clear();
            this.themesTreeView.Nodes[1].Nodes.Clear();

            Themes.Instance.InstalledThemes.ForEach(t => this.AddTheme(t));
            this.themesTreeView.ExpandAll();

            if (this.themesTreeView.Nodes[0].Nodes.Count > 0)
            {
                this.themesTreeView.SelectedNode = this.themesTreeView.Nodes[0].Nodes[0];
            }
        }

        /// <summary>
        /// Loads a preview of the specified theme.
        /// </summary>
        /// <param name="theme">The theme to load a preview of.</param>
        private void LoadThemePreview(Theme theme)
        {
            this.groupBox1.Controls.Remove(this.themePreviewBrowser);
            this.themePreviewBrowser = new WebBrowser();
            this.themePreviewBrowser.WebBrowserShortcutsEnabled = false;
            this.themePreviewBrowser.IsWebBrowserContextMenuEnabled = false;
            this.themePreviewBrowser.Dock = DockStyle.Fill;
            this.themePreviewBrowser.Visible = true;

            // Load a blank document
            this.themePreviewBrowser.Navigate("about:blank");
            if (this.themePreviewBrowser.Document != null)
            {
                this.themePreviewBrowser.Document.Write(string.Empty);
            }

            // Load the appropriate resources into the web browser
            string resourceName = "Yaircc.UI.ThemeSample.htm";
            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    this.themePreviewBrowser.Document.Write(reader.ReadToEnd());
                }
            }

            string style = File.ReadAllText(theme.Path);
            this.themePreviewBrowser.Document.InvokeScript("setStyle", new object[] { style });
            this.groupBox1.Controls.Add(this.themePreviewBrowser);
            this.themePreviewBrowser.Document.Window.ScrollTo(0, 9999);
        }

        /// <summary>
        /// Handles the AfterSelect event of System.Windows.Forms.TreeView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ThemesTreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag is Theme)
            {
                Theme theme = e.Node.Tag as Theme;
                this.themeNameLabel.Text = theme.Name;
                this.themeDescriptionLabel.Text = theme.Description;
                this.createdByLinkLabel.Text = string.Format("Created by {0}", theme.Author);
                this.createdByLinkLabel.Links[0].LinkData = theme.Website;

                this.removeButton.Enabled = !theme.IsOfficial;
                this.defaultButton.Enabled = !theme.Path.Equals(GlobalSettings.Instance.ThemeFileName);
                this.LoadThemePreview(theme);               
            }
        }

        /// <summary>
        /// Handles the LinkClicked event of System.Windows.Forms.LinkLabel.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CreatedByLinkLabel_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Uri uri = new Uri(e.Link.LinkData.ToString());
            System.Diagnostics.Process.Start(uri.ToString());
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
        /// Handles the BeforeSelect event of System.Windows.Forms.TreeView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ThemesTreeView_BeforeSelect(object sender, TreeViewCancelEventArgs e)
        {
            e.Cancel = e.Node.Level == 0;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void DefaultButton_Click(object sender, EventArgs e)
        {
            if (this.themesTreeView.SelectedNode.Tag is Theme)
            {
                GlobalSettings.Instance.ThemeFileName = (this.themesTreeView.SelectedNode.Tag as Theme).Path;
                GlobalSettings.Instance.Save();
                string layoutKey = "layout";
                string tickKey = "tick";

                this.isDirty = true;
                this.defaultButton.Enabled = false;

                for (int i = 0; i < this.themesTreeView.Nodes[0].Nodes.Count; i++)
                {
                    TreeNode node = this.themesTreeView.Nodes[0].Nodes[i];
                    if (node == this.themesTreeView.SelectedNode)
                    {
                        node.SelectedImageKey = tickKey;
                        node.ImageKey = tickKey;
                    }
                    else
                    {
                        node.SelectedImageKey = layoutKey;
                        node.ImageKey = layoutKey;
                    }
                }

                for (int i = 0; i < this.themesTreeView.Nodes[1].Nodes.Count; i++)
                {
                    TreeNode node = this.themesTreeView.Nodes[1].Nodes[i];
                    if (node == this.themesTreeView.SelectedNode)
                    {
                        node.SelectedImageKey = tickKey;
                        node.ImageKey = tickKey;
                    }
                    else
                    {
                        node.SelectedImageKey = layoutKey;
                        node.ImageKey = layoutKey;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (this.themesTreeView.SelectedNode.Tag is Theme)
            {
                Theme theme = this.themesTreeView.SelectedNode.Tag as Theme;
                try
                {
                    Themes.Instance.UninstallTheme(theme);
                }
                catch (Exception ex)
                {
                    DialogResult result = MessageBox.Show("The theme could not be removed: " + ex.Message, "Error", MessageBoxButtons.RetryCancel, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                    if (result == DialogResult.Retry)
                    {
                        this.RemoveButton_Click(sender, e);
                    }

                    return;
                }
                finally
                {
                    this.themesTreeView.Nodes.Remove(this.themesTreeView.SelectedNode);
                    this.themesTreeView.SelectedNode = this.themesTreeView.Nodes[0].Nodes[0];
                }
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void AddButton_Click(object sender, EventArgs e)
        {
            if (this.openFileDialog.ShowDialog() == DialogResult.OK)
            {
                Theme theme = Themes.Instance.InstallTheme(this.openFileDialog.FileName);
                if (theme != null)
                {
                    MessageBox.Show(string.Format("{0} has been installed.", theme.Name), "Installation Finished", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    this.LoadThemes();
                }
                else
                {
                    MessageBox.Show("The selected file is not a valid yaircc theme.", "Installation Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        #endregion
    }
}