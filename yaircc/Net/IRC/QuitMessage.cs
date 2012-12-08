//-----------------------------------------------------------------------
// <copyright file="QuitMessage.cs" company="intninety">
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
