//-----------------------------------------------------------------------
// <copyright file="WebBrowserForm.cs" company="rastating">
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
    using System.ComponentModel;
    using System.Windows.Forms;
    using CefSharp.WinForms;
    using Yaircc.UI;

    /// <summary>
    /// Represents the private web browsing form.
    /// </summary>
    public partial class WebBrowserForm : Form
    {
        #region Fields

        /// <summary>
        /// The WebView that displays the web page.
        /// </summary>
        private WebView webView;

        /// <summary>
        /// The ToolStripTextBox used to specify a URL to navigate to.
        /// </summary>
        private ToolStripSpringTextBox addressTextBox;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="WebBrowserForm"/> class.
        /// </summary>
        /// <param name="uri">The URI of the resource to navigate to.</param>
        public WebBrowserForm(string uri)
        {
            this.InitializeComponent();

            this.webView = new WebView(uri, new CefSharp.BrowserSettings());
            this.webView.Dock = DockStyle.Fill;
            this.webView.PropertyChanged += this.WebView_PropertyChanged;
            this.webView.LifeSpanHandler = new WebViewLifeSpanHandler(this);
            this.toolStripContainer1.ContentPanel.Controls.Add(this.webView);
            this.addressTextBox = new ToolStripSpringTextBox();
            this.addressTextBox.KeyDown += this.AddressTextBox_KeyDown;
            this.addressTextBox.ShortcutsEnabled = true;
            this.toolStrip.Items.Add(this.addressTextBox);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Handles the KeyDown event of Yaircc.UI.ToolStripSpringTextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void AddressTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Return)
            {
                this.GoToolStripButton_Click(sender, e);
                this.webView.Focus();
            }
        }

        /// <summary>
        /// Handles the PropertyChanged event of CefSharp.WinForms.WebView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WebView_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (e.PropertyName.Equals("Title", StringComparison.OrdinalIgnoreCase))
            {
                this.InvokeAction(() => this.Text = this.webView.Title);
            }
            else if (e.PropertyName.Equals("IsLoading", StringComparison.OrdinalIgnoreCase))
            {
                this.InvokeAction(() => this.addressTextBox.Text = this.webView.Address);
            }
            else if (e.PropertyName.Equals("CanGoBack", StringComparison.OrdinalIgnoreCase))
            {
                this.InvokeAction(() => this.backToolStripButton.Enabled = this.webView.CanGoBack);
            }
            else if (e.PropertyName.Equals("CanGoForward", StringComparison.OrdinalIgnoreCase))
            {
                this.InvokeAction(() => this.forwardToolStripButton.Enabled = this.webView.CanGoForward);
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void BackToolStripButton_Click(object sender, EventArgs e)
        {
            this.webView.Back();
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ForwardToolStripButton_Click(object sender, EventArgs e)
        {
            this.webView.Forward();
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RefreshToolStripButton_Click(object sender, EventArgs e)
        {
            this.webView.Load(this.webView.Address);
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.ToolStripButton.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void GoToolStripButton_Click(object sender, EventArgs e)
        {
            this.webView.Load(this.addressTextBox.Text);
        }

        #endregion
    }
}
