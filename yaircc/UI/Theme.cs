//-----------------------------------------------------------------------
// <copyright file="Theme.cs" company="intninety">
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
    using System;

    /// <summary>
    /// Represents a theme that can be used in yaircc.
    /// </summary>
    public class Theme
    {
        #region Fields

        /// <summary>
        /// The unique ID of the theme.
        /// </summary>
        private Guid id;

        /// <summary>
        /// The name of the theme.
        /// </summary>
        private string name;

        /// <summary>
        /// The name and or alias of the author.
        /// </summary>
        private string author;

        /// <summary>
        /// The author's website.
        /// </summary>
        private string website;

        /// <summary>
        /// An overview of the theme.
        /// </summary>
        private string description;

        /// <summary>
        /// A value indicating whether or not the theme is packaged with yaircc as standard.
        /// </summary>
        private bool isOfficial;

        /// <summary>
        /// The full path to the theme.
        /// </summary>
        private string path;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="Theme" /> class.
        /// </summary>
        /// <param name="id">The unique ID of the theme.</param>
        /// <param name="name">The name of the theme.</param>
        /// <param name="author">The name and or alias of the author.</param>
        /// <param name="website">The author's website.</param>
        /// <param name="description">An overview of the theme.</param>
        /// <param name="isOfficial">A value indicating whether or not the theme is packaged with yaircc as standard.</param>
        /// <param name="path">The full path to the theme.</param>
        public Theme(Guid id, string name, string author, string website, string description, bool isOfficial, string path)
        {
            this.id = id;
            this.name = name;
            this.author = author;
            this.website = website;
            this.description = description;
            this.isOfficial = isOfficial;
            this.path = path;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the unique ID of the theme.
        /// </summary>
        public Guid ID
        {
            get { return this.id; }
        }

        /// <summary>
        /// Gets the name of the theme.
        /// </summary>
        public string Name
        {
            get { return this.name; }
        }

        /// <summary>
        /// Gets the name and or alias of the author.
        /// </summary>
        public string Author
        {
            get { return this.author; }
        }

        /// <summary>
        /// Gets the author's website.
        /// </summary>
        public string Website
        {
            get { return this.website; }
        }

        /// <summary>
        /// Gets an overview of the theme.
        /// </summary>
        public string Description
        {
            get { return this.description; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the theme is packaged with yaircc as standard.
        /// </summary>
        public bool IsOfficial
        {
            get { return this.isOfficial; }
        }

        /// <summary>
        /// Gets the full path to the theme.
        /// </summary>
        public string Path
        {
            get { return this.path; }
        }

        #endregion
    }
}
