//-----------------------------------------------------------------------
// <copyright file="NickMessage.cs" company="rastating">
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
    /// Represents a NICK command.
    /// </summary>
    public class NickMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="NickMessage"/> class.
        /// </summary>
        public NickMessage()
            : base("NICK")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="NickMessage"/> class.
        /// </summary>
        /// <param name="nickname">The new nick name to use.</param>
        public NickMessage(string nickname)
            : base("NICK", new string[] { nickname })
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
                return Strings_General.NotificationSource;
            }
        }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                if (this.NewNickname.Equals(this.AssociatedConnection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    this.AssociatedConnection.Nickname = this.NewNickname;
                    return string.Format("YOU are now known as {0}", this.NewNickname);
                }
                else
                {
                    return string.Format("{0} is now known as {1}", base.Source, this.NewNickname);
                }
            }
        }

        /// <summary>
        /// Gets the old nick name.
        /// </summary>
        public string OldNickname
        {
            get { return base.Source; }
        }

        /// <summary>
        /// Gets the new nick name.
        /// </summary>
        public string NewNickname
        {
            get { return this.Parameters.Length > 0 ? this.Parameters[0] : this.TrailingParameter; }
        }

        /// <summary>
        /// Gets a value indicating whether the message should be handled by all active channels.
        /// </summary>
        public override bool IsMultiChannelMessage
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
            string pattern = @"/nick\s+(?<nickname>[^\s]+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                this.Parameters = new string[] { match.Groups["nickname"].Value };
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
