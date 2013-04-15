//-----------------------------------------------------------------------
// <copyright file="IRCUser.cs" company="rastating">
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
