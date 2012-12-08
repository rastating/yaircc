//-----------------------------------------------------------------------
// <copyright file="PrefixType.cs" company="intninety">
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

namespace Yaircc.Net
{
    /// <summary>
    /// Specifies the type of data contained within an IRC command prefix.
    /// </summary>
    [System.Flags]
    public enum PrefixType
    {
        /// <summary>
        /// No prefix is present.
        /// </summary>
        None = 0x0,

        /// <summary>
        /// The name of the server.
        /// </summary>
        ServerName = 0x1,

        /// <summary>
        /// The nick name of the sender.
        /// </summary>
        NickName = 0x2,

        /// <summary>
        /// The user name of the sender.
        /// </summary>
        UserExtension = 0x4,

        /// <summary>
        /// The host of the sender.
        /// </summary>
        HostExtension = 0x8
    }
}