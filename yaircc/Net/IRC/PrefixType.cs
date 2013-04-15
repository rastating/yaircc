//-----------------------------------------------------------------------
// <copyright file="PrefixType.cs" company="rastating">
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

namespace Yaircc.Net
{
    /// <summary>
    /// Specifies the type of data contained within an IRC command prefix.
    /// </summary>
    [System.Flags]
    public enum PrefixType
    {
        /// <summary>
        /// No prefix is present.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The name of the server.
        /// </summary>
        ServerName = 0x1,

        /// <summary>
        /// The nick name of the sender.
        /// </summary>
        NickName = 0x2,

        /// <summary>
        /// The user name of the sender.
        /// </summary>
        UserExtension = 0x4,

        /// <summary>
        /// The host of the sender.
        /// </summary>
        HostExtension = 0x8
    }
}