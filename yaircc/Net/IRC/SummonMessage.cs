//-----------------------------------------------------------------------
// <copyright file="SummonMessage.cs" company="intninety">
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
    /// <summary>
    /// Represents a SUMMON command.
    /// </summary>
    public class SummonMessage : Message
    {
        /// <summary>
        /// Initialises a new instance of the <see cref="SummonMessage"/> class.
        /// </summary>
        /// <param name="user">The user to summon.</param>
        public SummonMessage(string user)
            : base("SUMMON", new string[] { user })
        {
        }
        
        /// <summary>
        /// Initialises a new instance of the <see cref="SummonMessage"/> class.
        /// </summary>
        /// <param name="user">The user to summon.</param>
        /// <param name="target">The server in which the user is on.</param>
        public SummonMessage(string user, string target)
            : base("SUMMON", new string[] { user, target })
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="SummonMessage"/> class.
        /// </summary>
        /// <param name="user">The user to summon.</param>
        /// <param name="target">The server in which the user is on.</param>
        /// <param name="channel">The channel in which to summon the user to.</param>
        public SummonMessage(string user, string target, string channel)
            : base("SUMMON", new string[] { user, target, channel })
        {
        }
    }
}
