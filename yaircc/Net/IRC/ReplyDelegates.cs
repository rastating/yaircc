//-----------------------------------------------------------------------
// <copyright file="ReplyDelegates.cs" company="rastating">
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
    using System;

    /// <summary>
    /// Provides delegates that can be used to format replies from IRC commands.
    /// </summary>
    public static class ReplyDelegates
    {
        /// <summary>
        /// Delegate for formatting IRC replies.
        /// </summary>
        /// <param name="message">The message to format.</param>
        /// <returns>A formatted reply.</returns>
        public delegate string ReplyDelegate(Message message);

        /// <summary>
        /// Transforms a RPL_TOPICWHOTIME reply into a string indicating what time the topic was set.
        /// </summary>
        /// <param name="message">The message to format.</param>
        /// <returns>A formatted reply.</returns>
        public static string TransformTopicWhoTimeReply(Message message)
        {
            int timestamp = int.Parse(message.Parameters[3]);
            DateTime dateTime = new DateTime(1970, 1, 1, 0, 0, 0).AddSeconds(timestamp).ToLocalTime();
            return string.Format("Topic set by {0} on {1}", message.Parameters[2], dateTime);
        }
    }
}