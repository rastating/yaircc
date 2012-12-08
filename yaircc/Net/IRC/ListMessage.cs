//-----------------------------------------------------------------------
// <copyright file="ListMessage.cs" company="intninety">
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
    using System;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a LIST command.
    /// </summary>
    public class ListMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ListMessage"/> class.
        /// </summary>
        public ListMessage()
            : base("LIST")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ListMessage"/> class.
        /// </summary>
        /// <param name="channel">The comma-separated list of channels to return the topics of.</param>
        public ListMessage(string channel)
            : base("LIST", new string[] { channel })
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
            string pattern = @"/list(\s+(?<channel>[#&!\+][^\x07\x2C\s]{1,199}))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["channel"].Success)
                {
                    this.Parameters = new string[] { match.Groups["channel"].Value };
                }

                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, string.Empty);
            }

            return retval;
        }

        #endregion
    }
}
