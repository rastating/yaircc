//-----------------------------------------------------------------------
// <copyright file="WhoMessage.cs" company="intninety">
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
    /// Represents a WHO command.
    /// </summary>
    public class WhoMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoMessage"/> class.
        /// </summary>
        public WhoMessage()
            : base("WHO") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoMessage"/> class.
        /// </summary>
        /// <param name="name">The name of the user to get the who information of.</param>
        public WhoMessage(string name)
            : base("WHO", new string[] { name }) 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoMessage"/> class.
        /// </summary>
        /// <param name="name">The name of the user to get the who information of.</param>
        /// <param name="operatorFlag">A flag to indicate whether or not to return information about only IRC operators.</param>
        public WhoMessage(string name, string operatorFlag)
            : base("WHO", new string[] { name, operatorFlag }) 
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
            string pattern = @"/who\s+(?<mask>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                this.Parameters = match.Groups["mask"].Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (this.Parameters.Length > 0)
                {
                    retval = new ParseResult(true, string.Empty);
                }
                else
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Mask);
                }
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Mask);
            }

            return retval;
        }

        #endregion
    }
}
