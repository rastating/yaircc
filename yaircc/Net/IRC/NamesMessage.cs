//-----------------------------------------------------------------------
// <copyright file="NamesMessage.cs" company="intninety">
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
    /// Represents a NAMES command.
    /// </summary>
    public class NamesMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="NamesMessage"/> class.
        /// </summary>
        public NamesMessage()
            : base("NAMES")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="NamesMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to retrieve the names list from.</param>
        public NamesMessage(string channel)
            : base(string.Empty, "NAMES", new string[] { channel }, string.Empty)
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
            string pattern = @"/names(\s+(?<channel>[#&!\+][^\x07\x2C\s]{1,199}))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                string channel = this.ChannelName;
                if (match.Groups["channel"].Success)
                {
                    channel = match.Groups["channel"].Value;
                }

                if (string.IsNullOrEmpty(channel))
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.Names_MissingChannel);
                }
                else
                {
                    this.Parameters = new string[] { channel };
                    retval = new ParseResult(true, string.Empty);
                }
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
