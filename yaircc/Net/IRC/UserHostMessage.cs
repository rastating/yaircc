//-----------------------------------------------------------------------
// <copyright file="UserHostMessage.cs" company="rastating">
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
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a USERHOST command.
    /// </summary>
    public class UserHostMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="UserHostMessage"/> class.
        /// </summary>
        public UserHostMessage()
            : base("USERHOST") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UserHostMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name to retrieve information about.</param>
        public UserHostMessage(string nickname)
            : base("USERHOST", new string[] { nickname }) 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="UserHostMessage"/> class.
        /// </summary>
        /// <param name="nicknames">The nick names to retrieve information about.</param>
        public UserHostMessage(string[] nicknames)
            : base("USERHOST", nicknames) 
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
            string pattern = @"/userhost\s+(?<nicknames>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                string[] nicknames = match.Groups["nicknames"].Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);
                if (nicknames.GetLength(0) > 0)
                {
                    this.Parameters = nicknames;
                    retval = new ParseResult(true, string.Empty);
                }
                else
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Nickname);
                }
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
