//-----------------------------------------------------------------------
// <copyright file="UserHostMessage.cs" company="intninety">
//     Copyright 2012-2013 Robert Carr
//     Licensed under the Apache License, Version 2.0 (the "License");
//     you may not use this file except in compliance with the License.
//     You may obtain a copy of the License at
//     
//     http://www.apache.org/licenses/LICENSE-2.0
//     
//     Unless required by applicable law or agreed to in writing, software
//     distributed under the License is distributed on an "AS IS" BASIS,
//     WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
//     See the License for the specific language governing permissions and
//     limitations under the License.
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
