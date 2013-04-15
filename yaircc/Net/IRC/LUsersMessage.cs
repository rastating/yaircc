//-----------------------------------------------------------------------
// <copyright file="LUsersMessage.cs" company="rastating">
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
    /// <summary>
    /// Represents an LUSERS command.
    /// </summary>
    public class LUsersMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="LUsersMessage"/> class.
        /// </summary>
        public LUsersMessage()
            : base("LUSERS")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LUsersMessage"/> class.
        /// </summary>
        /// <param name="mask">The mask to use when selecting the statistics.</param>
        public LUsersMessage(string mask)
            : base("LUSERS", new string[] { mask })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="LUsersMessage"/> class.
        /// </summary>
        /// <param name="mask">The mask to use when selecting the statistics.</param>
        /// <param name="server">The server to forward the command to.</param>
        public LUsersMessage(string mask, string server)
            : base("LUSERS", new string[] { mask, server })
        {
        }

        #endregion
    }
}
