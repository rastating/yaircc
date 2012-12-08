//-----------------------------------------------------------------------
// <copyright file="ParseResult.cs" company="intninety">
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
