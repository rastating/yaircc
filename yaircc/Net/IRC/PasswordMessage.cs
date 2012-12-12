//-----------------------------------------------------------------------
// <copyright file="PasswordMessage.cs" company="intninety">
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
    /// Represents a PASS command.
    /// </summary>
    public class PasswordMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="PasswordMessage"/> class.
        /// </summary>
        /// <param name="password">The password.</param>
        public PasswordMessage(string password)
            : base(string.Empty, "PASS", new string[] { password }, string.Empty)
        {
            // If no password was specified we still need to send something
            // as otherwise some servers (snircd being one) will complain that
            // not enough parameters were specified.
            if (string.IsNullOrEmpty(password))
            {
                this.Parameters = new string[] { "*" };
            }
        }

        #endregion
    }
}
