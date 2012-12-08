//-----------------------------------------------------------------------
// <copyright file="ConnectionFailedEventArgs.cs" company="intninety">
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
    /// Represents the event data for failed connection attempts.
    /// </summary>
    public class ConnectionFailedEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// The exception (if any) that occurred.
        /// </summary>
        private Exception exception;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the ConnectionFailedEventArgs class.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        public ConnectionFailedEventArgs(Exception exception)
        {
            this.exception = exception;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the exception (if any) that occurred.
        /// </summary>
        public Exception Exception
        {
            get { return this.exception; }
        }

        #endregion    
    }
}