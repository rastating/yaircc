//-----------------------------------------------------------------------
// <copyright file="MessageParseResult.cs" company="rastating">
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
