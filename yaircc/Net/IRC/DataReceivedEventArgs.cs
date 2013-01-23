//-----------------------------------------------------------------------
// <copyright file="DataReceivedEventArgs.cs" company="intninety">
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