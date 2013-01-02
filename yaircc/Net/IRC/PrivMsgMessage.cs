//-----------------------------------------------------------------------
// <copyright file="PrivMsgMessage.cs" company="intninety">
//     Copyright 2012-2013 Robert Carr
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
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a PRIVMSG command.
    /// </summary>
    public class PrivMsgMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="PrivMsgMessage"/> class.
        /// </summary>
        public PrivMsgMessage()
            : base("PRIVMSG")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PrivMsgMessage"/> class.
        /// </summary>
        /// <param name="receiver">The recipient of the message.</param>
        /// <param name="message">The message.</param>
        public PrivMsgMessage(string receiver, string message)
            : base(string.Empty, "PRIVMSG", new string[0], message)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the recipient of the message.
        /// </summary>
        public string Recipient
        {
            get
            {
                if (this.Parameters.Length > 0)
                {
                    return this.Parameters[0].RemoveCarriageReturns();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Gets the recipient the message was to be processed by. This can be either a channel name or a nick name.
        /// </summary>
        public override string Target
        {
            get
            {
                return this.Recipient;
            }
        }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                if (this.Type == MessageType.ActionMessage)
                {
                    return this.TrailingParameter.Replace(string.Format("{0}ACTION ", (char)1), string.Empty).TrimEnd((char)1);
                }
                else
                {
                    return this.TrailingParameter.RemoveCarriageReturns();
                }
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the message must be handled by its target.
        /// </summary>
        public override bool MustBeHandledByTarget
        {
            get
            {
                return true;
            }
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        public override MessageType Type
        {
            get
            {
                if (this.TrailingParameter.StartsWith(string.Format("{0}ACTION ", (char)1)))
                {
                    return MessageType.ActionMessage;
                }
                else
                {
                    return MessageType.UserMessage;
                }
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Attempts to parse the input specified by the user into the Message.
        /// </summary>
        /// <param name="input">The input from the user.</param>
        /// <returns>True on success, false on failure.</returns>
        public override ParseResult TryParse(string input)
        {
            if (input.StartsWith("/me", StringComparison.OrdinalIgnoreCase))
            {
                return this.ParseActionMessage(input);
            }
            else if (input.StartsWith("/msg", StringComparison.OrdinalIgnoreCase))
            {
                return this.ParsePrivateMessage(input);
            }
            else
            {
                return this.ParseRegularMessage(input);
            }
        }

        /// <summary>
        /// Parse an action message.
        /// </summary>
        /// <param name="payload">The raw message.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParseActionMessage(string payload)
        {
            ParseResult retval;
            string pattern = @"/me\s+(?<action>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(payload);

            if (string.IsNullOrEmpty(this.ChannelName))
            {
                retval = new ParseResult(false, Strings_MessageParseResults.Me_InvalidContext);
            }
            else
            {
                if (match.Success)
                {
                    this.Parameters = new string[] { this.ChannelName };
                    this.TrailingParameter = string.Format("{0}ACTION {1}{0}", (char)1, match.Groups["action"].Value);
                    retval = new ParseResult(true, string.Empty);
                }
                else
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Action);
                }
            }

            return retval;
        }

        /// <summary>
        /// Parse a standard message.
        /// </summary>
        /// <param name="payload">The raw message.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParseRegularMessage(string payload)
        {
            if (string.IsNullOrEmpty(this.ChannelName))
            {
                return new ParseResult(false, Strings_MessageParseResults.Server_InvalidMessageContext);
            }
            else
            {
                this.Parameters = new string[] { this.ChannelName };
                this.TrailingParameter = payload;
                return new ParseResult(true, string.Empty);
            }
        }

        /// <summary>
        /// Parse a private message.
        /// </summary>
        /// <param name="payload">The raw message.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParsePrivateMessage(string payload)
        {
            ParseResult retval;
            string pattern = @"/msg\s+(?<nickname>[^\s]+)\s+(?<message>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(payload);

            if (match.Success)
            {
                this.Parameters = new string[] { match.Groups["nickname"].Value };
                this.TrailingParameter = match.Groups["message"].Value;
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameters);
            }

            return retval;
        }

        #endregion
    }
}
