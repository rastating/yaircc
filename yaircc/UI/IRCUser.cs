//-----------------------------------------------------------------------
// <copyright file="IRCUser.cs" company="intninety">
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

namespace Yaircc.UI
{
    using System;

    /// <summary>
    /// Represents an IRC user.
    /// </summary>
    public class IRCUser
    {
        #region Fields

        /// <summary>
        /// The user's nick name.
        /// </summary>
        private string nickName;

        /// <summary>
        /// The user's mode.
        /// </summary>
        private IRCUserMode mode;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="IRCUser"/> class.
        /// </summary>
        /// <param name="nickname">The user's nick name.</param>
        /// <param name="mode">The user's mode.</param>
        public IRCUser(string nickname, IRCUserMode mode)
        {
            this.nickName = nickname;
            this.mode = mode;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the user's nick name.
        /// </summary>
        public string NickName
        {
            get { return this.nickName; }
            set { this.nickName = value; }
        }

        /// <summary>
        /// Gets or sets the user's mode.
        /// </summary>
        public IRCUserMode Mode
        {
            get { return this.mode; }
            set { this.mode = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Creates an IRC user from a names command fragment.
        /// </summary>
        /// <param name="input">The names command fragment.</param>
        /// <returns>The parsed user.</returns>
        public static IRCUser Parse(string input)
        {
            IRCUserMode mode = IRCUserMode.Normal;
            string nickname = input;

            if (input.StartsWith("+", StringComparison.OrdinalIgnoreCase))
            {
                mode = IRCUserMode.Voice;
            }
            else if (input.StartsWith("%", StringComparison.OrdinalIgnoreCase))
            {
                mode = IRCUserMode.HalfOperator;
            }
            else if (input.StartsWith("@", StringComparison.OrdinalIgnoreCase))
            {
                mode = IRCUserMode.Operator;
            }
            else if (input.StartsWith("&", StringComparison.OrdinalIgnoreCase))
            {
                mode = IRCUserMode.Admin;
            }
            else if (input.StartsWith("~", StringComparison.OrdinalIgnoreCase))
            {
                mode = IRCUserMode.Founder;
            }

            if (mode != IRCUserMode.Normal)
            {
                nickname = input.Substring(1);
            }

            return new IRCUser(nickname, mode);
        }

        #endregion
    }
}
