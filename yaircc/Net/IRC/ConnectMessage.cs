//-----------------------------------------------------------------------
// <copyright file="ConnectMessage.cs" company="intninety">
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
