//-----------------------------------------------------------------------
// <copyright file="ThemeManager.cs" company="rastating">
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
    using System.IO;
    using System.Reflection;
    using System.Windows.Forms;
    using CefSharp;
    using CefSharp.WinForms;
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

        /// <summary>
        /// The WebView used to preview the selected theme.
        /// </summary>
        private WebView webView;

        /// <summary>
        /// A theme pending to be loaded into the WebView.
        /// </summary>
        private Theme pendingTheme;

        /// <summary>
        /// A value indicating whether or not the WebView is ready.
        /// </summary>
        private bool webViewIsReady;

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

        /// <summary>
        /// Gets the WebView used to preview the selected theme.
        /// </summary>
        private WebView WebView
        {
            get
            {
                if (this.webView == null)
                {
                    // Disable caching.
                    BrowserSettings settings = new BrowserSettings();
                    settings.ApplicationCacheDisabled = true;
                    settings.PageCacheDisabled = true;

                    // Initialise the WebView.
                    this.webView = new WebView(string.Empty, settings);
                    this.webView.Name = string.Format("{0}WebView", this.Name);
                    this.webView.Dock = DockStyle.Fill;
                    this.webView.PropertyChanged += this.WebView_PropertyChanged;
                    this.groupBox1.Controls.Add(this.webView);
                }

                return this.webView;
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
            if (this.WebView != null && this.webViewIsReady && !this.WebView.IsLoading)
            {
                string style = File.ReadAllText(theme.Path);

                style = style.Replace("\r", " ").Replace("\n", " ").Replace("'", @"\'");
                string script = @"setStyle('" + style + "')";

                this.WebView.ExecuteScript(script);
                this.WebView.ExecuteScript("$(window).scrollTop(document.body.scrollHeight);");
                this.pendingTheme = null;
            }
            else
            {
                this.pendingTheme = theme;
            }
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

        /// <summary>
        /// Handles the PropertyChanged event of CefSharp.WinForms.WebView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WebView_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            // Once the browser is initialised, load the HTML for the tab.
            if (!this.webViewIsReady)
            {
                if (e.PropertyName.Equals("IsBrowserInitialized", StringComparison.OrdinalIgnoreCase))
                {
                    this.webViewIsReady = this.WebView.IsBrowserInitialized;
                    if (this.webViewIsReady)
                    {
                        string resourceName = "Yaircc.UI.ThemeSample.htm";
                        using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
                        {
                            using (StreamReader reader = new StreamReader(stream))
                            {
                                this.WebView.LoadHtml(reader.ReadToEnd());
                            }
                        }
                    }
                }
            }

            // Once the HTML has finished loading, begin loading the initial content.
            if (e.PropertyName.Equals("IsLoading", StringComparison.OrdinalIgnoreCase))
            {
                if (!this.WebView.IsLoading && this.pendingTheme != null)
                {
                    this.LoadThemePreview(this.pendingTheme);
                }
            }
        }

        #endregion
    }
}