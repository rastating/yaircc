//-----------------------------------------------------------------------
// <copyright file="KickMessage.cs" company="rastating">
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
    using System;
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a KICK command.
    /// </summary>
    public class KickMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="KickMessage"/> class.
        /// </summary>
        public KickMessage()
            : base("KICK")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KickMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel in which the user to be kicked is in.</param>
        /// <param name="nickname">The nick name of the user to be kicked.</param>
        public KickMessage(string channel, string nickname)
            : base("KICK", new string[] { channel, nickname })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KickMessage"/> class.
        /// </summary>
        /// <param name="channel">The channel in which the user to be kicked is in.</param>
        /// <param name="nickname">The nick name of the user to be kicked.</param>
        /// <param name="reason">The reason for kicking the user.</param>
        public KickMessage(string channel, string nickname, string reason)
            : base(string.Empty, "KICK", new string[] { channel, nickname }, reason)
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
                if (this.Parameters[1].Equals(this.AssociatedConnection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    return string.Format("YOU were kicked from {0} by {1} ({2})", this.Parameters[0], base.Source, this.TrailingParameter);
                }
                else
                {
                    return string.Format("{0} was kicked from {1} by {2} ({3})", this.Parameters[1], this.Parameters[0], base.Source, this.TrailingParameter);
                }
            }
        }

        /// <summary>
        /// Gets the nick name of the user being kicked.
        /// </summary>
        public string NickName
        {
            get { return this.Parameters[1]; }
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
        /// Gets the type of the message.
        /// </summary>
        public override MessageType Type
        {
            get
            {
                if (this.Parameters[1].Equals(this.AssociatedConnection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    return MessageType.ErrorMessage;
                }
                else
                {
                    return MessageType.NotificationMessage;
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
            string pattern = @"/kick\s(?<nickname>[^\s]+)+?(\s(?<reason>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                this.Parameters = new string[] { this.ChannelName, match.Groups["nickname"].Value };

                if (match.Groups["reason"].Success)
                {
                    this.TrailingParameter = match.Groups["reason"].Value;
                }

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
