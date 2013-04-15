//-----------------------------------------------------------------------
// <copyright file="OperMessage.cs" company="rastating">
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
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents an OPER command.
    /// </summary>
    public class OperMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="OperMessage"/> class.
        /// </summary>
        /// <param name="user">The username.</param>
        /// <param name="password">The password.</param>
        public OperMessage(string user, string password)
            : base(string.Empty, "OPER", new string[] { user, password }, string.Empty) 
        { 
        }
        
        /// <summary>
        /// Initialises a new instance of the <see cref="OperMessage"/> class.
        /// </summary>
        public OperMessage()
            : base("OPER") 
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
            string pattern = @"/oper\s+(?<username>[^\s]+)\s+(?<password>[^\s]+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                this.Parameters = new string[] { match.Groups["username"].Value, match.Groups["password"].Value };
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameters);
            }

            return retval;
        }

        #endregion
    }
}
