//-----------------------------------------------------------------------
// <copyright file="PasswordMessage.cs" company="intninety">
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
    /// <summary>
    /// Represents a PASS command.
    /// </summary>
    public class PasswordMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="PasswordMessage"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        public PasswordMessage(string password)
            : base(string.Empty, "PASS", new string[] { password }, string.Empty)
        {
            // If no password was specified we still need to send something
            // as otherwise some servers (snircd being one) will complain that
            // not enough parameters were specified.
            if (string.IsNullOrEmpty(password))
            {
                this.Parameters = new string[] { "*" };
            }
        }

        #endregion
    }
}
