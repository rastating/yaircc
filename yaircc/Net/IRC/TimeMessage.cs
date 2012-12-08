//-----------------------------------------------------------------------
// <copyright file="TimeMessage.cs" company="intninety">
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

    /// <summary>
    /// Represents a TIME command.
    /// </summary>
    public class TimeMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TimeMessage"/> class.
        /// </summary>
        public TimeMessage()
            : base("TIME")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TimeMessage"/> class.
        /// </summary>
        /// <param name="server">The server in which to get the time from.</param>
        public TimeMessage(string server)
            : base("TIME", new string[] { server })
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
            string pattern = @"/time(\s+(?<server>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["server"].Success)
                {
                    this.Parameters = new string[] { match.Groups["server"].Value };
                }
            }

            return new ParseResult(true, string.Empty);
        }

        #endregion
    }
}
