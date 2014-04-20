//-----------------------------------------------------------------------
// <copyright file="ReleaseNotes.cs" company="rastating">
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
    /// Represents a set of release notes for a given yaircc release.
    /// </summary>
    public class ReleaseNotes
    {
        #region Fields

        /// <summary>
        /// The URL from which the release notes can be downloaded.
        /// </summary>
        private static Uri releaseNotesUri = new Uri(@"http://www.yaircc.com/release-notes.xml");

        /// <summary>
        /// The date of the release.
        /// </summary>
        private string date;

        /// <summary>
        /// The release notes.
        /// </summary>
        private string summary;

        /// <summary>
        /// The version number of the release.
        /// </summary>
        private Version version;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ReleaseNotes" /> class.
        /// </summary>
        /// <param name="version">The version number of the release.</param>
        /// <param name="summary">The release notes.</param>
        /// <param name="date">The date of the release.</param>
        public ReleaseNotes(string version, string summary, string date)
        {
            this.date = date;
            this.summary = summary;
            this.version = new Version(version);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the date of the release.
        /// </summary>
        public string Date
        {
            get { return this.date; }
        }

        /// <summary>
        /// Gets the release notes.
        /// </summary>
        public string Summary
        {
            get { return this.summary; }
        }

        /// <summary>
        /// Gets the version of the release.
        /// </summary>
        public Version Version
        {
            get { return this.version; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Fetches, parses and returns a list of ReleaseNotes.
        /// </summary>
        /// <param name="version">The version preceding the release notes to fetch.</param>
        /// <returns>A list of ReleaseNotes</returns>
        public static List<ReleaseNotes> GetReleaseNotes(Version version)
        {
            List<ReleaseNotes> retval = new List<ReleaseNotes>();

            using (WebClient client = new WebClient())
            {
                string xml = client.DownloadString(ReleaseNotes.releaseNotesUri);
                XElement root = XElement.Parse(xml);
                IEnumerable<ReleaseNotes> updates = from update in root.Descendants("update")
                                                    select new ReleaseNotes(
                                                        update.Element("version").Value,
                                                        update.Element("summary").Value,
                                                        update.Element("date").Value);
                retval.AddRange(updates.Where(i => i.Version > version).OrderByDescending(i => i.Version));
            }

            return retval;
        }

        #endregion
    }
}
