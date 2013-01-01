﻿//-----------------------------------------------------------------------
// <copyright file="PongMessage.cs" company="intninety">
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
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a PONG command.
    /// </summary>
    public class PongMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="PongMessage"/> class.
        /// </summary>
        public PongMessage()
            : base("PONG")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PongMessage"/> class.
        /// </summary>
        /// <param name="daemon">The daemon to use.</param>
        public PongMessage(string daemon)
            : base(string.Empty, "PONG", new string[0], daemon)
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
            string pattern = @"PING\s+:(?<daemon>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                this.TrailingParameter = match.Groups["daemon"].Value.RemoveCarriageReturns();
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
