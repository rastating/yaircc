//-----------------------------------------------------------------------
// <copyright file="KnockMessage.cs" company="intninety">
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
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a KNOCK command.
    /// </summary>
    public class KnockMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="KnockMessage"/> class.
        /// </summary>
        public KnockMessage()
            : base("KNOCK")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KnockMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to send the knock command to.</param>
        /// <param name="message">The message to be shown with the knock.</param>
        public KnockMessage(string channel, string message)
            : base(string.Empty, "KNOCK", new string[] { channel }, message)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KnockMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to send the knock command to.</param>
        public KnockMessage(string channel)
            : base("KNOCK", new string[] { channel })
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
            string pattern = @"/knock\s+(?<channel>[#&][^\x07\x2C\s]{1,199})(\s+(?<reason>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["reason"].Success)
                {
                    this.TrailingParameter = match.Groups["reason"].Value;
                }

                this.Parameters = new string[] { match.Groups["channel"].Value };
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Channel);
            }

            return retval;
        }

        #endregion
    }
}
