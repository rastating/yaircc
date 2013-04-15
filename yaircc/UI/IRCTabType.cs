//-----------------------------------------------------------------------
// <copyright file="IRCTabType.cs" company="rastating">
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

namespace Yaircc.UI
{
    /// <summary>
    /// Represents a specific type of <see cref="IRCTabPage"/>
    /// </summary>
    public enum IRCTabType
    {
        /// <summary>
        /// A console tab
        /// </summary>
        Console = 0,

        /// <summary>
        /// A server tab
        /// </summary>
        Server = 1,

        /// <summary>
        /// A channel tab
        /// </summary>
        Channel = 2,

        /// <summary>
        /// A private message tab
        /// </summary>
        PM = 3
    }
}