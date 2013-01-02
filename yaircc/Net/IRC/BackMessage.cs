//-----------------------------------------------------------------------
// <copyright file="BackMessage.cs" company="intninety">
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

    /// <summary>
    /// Represents a message used to remove an away flag.
    /// </summary>
    public class BackMessage : Message
    {
        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.BackMessage class.
        /// </summary>
        public BackMessage()
            : base("AWAY")
        {
        }

        /// <summary>
        /// Attempts to parse the input specified by the user into the Message.
        /// </summary>
        /// <param name="input">The input from the user.</param>
        /// <returns>True on success, false on failure.</returns>
        public override ParseResult TryParse(string input)
        {
            return new ParseResult(true, string.Empty);
        }
    }
}
