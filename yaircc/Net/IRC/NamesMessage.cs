//-----------------------------------------------------------------------
// <copyright file="NamesMessage.cs" company="rastating">
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

namespace Yaircc.Net.IRC
{
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a NAMES command.
    /// </summary>
    public class NamesMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="NamesMessage"/> class.
        /// </summary>
        public NamesMessage()
            : base("NAMES")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="NamesMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to retrieve the names list from.</param>
        public NamesMessage(string channel)
            : base(string.Empty, "NAMES", new string[] { channel }, string.Empty)
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
            string pattern = @"/names(\s+(?<channel>[#&!\+][^\x07\x2C\s]{1,199}))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                string channel = this.ChannelName;
                if (match.Groups["channel"].Success)
                {
                    channel = match.Groups["channel"].Value;
                }

                if (string.IsNullOrEmpty(channel))
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.Names_MissingChannel);
                }
                else
                {
                    this.Parameters = new string[] { channel };
                    retval = new ParseResult(true, string.Empty);
                }
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
