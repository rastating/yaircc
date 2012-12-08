//-----------------------------------------------------------------------
// <copyright file="DataReceivedEventArgs.cs" company="intninety">
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
    /// Represents the event data when data is received via an IRC connection.
    /// </summary>
    public class DataReceivedEventArgs : EventArgs
    {
        #region Fields

        /// <summary>
        /// The data received.
        /// </summary>
        private string data;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="DataReceivedEventArgs"/> class.
        /// </summary>
        /// <param name="data">The received data.</param>
        public DataReceivedEventArgs(string data)
        {
            this.data = data;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the data received.
        /// </summary>
        public string Data
        {
            get { return this.data; }
        }

        #endregion
    }
}