//-----------------------------------------------------------------------
// <copyright file="UserNodeSorter.cs" company="intninety">
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

namespace Yaircc.UI
{
    using System;
    using System.Collections;
    using System.Windows.Forms;

    /// <summary>
    /// Represents an IComparer capable of sorting user nodes.
    /// </summary>
    public class UserNodeSorter : IComparer
    {
        #region Constants

        /// <summary>
        /// The founder group identifier.
        /// </summary>
        public const int FounderGroup = 0;

        /// <summary>
        /// The admin group identifier.
        /// </summary>
        public const int AdminGroup = 1;

        /// <summary>
        /// The operator group identifier.
        /// </summary>
        public const int Operator = 2;

        /// <summary>
        /// The half operator group identifier.
        /// </summary>
        public const int HalfOperator = 3;

        /// <summary>
        /// The voice group identifier.
        /// </summary>
        public const int Voice = 4;

        /// <summary>
        /// The normal group identifier.
        /// </summary>
        public const int Normal = 5;

        /// <summary>
        /// The users group identifier.
        /// </summary>
        public const int Users = 6;

        #endregion

        #region Fields

        /// <summary>
        /// A value indicating whether to sort by mode.
        /// </summary>
        private bool sortByIRCMode;

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets a value indicating whether to sort by mode.
        /// </summary>
        public bool SortByIRCMode
        {
            get { return this.sortByIRCMode; }
            set { this.sortByIRCMode = value; }
        }

        #endregion

        #region IComparer Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to or greater than the other.
        /// </summary>
        /// <param name="x">First object to compare.</param>
        /// <param name="y">Second object to compare.</param>
        /// <returns>Less than zero: x is less than y. Zero: x equals y. Greater than zero: x is greater than y.</returns>
        public int Compare(object x, object y)
        {
            TreeNode nodeX = x as TreeNode;
            TreeNode nodeY = y as TreeNode;

            if (nodeX.Level == 1)
            {
                return nodeX.Text.CompareTo(nodeY.Text);
            }
            else
            {
                if ((nodeX.Tag is int) && (nodeY.Tag is int))
                {
                    return Convert.ToInt32(nodeX.Tag).CompareTo(Convert.ToInt32(nodeY.Tag));
                }
                else if ((nodeX.Tag is IRCUser) && (nodeY.Tag is IRCUser))
                {
                    IRCUser userX = nodeX.Tag as IRCUser;
                    IRCUser userY = nodeY.Tag as IRCUser;

                    if (this.sortByIRCMode)
                    {
                        if (userX.Mode < userY.Mode)
                        {
                            return 1;
                        }
                        else if (userX.Mode == userY.Mode)
                        {
                            return userX.NickName.CompareTo(userY.NickName);
                        }
                        else
                        {
                            return -1;
                        }
                    }
                    else
                    {
                        return userX.NickName.CompareTo(userY.NickName);
                    }
                }
                else
                {
                    return nodeX.Text.CompareTo(nodeY.Text);
                }
            }
        }

        #endregion
    }
}
