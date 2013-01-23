//-----------------------------------------------------------------------
// <copyright file="AwayMessage.cs" company="intninety">
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
    /// Represents an AWAY command.
    /// </summary>
    public class AwayMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.AwayMessage class.
        /// </summary>
        public AwayMessage()
            : base("AWAY")
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.AwayMessage class.
        /// </summary>
        /// <param name="message">The message to be sent when messaged by other users.</param>
        public AwayMessage(string message)
            : base(string.Empty, "AWAY", new string[0], message)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                return base.Content;
            }
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
            string pattern = @"/away(\s+(?<message>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["message"].Success)
                {
                    this.TrailingParameter = match.Groups["message"].Value;
                }
            }

            return new ParseResult(true, string.Empty);
        }

        #endregion
    }
}
