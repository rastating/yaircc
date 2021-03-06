﻿//-----------------------------------------------------------------------
// <copyright file="ServListMessage.cs" company="rastating">
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
    /// Represents a SERVLIST command.
    /// </summary>
    public class ServListMessage : Message
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ServListMessage"/> class.
        /// </summary>
        public ServListMessage()
            : base("SERVLIST")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ServListMessage"/> class.
        /// </summary>
        /// <param name="mask">The mask to use.</param>
        public ServListMessage(string mask)
            : base("SERVLIST", new string[] { mask })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ServListMessage"/> class.
        /// </summary>
        /// <param name="mask">The mask to use.</param>
        /// <param name="type">The type of service to list.</param>
        public ServListMessage(string mask, string type)
            : base("SERVLIST", new string[] { mask, type })
        {
        }
    }
}
