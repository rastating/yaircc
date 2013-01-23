//-----------------------------------------------------------------------
// <copyright file="ConnectMessage.cs" company="intninety">
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
    /// Represents a CONNECT command.
    /// </summary>
    public class ConnectMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ConnectMessage class.
        /// </summary>
        /// <param name="targetServer">The server to connect to.</param>
        public ConnectMessage(string targetServer)
            : base("CONNECT", new string[] { targetServer })
        {
        }

        /// <summary>
        /// Initialises a new instance of the ConnectMessage class.
        /// </summary>
        /// <param name="targetServer">The server to connect to.</param>
        /// <param name="port">The port to connect on.</param>
        public ConnectMessage(string targetServer, string port)
            : base("CONNECT", new string[] { targetServer, port })
        {
        }

        /// <summary>
        /// Initialises a new instance of the ConnectMessage class.
        /// </summary>
        /// <param name="targetServer">The server to connect to.</param>
        /// <param name="port">The port to connect on.</param>
        /// <param name="remoteServer">The remote server.</param>
        public ConnectMessage(string targetServer, string port, string remoteServer)
            : base("CONNECT", new string[] { targetServer, port, remoteServer })
        {
        }

        #endregion
    }
}
