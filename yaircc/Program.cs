//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="rastating">
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
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Threading;
    using System.Windows.Forms;
    using System.Xml.Serialization;
    using CefSharp;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents the application.
    /// </summary>
    public static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        public static void Main()
        {
            if (!System.Diagnostics.Debugger.IsAttached)
            {
                Application.SetUnhandledExceptionMode(UnhandledExceptionMode.ThrowException);
                AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(CurrentDomain_UnhandledException);
            }

            CefSharp.Settings settings = new CefSharp.Settings();
            settings.CachePath = string.Empty;
            CEF.Initialize(settings);
            Application.ApplicationExit += Application_ApplicationExit;
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }

        /// <summary>
        /// Post an error log to the web server.
        /// </summary>
        /// <param name="log">The log to post.</param>
        public static void PostErrorLog(string log)
        {
            using (WebClient client = new WebClient())
            {
                string url = "https://www.yaircc.com/error.php";
                NameValueCollection data = new NameValueCollection();
                data["payload"] = log;
                client.UploadValues(url, "POST", data);
            }
        }

        /// <summary>
        /// Handles unhandled exceptions within the application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public static void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            try
            {
                Exception ex = e.ExceptionObject as Exception;
                DialogResult result = MessageBox.Show(Strings_General.BugReportRequest, "Doh!", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
                if (result == DialogResult.Yes)
                {
                    ErrorLog log = new ErrorLog(ex);
                    XmlSerializer serializer = new XmlSerializer(typeof(ErrorLog));
                    using (TextWriter writer = new StringWriter())
                    {
                        serializer.Serialize(writer, log);
                        PostErrorLog(writer.ToString());
                    }
                }
            }
            catch (Exception)
            {
            }
            finally
            {
                Environment.Exit(1);
            }
        }

        /// <summary>
        /// Handles the ApplicationExit of System.Windows.Forms.Application.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private static void Application_ApplicationExit(object sender, EventArgs e)
        {
            CEF.Shutdown();
        }
    }
}
