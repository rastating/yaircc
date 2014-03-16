//-----------------------------------------------------------------------
// <copyright file="ChromiumMarshal.cs" company="rastating">
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

namespace Yaircc.UI
{
    using System;
    using System.Diagnostics;
    using System.IO;
    using System.Text;
    using System.Text.RegularExpressions;
    using CefSharp.WinForms;
    using Yaircc.Net.IRC;
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
        /// The IRCTabPage that the marshal is owned by.
        /// </summary>
        private IRCTabPage owner;

        /// <summary>
        /// Initialises a new instance of the <see cref="ChromiumMarshal"/> class.
        /// </summary>
        /// <param name="initialisationAction">The action to invoke after the DOM has been loaded.</param>
        /// <param name="owner">The owning form.</param>
        public ChromiumMarshal(Action initialisationAction, IRCTabPage owner)
        {
            this.initialisationAction = initialisationAction;
            this.owner = owner;
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
        /// Joins the specified channel.
        /// </summary>
        /// <param name="channelName">The channel to join.</param>
        public void JoinChannel(string channelName)
        {
            if (this.owner.Marshal != null)
            {
                JoinMessage message = new JoinMessage(channelName);
                this.owner.Marshal.Send(this.owner, message);
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
                // If the URL is in the format of a Spotify URI then we need to shell this instead of opening
                // in the private browsing mode form.
                if (Regex.IsMatch(url, @"spotify:([^\s]+:)+[a-zA-Z0-9]+"))
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

        /// <summary>
        /// Inserts the specified nick name into the input textbox.
        /// </summary>
        /// <param name="nickName">The nick name to insert.</param>
        public void InsertNickNameIntoMessage(string nickName)
        {
            this.owner.InvokeAction(() =>
            {
                this.owner.OwningForm.InsertNickNameIntoMessage(nickName);
            });
        }
    }
}