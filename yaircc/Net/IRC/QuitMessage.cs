//-----------------------------------------------------------------------
// <copyright file="QuitMessage.cs" company="rastating">
//     yaircc - the free, open-source IRC client for Windows.
//     Copyright (C) 2012-2014 Robert Carr
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
    /// Represents a QUIT command.
    /// </summary>
    public class QuitMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="QuitMessage"/> class.
        /// </summary>
        public QuitMessage()
            : base("QUIT")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="QuitMessage"/> class.
        /// </summary>
        /// <param name="message">The reason for quitting.</param>
        public QuitMessage(string message)
            : base(string.Empty, "QUIT", new string[0], message)
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
                    return string.Format("{0} has left {1}", base.Source, this.AssociatedConnection.ToString());
                }
                else
                {
                    return string.Format("{0} has left {2} ({1})", base.Source, this.TrailingParameter, this.AssociatedConnection.ToString());
                }
            }
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
        /// Gets the nick name of the user that has quit.
        /// </summary>
        public string NickName
        {
            get { return base.Source; }
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
            string pattern = @"/quit(\s+(?<message>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                if (match.Groups["message"].Success)
                {
                    this.TrailingParameter = match.Groups["message"].Value;
                }
            }

            return new ParseResult(true, string.Empty);
        }

        #endregion
    }
}
