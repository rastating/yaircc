//-----------------------------------------------------------------------
// <copyright file="Program.cs" company="intninety">
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
    using System.Collections.Specialized;
    using System.IO;
    using System.Net;
    using System.Windows.Forms;
    using System.Xml.Serialization;
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
                string url = "http://www.yaircc.com/error.php";
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
    }
}
