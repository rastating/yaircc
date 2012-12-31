//-----------------------------------------------------------------------
// <copyright file="IRCTabPage.cs" company="intninety">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using System.Windows.Forms;
    using Yaircc.Localisation;
    using Yaircc.Net.IRC;
    using Yaircc.Settings;

    /// <summary>
    /// Represents a specific type of <see cref="IRCTabPage"/>
    /// </summary>
    public enum IRCTabType
    {
        /// <summary>
        /// A console tab
        /// </summary>
        Console = 0,

        /// <summary>
        /// A server tab
        /// </summary>
        Server = 1,

        /// <summary>
        /// A channel tab
        /// </summary>
        Channel = 2,

        /// <summary>
        /// A private message tab
        /// </summary>
        PM = 3
    }

    /// <summary>
    /// Represents a TabPage that can communicate with an IRC server
    /// </summary>
    public class IRCTabPage : TabPage
    {
        #region Fields

        /// <summary>
        /// The connection the tab is associated with.
        /// </summary>
        private Connection connection;

        /// <summary>
        /// The WebBrowser control that the tab prints messages to.
        /// </summary>
        private WebBrowser webBrowser;

        /// <summary>
        /// The type of IRC entity the tab represents.
        /// </summary>
        private IRCTabType type;

        /// <summary>
        /// The name of the tab in relation to its connection e.g. #yaircc, or a blank string for a server window
        /// </summary>
        private string connectionSpecificName;

        /// <summary>
        /// The marshal that sits between the TabPage and the server.
        /// </summary>
        private IRCMarshal marshal;

        /// <summary>
        /// The TreeView that displays the users in the current channel.
        /// </summary>
        private TreeView userTreeView;

        /// <summary>
        /// The image index of the normal state tab page icon.
        /// </summary>
        private int normalImageIndex;

        /// <summary>
        /// A value indicating whether or not the "group by mode" function is being used.
        /// </summary>
        private bool groupingByMode;

        /// <summary>
        /// The name of the admin group in <see cref="userTreeView"/>.
        /// </summary>
        private string adminGroupName;

        /// <summary>
        /// The name of the founder group in <see cref="userTreeView"/>.
        /// </summary>
        private string founderGroupName;

        /// <summary>
        /// The name of the half operator group in <see cref="userTreeView"/>.
        /// </summary>
        private string halfOperatorGroupName;

        /// <summary>
        /// The name of the operator group in <see cref="userTreeView"/>.
        /// </summary>
        private string operatorGroupName;

        /// <summary>
        /// The name of the voice group in <see cref="userTreeView"/>.
        /// </summary>
        private string voiceGroupName;

        /// <summary>
        /// The name of the normal group in <see cref="userTreeView"/>.
        /// </summary>
        private string normalGroupName;

        /// <summary>
        /// A value indicating whether or not the admin group in <see cref="userTreeView"/> is expanded.
        /// </summary>
        private bool adminGroupExpanded;

        /// <summary>
        /// A value indicating whether or not the founder group in <see cref="userTreeView"/> is expanded.
        /// </summary>
        private bool founderGroupExpanded;

        /// <summary>
        /// A value indicating whether or not the half operator group in <see cref="userTreeView"/> is expanded.
        /// </summary>
        private bool halfOperatorGroupExpanded;

        /// <summary>
        /// A value indicating whether or not the operator group in <see cref="userTreeView"/> is expanded.
        /// </summary>
        private bool operatorGroupExpanded;

        /// <summary>
        /// A value indicating whether or not the voice group in <see cref="userTreeView"/> is expanded.
        /// </summary>
        private bool voiceGroupExpanded;

        /// <summary>
        /// A value indicating whether or not the normal group in <see cref="userTreeView"/> is expanded.
        /// </summary>
        private bool normalGroupExpanded;

        /// <summary>
        /// Occurs when the user's nick name is mentioned in the tab.
        /// </summary>
        private NickNameMentionedHandler nickNameMentioned;

        /// <summary>
        /// The form that the tab page is displayed on.
        /// </summary>
        private MainForm owningForm;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="IRCTabPage"/> class.
        /// </summary>
        /// <param name="owningForm">The form that the tab page is displayed on.</param>
        /// <param name="name">The name of the tab page.</param>
        /// <param name="text">The text to display in the caption.</param>
        /// <param name="type">The type of IRC entity being represented.</param>
        public IRCTabPage(MainForm owningForm, string name, string text, IRCTabType type)
            : base(text)
        {
            GlobalSettings settings = GlobalSettings.Instance;

            this.owningForm = owningForm;

            this.adminGroupExpanded = true;
            this.founderGroupExpanded = true;
            this.halfOperatorGroupExpanded = true;
            this.operatorGroupExpanded = true;
            this.voiceGroupExpanded = true;
            this.normalGroupExpanded = true;

            this.Name = name;
            this.type = type;
            this.ConnectionSpecificName = string.Empty;
            this.groupingByMode = settings.GroupUsersByMode == GlobalSettings.Boolean.Yes;

            switch (type)
            {
                case IRCTabType.Console:
                    this.normalImageIndex = 0;
                    break;

                case IRCTabType.Server:
                    this.normalImageIndex = 1;
                    break;

                case IRCTabType.Channel:
                    this.normalImageIndex = 2;
                    break;

                case IRCTabType.PM:
                    this.normalImageIndex = 3;
                    break;
            }

            this.ImageIndex = this.NormalImageIndex;
            this.Initialise();
        }

        #endregion

        #region Delegates

        /// <summary>
        /// Delegate for event <see cref="NickNameMentioned"/>.
        /// </summary>
        public delegate void NickNameMentionedHandler();

        #endregion

        #region Events

        /// <summary>
        /// Occurs when the user's nick name is mentioned in the tab.
        /// </summary>
        public event NickNameMentionedHandler NickNameMentioned
        {
            add { this.nickNameMentioned += value; }
            remove { this.nickNameMentioned -= value; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the name of the tab in relation to its connection e.g. #yaircc, or a blank string for a server window
        /// </summary>
        public string ConnectionSpecificName
        {
            get { return this.connectionSpecificName; }
            set { this.connectionSpecificName = value; }
        }

        /// <summary>
        /// Gets the WebBrowser used to display the tab's content
        /// </summary>
        public WebBrowser WebBrowser
        {
            get { return this.webBrowser; }
        }

        /// <summary>
        /// Gets or sets the Connection used to communicate with the associated IRC server
        /// </summary>
        public Connection Connection
        {
            get { return this.connection;  }
            set { this.connection = value; }
        }

        /// <summary>
        /// Gets or sets the marshal that sits between the TabPage and the server.
        /// </summary>
        public IRCMarshal Marshal
        {
            get { return this.marshal; }
            set { this.marshal = value; }
        }

        /// <summary>
        /// Gets the type of window the tab contains
        /// </summary>
        public IRCTabType TabType
        {
            get { return this.type; }
        }

        /// <summary>
        /// Gets or sets the TreeView that displays the users in the current channel.
        /// </summary>
        public TreeView UserTreeView
        {
            get 
            { 
                return this.userTreeView; 
            }

            set 
            { 
                this.userTreeView = value;

                this.adminGroupName = string.Format("{0}_AdminGroup", this.UserTreeView.Name);
                this.founderGroupName = string.Format("{0}_FounderGroup", this.UserTreeView.Name);
                this.halfOperatorGroupName = string.Format("{0}_HalfOperatorGroup", this.UserTreeView.Name);
                this.operatorGroupName = string.Format("{0}_OperatorGroup", this.UserTreeView.Name);
                this.voiceGroupName = string.Format("{0}_VoiceGroup", this.UserTreeView.Name);
                this.normalGroupName = string.Format("{0}_NormalGroup", this.UserTreeView.Name);

                this.userTreeView.AfterExpand += new TreeViewEventHandler(this.UserTreeView_AfterExpand);
                this.userTreeView.AfterCollapse += new TreeViewEventHandler(this.UserTreeView_AfterCollapse);
            }
        }

        /// <summary>
        /// Gets the image index of the normal state tab page icon.
        /// </summary>
        public int NormalImageIndex
        {
            get { return this.normalImageIndex; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the "group by mode" function is being used.
        /// </summary>
        public bool GroupingByMode
        {
            get { return this.groupingByMode; }
        }

        /// <summary>
        /// Gets the form that the tab page is displayed on.
        /// </summary>
        public MainForm OwningForm
        {
            get { return this.owningForm; }
        }

        /// <summary>
        /// Gets a value indicating whether or not Internet Explorer has JavaScript enabled.
        /// </summary>
        public bool JavaScriptIsAvailable
        {
            get
            {
                // Verify that JavaScript is enabled by making a call to the *javaScriptIsAvailable* method.
                return this.WebBrowser.Document.InvokeScript("javaScriptIsAvailable") != null;
            }
        }

        #endregion

        #region Instance Method

        /// <summary>
        /// Toggles whether or not grouping is enabled.
        /// </summary>
        public void ToggleGrouping()
        {
            this.groupingByMode = !this.groupingByMode;
            this.UserTreeView.Nodes.Clear();
            this.Marshal.GetChannelByTab(this).Users.ForEach(i => this.AddUserToList(i));
            this.UserTreeView.Sort();
            this.UserTreeView.ExpandAll();
        }

        /// <summary>
        /// Sets the sorting mode of the user tree view.
        /// </summary>
        /// <param name="sortByMode">A value indicating whether or not to sort by mode.</param>
        public void SortByMode(bool sortByMode)
        {
            UserNodeSorter sorter = this.UserTreeView.TreeViewNodeSorter as UserNodeSorter;
            if (sorter.SortByIRCMode != sortByMode)
            {
                sorter.SortByIRCMode = sortByMode;
                this.UserTreeView.TreeViewNodeSorter = sorter;
                this.UserTreeView.Sort();
            }
        }

        /// <summary>
        /// Adds a user to the tree view list.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <param name="refresh">A value indicating whether or not to refresh the sorting.</param>
        public void AddUserToList(IRCUser user, bool refresh)
        {
            this.UserTreeView.InvokeAction(() =>
            {
                TreeNode node = this.AddUserToList(user);
                if (refresh)
                {
                    this.UserTreeView.Sort();

                    // If we are in grouping mode and the user's group only has one
                    // node, then it is has been newly created and should be expanded
                    if (this.groupingByMode && node.Parent.Nodes.Count == 1)
                    {
                        node.Parent.Expand();
                    }
                }
            });
        }

        /// <summary>
        /// Adds a user to the tree view list.
        /// </summary>
        /// <param name="user">The user to add.</param>
        /// <returns>The newly created TreeNode.</returns>
        public TreeNode AddUserToList(IRCUser user)
        {
            TreeNode node = new TreeNode();
            this.UserTreeView.InvokeAction(() =>
            {
                TreeNode root = null;
                node.Name = string.Format("{0}_{1}_node", this.Name, user.NickName);
                node.Text = user.NickName;
                node.Tag = user;

                switch (user.Mode)
                {
                    case IRCUserMode.Admin:
                        node.ImageKey = "orange";
                        node.SelectedImageKey = "orange";
                        node.StateImageKey = "orange";
                        break;

                    case IRCUserMode.Founder:
                        node.ImageKey = "purple";
                        node.SelectedImageKey = "purple";
                        node.StateImageKey = "purple";
                        break;

                    case IRCUserMode.HalfOperator:
                        node.ImageKey = "blue";
                        node.SelectedImageKey = "blue";
                        node.StateImageKey = "blue";
                        break;

                    case IRCUserMode.Operator:
                        node.ImageKey = "green";
                        node.SelectedImageKey = "green";
                        node.StateImageKey = "green";
                        break;

                    case IRCUserMode.Voice:
                        node.ImageKey = "black";
                        node.SelectedImageKey = "black";
                        node.StateImageKey = "black";
                        break;

                    case IRCUserMode.Normal:
                        node.ImageKey = "white";
                        node.SelectedImageKey = "white";
                        node.StateImageKey = "white";
                        break;
                }

                if (this.groupingByMode)
                {
                    switch (user.Mode)
                    {
                        case IRCUserMode.Admin:
                            if (this.UserTreeView.Nodes.ContainsKey(adminGroupName))
                            {
                                root = this.UserTreeView.Nodes[adminGroupName];
                            }
                            else
                            {
                                root = this.UserTreeView.Nodes.Add(adminGroupName, Strings_General.Admin, "group", "group");
                                root.Tag = UserNodeSorter.AdminGroup;
                            }

                            break;

                        case IRCUserMode.Founder:
                            if (this.UserTreeView.Nodes.ContainsKey(founderGroupName))
                            {
                                root = this.UserTreeView.Nodes[founderGroupName];
                            }
                            else
                            {
                                root = this.UserTreeView.Nodes.Add(founderGroupName, Strings_General.Founder, "group", "group");
                                root.Tag = UserNodeSorter.FounderGroup;
                            }

                            break;

                        case IRCUserMode.HalfOperator:
                            if (this.UserTreeView.Nodes.ContainsKey(halfOperatorGroupName))
                            {
                                root = this.UserTreeView.Nodes[halfOperatorGroupName];
                            }
                            else
                            {
                                root = this.UserTreeView.Nodes.Add(halfOperatorGroupName, Strings_General.HalfOperator, "group", "group");
                                root.Tag = UserNodeSorter.HalfOperator;
                            }

                            break;

                        case IRCUserMode.Operator:
                            if (this.UserTreeView.Nodes.ContainsKey(operatorGroupName))
                            {
                                root = this.UserTreeView.Nodes[operatorGroupName];
                            }
                            else
                            {
                                root = this.UserTreeView.Nodes.Add(operatorGroupName, Strings_General.Operator, "group", "group");
                                root.Tag = UserNodeSorter.Operator;
                            }

                            break;

                        case IRCUserMode.Voice:
                            if (this.UserTreeView.Nodes.ContainsKey(voiceGroupName))
                            {
                                root = this.UserTreeView.Nodes[voiceGroupName];
                            }
                            else
                            {
                                root = this.UserTreeView.Nodes.Add(voiceGroupName, Strings_General.Voice, "group", "group");
                                root.Tag = UserNodeSorter.Voice;
                            }

                            break;

                        case IRCUserMode.Normal:
                            if (this.UserTreeView.Nodes.ContainsKey(normalGroupName))
                            {
                                root = this.UserTreeView.Nodes[normalGroupName];
                            }
                            else
                            {
                                root = this.UserTreeView.Nodes.Add(normalGroupName, Strings_General.Normal, "group", "group");
                                root.Tag = UserNodeSorter.Normal;
                            }

                            break;
                    }
                }

                if (root != null)
                {
                    root.Nodes.Add(node);
                }
                else
                {
                    this.UserTreeView.Nodes.Add(node);
                }
            });

            return node;
        }

        /// <summary>
        /// Refreshes the collapse state of each tree node.
        /// </summary>
        public void RefreshTreeNodeCollapseState()
        {
            if (this.groupingByMode)
            {
                this.ChangeTreeNodeState(this.adminGroupName, !this.adminGroupExpanded);
                this.ChangeTreeNodeState(this.founderGroupName, !this.founderGroupExpanded);
                this.ChangeTreeNodeState(this.halfOperatorGroupName, !this.halfOperatorGroupExpanded);
                this.ChangeTreeNodeState(this.operatorGroupName, !this.operatorGroupExpanded);
                this.ChangeTreeNodeState(this.voiceGroupName, !this.voiceGroupExpanded);
                this.ChangeTreeNodeState(this.normalGroupName, !this.normalGroupExpanded);
            }
        }

        /// <summary>
        /// Removes a user from the user tree view.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        public void RemoveUserFromList(IRCUser user)
        {
            this.UserTreeView.InvokeAction(() =>
            {
                if (this.GroupingByMode)
                {
                    this.RemoveUserFromList(user, this.UserTreeView.Nodes);

                    List<int> emptyGroups = new List<int>(this.UserTreeView.Nodes.Count);
                    for (int i = 0; i < this.UserTreeView.Nodes.Count; i++)
                    {
                        if (this.UserTreeView.Nodes[i].Nodes.Count == 0)
                        {
                            emptyGroups.Add(i);
                        }
                    }

                    for (int i = 0; i < emptyGroups.Count; i++)
                    {
                        this.UserTreeView.Nodes.RemoveAt(emptyGroups[i]);
                    }
                }
                else
                {
                    this.RemoveUserFromTopLevelNodes(user);
                }
            });
        }

        /// <summary>
        /// Clear the message log.
        /// </summary>
        public void ClearLog()
        {
            this.WebBrowser.Document.InvokeScript("clearMessageLog");
        }

        /// <summary>
        /// Lists the supported emoticons in the tab.
        /// </summary>
        public void ListEmoticons()
        {
            this.WebBrowser.Document.InvokeScript("listEmoticons", new object[] { "[" + DateTime.Now.ToString("HH:mm") + "]", "[INFO]" });
        }

        /// <summary>
        /// Load a theme into the web browser control.
        /// </summary>
        /// <param name="path">The full path of the theme to load.</param>
        public void LoadTheme(string path)
        {
            string style = File.ReadAllText(path);
            this.WebBrowser.Document.InvokeScript("setStyle", new object[] { style });
        }

        /// <summary>
        /// Appends a message to the tab's browser control.
        /// </summary>
        /// <param name="command">The command that invoked the call.</param>
        /// <param name="source">The source of the message.</param>
        /// <param name="payload">The message's payload.</param>
        /// <param name="type">The type of message.</param>
        /// <param name="transform">A delegate that will be called to transform the payload content before appending.</param>
        public void AppendMessage(string command, string source, string payload, MessageType type, Func<string, string> transform)
        {
            this.webBrowser.InvokeAction(() =>
            {
                try
                {
                    string timestamp = string.Format("[{0:HH:mm}]", DateTime.Now);
                    string classes = type.ToString();

                    if (!string.IsNullOrEmpty(command))
                    {
                        classes = string.Format("{0} {1}", classes, command.ToUpper());
                    }

                    source = type == MessageType.UserMessage ? string.Format("<{0}>", source) : source;
                    payload = transform.Invoke(payload);

                    object[] args = new object[] { timestamp, source, payload, classes };
                    bool scroll = false;

                    // If the selected tab is the one that we are appending to, then use the JavaScript
                    // method "isScrolledToTheBottom" to determine whether or not we should scroll.
                    if (this.Parent != null)
                    {
                        if ((this.Parent as TabControl).SelectedTab == this)
                        {
                            scroll = (bool)this.WebBrowser.Document.InvokeScript("isScrolledToTheBottom");
                        }
                    }

                    this.WebBrowser.Document.InvokeScript("appendMessage", args);

                    if (scroll)
                    {
                        this.ScrollToBottom();
                    }

                    if (this.Marshal != null)
                    {
                        if (this.TabType == IRCTabType.PM || payload.Contains(this.Marshal.Connection.Nickname))
                        {
                            if ((this.Parent as TabControl).SelectedTab != this || !this.OwningForm.IsForegroundWindow)
                            {
                                this.ImageKey = "exclamation";
                                if (this.nickNameMentioned != null)
                                {
                                    this.nickNameMentioned.Invoke();
                                }
                            }
                        }
                    }
                }
                catch (ObjectDisposedException)
                {
                }
            });
        }

        /// <summary>
        /// Appends a message to the tab's browser control and transforms the payload accordingly to show custom font styles / colours.
        /// </summary>
        /// <param name="command">The command that invoked the call.</param>
        /// <param name="source">The source of the message.</param>
        /// <param name="payload">The message's payload.</param>
        /// <param name="type">The type of message.</param>
        public void AppendMessage(string command, string source, string payload, MessageType type)
        {
            this.AppendMessage(
                command, 
                source, 
                payload, 
                type, 
                (originalPayload) => 
                {
                    string retval = System.Security.SecurityElement.Escape(originalPayload).Replace("&apos;", "'");

                    /*
                     * As there are issues with using <pre> in a table cell
                     * and as IE does not support the pre-wrap property of 
                     * white-space, we need to alternate between &nbsp; and
                     * <wbr/> so that a break can occur after each space.
                     *
                     * Thanks to ponix for figuring this one out! https://twitter.com/ponix
                     */
                    retval = retval.Replace(" ", "&nbsp;<wbr/>");

                    string closeTag = "</span>";
                    int openTags = 0;
                    int openBoldTags = 0;
                    int openUnderlineTags = 0;
                    int openItalicTags = 0;
                    Regex colourRegex = new Regex(@"^\x03(?<foreground>[0-9]{1,2})(,(?<background>[0-9]{1,2}))?", RegexOptions.IgnoreCase);
                    int currentBackgroundColour = -1;
                    int currentForegroundColour = -1;
                    int openColourTags = 0;
                    string[] colours = new string[17];
                    colours[0] = "#FFFFFF";
                    colours[1] = "#000000";
                    colours[2] = "#000080";
                    colours[3] = "#008000";
                    colours[4] = "#FF0000";
                    colours[5] = "#A52A2A";
                    colours[6] = "#800080";
                    colours[7] = "#FFA500";
                    colours[8] = "#FFFF00";
                    colours[9] = "#32CD32";
                    colours[10] = "#008080";
                    colours[11] = "#00FFFF";
                    colours[12] = "#4169E1";
                    colours[13] = "#FF69B4";
                    colours[14] = "#A9A9A9";
                    colours[15] = "#D3D3D3";
                    colours[16] = "#FFFFFF";

                    Func<int, bool, int> insertBoldTag = (i, remove) =>
                    {
                        string newTag = "<span style=\"font-weight: bold;\">";

                        retval = retval.Insert(i, newTag);
                        i += newTag.Length;

                        if (remove)
                        {
                            retval = retval.Remove(i, 1);
                            i -= 1;
                        }

                        openBoldTags++;
                        openTags++;

                        return i;
                    };

                    Func<int, bool, int> insertUnderlineTag = (i, remove) =>
                    {
                        string newTag = "<span style=\"text-decoration: underline;\">";

                        retval = retval.Insert(i, newTag);
                        i += newTag.Length;

                        if (remove)
                        {
                            retval = retval.Remove(i, 1);
                            i -= 1;
                        }

                        openUnderlineTags++;
                        openTags++;

                        return i;
                    };

                    Func<int, bool, int> insertItalicTag = (i, remove) =>
                    {
                        string newTag = "<span style=\"font-style: italic;\">";

                        retval = retval.Insert(i, newTag);
                        i += newTag.Length;

                        if (remove)
                        {
                            retval = retval.Remove(i, 1);
                            i -= 1;
                        }

                        openItalicTags++;
                        openTags++;

                        return i;
                    };

                    for (int i = 0; i < retval.Length; i++)
                    {
                        if ((int)retval[i] == 3)
                        {
                            if (openColourTags >= 1)
                            {
                                // Close all tags, and reopen any bold, italic or underline tags that were previously open
                                while (openTags > 0)
                                {
                                    retval = retval.Insert(i, closeTag);
                                    openTags--;
                                    i += closeTag.Length;
                                }

                                openColourTags = 0;

                                int count = openBoldTags;
                                openBoldTags = 0;
                                for (int t = 0; t < count; t++)
                                {
                                    i = insertBoldTag.Invoke(i, false);
                                }

                                count = openItalicTags;
                                openItalicTags = 0;
                                for (int t = 0; t < count; t++)
                                {
                                    i = insertItalicTag.Invoke(i, false);
                                }

                                count = openUnderlineTags;
                                openUnderlineTags = 0;
                                for (int t = 0; t < count; t++)
                                {
                                    i = insertUnderlineTag.Invoke(i, false);
                                }
                            }

                            Match match = colourRegex.Match(retval.Substring(i));

                            if (match.Success)
                            {
                                if (match.Groups["background"].Success)
                                {
                                    currentBackgroundColour = int.Parse(match.Groups["background"].Value);
                                }

                                currentForegroundColour = int.Parse(match.Groups["foreground"].Value);
                                
                                if (currentForegroundColour >= 0 && currentForegroundColour <= 16)
                                {
                                    // Reset any background colour higher than 16 to -1.
                                    currentBackgroundColour = currentBackgroundColour > 16 ? -1 : currentBackgroundColour;

                                    string newTag = string.Empty;
                                    newTag = string.Format(
                                        "<span style=\"color: {0}; background-color: {1};\">",
                                        colours[currentForegroundColour],
                                        currentBackgroundColour == -1 ? "transparent" : colours[currentBackgroundColour]);

                                    retval = retval.Insert(i, newTag);
                                    i += newTag.Length;
                                    retval = retval.Remove(i, match.Length);
                                    i -= 1;

                                    openColourTags++;
                                    openTags++;
                                }
                                else
                                {
                                    // If the colour specified is invalid, remove it from the output
                                    retval = retval.Remove(i, match.Length);
                                    i -= 1;
                                }
                            }
                        }
                        else if ((int)retval[i] == 2)
                        {
                            i = insertBoldTag.Invoke(i, true);
                        }
                        else if ((int)retval[i] == 31)
                        {
                            i = insertUnderlineTag.Invoke(i, true);
                        }
                        else if ((int)retval[i] == 29)
                        {
                            i = insertItalicTag.Invoke(i, true);
                        }
                        else if ((int)retval[i] == 15)
                        {
                            currentBackgroundColour = -1;
                            currentForegroundColour = -1;

                            while (openTags > 0)
                            {
                                retval = retval.Insert(i, closeTag);
                                openTags--;
                                i += closeTag.Length;
                            }
                        }
                    }

                    while (openTags > 0)
                    {
                        retval = retval + closeTag;
                        openTags--;
                    }

                    // Replace char 65533 with &raquo; (») in channels, and with &nbsp;
                    // in server tabs as it's used by some servers for blank characters
                    // in the MOTD;
                    if (this.TabType == IRCTabType.Channel)
                    {
                        retval = retval.Replace(((char)65533).ToString(), "&raquo;");
                    }
                    else
                    {
                        retval = retval.Replace(((char)65533).ToString(), "&nbsp;<wbr/>");
                    }

                    return retval;
                });
        }

        /// <summary>
        /// Scrolls to the bottom of the chat log.
        /// </summary>
        public void ScrollToBottom()
        {
            this.WebBrowser.Document.Window.ScrollTo(0, this.WebBrowser.Document.Window.Size.Height);
        }

        /// <summary>
        /// Releases the unmanaged resources used by the System.Windows.Forms.Control and its child controls and optionally releases the managed resources.
        /// </summary>
        /// <param name="disposing">true to release both managed and unmanaged resources; false to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            if (this.UserTreeView != null)
            {
                if ((this.UserTreeView.Parent != null) && (this.UserTreeView.Parent.Controls != null))
                {
                    this.UserTreeView.Parent.Controls.Remove(this.userTreeView);
                }

                this.UserTreeView.Dispose();
            }

            base.Dispose(disposing);
        }

        /// <summary>
        /// Handles the DocumentCompleted event of System.Windows.Forms.WebBrowser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WebBrowser_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            this.webBrowser.Document.Body.ScrollIntoView(false);
            this.webBrowser.Tag = true;
        }

        /// <summary>
        /// Handles the PreviewKeyDown event of System.Windows.Forms.WebBrowser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WebBrowser_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
            if ((e.Modifiers == Keys.Control) && (e.KeyCode == Keys.C))
            {
                this.webBrowser.Document.ExecCommand("Copy", false, null);
            }
        }

        /// <summary>
        /// Handles the Navigating event of System.Windows.Forms.WebBrowser.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void WebBrowser_Navigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            if (!e.Url.ToString().Equals("about:blank"))
            {
                System.Diagnostics.Process.Start(e.Url.ToString());
                e.Cancel = true;
            }

            if (this.webBrowser.Tag != null && this.webBrowser.Tag.Equals(true))
            {
                e.Cancel = true;
            }
        }

        /// <summary>
        /// Set the splash text that is shown at the top of a tab's browser control
        /// </summary>
        private void SetSplashText()
        {
            HtmlElement splashText = this.webBrowser.Document.Body.Children.GetElementsByName("splash")[0];
            splashText.InnerText = this.Text;
        }

        /// <summary>
        /// Change the collapsed state of a node in <see cref="UserTreeView"/>.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <param name="collapsed">A value indicating whether or not the node should be collapsed.</param>
        private void ChangeTreeNodeState(string name, bool collapsed)
        {
            TreeNode node = this.UserTreeView.Nodes[name];
            if (node != null)
            {
                if (collapsed)
                {
                    node.Collapse();
                }
                else
                {
                    node.Expand();
                }
            }
        }

        /// <summary>
        /// Removes a user from the user tree view.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <param name="nodes">The nodes to search within.</param>
        /// <returns>true on removal, false if the node was not found.</returns>
        private bool RemoveUserFromList(IRCUser user, TreeNodeCollection nodes)
        {
            for (int i = 0; i < nodes.Count; i++)
            {
                if ((nodes[i].Tag != null) && (nodes[i].Tag == user))
                {
                    nodes.RemoveAt(i);
                    return true;
                }
                else
                {
                    if (this.RemoveUserFromList(user, nodes[i].Nodes))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Removes a user from the top level nodes in <see cref="UserTreeView"/>.
        /// </summary>
        /// <param name="user">The user to remove.</param>
        /// <returns>true upon removal, false if not found.</returns>
        private bool RemoveUserFromTopLevelNodes(IRCUser user)
        {
            for (int i = 0; i < this.UserTreeView.Nodes.Count; i++)
            {
                if (this.UserTreeView.Nodes[i].Tag == user)
                {
                    this.UserTreeView.Nodes.RemoveAt(i);
                    break;
                }
            }

            return true;
        }

        /// <summary>
        /// Update the fields that store the collapse state of the various "group nodes".
        /// </summary>
        /// <param name="node">The node to check.</param>
        private void HandleCollapseAndExpansion(TreeNode node)
        {
            if (node.Name.Equals(this.adminGroupName))
            {
                this.adminGroupExpanded = node.IsExpanded;
            }
            else if (node.Name.Equals(this.founderGroupName))
            {
                this.founderGroupExpanded = node.IsExpanded;
            }
            else if (node.Name.Equals(this.halfOperatorGroupName))
            {
                this.halfOperatorGroupExpanded = node.IsExpanded;
            }
            else if (node.Name.Equals(this.operatorGroupName))
            {
                this.operatorGroupExpanded = node.IsExpanded;
            }
            else if (node.Name.Equals(this.voiceGroupName))
            {
                this.voiceGroupExpanded = node.IsExpanded;
            }
            else if (node.Name.Equals(this.normalGroupName))
            {
                this.normalGroupExpanded = node.IsExpanded;
            }
        }

        /// <summary>
        /// Handles the AfterExpand event of System.Windows.Forms.TreeView
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UserTreeView_AfterExpand(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                this.HandleCollapseAndExpansion(e.Node);
            }
        }

        /// <summary>
        /// Handles the AfterCollapse event of System.Windows.Forms.TreeView
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void UserTreeView_AfterCollapse(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Level == 0)
            {
                this.HandleCollapseAndExpansion(e.Node);
            }
        }

        /// <summary>
        /// Sets up the default content for the console tab.
        /// </summary>
        private void SetupConsoleContent()
        {
            string source = "[HELLO]";
            this.AppendMessage(null, source, "Welcome to yaircc! For more information about yaircc, visit the official website at http://www.yaircc.com/.", MessageType.WelcomeMessage);
            this.AppendMessage(null, source, "To see a list of the supported commands type /commands.", MessageType.WelcomeMessage);
        }

        /// <summary>
        /// Initialise the WebBrowser control
        /// </summary>
        private void InitialiseWebBrowser()
        {
            // WebBrowser
            this.webBrowser = new WebBrowser();
            this.webBrowser.Name = string.Format("{0}WebBrowser", this.Name);

            // Setup the event handlers for the WebBrowser
            this.webBrowser.DocumentCompleted += new WebBrowserDocumentCompletedEventHandler(this.WebBrowser_DocumentCompleted);
            this.webBrowser.Navigating += new WebBrowserNavigatingEventHandler(this.WebBrowser_Navigating);
            this.webBrowser.PreviewKeyDown += new PreviewKeyDownEventHandler(this.WebBrowser_PreviewKeyDown);

            // Setup properties to fill the tab and prevent shortcuts
            this.webBrowser.Dock = DockStyle.Fill;
            this.webBrowser.WebBrowserShortcutsEnabled = false;
            this.webBrowser.IsWebBrowserContextMenuEnabled = false;

            // Load a blank document
            this.webBrowser.Navigate("about:blank");
            if (this.webBrowser.Document != null)
            {
                this.webBrowser.Document.Write(string.Empty);
            }

            // Load the appropriate resources into the web browser
            string resourceName = "Yaircc.UI.default.htm";
            switch (this.type)
            {
                case IRCTabType.Console:
                    resourceName = "Yaircc.UI.default.htm";
                    break;
            }

            using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourceName))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    this.webBrowser.Document.Write(reader.ReadToEnd());
                }
            }

            if (this.JavaScriptIsAvailable)
            {
                this.SetSplashText();

                if (this.type == IRCTabType.Console)
                {
                    this.SetupConsoleContent();
                }
            }
            else
            {
                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("Yaircc.UI.JavaScriptWarning.htm"))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        this.WebBrowser.Document.OpenNew(true);
                        this.WebBrowser.Document.Write(reader.ReadToEnd());
                    }
                }
            }

            this.Controls.Add(this.webBrowser);
        }

        /// <summary>
        /// Initialise the required components
        /// </summary>
        private void Initialise()
        {
            this.InitialiseWebBrowser();

            GlobalSettings settings = GlobalSettings.Instance;
            this.LoadTheme(settings.ThemeFileName);
        }

        /// <summary>
        /// Initialise the component.
        /// </summary>
        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ResumeLayout(false);
        }

        #endregion
    }
}