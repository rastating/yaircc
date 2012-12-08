//-----------------------------------------------------------------------
// <copyright file="WhoisMessage.cs" company="intninety">
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
    /// Represents a WHOIS command.
    /// </summary>
    public class WhoisMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoisMessage"/> class.
        /// </summary>
        public WhoisMessage()
            : base("WHOIS") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoisMessage"/> class.
        /// </summary>
        /// <param name="nickmask">The nick mask to retrieve information about.</param>
        public WhoisMessage(string nickmask)
            : base("WHOIS", new string[] { nickmask })
        {
        }
        
        /// <summary>
        /// Initialises a new instance of the <see cref="WhoisMessage"/> class.
        /// </summary>
        /// <param name="server">The server in which the user(s) reside on.</param>
        /// <param name="nickmask">The nick mask to retrieve information about.</param>
        public WhoisMessage(string server, string nickmask)
            : base("WHOIS", new string[] { server, nickmask })
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
            string pattern = @"/whois\s+(?<nickname>[^\s]+)";
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
