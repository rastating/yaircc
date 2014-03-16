//-----------------------------------------------------------------------
// <copyright file="ParseResult.cs" company="rastating">
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
    /// Represents the result of a parsing operation.
    /// </summary>
    public class ParseResult
    {
        #region Fields

        /// <summary>
        /// A value indicating whether or not the operation was successful.
        /// </summary>
        private bool success;

        /// <summary>
        /// A summary of the operation.
        /// </summary>
        private string message;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ParseResult"/> class.
        /// </summary>
        /// <param name="success">A value indicating whether or not the operation was successful.</param>
        /// <param name="message">A summary of the operation.</param>
        public ParseResult(bool success, string message)
        {
            this.success = success;
            this.message = message;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether or not the parse operation succeeded
        /// </summary>
        public bool Success
        {
            get { return this.success; }
            protected set { this.success = value; }
        }

        /// <summary>
        /// Gets or sets a message containing the (if any) details about the parse operation
        /// </summary>
        public string Message
        {
            get { return this.message; }
            protected set { this.message = value; }
        }

        #endregion
    }
}
