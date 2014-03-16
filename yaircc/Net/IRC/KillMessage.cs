//-----------------------------------------------------------------------
// <copyright file="KillMessage.cs" company="rastating">
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
    using System;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a KILL command.
    /// </summary>
    public class KillMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="KillMessage"/> class.
        /// </summary>
        public KillMessage()
            : base("KILL")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="KillMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name of the user to remove from the network.</param>
        /// <param name="comment">The reason behind removing the user.</param>
        public KillMessage(string nickname, string comment)
            : base(string.Empty, "KILL", new string[] { nickname }, comment)
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
                if (this.Parameters[0].Equals(this.AssociatedConnection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    return string.Format("YOUR connection was killed by {0}", base.Source);
                }
                else
                {
                    return string.Format("{0}'s connection was killed by {1}", this.Parameters[0], base.Source);
                }
            }
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

        #endregion
    }
}
