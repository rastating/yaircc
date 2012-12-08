//-----------------------------------------------------------------------
// <copyright file="ErrorLog.cs" company="intninety">
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

namespace Yaircc
{
    using System;
    using System.Windows.Forms;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents logs of errors that occur during application execution.
    /// </summary>
    [Serializable]
    public class ErrorLog
    {
        #region Fields

        /// <summary>
        /// The exception that triggered the log to be created.
        /// </summary>
        private SerializableException exception;

        /// <summary>
        /// The current platform identifier and version number.
        /// </summary>
        private string osVersion;

        /// <summary>
        /// The date and time that the error log was created.
        /// </summary>
        private DateTime creationDateTime;

        /// <summary>
        /// The version number of the application that created the error log.
        /// </summary>
        private string applicationVersion;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the Yaircc.ErrorLog class.
        /// </summary>
        /// <param name="exception">The exception that triggered the log to be created.</param>
        public ErrorLog(Exception exception)
        {
            this.exception = new SerializableException(exception);
            this.osVersion = Environment.OSVersion.ToString();
            this.applicationVersion = Application.ProductVersion;
            this.creationDateTime = DateTime.Now;
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.ErrorLog class.
        /// </summary>
        public ErrorLog()
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the exception that triggered the log to be created.
        /// </summary>
        public SerializableException Exception
        {
            get { return this.exception; }
            set { this.exception = value; }
        }

        /// <summary>
        /// Gets or sets the current platform identifier and version number.
        /// </summary>
        public string OSVersion
        {
            get { return this.osVersion; }
            set { this.osVersion = value; }
        }

        #endregion
    }
}
