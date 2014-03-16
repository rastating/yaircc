//-----------------------------------------------------------------------
// <copyright file="ConnectionFailedEventArgs.cs" company="rastating">
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