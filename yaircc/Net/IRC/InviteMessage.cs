//-----------------------------------------------------------------------
// <copyright file="InviteMessage.cs" company="intninety">
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
    /// Represents an INVITE command.
    /// </summary>
    public class InviteMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="InviteMessage"/> class.
        /// </summary>
        public InviteMessage()
            : base("INVITE")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="InviteMessage"/> class.
        /// </summary>
        /// <param name="nickName">The nick name of the user to invite.</param>
        /// <param name="channel">The channel to invite the user to.</param>
        public InviteMessage(string nickName, string channel)
            : base("INVITE", new string[] { nickName, channel })
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                return string.Format("{0} has invited you to join {1}.", base.Source, this.Parameters[1]);
            }
        }

        /// <summary>
        /// Gets the source of the message.
        /// </summary>
        public override string Source
        {
            get
            {
                return Strings_General.DirectNoticeSource;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the message should be handled by all channels and the server window itself.
        /// </summary>
        public override bool IsGlobalMessage
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
            string pattern = @"/invite\s+(?<nickname>[^\s]+)(\s+(?<channel>[#&!\+][^\x07\x2C\s]{1,199}))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                string channel = this.ChannelName;
                if (match.Groups["channel"].Success)
                {
                    channel = match.Groups["channel"].Value;
                }

                if (string.IsNullOrEmpty(channel))
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.Invite_MissingChannel);
                }

                this.Parameters = new string[] { match.Groups["nickname"].Value, channel };
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Nickname);
            }

            return retval;
        }

        #endregion
    }
}
