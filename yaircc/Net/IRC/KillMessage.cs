//-----------------------------------------------------------------------
// <copyright file="KillMessage.cs" company="intninety">
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
    using System;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a KILL command.
    /// </summary>
    public class KillMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="KillMessage"/> class.
        /// </summary>
        /// <param name="nickname">The nick name of the user to remove from the network.</param>
        /// <param name="comment">The reason behind removing the user.</param>
        public KillMessage(string nickname, string comment)
            : base(string.Empty, "KILL", new string[] { nickname }, comment)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                if (this.Parameters[0].Equals(this.AssociatedConnection.Nickname, StringComparison.OrdinalIgnoreCase))
                {
                    return string.Format("YOUR connection was killed by {0}", base.Source);
                }
                else
                {
                    return string.Format("{0}'s connection was killed by {1}", this.Parameters[0], base.Source);
                }
            }
        }

        /// <summary>
        /// Gets the source of the message.
        /// </summary>
        public override string Source
        {
            get
            {
                return Strings_General.NotificationSource;
            }
        }

        #endregion
    }
}
