//-----------------------------------------------------------------------
// <copyright file="ServListMessage.cs" company="intninety">
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
    /// Represents a SERVLIST command.
    /// </summary>
    public class ServListMessage : Message
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="ServListMessage"/> class.
        /// </summary>
        public ServListMessage()
            : base("SERVLIST")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ServListMessage"/> class.
        /// </summary>
        /// <param name="mask">The mask to use.</param>
        public ServListMessage(string mask)
            : base("SERVLIST", new string[] { mask })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ServListMessage"/> class.
        /// </summary>
        /// <param name="mask">The mask to use.</param>
        /// <param name="type">The type of service to list.</param>
        public ServListMessage(string mask, string type)
            : base("SERVLIST", new string[] { mask, type })
        {
        }
    }
}
