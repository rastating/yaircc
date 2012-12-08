//-----------------------------------------------------------------------
// <copyright file="JoinMessage.cs" company="intninety">
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
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a JOIN command.
    /// </summary>
    public class JoinMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.JoinMessage class.
        /// </summary>
        public JoinMessage()
            : base("JOIN")
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.JoinMessage class.
        /// </summary>
        /// <param name="channelName">The name of the channel to join.</param>
        public JoinMessage(string channelName)
            : base(string.Empty, "JOIN", new string[] { channelName }, string.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.JoinMessage class.
        /// </summary>
        /// <param name="channelName">The name of the channel to join.</param>
        /// <param name="key">The channel password.</param>
        public JoinMessage(string channelName, string key)
            : base(string.Empty, "JOIN", new string[] { channelName, key }, string.Empty)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the source of the message.
        /// </summary>
        public override string Source
        {
            get
            {
                return Strings_General.InSource;
            }
        }

        /// <summary>
        /// Gets the recipient the message was to be processed by. This can be either a channel name or a nick name.
        /// </summary>
        public override string Target
        {
            get
            {
                if (this.Parameters.Length > 0)
                {
                    return this.Parameters[0].RemoveCarriageReturns();
                }
                else
                {
                    return this.TrailingParameter.RemoveCarriageReturns();
                }
            }
        }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                if (base.Source.Equals(this.AssociatedConnection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    return "YOU have joined the channel.";
                }
                else
                {
                    return string.Format("{0} has joined the channel.", base.Source);
                }
            }
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.NotificationMessage;
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

        #endregion

        #region Instance Methods

        /// <summary>
        /// Attempts to parse the input specified by the user into the Message.
        /// </summary>
        /// <param name="input">The input from the user.</param>
        /// <returns>True on success, false on failure.</returns>
        public override ParseResult TryParse(string input)
        {
            ParseResult retval;
            string pattern = @"/(j|join)\s+(?<channel>[#&][^\x07\x2C\s]{1,199})(\s+(?<key>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["key"].Success)
                {
                    this.Parameters = new string[] { match.Groups["channel"].Value, match.Groups["key"].Value };
                }
                else
                {
                    this.Parameters = new string[] { match.Groups["channel"].Value };
                }

                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Channel);
            }

            return retval;
        }

        #endregion
    }
}
