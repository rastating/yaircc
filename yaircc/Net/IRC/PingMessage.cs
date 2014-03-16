//-----------------------------------------------------------------------
// <copyright file="PingMessage.cs" company="rastating">
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
    /// Represents a PING command.
    /// </summary>
    public class PingMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="PingMessage"/> class.
        /// </summary>
        public PingMessage()
            : base("PING") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PingMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name of the user to ping.</param>
        public PingMessage(string nickname)
            : base("PING", new string[] { nickname }) 
        { 
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether or not the message should be printed to screen.
        /// </summary>
        public override bool ShouldPrint
        {
            get
            {
                return false;
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
            ParseResult retval;
            string pattern = @"/ping\s+(?<nickname>[^\s]+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                this.Parameters = new string[] { match.Groups["nickname"].Value };
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Nickname);
            }

            return retval;
        }

        #endregion
    }
}
