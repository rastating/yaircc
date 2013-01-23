//-----------------------------------------------------------------------
// <copyright file="ErrorLog.cs" company="intninety">
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
