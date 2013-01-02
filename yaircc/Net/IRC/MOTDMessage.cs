//-----------------------------------------------------------------------
// <copyright file="MOTDMessage.cs" company="intninety">
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

namespace Yaircc.Net.IRC
{
    /// <summary>
    /// Represents a MOTD command.
    /// </summary>
    public class MOTDMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="MOTDMessage"/> class.
        /// </summary>
        public MOTDMessage()
            : base("MOTD")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="MOTDMessage"/> class.
        /// </summary>
        /// <param name="server">The server to retrieve the MOTD for.</param>
        public MOTDMessage(string server)
            : base("MOTD", new string[] { server })
        {
        }

        #endregion
    }
}
