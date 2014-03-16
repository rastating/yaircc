//-----------------------------------------------------------------------
// <copyright file="SQuitMessage.cs" company="rastating">
//     yaircc - the free, open-source IRC client for Windows.
//     Copyright (C) 2012-2014 Robert Carr
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
    /// <summary>
    /// Represents an SQUIT command.
    /// </summary>
    public class SQuitMessage : Message
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SQuitMessage"/> class.
        /// </summary>
        /// <param name="server">The server to force quit.</param>
        /// <param name="comment">The reason for the quit.</param>
        public SQuitMessage(string server, string comment)
            : base(string.Empty, "SQUIT", new string[] { server }, comment)
        {
        }
    }
}
