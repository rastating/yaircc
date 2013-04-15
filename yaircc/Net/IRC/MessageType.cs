﻿//-----------------------------------------------------------------------
// <copyright file="MessageType.cs" company="rastating">
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
    /// Specifies the category of an IRC message. 
    /// </summary>
    public enum MessageType
    {
        /// <summary>
        /// A message that has no source.
        /// </summary>
        ServerMessage = 0,

        /// <summary>
        /// An error message.
        /// </summary>
        ErrorMessage = 1,

        /// <summary>
        /// A message received from another user on the network.
        /// </summary>
        UserMessage = 2,

        /// <summary>
        /// An action message received from another user on the network.
        /// </summary>
        ActionMessage = 3,

        /// <summary>
        /// A welcome message generated by yaircc's console tab.
        /// </summary>
        WelcomeMessage = 4,

        /// <summary>
        /// A warning message generated either by yaircc or received from the network.
        /// </summary>
        WarningMessage = 5,

        /// <summary>
        /// A notification, typically a result of a user invoked operation.
        /// </summary>
        NotificationMessage = 6,

        /// <summary>
        /// A notice message sent by the network or another user.
        /// </summary>
        NoticeMessage = 7
    }
}
