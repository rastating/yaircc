//-----------------------------------------------------------------------
// <copyright file="Connection.cs" company="rastating">
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
    using System;
    using System.Collections.Generic;
    using System.Net.Sockets;
    using System.Text;
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a connection to an IRC server.
    /// </summary>
    public class Connection
    {
        #region Fields

        /// <summary>
        /// The server URL / IP address.
        /// </summary>
        private string server;

        /// <summary>
        /// The port number the connection should be established on.
        /// </summary>
        private int port;

        /// <summary>
        /// The password of the IRC server.
        /// </summary>
        private string password;

        /// <summary>
        /// The underlying TCP socket.
        /// </summary>
        private TcpClient client;

        /// <summary>
        /// The encoding to use for incoming data.
        /// </summary>
        private Encoding encoding;

        /// <summary>
        /// The current set of unprocessed data.
        /// </summary>
        private string currentDataSet;

        /// <summary>
        /// A value indicating whether or not the socket is connected.
        /// </summary>
        private bool isConnected;

        /// <summary>
        /// The IRC user name to use.
        /// </summary>
        private string userName;

        /// <summary>
        /// The IRC nick name to use.
        /// </summary>
        private string nickname;

        /// <summary>
        /// The IRC "real name" to use.
        /// </summary>
        private string realName;

        /// <summary>
        /// The mode string to use.
        /// </summary>
        private string mode;

        /// <summary>
        /// A timer used to poll the connection.
        /// </summary>
        private System.Timers.Timer timer;

        /// <summary>
        /// Occurs when the connection is initiated.
        /// </summary>
        private BeganConnectingHandler beganConnecting;

        /// <summary>
        /// Occurs when an attempt to connect to the target fails.
        /// </summary>
        private ConnectionFailedHandler connectionFailed;

        /// <summary>
        /// Occurs when the connection was successfully established.
        /// </summary>
        private ConnectionEstablishedHandler connectionEstablished;

        /// <summary>
        /// Occurs when the connection is terminated.
        /// </summary>
        private ConnectionTerminatedHandler connectionTerminated;

        /// <summary>
        /// Occurs when data is received.
        /// </summary>
        private DataReceivedHandler dataReceived;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Connection class.
        /// </summary>
        /// <param name="server">The server that will be connected to.</param>
        /// <param name="port">The port number on which to connect to.</param>
        /// <param name="password">The password of the server.</param>
        public Connection(string server, int port, string password)
        {
            this.currentDataSet = string.Empty;
            this.server = server;
            this.port = port;
            this.password = password;
            this.encoding = Encoding.UTF8;
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Connection class.
        /// </summary>
        /// <param name="server">The server that will be connected to.</param>
        /// <param name="port">The port number on which to connect to.</param>
        public Connection(string server, int port)
            : this(server, port, string.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Connection class.
        /// </summary>
        /// <param name="server">The server that will be connected to.</param>
        public Connection(string server)
            : this(server, 6667, string.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Connection class.
        /// </summary>
        public Connection()
            : this(string.Empty, 0, string.Empty)
        {
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for event <see cref="BeganConnecting"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void BeganConnectingHandler(object sender, EventArgs e);

        /// <summary>
        /// Delegate for event <see cref="ConnectionFailed"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void ConnectionFailedHandler(object sender, ConnectionFailedEventArgs e);

        /// <summary>
        /// Delegate for event <see cref="ConnectionEstablished"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void ConnectionEstablishedHandler(object sender, EventArgs e);

        /// <summary>
        /// Delegate for event <see cref="DataReceived"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void DataReceivedHandler(object sender, DataReceivedEventArgs e);

        /// <summary>
        /// Delegate for event <see cref="ConnectionTerminated"/>.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        public delegate void ConnectionTerminatedHandler(object sender, EventArgs e);

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the connection is initiated.
        /// </summary>
        public event BeganConnectingHandler BeganConnecting
        {
            add { this.beganConnecting += value; }
            remove { this.beganConnecting -= value; }
        }

        /// <summary>
        /// Occurs when an attempt to connect to the target fails.
        /// </summary>
        public event ConnectionFailedHandler ConnectionFailed
        {
            add { this.connectionFailed += value; }
            remove { this.connectionFailed -= value; }
        }

        /// <summary>
        /// Occurs when the connection was successfully established.
        /// </summary>
        public event ConnectionEstablishedHandler ConnectionEstablished
        {
            add { this.connectionEstablished += value; }
            remove { this.connectionEstablished -= value; }
        }

        /// <summary>
        /// Occurs when the connection is terminated.
        /// </summary>
        public event ConnectionTerminatedHandler ConnectionTerminated
        {
            add { this.connectionTerminated += value; }
            remove { this.connectionTerminated -= value; }
        }

        /// <summary>
        /// Occurs when data is received.
        /// </summary>
        public event DataReceivedHandler DataReceived
        {
            add { this.dataReceived += value; }
            remove { this.dataReceived -= value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the server URL / IP address.
        /// </summary>
        public string Server
        {
            get { return this.server; }
        }

        /// <summary>
        /// Gets the port number the connection should be established on.
        /// </summary>
        public int Port
        {
            get { return this.port; }
        }

        /// <summary>
        /// Gets or sets the IRC user name to use.
        /// </summary>
        public string UserName
        {
            get { return this.userName; }
            set { this.userName = value; }
        }

        /// <summary>
        /// Gets or sets the IRC nick name to use.
        /// </summary>
        public string Nickname
        {
            get { return this.nickname; }
            set { this.nickname = value; }
        }

        /// <summary>
        /// Gets or sets the IRC "real name" to use.
        /// </summary>
        public string RealName
        {
            get { return this.realName; }
            set { this.realName = value; }
        }

        /// <summary>
        /// Gets or sets the mode string to use.
        /// </summary>
        public string Mode
        {
            get { return this.mode; }
            set { this.mode = value; }
        }

        /// <summary>
        /// Gets or sets the password of the IRC server.
        /// </summary>
        public string Password
        {
            get { return this.password; }
            set { this.password = value; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the socket is connected.
        /// </summary>
        public bool IsConnected
        {
            get 
            {
                if (!this.isConnected)
                {
                    return false;
                }
                else
                {
                    bool retval = this.client.Client.IsConnected();

                    if (this.isConnected != retval)
                    {
                        this.IsConnected = retval;
                    }

                    return retval;
                }
            }

            private set
            {
                if (this.isConnected != value)
                {
                    this.isConnected = value;

                    if (value)
                    {
                        this.timer = new System.Timers.Timer(5000);
                        this.timer.Elapsed += new System.Timers.ElapsedEventHandler(this.Timer_Elapsed);
                        this.timer.Enabled = true;
                    }
                    else
                    {
                        if (this.timer != null)
                        {
                            this.timer.Enabled = false;
                            this.timer.Dispose();
                            this.timer = null;
                        }

                        if (this.connectionTerminated != null)
                        {
                            this.connectionTerminated.Invoke(this, EventArgs.Empty);
                        }
                    }
                }
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns>True if the specified object is equal to the current object; otherwise, false.</returns>
        public override bool Equals(object obj)
        {
            if (obj is Connection)
            {
                return this.ToString().Equals((obj as Connection).ToString());
            }
            else
            {
                return base.Equals(obj);
            }
        }

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>A hash code for the current Object.</returns>
        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        /// <summary>
        /// Returns a string that represents the current object.
        /// </summary>
        /// <returns>A string that represents the current object.</returns>
        public override string ToString()
        {
            return string.Format("{0}:{1}", this.server, this.port);
        }

        /// <summary>
        /// Parses <paramref name="command"/> and returns a <see cref="ParseResult"/> object to indicate the result of the operation
        /// </summary>
        /// <param name="command">The command issued by the user to parse</param>
        /// <returns><see cref="ParseResult"/></returns>
        public ParseResult Parse(string command)
        {
            ParseResult retval;

            string server = null;
            int port = 6667;
            string portString = null;
            string password = null;

            Regex regex = new Regex(@"/(server|connect)(\s+)(?<server>[a-zA-Z\.0-9\-]+)(((\s+)(?<port>[0-9]+))?((\s+)(?<password>.*))?)?", RegexOptions.IgnoreCase);
            MatchCollection matches = regex.Matches(command);

            if (matches.Count > 0)
            {
                server = matches[0].Groups["server"].Value;
                portString = matches[0].Groups["port"].Value;
                password = matches[0].Groups["password"].Value.RemoveCarriageReturns();
            }

            if (string.IsNullOrEmpty(server))
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Server);
            }
            else if ((!string.IsNullOrEmpty(portString)) && (!int.TryParse(portString, out port)))
            {
                retval = new ParseResult(false, Strings_MessageParseResults.Server_InvalidPort);
            }
            else
            {
                this.server = server;
                this.port = port;

                if (!string.IsNullOrEmpty(password))
                {
                    this.password = password;
                }

                retval = new ParseResult(true, string.Empty);
            }

            return retval;
        }

        /// <summary>
        /// Sends the registration commands needed when initialising an IRC connection.
        /// </summary>
        /// <param name="password">The password of the server.</param>
        /// <param name="username">The user name to connect with.</param>
        /// <param name="nickname">The nick name to connect with.</param>
        /// <param name="realName">The real name to connect with.</param>
        public void RegisterClient(string password, string username, string nickname, string realName)
        {
            PasswordMessage passMessage = new PasswordMessage(password);
            NickMessage nickMessage = new NickMessage(nickname);
            UserMessage userMessage = new UserMessage(username, realName);

            this.Send(passMessage.ToString());
            this.Send(nickMessage.ToString());
            this.Send(userMessage.ToString());
        }

        /// <summary>
        /// Sends the registration commands needed when initialising an IRC connection.
        /// </summary>
        public void RegisterClient()
        {
            this.RegisterClient(this.Password, this.UserName, this.Nickname, this.RealName);
        }

        /// <summary>
        /// Send a string of data to the server
        /// </summary>
        /// <param name="data">The data to send</param>
        public void Send(string data)
        {
            byte[] bytes = this.encoding.GetBytes(data);
            this.Send(bytes);
        }

        /// <summary>
        /// Closes the socket used to interact with the server.
        /// </summary>
        public void Close()
        {
            this.client.Close();
        }

        /// <summary>
        /// Begin establishing the connection asynchronously.
        /// </summary>
        public void Connect()
        {
            this.client = new TcpClient();
            this.client.BeginConnect(this.server, this.port, this.ConnectCallback, null);

            if (this.beganConnecting != null)
            {
                this.beganConnecting(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Handles the connection request callback of the under lying socket.
        /// </summary>
        /// <param name="result">Represents the status of the asynchronous operation.</param>
        private void ConnectCallback(IAsyncResult result)
        {
            try
            {
                this.client.EndConnect(result);
            }
            catch (Exception ex)
            {
                this.IsConnected = false;
                if (this.connectionFailed != null)
                {
                    this.connectionFailed(this, new ConnectionFailedEventArgs(ex));
                    return;
                }
            }

            NetworkStream stream = this.client.GetStream();
            byte[] buffer = new byte[this.client.ReceiveBufferSize];

            this.IsConnected = true;
            if (this.connectionEstablished != null)
            {
                this.connectionEstablished(this, EventArgs.Empty);
            }

            stream.BeginRead(buffer, 0, buffer.Length, this.ReceiveCallback, buffer);
        }

        /// <summary>
        /// Handles the outgoing data callback of the under lying socket.
        /// </summary>
        /// <param name="result">Represents the status of the asynchronous operation.</param>
        private void SendCallback(IAsyncResult result)
        {
            try
            {
                NetworkStream stream = this.client.GetStream();

                // If the network stream is closed an ObjectDisposedException will be thrown
                // in the thread that EndWrite executes on, causing the global exception handle
                // to be invoked and result in yaircc crashing, so we must first check to see
                // if we explicitly can write to the network stream.
                if (stream.CanWrite)
                {
                    stream.EndWrite(result);
                }
            }
            catch (ObjectDisposedException)
            {
            }
            catch (InvalidOperationException)
            {
            }
        }

        /// <summary>
        /// Handles the incoming data callback of the under lying socket.
        /// </summary>
        /// <param name="result">Represents the status of the asynchronous operation.</param>
        private void ReceiveCallback(IAsyncResult result)
        {
            int bytesRead;
            NetworkStream stream;

            try
            {
                stream = this.client.GetStream();
                bytesRead = stream.EndRead(result);
            }
            catch
            {
                return;
            }

            // EndRead() blocks until data is received, so if we have nothing
            // then the connection has been terminated
            if (bytesRead == 0)
            {
                this.IsConnected = false;
                if (this.connectionTerminated != null)
                {
                    this.connectionTerminated(this, EventArgs.Empty);
                }

                return;
            }

            // Get the data, raise the received event and begin listening again
            byte[] buffer = result.AsyncState as byte[];
            string data = this.encoding.GetString(buffer, 0, bytesRead);
            this.currentDataSet = this.currentDataSet + data;

            // If we have received a full payload from the server then handle any pings and despatch to the subscribers
            while (this.currentDataSet.Contains("\n"))
            {
                int delimiterIndex = this.currentDataSet.IndexOf('\n') + 1;
                string payload = this.currentDataSet.Substring(0, delimiterIndex);

                if (payload.StartsWith("PING", StringComparison.OrdinalIgnoreCase))
                {
                    this.ReplyToPing(payload);
                }

                if (this.dataReceived != null)
                {
                    if (!payload.StartsWith("PING", StringComparison.OrdinalIgnoreCase))
                    {
                        this.dataReceived(this, new DataReceivedEventArgs(payload));
                    }
                }

                // If two messages were joined together, then remove the one we just processed and iterate
                // otherwise empty the current data set and begin listening again
                if (delimiterIndex < this.currentDataSet.Length)
                {
                    this.currentDataSet = this.currentDataSet.Substring(delimiterIndex);
                }
                else
                {
                    this.currentDataSet = string.Empty;
                }
            }

            stream.BeginRead(buffer, 0, buffer.Length, this.ReceiveCallback, buffer);
        }

        /// <summary>
        /// Handles the Elapsed event of System.Timers.Timer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            bool connected = this.client.Client.IsConnected();
            if (connected != this.isConnected)
            {
                this.IsConnected = connected;
            }
        }

        /// <summary>
        /// Send a reply to a ping command.
        /// </summary>
        /// <param name="rawData">The raw ping command.</param>
        private void ReplyToPing(string rawData)
        {
            PongMessage pongMessage = new PongMessage(string.Empty);
            if (pongMessage.TryParse(rawData).Success)
            {
                this.Send(pongMessage.ToString());
            }
        }

        /// <summary>
        /// Send a string of data to the server
        /// </summary>
        /// <param name="data">The data to send</param>
        private void Send(byte[] data)
        {
            NetworkStream stream = this.client.GetStream();
            stream.BeginWrite(data, 0, data.Length, this.SendCallback, null);
        }

        #endregion
    }
}
