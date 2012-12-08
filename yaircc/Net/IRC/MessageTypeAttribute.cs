//-----------------------------------------------------------------------
// <copyright file="MessageTypeAttribute.cs" company="intninety">
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
    using System;

    /// <summary>
    /// Represents the attributes of an IRC command.
    /// </summary>
    [System.AttributeUsage(AttributeTargets.Enum | AttributeTargets.Class | AttributeTargets.Field)]
    public class MessageTypeAttribute : Attribute
    {
        #region Fields

        /// <summary>
        /// The message type.
        /// </summary>
        private MessageType messageType;

        /// <summary>
        /// The format string to use when outputting the message to screen.
        /// </summary>
        private string outputFormat;

        /// <summary>
        /// The delimiter to separate the parameters by.
        /// </summary>
        private string parameterDelimiter;

        /// <summary>
        /// The source of the message.
        /// </summary>
        private string source;

        /// <summary>
        /// The delegate (if any) to be invoked when outputting the message to screen..
        /// </summary>
        private ReplyDelegates.ReplyDelegate outputAction;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MessageTypeAttribute"/> class.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        /// <param name="delegateType">The delegate type.</param>
        /// <param name="delegateName">The name of the delegate.</param>
        public MessageTypeAttribute(MessageType messageType, Type delegateType, string delegateName)
        {
            this.messageType = messageType;
            this.outputAction = (ReplyDelegates.ReplyDelegate)Delegate.CreateDelegate(typeof(ReplyDelegates.ReplyDelegate), delegateType.GetMethod(delegateName));
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MessageTypeAttribute"/> class.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        /// <param name="outputFormat">The format to be used when outputting the message to screen
        /// <para>{0} will be replaced with all parameters (excluding the trailing parameter) separated by a <paramref name="parameterDelimiter"/>.</para>
        /// <para>{1} will be replaced with the trailing parameter.</para>
        /// <para>{2} will be replaced with the prefix.</para>
        /// <para>All others parameters are sequentially accessed e.g. the first parameter would be {3}</para></param>
        /// <param name="parameterDelimiter">The delimiter to use when outputting all parameters.</param>
        public MessageTypeAttribute(MessageType messageType, string outputFormat, string parameterDelimiter)
        {
            this.messageType = messageType;
            this.outputFormat = outputFormat;
            this.parameterDelimiter = parameterDelimiter;
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MessageTypeAttribute"/> class.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        /// <param name="outputFormat">The format to be used when outputting the message to screen
        /// <para>{0} will be replaced with all parameters (excluding the trailing parameter) separated by a <paramref name="parameterDelimiter"/>.</para>
        /// <para>{1} will be replaced with the trailing parameter.</para>
        /// <para>{2} will be replaced with the prefix.</para>
        /// <para>All others parameters are sequentially accessed e.g. the first parameter would be {3}</para></param>
        public MessageTypeAttribute(MessageType messageType, string outputFormat)
            : this(messageType, outputFormat, " ") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MessageTypeAttribute"/> class.
        /// </summary>
        /// <param name="messageType">The message type.</param>
        public MessageTypeAttribute(MessageType messageType)
            : this(messageType, "{0}", " ") 
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the source of the message.
        /// </summary>
        public string Source
        {
            get { return this.source; }
            set { this.source = value; }
        }

        /// <summary>
        /// Gets the type of the message.
        /// </summary>
        public MessageType MessageType
        {
            get { return this.messageType; }
        }

        /// <summary>
        /// Gets the format to be used when outputting the message to screen.
        /// </summary>
        public string OutputFormat
        {
            get { return this.outputFormat; }
        }

        /// <summary>
        /// Gets a value indicating whether or not to remove the first parameter when outputting to screen.
        /// </summary>
        public bool RemoveFirstParameter
        {
            get { return true; }
        }

        /// <summary>
        /// Gets the parameter delimiter.
        /// </summary>
        public string ParameterDelimiter
        {
            get { return this.parameterDelimiter; }
        }

        /// <summary>
        /// Gets the delegate to be invoked when outputting the message to screen.
        /// </summary>
        public ReplyDelegates.ReplyDelegate OutputAction
        {
            get { return this.outputAction; }
        }

        #endregion
    }
}