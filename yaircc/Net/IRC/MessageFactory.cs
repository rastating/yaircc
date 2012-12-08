//-----------------------------------------------------------------------
// <copyright file="MessageFactory.cs" company="intninety">
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
    using System.Reflection;
    using System.Resources;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Provides factory methods to instantiate <see cref="Message"/> classes.
    /// </summary>
    internal static class MessageFactory
    {
        /// <summary>
        /// Gets the class name for the command that is being called in a friendly message.
        /// </summary>
        /// <param name="payload">The payload containing the command.</param>
        /// <returns>The matching class name for the command.</returns>
        internal static string GetClassNameFromUserCommand(string payload)
        {
            string retval = string.Empty;
            ResourceManager res = new ResourceManager("Yaircc.MessageReflectionMap", Assembly.GetExecutingAssembly());

            // If the input didn't begin with a forward slash then we aren't trying to parse a command, so
            // create a PrivMsgMessage that we will send to the channel. Otherwise, do some reflection magic...
            if (payload.StartsWith("/"))
            {
                string pattern = @"/(?<command>[a-zA-Z\-]+)";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = regex.Match(payload);

                if (match.Success)
                {
                    retval = res.GetString(match.Groups["command"].Value.ToLower());
                }
            }
            else
            {
                retval = res.GetString("msg");
            }

            return retval;
        }

        /// <summary>
        /// Gets the class name for the command that is being called in a raw command.
        /// </summary>
        /// <param name="command">The raw command.</param>
        /// <returns>The matching class name for the command.</returns>
        internal static string GetClassNameFromRawCommand(string command)
        {
            string retval = string.Empty;
            ResourceManager manager = new ResourceManager("Yaircc.IncomingReflectionMap", Assembly.GetExecutingAssembly());

            retval = manager.GetString(command.ToUpper());
            
            return retval;
        }

        /// <summary>
        /// Creates a new <see cref="Message"/>, encapsulating the data from the payload.
        /// </summary>
        /// <param name="payload">The payload to be encapsulated.</param>
        /// <param name="sourceChannel">The name of the channel the message is being generated on.</param>
        /// <returns>A <see cref="MessageParseResult"/> containing the result of the operation.</returns>
        internal static MessageParseResult CreateFromUserInput(string payload, string sourceChannel)
        {
            MessageParseResult retval = null;
            string className = GetClassNameFromUserCommand(payload);

            if (!string.IsNullOrEmpty(className))
            {
                Message message = (Message)System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(string.Format("Yaircc.Net.IRC.{0}", className));
                message.ChannelName = sourceChannel;
                ParseResult parseResult = message.TryParse(payload);
                retval = new MessageParseResult(parseResult.Success, parseResult.Message, message);
            }
            else
            {
                Message message = new Message();
                ParseResult parseResult = message.TryParse(payload);
                retval = new MessageParseResult(parseResult.Success, parseResult.Message, message);
            }

            return retval;
        }

        /// <summary>
        /// Assimilates the message into the appropriate subclass.
        /// </summary>
        /// <param name="message">The message to assimilate.</param>
        /// <returns>The assimilated message, or the original message if it cannot be mapped to a subclass.</returns>
        internal static Message AssimilateMessage(Message message)
        {
            Message retval = message;
            string className = GetClassNameFromRawCommand(message.Command);

            if (!string.IsNullOrEmpty(className))
            {
                retval = (Message)Assembly.GetExecutingAssembly().CreateInstance(string.Format("Yaircc.Net.IRC.{0}", className));
                retval.Assimilate(message);
            }

            return retval;
        }
    }
}
