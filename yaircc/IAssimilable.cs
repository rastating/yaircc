﻿//-----------------------------------------------------------------------
// <copyright file="IAssimilable.cs" company="intninety">
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
    /// <summary>
    /// Represents an object capable of assimilating data from another.
    /// </summary>
    public interface IAssimilable
    {
        /// <summary>
        /// Assimilate all the data stored in <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object in which to assimilate data from</param>
        void Assimilate(object obj);
    }
}