//-----------------------------------------------------------------------
// <copyright file="NickMessage.cs" company="intninety">
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
