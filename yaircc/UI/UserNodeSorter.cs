//-----------------------------------------------------------------------
// <copyright file="UserNodeSorter.cs" company="rastating">
//     yaircc - the free, open-source IRC client for Windows.
//     Copyright (C) 2012-2013 Robert Carr
//
//     This program is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//
//     This program is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with this program.  If not, see http://www.gnu.org/licenses/.
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
