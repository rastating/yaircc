//-----------------------------------------------------------------------
// <copyright file="Message.cs" company="intninety">
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
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// Represents a message received from an IRC server.
    /// </summary>
    public class Message : IAssimilable
    {
        #region Fields

        /// <summary>
        /// The prefix of the message; typically the source.
        /// </summary>
        private string prefix;

        /// <summary>
        /// The command the message represents.
        /// </summary>
        private string command;

        /// <summary>
        /// The parameters of the command.
        /// </summary>
        private string[] parameters;

        /// <summary>
        /// The type of prefix present.
        /// </summary>
        private PrefixType prefixType;

        /// <summary>
        /// The trailing parameter of the command, if any.
        /// </summary>
        private string trailingParameter;

        /// <summary>
        /// The connection that the message was received on.
        /// </summary>
        private Connection associatedConnection;

        /// <summary>
        /// The name of the channel the message was received on.
        /// </summary>
        private string channelName;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Message class.
        /// </summary>
        /// <param name="prefix">The prefix of the message.</param>
        /// <param name="command">The command the message represents.</param>
        /// <param name="parameters">The parameters of the command.</param>
        /// <param name="trailingParameter">The trailing parameter of the command.</param>
        public Message(string prefix, string command, string[] parameters, string trailingParameter)
        {
            this.Prefix = prefix;
            this.Command = command;
            this.Parameters = parameters;
            this.TrailingParameter = trailingParameter;
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Message class.
        /// </summary>
        /// <param name="command">The command the message represents.</param>
        /// <param name="parameters">The parameters of the command.</param>
        public Message(string command, string[] parameters)
            : this(string.Empty, command, parameters, string.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Message class.
        /// </summary>
        /// <param name="command">The command the message represents.</param>
        public Message(string command)
            : this(string.Empty, command, new string[0], string.Empty)
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.Message class.
        /// </summary>
        public Message()
            : this(string.Empty, string.Empty, new string[0], string.Empty)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the connection that the message was received on.
        /// </summary>
        public Connection AssociatedConnection
        {
            get { return this.associatedConnection; }
            set { this.associatedConnection = value; }
        }

        /// <summary>
        /// Gets the source of the message.
        /// </summary>
        public virtual string Source
        {
            get
            {
                if (string.IsNullOrEmpty(this.Prefix))
                {
                    return string.Empty;
                }
                else
                {
                    if (this.PrefixType == PrefixType.ServerName)
                    {
                        return this.Prefix;
                    }
                    else
                    {
                        if ((this.PrefixType & PrefixType.UserExtension) != 0)
                        {
                            return this.Prefix.Substring(0, this.Prefix.IndexOf("!"));
                        }
                        else if ((this.PrefixType & PrefixType.HostExtension) != 0)
                        {
                            return this.Prefix.Substring(0, this.Prefix.IndexOf("@"));
                        }
                        else
                        {
                            return this.Prefix;
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public virtual string Content
        {
            get 
            {
                return this.ToString("{0}", " ", true); 
            }
        }

        /// <summary>
        /// Gets the recipient the message was to be processed by. This can be either a channel name or a nick name.
        /// </summary>
        public virtual string Target
        {
            get { return string.Empty; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the message should be printed to screen.
        /// </summary>
        public virtual bool ShouldPrint
        {
            get { return true; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the message must be handled by its target.
        /// </summary>
        public virtual bool MustBeHandledByTarget
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the message should be handled by all active channels.
        /// </summary>
        public virtual bool IsMultiChannelMessage
        {
            get { return false; }
        }

        /// <summary>
        /// Gets a value indicating whether the message should be handled by all channels and the server window itself.
        /// </summary>
        public virtual bool IsGlobalMessage
        {
            get { return false; }
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        public virtual MessageType Type
        {
            get { return MessageType.ServerMessage; }
        }

        /// <summary>
        /// Gets or sets the prefix of the message.
        /// </summary>
        public string Prefix
        {
            get 
            { 
                return this.prefix; 
            }

            set 
            { 
                this.prefix = value;

                if (string.IsNullOrEmpty(value))
                {
                    this.prefixType = PrefixType.None;
                }
                else
                {
                    if (value.Contains("!") && value.Contains("@"))
                    {
                        this.prefixType = PrefixType.NickName | PrefixType.UserExtension | PrefixType.HostExtension;
                    }
                    else if (value.Contains("@"))
                    {
                        this.prefixType = PrefixType.NickName | PrefixType.HostExtension;
                    }
                    else if (value.Contains("!"))
                    {
                        this.prefixType = PrefixType.NickName | PrefixType.UserExtension;
                    }
                    else
                    {
                        this.prefixType = PrefixType.ServerName;
                    }
                }
            }
        }

        /// <summary>
        /// Gets the type of the prefix.
        /// </summary>
        public PrefixType PrefixType
        {
            get { return this.prefixType; }
        }

        /// <summary>
        /// Gets or sets the command the message represents.
        /// </summary>
        public string Command
        {
            get { return this.command; }
            set { this.command = value; }
        }

        /// <summary>
        /// Gets or sets the parameters of the command.
        /// </summary>
        public string[] Parameters
        {
            get { return this.parameters; }
            set { this.parameters = value; }
        }

        /// <summary>
        /// Gets or sets the trailing parameter of the command.
        /// </summary>
        public string TrailingParameter
        {
            get { return this.trailingParameter; }
            set { this.trailingParameter = value; }
        }

        /// <summary>
        /// Gets or sets the name of the channel that the message was received on.
        /// </summary>
        public string ChannelName
        {
            get { return this.channelName; }
            set { this.channelName = value; }
        }

        #endregion

        #region Static Methods

        /// <summary>
        /// Parses a text based message into a Yaircc.Net.IRC.Message.
        /// </summary>
        /// <param name="message">A string that meets the RFC 1459 or 2812 standard.</param>
        /// <returns>A new instance of Yaircc.Net.IRC.Message encapsulating the specified message.</returns>
        public static Message ParseMessage(string message)
        {
            Message retval = new Message();

            // The format of the message must meet the RFC 1459 standard
            // that being: [':' <prefix> <SPACE> ] <command> <params> <crlf>

            // -- Breakdown of Message Sections --
            // <prefix>     :=  <servername> | <nick> [ '!' <user> ] [ '@' <host> ]
            // <command>    :=  <letter> { <letter> } | <number> <number> <number>
            // <params>     :=  <SPACE> [ ':' <trailing> | <middle> <params> ]
            // <middle>     :=  <Any *non-empty* sequence of octets not including SPACE or NUL or CR or LF, the first of which may not be ':'>
            // <trailing>   :=  <Any, possibly *empty*, sequence of octets not including NUL or CR or LF>
            try
            {
                int prefixEnd = -1;
                int trailingStart = message.Length;

                string prefix = null;
                string command = null;
                string[] parameters = null;
                string trailingParameter = null;

                // Get the prefix
                if (message.StartsWith(":"))
                {
                    prefixEnd = message.IndexOf(' ');
                    prefix = message.Substring(1, prefixEnd - 1);
                }

                // Grab the trailing parameter
                trailingStart = message.IndexOf(" :");
                if (trailingStart >= 0)
                {
                    trailingParameter = message.Substring(trailingStart + 2);
                }
                else
                {
                    trailingStart = message.Length;
                }

                // Use the prefix end position and trailing parameter start position to get the command and remaining parameters
                string[] commandAndParameters = message.Substring(prefixEnd + 1, trailingStart - prefixEnd - 1).Split(' ');
                command = commandAndParameters[0];

                // If there were parameters add them all in
                if (commandAndParameters.Length > 1)
                {
                    // Loop through the standard parameters and add them to the array
                    parameters = new string[commandAndParameters.Length - 1];
                    for (int i = 0; i < (commandAndParameters.Length - 1); i++)
                    {
                        parameters[i] = commandAndParameters[i + 1].Trim(new char[] { ' ', '\r', '\n' });
                    }
                }

                if (!string.IsNullOrEmpty(prefix))
                {
                    retval.Prefix = prefix;
                }

                if (!string.IsNullOrEmpty(command))
                {
                    retval.Command = command;
                }

                if (parameters != null)
                {
                    retval.Parameters = parameters;
                }

                if (!string.IsNullOrEmpty(trailingParameter))
                {
                    retval.TrailingParameter = trailingParameter.Trim(new char[] { '\r', '\n' });
                }
            }
            catch
            {
            }

            return retval;
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Attempts to parse the input specified by the user into the Message.
        /// </summary>
        /// <param name="input">The input from the user.</param>
        /// <returns>True on success, false on failure.</returns>
        public virtual ParseResult TryParse(string input)
        {
            // If we are dealing with a command we don't know of, then strip the /
            // preceeding the command, and throw it to ParseMessage and assimilate it
            // and we'll hope for the best (hey, what could go wrong, right?)
            Message message = Message.ParseMessage(input.Remove(0, 1));
            this.Assimilate(message);

            return new ParseResult(true, string.Empty);
        }

        /// <summary>
        /// Outputs the message as a string.
        /// </summary>
        /// <returns>The message as will be received / sent by the server.</returns>
        public override string ToString()
        {
            // Start the capacity at 2 as the message must end with \r\n
            int estimatedCapacity = 2;

            if (!string.IsNullOrEmpty(this.Prefix))
            {
                estimatedCapacity += this.Prefix.Length + 2;
            }

            estimatedCapacity += this.Command.Length + 1;

            for (int i = 0; i < this.Parameters.Length; i++)
            {
                if (!string.IsNullOrEmpty(this.Parameters[i]))
                {
                    estimatedCapacity += this.Parameters[i].Length + 1;
                }
            }

            if (!string.IsNullOrEmpty(this.TrailingParameter))
            {
                estimatedCapacity += this.TrailingParameter.Length + 2;
            }

            StringBuilder sb = new StringBuilder(estimatedCapacity);

            if (!string.IsNullOrEmpty(this.Prefix))
            {
                sb.Append(":");
                sb.Append(this.Prefix);
                sb.Append(" ");
            }
         
            sb.Append(this.Command);

            if (this.Parameters.Length > 0)
            {
                for (int i = 0; i < this.Parameters.Length; i++)
                {
                    if (!string.IsNullOrEmpty(this.Parameters[i]))
                    {
                        sb.Append(" ");
                        sb.Append(this.Parameters[i]);
                    }
                }
            }

            if (!string.IsNullOrEmpty(this.TrailingParameter))
            {
                sb.Append(" :");
                sb.Append(this.TrailingParameter);
            }

            sb.Append("\r\n");

            return sb.ToString();            
        }

        /// <summary>
        /// Returns the message using the <paramref name="format"/> specified.
        /// </summary>
        /// <param name="format">The format to use. 
        /// <para>{0} will be replaced with all parameters separated by a <paramref name="parameterDelimiter"/>.</para>
        /// <para>{1} will be replaced with the trailing parameter.</para>
        /// <para>{2} will be replaced with the prefix.</para>
        /// <para>All others parameters are sequentially accessed e.g. the first parameter would be {3}.</para></param>
        /// <param name="parameterDelimiter">The delimiter that will separate the parameter list.</param>
        /// <param name="removeFirstParameter">A value that indicates whether or not to remove the first parameter from the input.</param>
        /// <returns>The message in the specified format.</returns>
        public string ToString(string format, string parameterDelimiter, bool removeFirstParameter)
        {
            List<string> parameterList = new List<string>();
            List<string> parameters = new List<string>(this.parameters);
            string concatenatedParameters = string.Empty;
            
            if (this.parameters.Length > 0)
            {
                if (removeFirstParameter)
                {
                    parameters.RemoveAt(0);
                }

                if (!string.IsNullOrEmpty(this.trailingParameter))
                {
                    parameters.Add(this.trailingParameter);
                }

                concatenatedParameters = string.Join(parameterDelimiter, parameters.ToArray());
            }

            parameterList.Add(concatenatedParameters);
            parameterList.Add(this.trailingParameter);
            parameterList.Add(this.prefix);
            parameterList.AddRange(parameters);

            try
            {
                return string.Format(format, parameterList.ToArray());
            }
            catch (Exception)
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Returns the message using the <paramref name="format"/> specified.
        /// </summary>
        /// <param name="format">The format to use. 
        /// <para>{0} will be replaced with all parameters separated by a <paramref name="parameterDelimiter"/>.</para>
        /// <para>{1} will be replaced with the trailing parameter.</para>
        /// <para>{2} will be replaced with the prefix.</para>
        /// <para>All others parameters are sequentially accessed e.g. the first parameter would be {3}</para></param>
        /// <returns>The message in the specified format.</returns>
        public string ToString(string format)
        {
            return this.ToString(format, ", ", true);
        }

        #endregion

        #region IAssimilable Members

        /// <summary>
        /// Assimilate all the data stored in <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object in which to assimilate data from</param>
        public void Assimilate(object obj)
        {
            if (obj is Message)
            {
                Message message = obj as Message;
                this.Command = message.Command;
                this.Parameters = message.Parameters;
                this.Prefix = message.Prefix;
                this.TrailingParameter = message.TrailingParameter;
                this.ChannelName = message.ChannelName;
                this.AssociatedConnection = message.AssociatedConnection;
            }
        }

        #endregion
    }
}