//-----------------------------------------------------------------------
// <copyright file="WebViewLifeSpanHandler.cs" company="rastating">
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
    using System.Drawing;
    using CefSharp;

    /// <summary>
    /// Represents the life span handler used in private browsing mode.
    /// </summary>
    public class WebViewLifeSpanHandler : ILifeSpanHandler
    {
        /// <summary>
        /// The form that owns the associated WebView.
        /// </summary>
        private WebBrowserForm owner;

        /// <summary>
        /// Initialises a new instance of the <see cref="WebViewLifeSpanHandler"/> class.
        /// </summary>
        /// <param name="owner">The form that owns the associated WebView.</param>
        public WebViewLifeSpanHandler(WebBrowserForm owner)
        {
            this.owner = owner;
        }

        /// <summary>
        /// Called before a new popup window is created.
        /// </summary>
        /// <param name="browser">The parent browser window.</param>
        /// <param name="url">The URL of the popup.</param>
        /// <param name="x">The x-position of the popup.</param>
        /// <param name="y">The y-position of the popup.</param>
        /// <param name="width">The width of the popup.</param>
        /// <param name="height">The height of the popup.</param>
        /// <returns>Returns true to cancel creation of the popup window.</returns>
        public bool OnBeforePopup(IWebBrowser browser, string url, ref int x, ref int y, ref int width, ref int height)
        {
            WebBrowserForm popup = new WebBrowserForm(url);

            if (x > 0 && y > 0)
            {
                popup.Location = new Point(x, y);
            }

            if (width > 0 && height > 0)
            {
                popup.Size = new Size(width, height);
            }

            popup.Show();
            return true;
        }

        /// <summary>
        /// Called just before a window is closed. If this is a modal window and a custom modal loop implementation was provided in RunModal() this callback should be used to exit the custom modal loop.
        /// </summary>
        /// <param name="browser">The browser being closed.</param>
        public void OnBeforeClose(IWebBrowser browser)
        {
            if (this.owner != null)
            {
                this.owner.Close();
            }
        }
    }
}
