//-----------------------------------------------------------------------
// <copyright file="PartMessage.cs" company="intninety">
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
                    return string.Format("{0} has left {1}", base.Source, this.Channel);
                }
                else
                {
                    return string.Format("{0} has left {1} ({2})", base.Source, this.Channel, this.TrailingParameter);
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
            get { return this.Channel; }
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
            get 
            {
                // In some instances the channel name is sent in the trailing parameter
                // so first make sure the parameter count is more than zero, if it is we
                // will use the channel in the parameters at index zero (as it should be).
                //
                // However, if it is not greater than zero we will check the trailing 
                // parameter, and if there is still no data we will return a blank target.
                if (this.Parameters.Length > 0)
                {
                    return this.Parameters[0];
                }
                else if (!string.IsNullOrEmpty(this.TrailingParameter) && Regex.IsMatch(this.TrailingParameter, RegularExpressions.ChannelName))
                {
                    return this.TrailingParameter;
                }
                else
                {
                    return string.Empty;
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
