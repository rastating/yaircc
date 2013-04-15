//-----------------------------------------------------------------------
// <copyright file="TopicMessage.cs" company="rastating">
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
