//-----------------------------------------------------------------------
// <copyright file="Server.cs" company="intninety">
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

namespace Yaircc.Settings
{
    using System;
    using System.Collections.Generic;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents an IRC server.
    /// </summary>
    [Serializable]
    public class Server
    {
        #region Fields

        /// <summary>
        /// The alias to identify the server by.
        /// </summary>
        private string alias;

        /// <summary>
        /// The IP address or FQDN of the server.
        /// </summary>
        private string address;

        /// <summary>
        /// The port number the service is running on.
        /// </summary>
        private int port;

        /// <summary>
        /// A value indicating whether to automatically connect to the server on start-up.
        /// </summary>
        private bool automaticallyConnect;

        /// <summary>
        /// The preferred nick name to use when connecting to the server.
        /// </summary>
        private string nickName;

        /// <summary>
        /// The preferred user name to use when connecting to the server.
        /// </summary>
        private string userName;

        /// <summary>
        /// The real name to use when connecting to the server.
        /// </summary>
        private string realName;

        /// <summary>
        /// The default mode string to use when connecting to the server.
        /// </summary>
        private string mode;

        /// <summary>
        /// A list of commands that will be executed upon connecting to the server.
        /// </summary>
        private List<string> commands;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Server" /> class.
        /// </summary>
        public Server()
        {
            this.alias = Strings_FavouriteServers.NewServer;
            this.address = string.Empty;
            this.port = 6667;
            this.automaticallyConnect = false;
            this.nickName = GlobalSettings.Instance.NickName;
            this.realName = GlobalSettings.Instance.RealName;
            this.userName = GlobalSettings.Instance.UserName;
            this.mode = GlobalSettings.Instance.Mode;
            this.commands = new List<string>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the IP address or FQDN of the server.
        /// </summary>
        public string Address
        {
            get { return this.address; }
            set { this.address = value; }
        }

        /// <summary>
        /// Gets or sets the alias to identify the server by.
        /// </summary>
        public string Alias
        {
            get { return this.alias; }
            set { this.alias = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to automatically connect to the server on start-up.
        /// </summary>
        public bool AutomaticallyConnect
        {
            get { return this.automaticallyConnect; }
            set { this.automaticallyConnect = value; }
        }

        /// <summary>
        /// Gets or sets the port number the service is running on.
        /// </summary>
        public int Port
        {
            get { return this.port; }
            set { this.port = value; }
        }

        /// <summary>
        /// Gets or sets the preferred nick name to use when connecting to the server.
        /// </summary>
        public string NickName
        {
            get { return this.nickName; }
            set { this.nickName = value; }
        }

        /// <summary>
        /// Gets or sets the preferred user name to use when connecting to the server.
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        /// <summary>
        /// Gets or sets the real name to use when connecting to the server.
        /// </summary>
        public string RealName
        {
            get { return this.realName; }
            set { this.realName = value; }
        }

        /// <summary>
        /// Gets or sets the default mode string to use when connecting to the server.
        /// </summary>
        public string Mode
        {
            get { return this.mode; }
            set { this.mode = value; }
        }

        /// <summary>
        /// Gets or sets a list of commands that will be executed upon connecting to the server.
        /// </summary>
        public List<string> Commands
        {
            get { return this.commands; }
            set { this.commands = value; }
        }

        #endregion
    }
}