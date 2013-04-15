//-----------------------------------------------------------------------
// <copyright file="WhowasMessage.cs" company="rastating">
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
    /// Represents a WHOWAS command.
    /// </summary>
    public class WhowasMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="WhowasMessage"/> class.
        /// </summary>
        public WhowasMessage()
            : base("WHOWAS") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhowasMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name to get the who was information of.</param>
        public WhowasMessage(string nickname)
            : base("WHOWAS", new string[] { nickname })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhowasMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name to get the who was information of.</param>
        /// <param name="count">The maximum number of uses to show.</param>
        public WhowasMessage(string nickname, string count)
            : base("WHOWAS", new string[] { nickname, count })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhowasMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name to get the who was information of.</param>
        /// <param name="count">The maximum number of uses to show.</param>
        /// <param name="server">The server in which the nick name was used.</param>
        public WhowasMessage(string nickname, string count, string server)
            : base("WHOWAS", new string[] { nickname, count, server })
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
            string pattern = @"/whowas\s+(?<nickname>[^\s]+)(\s+(?<limit>[0-9]+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["limit"].Success)
                {
                    this.Parameters = new string[] { match.Groups["nickname"].Value, match.Groups["limit"].Value };
                }
                else
                {
                    this.Parameters = new string[] { match.Groups["nickname"].Value };
                }

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
