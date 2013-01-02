//-----------------------------------------------------------------------
// <copyright file="IRCUserMode.cs" company="intninety">
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

namespace Yaircc.UI
{
    /// <summary>
    /// Specifies the user mode of an IRC user.
    /// </summary>
    public enum IRCUserMode
    {
        /// <summary>
        /// Normal
        /// </summary>
        Normal = 0,

        /// <summary>
        /// Voice
        /// </summary>
        Voice = 1,

        /// <summary>
        /// Half operator
        /// </summary>
        HalfOperator = 2,

        /// <summary>
        /// Operator
        /// </summary>
        Operator = 3,

        /// <summary>
        /// Admin
        /// </summary>
        Admin = 4,

        /// <summary>
        /// Founder
        /// </summary>
        Founder = 5
    }
}
