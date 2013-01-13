//-----------------------------------------------------------------------
// <copyright file="ChannelBrowser.cs" company="intninety">
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

namespace Yaircc
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Yaircc.Localisation;
    using Yaircc.Net.IRC;
    using Yaircc.UI;
    using IRCMessage = Yaircc.Net.IRC.Message;

    /// <summary>
    /// Represents the channel browser form.
    /// </summary>
    public partial class ChannelBrowser : Form
    {
        #region Fields

        /// <summary>
        /// The list of channels available on the server.
        /// </summary>
        private List<ListViewItem> items;

        /// <summary>
        /// The cache of items that are currently available in the ListView
        /// </summary>
        private ListViewItem[] cache;

        /// <summary>
        /// The marshal associated with the form.
        /// </summary>
        private IRCMarshal marshal;

        /// <summary>
        /// The ItemSorter for the sorting of the cache.
        /// </summary>
        private ItemSorter sorter;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ChannelBrowser" /> class.
        /// </summary>
        /// <param name="marshal">The associated marshal.</param>
        public ChannelBrowser(IRCMarshal marshal)
        {
            this.InitializeComponent();

            this.items = new List<ListViewItem>();
            this.marshal = marshal;
            this.sorter = new ItemSorter();
            this.channelListView.Columns[1].Tag = typeof(int);
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Add a channel to the list of channels that will be displayed upon flushing them to the screen.
        /// </summary>
        /// <param name="reply">The LIST reply to create an item from.</param>
        public void AddChannel(IRCMessage reply)
        {
            string cleanTopic = reply.TrailingParameter;
            cleanTopic = Regex.Replace(cleanTopic, @"(\x03+[0-9]{1,2}(,[0-9]{1,2})*|\x02|\x31|\x29|\x15|\x03)", string.Empty, RegexOptions.IgnoreCase);
            string[] subitems = new string[] { reply.Parameters[1], reply.Parameters[2], cleanTopic };
            this.items.Add(new ListViewItem(subitems));
        }

        /// <summary>
        /// Prepare the form for a refresh of the data.
        /// </summary>
        /// <param name="showForm">A value indicating whether or not to show the form.</param>
        public void BeginRefresh(bool showForm)
        {
            this.items.Clear();
            this.channelListView.Enabled = false;
            this.filterTextBox.Enabled = false;
            this.joinButton.Enabled = false;
            this.statusToolStripProgressBar.Visible = true;
            this.statusToolStripStatusLabel.Text = Strings_ChannelBrowser.FetchingChannels;
            this.refreshButton.Enabled = false;
            this.applyFilterButton.Enabled = false;
            this.clearFilterButton.Enabled = false;

            if (showForm)
            {
                this.Show();
                this.BringToFront();
            }
        }

        /// <summary>
        /// End a refresh operation.
        /// </summary>
        public void EndRefresh()
        {
            this.refreshButton.Enabled = true;
            this.channelListView.Enabled = true;
            this.filterTextBox.Enabled = true;
            this.joinButton.Enabled = true;
            this.statusToolStripStatusLabel.Text = string.Format(Strings_ChannelBrowser.DisplayingChannels, this.channelListView.Items.Count, this.items.Count);
            this.statusToolStripProgressBar.Visible = false;
            this.lastUpdatedLabel.Text = string.Format(Strings_ChannelBrowser.LastRefreshed, DateTime.Now);
            this.lastUpdatedLabel.Visible = true;
            this.applyFilterButton.Enabled = true;
            this.clearFilterButton.Enabled = false;
        }

        /// <summary>
        /// Flush the channels added via the marshal to the ListView.
        /// </summary>
        public void FlushChannels()
        {
            this.statusToolStripStatusLabel.Text = Strings_ChannelBrowser.PopulatingList;
            this.filterTextBox.Text = string.Empty;
            this.PopulateListView(this.items.ToArray(), 0, SortOrder.Ascending);
            this.EndRefresh();
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ApplyFilterButton_Click(object sender, EventArgs e)
        {
            this.cache = this.items.Where(i => i.Text.Contains(this.filterTextBox.Text) || i.SubItems[2].Text.Contains(this.filterTextBox.Text)).ToArray();
            this.PopulateListView(this.cache, 0, SortOrder.Ascending);
            this.statusToolStripStatusLabel.Text = string.Format(Strings_ChannelBrowser.DisplayingChannels, this.cache.Length, this.items.Count);
            this.clearFilterButton.Enabled = true;
        }

        /// <summary>
        /// Handles the FormClosing event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelBrowser_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Hide();
            e.Cancel = true;
        }

        /// <summary>
        /// Handles the Shown event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelBrowser_Shown(object sender, EventArgs e)
        {
            this.Text = string.Format(Strings_ChannelBrowser.Title, this.marshal.ServerTab.Text);
            this.filterTextBox.Focus();

            if (this.items.Count == 0)
            {
                this.RefreshButton_Click(sender, e);
            }
        }

        /// <summary>
        /// Handles the ColumnClick event of System.Windows.Forms.ListView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelListView_ColumnClick(object sender, ColumnClickEventArgs e)
        {
            if (this.sorter.Order == SortOrder.Ascending)
            {
                this.sorter.Order = SortOrder.Descending;
            }
            else
            {
                this.sorter.Order = SortOrder.Ascending;
            }

            this.SortItems(e.Column);

            this.channelListView.BeginUpdate();
            this.channelListView.Items.Clear();
            this.channelListView.Items.AddRange(this.cache);
            this.channelListView.EndUpdate();
        }

        /// <summary>
        /// Handles the Resize event of System.Windows.Forms.Form.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelListView_Resize(object sender, EventArgs e)
        {
            int width = this.channelListView.Width - (this.channelListView.Columns[0].Width + this.channelListView.Columns[1].Width);
            this.channelListView.Columns[2].Width = width - 25;
        }

        /// <summary>
        /// Handles the SelectedIndexChanged event of System.Windows.Forms.ListView.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ChannelListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.joinButton.Enabled = this.channelListView.SelectedItems.Count > 0;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ClearFilterButton_Click(object sender, EventArgs e)
        {
            this.PopulateListView(this.items.ToArray(), 0, SortOrder.Ascending);

            this.statusToolStripStatusLabel.Text = string.Format(Strings_ChannelBrowser.DisplayingChannels, this.items.Count, this.items.Count);
            this.filterTextBox.Text = string.Empty;
            this.clearFilterButton.Enabled = false;
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void CloseButton_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        /// <summary>
        /// Handles the KeyPress event of System.Windows.Forms.TextBox.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void FilterTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == 13)
            {
                this.ApplyFilterButton_Click(sender, e);
                e.Handled = true;
            }
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void JoinButton_Click(object sender, EventArgs e)
        {
            string channelName = this.channelListView.SelectedItems[0].Text;
            this.marshal.Send(this.marshal.ServerTab, new JoinMessage(channelName));
            this.Hide();
        }

        /// <summary>
        /// Populate the list view with and set up the initial sorting.
        /// </summary>
        /// <param name="items">The items to add.</param>
        /// <param name="sortColumn">The column index to sort by.</param>
        /// <param name="order">The order in which to sort by.</param>
        private void PopulateListView(ListViewItem[] items, int sortColumn, SortOrder order)
        {
            this.cache = items;
            this.sorter.Order = order;
            this.SortItems(sortColumn);

            this.channelListView.BeginUpdate();
            this.channelListView.Items.Clear();
            this.channelListView.Items.AddRange(this.cache);
            this.channelListView.EndUpdate();
        }

        /// <summary>
        /// Handles the Click event of System.Windows.Forms.Button.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            this.BeginRefresh(false);
            this.marshal.Send(this.marshal.ServerTab, new ListMessage());
        }

        /// <summary>
        /// Sort the items in the cache.
        /// </summary>
        /// <param name="column">The column index to sort by.</param>
        private void SortItems(int column)
        {
            if (column != this.sorter.Column)
            {
                this.sorter.Order = SortOrder.Ascending;
                this.sorter.Column = column;
            }

            this.cache = this.cache.OrderBy(i => i, this.sorter).ToArray();
        }

        #endregion
    }
}