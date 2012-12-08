//-----------------------------------------------------------------------
// <copyright file="ReplyDelegates.cs" company="intninety">
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