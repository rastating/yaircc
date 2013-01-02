//-----------------------------------------------------------------------
// <copyright file="TopicMessage.cs" company="intninety">
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
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a TOPIC command.
    /// </summary>
    public class TopicMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TopicMessage"/> class.
        /// </summary>
        public TopicMessage()
            : base("TOPIC") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TopicMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to set the topic of.</param>
        /// <param name="topic">The new topic.</param>
        public TopicMessage(string channel, string topic)
            : base(string.Empty, "TOPIC", new string[] { channel }, topic)
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="TopicMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel to retrieve the topic of.</param>
        public TopicMessage(string channel)
            : base(string.Empty, "TOPIC", new string[] { channel }, string.Empty)
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
                if (string.IsNullOrEmpty(this.TrailingParameter))
                {
                    return string.Format("{0} has changed the topic to \"\"", base.Source);
                }
                else
                {
                    return string.Format("{1} has changed the topic to \"{0}\"", this.TrailingParameter, base.Source);
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
        /// Gets the source of the message.
        /// </summary>
        public override string Source
        {
            get
            {
                return Strings_General.NotificationSource;
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
            if (string.IsNullOrEmpty(this.ChannelName))
            {
                retval = new ParseResult(false, Strings_MessageParseResults.Topic_MissingChannel);
            }
            else
            {
                string pattern = @"/topic(\s+(?<topic>.+))?";
                Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
                Match match = regex.Match(input);

                if (match.Success)
                {
                    this.Parameters = new string[] { this.ChannelName };

                    if (match.Groups["topic"].Success)
                    {
                        this.TrailingParameter = match.Groups["topic"].Value;
                    }

                    retval = new ParseResult(true, string.Empty);
                }
                else
                {
                    retval = new ParseResult(false, string.Empty);
                }
            }

            return retval;
        }

        #endregion
    }
}
