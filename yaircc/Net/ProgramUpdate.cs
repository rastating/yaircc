//-----------------------------------------------------------------------
// <copyright file="ProgramUpdate.cs" company="intninety">
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

namespace Yaircc.Net
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Net;
    using System.Xml.Linq;

    /// <summary>
    /// Represents an update available for the application.
    /// </summary>
    public class ProgramUpdate
    {
        #region Fields

        /// <summary>
        /// The Uri from which to fetch the latest update XML from.
        /// </summary>
        private Uri latestVersionUri;

        /// <summary>
        /// The version number of the update.
        /// </summary>
        private string version;

        /// <summary>
        /// A summary of what the update contains.
        /// </summary>
        private string summary;

        /// <summary>
        /// The date the update was released.
        /// </summary>
        private string date;

        /// <summary>
        /// The location the update can be downloaded from.
        /// </summary>
        private string uri;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.ProgramUpdate class.
        /// </summary>
        public ProgramUpdate()
        {
            this.latestVersionUri = new Uri(@"http://www.yaircc.com/latest.xml");
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the version number of the update.
        /// </summary>
        public string Version
        {
            get { return this.version; }
        }

        /// <summary>
        /// Gets a summary of what the update contains.
        /// </summary>
        public string Summary
        {
            get { return this.summary; }
        }

        /// <summary>
        /// Gets the date the update was released.
        /// </summary>
        public string Date
        {
            get { return this.date; }
        }

        /// <summary>
        /// Gets the Uri the update can be downloaded from.
        /// </summary>
        public Uri URI
        {
            get { return new Uri(this.uri); }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fetch the information on the latest version of yaircc.
        /// </summary>
        /// <returns>Returns true if the information was downloaded.</returns>
        public bool Fetch()
        {
            bool retval = true;
            using (WebClient client = new WebClient())
            {
                try
                {
                    string xml = client.DownloadString(this.latestVersionUri);
                    retval = this.Parse(xml);
                }
                catch
                {
                    retval = false;
                }
            }

            return retval;
        }

        /// <summary>
        /// Parse the XML into the ProgramUpdate.
        /// </summary>
        /// <param name="xml">The XML to parse.</param>
        /// <returns>Returns true if parsed.</returns>
        private bool Parse(string xml)
        {
            bool retval = true;
            try
            {
                XElement root = XElement.Parse(xml);
                IEnumerable<string> version =
                    from item in root.Descendants("version")
                    select (string)item.Value;
                IEnumerable<string> summary =
                    from item in root.Descendants("summary")
                    select (string)item.Value;
                IEnumerable<string> date =
                    from item in root.Descendants("date")
                    select (string)item.Value;
                IEnumerable<string> uri =
                    from item in root.Descendants("uri")
                    select (string)item.Value;

                this.version = version.First();
                this.summary = summary.First();
                this.date = date.First();
                this.uri = uri.First();
            }
            catch
            {
                retval = false;
            }

            return retval;
        }

        #endregion
    }
}
