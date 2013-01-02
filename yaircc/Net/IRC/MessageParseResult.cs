//-----------------------------------------------------------------------
// <copyright file="MessageParseResult.cs" company="intninety">
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
    /// <summary>
    /// Represents the result of attempting to parse an IRC message.
    /// </summary>
    internal class MessageParseResult : ParseResult, IAssimilable
    {
        #region Fields

        /// <summary>
        /// The processed message.
        /// </summary>
        private Message ircMessage;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MessageParseResult"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether or not the operation was success.</param>
        /// <param name="message">A summary of the operation.</param>
        /// <param name="ircMessage">The processed message.</param>
        public MessageParseResult(bool success, string message, Message ircMessage)
            : base(success, message)
        {
            this.ircMessage = ircMessage;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the processed message.
        /// </summary>
        public Message IRCMessage
        {
            get { return this.ircMessage; }
        }

        #endregion

        #region IAssimilable Members

        /// <summary>
        /// Assimilate all the data stored in <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object in which to assimilate data from</param>
        public void Assimilate(object obj)
        {
            if (obj is ParseResult)
            {
                ParseResult result = obj as ParseResult;
                this.Success = result.Success;
                this.Message = result.Message;
            }
        }

        #endregion
    }
}
