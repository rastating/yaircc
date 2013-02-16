//-----------------------------------------------------------------------
// <copyright file="ChromiumMarshal.cs" company="intninety">
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

namespace Yaircc.UI
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using CefSharp.WinForms;
    using Yaircc.Settings;

    /// <summary>
    /// Represents the the Chromium marshal.
    /// </summary>
    public class ChromiumMarshal
    {
        /// <summary>
        /// The action to invoke after the DOM has been loaded.
        /// </summary>
        private Action initialisationAction;

        /// <summary>
        /// Initialises a new instance of the <see cref="ChromiumMarshal"/> class.
        /// </summary>
        /// <param name="initialisationAction">The action to invoke after the DOM has been loaded.</param>
        public ChromiumMarshal(Action initialisationAction)
        {
            this.initialisationAction = initialisationAction;
        }

        /// <summary>
        /// Begin exporting the chat log in <paramref name="webView"/> to <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">The path to export the chat log to.</param>
        /// <param name="webView">The WebView to export.</param>
        public static void BeginExport(string fileName, WebView webView)
        {
            string script = string.Format(@"exportHtml('{0}')", fileName.Replace(@"\", @"\\"));
            webView.ExecuteScript(script);
        }

        /// <summary>
        /// Invoke the initialisation action for the marshal subscribers.
        /// </summary>
        public void InitialiseSubscribers()
        {
            if (this.initialisationAction != null)
            {
                this.initialisationAction.Invoke();
            }
        }

        /// <summary>
        /// Export the <paramref name="html"/> to <paramref name="fileName"/>
        /// </summary>
        /// <param name="fileName">The path to export the HTML to.</param>
        /// <param name="html">The HTML to export.</param>
        public void ExportHtml(string fileName, string html)
        {
            File.WriteAllText(fileName, html, Encoding.UTF8);
        }

        /// <summary>
        /// Open a URL in the default browser.
        /// </summary>
        /// <param name="url">The URL to open.</param>
        public void OpenUrl(string url)
        {
            if (GlobalSettings.Instance.UsePrivateBrowsing == GlobalSettings.Boolean.No || url.StartsWith("mailto:", StringComparison.OrdinalIgnoreCase))
            {
                Process.Start(url);
            }
            else
            {
                WebBrowserForm browser = new WebBrowserForm(url);
                browser.Show();
            }
        }
    }
}