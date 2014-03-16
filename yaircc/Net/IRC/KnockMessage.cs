//-----------------------------------------------------------------------
// <copyright file="KnockMessage.cs" company="rastating">
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
    using System;
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a KNOCK command.
    /// </summary>
    public class KnockMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="KnockMessage"/> class.
        /// </summary>
        public KnockMessage()
            : base("KNOCK")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KnockMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to send the knock command to.</param>
        /// <param name="message">The message to be shown with the knock.</param>
        public KnockMessage(string channel, string message)
            : base(string.Empty, "KNOCK", new string[] { channel }, message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KnockMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to send the knock command to.</param>
        public KnockMessage(string channel)
            : base("KNOCK", new string[] { channel })
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
            string pattern = @"/knock\s+(?<channel>[#&][^\x07\x2C\s]{1,199})(\s+(?<reason>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["reason"].Success)
                {
                    this.TrailingParameter = match.Groups["reason"].Value;
                }

                this.Parameters = new string[] { match.Groups["channel"].Value };
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Channel);
            }

            return retval;
        }

        #endregion
    }
}
