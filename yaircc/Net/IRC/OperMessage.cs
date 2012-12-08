//-----------------------------------------------------------------------
// <copyright file="OperMessage.cs" company="intninety">
//     Copyright 2012 Robert Carr
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
