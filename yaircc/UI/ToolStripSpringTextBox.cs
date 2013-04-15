//-----------------------------------------------------------------------
// <copyright file="ToolStripSpringTextBox.cs" company="rastating">
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
    using System.Drawing;
    using System.Windows.Forms;

    /// <summary>
    /// Represents an auto-sizing ToolStripTextBox.
    /// </summary>
    public class ToolStripSpringTextBox : ToolStripTextBox
    {
        /// <summary>
        /// Retrieves the size of a rectangular area into which a control can be fitted.
        /// </summary>
        /// <param name="constrainingSize">The custom-sized area for a control.</param>
        /// <returns>An ordered pair of type System.Drawing.Size representing the width and height of a rectangle.</returns>
        public override Size GetPreferredSize(Size constrainingSize)
        {
            // Use the default size if the text box is on the overflow menu 
            // or is on a vertical ToolStrip. 
            if (this.IsOnOverflow || this.Owner.Orientation == Orientation.Vertical)
            {
                return this.DefaultSize;
            }

            // Declare a variable to store the total available width as  
            // it is calculated, starting with the display width of the  
            // owning ToolStrip.
            int width = Owner.DisplayRectangle.Width;

            // Subtract the width of the overflow button if it is displayed.  
            if (Owner.OverflowButton.Visible)
            {
                width = width - Owner.OverflowButton.Width -
                    Owner.OverflowButton.Margin.Horizontal;
            }

            // Declare a variable to maintain a count of ToolStripSpringTextBox  
            // items currently displayed in the owning ToolStrip. 
            int springBoxCount = 0;

            foreach (ToolStripItem item in Owner.Items)
            {
                // Ignore items on the overflow menu. 
                if (item.IsOnOverflow)
                {
                    continue;
                }

                if (item is ToolStripSpringTextBox)
                {
                    // For ToolStripSpringTextBox items, increment the count and  
                    // subtract the margin width from the total available width.
                    springBoxCount++;
                    width -= item.Margin.Horizontal;
                }
                else
                {
                    // For all other items, subtract the full width from the total 
                    // available width.
                    width = width - item.Width - item.Margin.Horizontal;
                }
            }

            // If there are multiple ToolStripSpringTextBox items in the owning 
            // ToolStrip, divide the total available width between them.  
            if (springBoxCount > 1)
            {
                width /= springBoxCount;
            }

            // If the available width is less than the default width, use the 
            // default width, forcing one or more items onto the overflow menu. 
            if (width < DefaultSize.Width)
            {
                width = DefaultSize.Width;
            }

            // Retrieve the preferred size from the base class, but change the 
            // width to the calculated width. 
            Size size = base.GetPreferredSize(constrainingSize);
            size.Width = width;
            return size;
        }
    }
}