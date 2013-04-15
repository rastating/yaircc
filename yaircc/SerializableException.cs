//-----------------------------------------------------------------------
// <copyright file="SerializableException.cs" company="rastating">
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

namespace Yaircc
{
    using System;

    /// <summary>
    /// Represents an exception that can be serialised.
    /// </summary>
    [Serializable]
    public class SerializableException
    {
        #region Fields

        /// <summary>
        /// The inner exception.
        /// </summary>
        private SerializableException innerException;

        /// <summary>
        /// The message for the exception.
        /// </summary>
        private string message;

        /// <summary>
        /// The stack trace for the exception.
        /// </summary>
        private string stackTrace;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="SerializableException"/> class.
        /// </summary>
        /// <param name="ex">The exception that occurred.</param>
        public SerializableException(Exception ex)
        {
            this.stackTrace = ex.StackTrace;
            this.message = ex.Message;
            if (ex.InnerException != null)
            {
                this.innerException = new SerializableException(ex.InnerException);
            }
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SerializableException"/> class.
        /// </summary>
        public SerializableException()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the inner exception.
        /// </summary>
        public SerializableException InnerException
        {
            get { return this.innerException; }
            set { this.innerException = value; }
        }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        public string Message
        {
            get { return this.message; }
            set { this.message = value; }
        }

        /// <summary>
        /// Gets or sets the stack trace.
        /// </summary>
        public string StackTrace
        {
            get { return this.stackTrace; }
            set { this.stackTrace = value; }
        }

        #endregion
    }
}
