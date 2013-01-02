//-----------------------------------------------------------------------
// <copyright file="AdminMessage.cs" company="intninety">
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
    /// Represents an ADMIN command.
    /// </summary>
    public class AdminMessage : Message
    {
        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.AdminMessage class.
        /// </summary>
        public AdminMessage()
            : base("ADMIN")
        {
        }

        /// <summary>
        /// Initialises a new instance of the Yaircc.Net.IRC.AdminMessage class.
        /// </summary>
        /// <param name="server">The server to return information about.</param>
        public AdminMessage(string server)
            : base("ADMIN", new string[] { server })
        {
        }
    }
}
