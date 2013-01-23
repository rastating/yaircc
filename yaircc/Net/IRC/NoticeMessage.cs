//-----------------------------------------------------------------------
// <copyright file="NoticeMessage.cs" company="intninety">
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
    /// Represents a NOTICE command.
    /// </summary>
    public class NoticeMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="NoticeMessage"/> class.
        /// </summary>
        public NoticeMessage()
            : base("NOTICE")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="NoticeMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name of the user to send a notice to.</param>
        /// <param name="message">The content of the notice.</param>
        public NoticeMessage(string nickname, string message)
            : base(string.Empty, "NOTICE", new string[] { nickname }, message)
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
                if (this.PrefixType == PrefixType.ServerName || this.PrefixType == PrefixType.None)
                {
                    return Strings_General.DirectNoticeSource;
                }
                else
                {
                    return string.Format("from({0})", base.Source);
                }
            }
        }

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
                return this.TrailingParameter.RemoveCarriageReturns();
            }
        }

        /// <summary>
        /// Gets a value indicating whether or not the message must be handled by its target.
        /// </summary>
        public override bool MustBeHandledByTarget
        {
            get
            {
                return false;
            }
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        public override MessageType Type
        {
            get
            {
                return MessageType.NoticeMessage;
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
            string pattern = @"/notice\s+(?<nickname>[^\s]+)\s+(?<message>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

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
