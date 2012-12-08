//-----------------------------------------------------------------------
// <copyright file="PartMessage.cs" company="intninety">
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
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a PART command.
    /// </summary>
    public class PartMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="PartMessage"/> class.
        /// </summary>
        public PartMessage()
            : base("PART")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="PartMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to leave.</param>
        public PartMessage(string channel)
            : base(string.Empty, "PART", new string[] { channel }, string.Empty)
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
                return Strings_General.OutSource;
            }
        }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                if (string.IsNullOrEmpty(this.TrailingParameter))
                {
                    return string.Format("{0} has left {1}", base.Source, this.Parameters[0]);
                }
                else
                {
                    return string.Format("{0} has left {1} ({2})", base.Source, this.Parameters[0], this.TrailingParameter);
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
                return MessageType.WarningMessage;
            }
        }

        /// <summary>
        /// Gets the recipient the message was to be processed by. This can be either a channel name or a nick name.
        /// </summary>
        public override string Target
        {
            get
            {
                return this.Parameters[0];
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
        /// Gets the nick name of the user that has left the channel.
        /// </summary>
        public string NickName
        {
            get { return base.Source; }
        }

        /// <summary>
        /// Gets the channel name that the user has left.
        /// </summary>
        public string Channel
        {
            get { return this.Parameters[0]; }
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
            string pattern = @"/(leave|part)(\s+(?<channel>[#&][^\x07\x2C\s]{1,199}))?(\s+(?<message>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                string channel = this.ChannelName;
                if (match.Groups["channel"].Success)
                {
                    channel = match.Groups["channel"].Value;
                }

                if (match.Groups["message"].Success)
                {
                    this.TrailingParameter = match.Groups["message"].Value;
                }

                if (string.IsNullOrEmpty(channel))
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.Leave_MissingChannel);
                }
                else
                {
                    this.Parameters = new string[] { channel };
                    retval = new ParseResult(true, string.Empty);
                }
            }
            else
            {
                retval = new ParseResult(false, string.Empty);
            }

            return retval;
        }

        #endregion
    }
}
