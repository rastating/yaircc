//-----------------------------------------------------------------------
// <copyright file="ListMessage.cs" company="intninety">
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
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a LIST command.
    /// </summary>
    public class ListMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ListMessage"/> class.
        /// </summary>
        public ListMessage()
            : base("LIST")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListMessage"/> class.
        /// </summary>
        /// <param name="channel">The comma-separated list of channels to return the topics of.</param>
        public ListMessage(string channel)
            : base("LIST", new string[] { channel })
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
            ParseResult retval;
            string pattern = @"/list(\s+(?<channel>[#&!\+][^\x07\x2C\s]{1,199}))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["channel"].Success)
                {
                    this.Parameters = new string[] { match.Groups["channel"].Value };
                }

                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, string.Empty);
            }

            return retval;
        }

        #endregion
    }
}
