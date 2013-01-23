//-----------------------------------------------------------------------
// <copyright file="StatsMessage.cs" company="intninety">
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

namespace Yaircc.Net.IRC
{
    /// <summary>
    /// Represents a STATS command.
    /// </summary>
    public class StatsMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="StatsMessage"/> class.
        /// </summary>
        public StatsMessage()
            : base("STATS")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="StatsMessage"/> class.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        public StatsMessage(string query)
            : base("STATS", new string[] { query })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="StatsMessage"/> class.
        /// </summary>
        /// <param name="query">The query to execute.</param>
        /// <param name="server">The server to execute the query against.</param>
        public StatsMessage(string query, string server)
            : base("STATS", new string[] { query, server })
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Attempts to parse the input specified by the user into the Message.
        /// </summary>
        /// <param name="input">The input from the user.</param>
        /// <returns>True on success, false on failure.</returns>
        public override ParseResult TryParse(string input)
        {
            return new ParseResult(true, string.Empty);
        }

        #endregion
    }
}
