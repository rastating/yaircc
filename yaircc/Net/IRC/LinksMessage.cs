//-----------------------------------------------------------------------
// <copyright file="LinksMessage.cs" company="intninety">
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

    /// <summary>
    /// Represents a LINKS command.
    /// </summary>
    public class LinksMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LinksMessage"/> class.
        /// </summary>
        public LinksMessage()
            : base("LINKS")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LinksMessage"/> class.
        /// </summary>
        /// <param name="serverMask">The server mask to use.</param>
        public LinksMessage(string serverMask)
            : base("LINKS", new string[] { serverMask })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LinksMessage"/> class.
        /// </summary>
        /// <param name="remoteServer">The remote server to get the links from.</param>
        /// <param name="serverMask">The server mask to use.</param>
        public LinksMessage(string remoteServer, string serverMask)
            : base("LINKS", new string[] { remoteServer, serverMask })
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
            if (input.ToLower().StartsWith("/leave"))
            {
                return new ParseResult(true, string.Empty);
            }
            else
            {
                return new ParseResult(false, string.Empty);
            }
        }

        #endregion
    }
}
