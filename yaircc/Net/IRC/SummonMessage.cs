//-----------------------------------------------------------------------
// <copyright file="SummonMessage.cs" company="rastating">
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
    /// Represents a SUMMON command.
    /// </summary>
    public class SummonMessage : Message
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SummonMessage"/> class.
        /// </summary>
        /// <param name="user">The user to summon.</param>
        public SummonMessage(string user)
            : base("SUMMON", new string[] { user })
        {
        }
        
        /// <summary>
        /// Initialises a new instance of the <see cref="SummonMessage"/> class.
        /// </summary>
        /// <param name="user">The user to summon.</param>
        /// <param name="target">The server in which the user is on.</param>
        public SummonMessage(string user, string target)
            : base("SUMMON", new string[] { user, target })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SummonMessage"/> class.
        /// </summary>
        /// <param name="user">The user to summon.</param>
        /// <param name="target">The server in which the user is on.</param>
        /// <param name="channel">The channel in which to summon the user to.</param>
        public SummonMessage(string user, string target, string channel)
            : base("SUMMON", new string[] { user, target, channel })
        {
        }
    }
}
