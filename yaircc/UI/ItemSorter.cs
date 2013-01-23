//-----------------------------------------------------------------------
// <copyright file="ItemSorter.cs" company="intninety">
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
    using System.Windows.Forms;

    /// <summary>
    /// Represents an IComparer capable of sorting ListViewItems.
    /// </summary>
    public class ItemSorter : System.Collections.Generic.IComparer<ListViewItem>
    {
        #region Fields

        /// <summary>
        /// The order in which to sort by.
        /// </summary>
        private SortOrder order;

        /// <summary>
        /// The column index of the ListView to sort by.
        /// </summary>
        private int column;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ItemSorter" /> class.
        /// </summary>
        public ItemSorter()
        {
            this.column = 0;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the order in which to sort by.
        /// </summary>
        public SortOrder Order
        {
            get { return this.order; }
            set { this.order = value; }
        }

        /// <summary>
        /// Gets or sets the column index of the ListView to sort by.
        /// </summary>
        public int Column
        {
            get { return this.column; }
            set { this.column = value; }
        }

        #endregion

        #region IComparer<ListViewItem> Members

        /// <summary>
        /// Compares two objects and returns a value indicating whether one is less than, equal to, or greater than the other.
        /// </summary>
        /// <param name="x">The first object to compare.</param>
        /// <param name="y">The second object to compare.</param>
        /// <returns>A signed integer that indicates the relative values of x and y.</returns>
        public int Compare(ListViewItem x, ListViewItem y)
        {
            if (x.ListView != null)
            {
                if (x.ListView.Columns[this.Column].Tag == typeof(int))
                {
                    if (this.Order == SortOrder.Descending)
                    {
                        return int.Parse(y.SubItems[this.Column].Text).CompareTo(int.Parse(x.SubItems[this.Column].Text));
                    }
                    else
                    {
                        return int.Parse(x.SubItems[this.Column].Text).CompareTo(int.Parse(y.SubItems[this.Column].Text));
                    }
                }
                else
                {
                    if (this.Order == SortOrder.Descending)
                    {
                        return y.SubItems[this.Column].Text.CompareTo(x.SubItems[this.Column].Text);
                    }
                    else
                    {
                        return x.SubItems[this.Column].Text.CompareTo(y.SubItems[this.Column].Text);
                    }
                }
            }
            else
            {
                return x.SubItems[this.Column].Text.CompareTo(y.SubItems[this.Column].Text);
            }
        }

        #endregion
    }
}
