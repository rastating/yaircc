//-----------------------------------------------------------------------
// <copyright file="ItemSorter.cs" company="intninety">
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
