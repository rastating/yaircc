//-----------------------------------------------------------------------
// <copyright file="LinksMessage.cs" company="intninety">
//     yaircc - the free, open-source IRC client for Windows.
//     Copyright (C) 2012-2013 Robert Carr
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see http://www.gnu.org/licenses/.
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
