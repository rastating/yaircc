//-----------------------------------------------------------------------
// <copyright file="ProgramUpdate.cs" company="rastating">
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
        /// The Uri from which to fetch the release notes XML from.
        /// </summary>
        private Uri releaseNotesUri;

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

        /// <summary>
        /// The hash used to check the file integrity.
        /// </summary>
        private string hash;

        /// <summary>
        /// The release notes applicable between this version and the user's version.
        /// </summary>
        private List<ReleaseNotes> releaseNotes;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.ProgramUpdate class.
        /// </summary>
        public ProgramUpdate()
        {
            this.latestVersionUri = new Uri(@"http://www.yaircc.com/latest.xml");
            this.releaseNotesUri = new Uri(@"http://www.yaircc.com/release-notes.xml");
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

        /// <summary>
        /// Gets the hash used to check the file integrity.
        /// </summary>
        public string Hash
        {
            get { return this.hash; }
        }

        /// <summary>
        /// Gets the release notes applicable between this version and the user's version.
        /// </summary>
        public List<ReleaseNotes> ReleaseNotes
        {
            get { return this.releaseNotes; }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Fetches and parses the release notes from the user's version up to this version.
        /// </summary>
        /// <param name="version">The version of the current executable.</param>
        /// <returns>true if successful, otherwise false.</returns>
        public bool FetchReleaseNotes(Version version)
        {
            bool retval = true;

            try
            {
                this.releaseNotes = Net.ReleaseNotes.GetReleaseNotes(version);
            }
            catch
            {
                retval = false;
            }

            return retval;
        }

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
                IEnumerable<string> hash =
                    from item in root.Descendants("hash")
                    select (string)item.Value;
                
                this.version = version.First();
                this.summary = summary.First();
                this.date = date.First();
                this.uri = uri.First();
                this.hash = hash.First();
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