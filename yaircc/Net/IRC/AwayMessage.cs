//-----------------------------------------------------------------------
// <copyright file="AwayMessage.cs" company="intninety">
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
    /// Represents an AWAY command.
    /// </summary>
    public class AwayMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.AwayMessage class.
        /// </summary>
        public AwayMessage()
            : base("AWAY")
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.AwayMessage class.
        /// </summary>
        /// <param name="message">The message to be sent when messaged by other users.</param>
        public AwayMessage(string message)
            : base(string.Empty, "AWAY", new string[0], message)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                return base.Content;
            }
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
            string pattern = @"/away(\s+(?<message>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["message"].Success)
                {
                    this.TrailingParameter = match.Groups["message"].Value;
                }
            }

            return new ParseResult(true, string.Empty);
        }

        #endregion
    }
}
