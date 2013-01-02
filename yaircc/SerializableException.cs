//-----------------------------------------------------------------------
// <copyright file="SerializableException.cs" company="intninety">
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
