﻿//-----------------------------------------------------------------------
// <copyright file="MessageType.cs" company="intninety">
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
