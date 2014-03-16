//-----------------------------------------------------------------------
// <copyright file="GlobalSettings.cs" company="rastating">
//     yaircc - the free, open-source IRC client for Windows.
//     Copyright (C) 2012-2014 Robert Carr
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

namespace Yaircc.Settings
{
    using System;
    using System.ComponentModel;
    using System.IO;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents the global settings of the application.
    /// </summary>
    public sealed class GlobalSettings
    {
        #region Fields

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly GlobalSettings SingletonInstance = new GlobalSettings();

        /// <summary>
        /// The preferred nick name to use when connecting to a server.
        /// </summary>
        private string nickName;

        /// <summary>
        /// The preferred user name to use when connecting to a server.
        /// </summary>
        private string userName;

        /// <summary>
        /// The real name to use when connecting to a server.
        /// </summary>
        private string realName;

        /// <summary>
        /// The default mode string to use when connecting to a server.
        /// </summary>
        private string defaultMode;

        /// <summary>
        /// The flag that indicates whether or not to group users by their role in the users pane.
        /// </summary>
        private GlobalSettings.Boolean groupUsersByMode;

        /// <summary>
        /// The full path to the theme to use.
        /// </summary>
        private string themeFileName;

        /// <summary>
        /// The flag that indicates whether or not to enable the developer debugging features.
        /// </summary>
        private GlobalSettings.Boolean debugMode;

        /// <summary>
        /// The flag that indicates whether or not to check for updates to the application when it is started.
        /// </summary>
        private GlobalSettings.Boolean checkForUpdateOnStart;

        /// <summary>
        /// The flag that indicates whether or not to replace textual emoticons with graphical ones.
        /// </summary>
        private GlobalSettings.Boolean useEmoticons;

        /// <summary>
        /// The flag that indicates whether or not to open links in a private browser.
        /// </summary>
        private GlobalSettings.Boolean usePrivateBrowsing;

        /// <summary>
        /// The flag that indicates whether or not to use embedded media players when a user posts a media link.
        /// </summary>
        private GlobalSettings.Boolean useEmbeddedMedia;

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="GlobalSettings" /> class from being created.
        /// </summary>
        private GlobalSettings()
        {
            Yaircc.Properties.Settings settings = Yaircc.Properties.Settings.Default;
            string defaultName = Regex.Replace(Environment.MachineName, @"[^a-zA-Z0-9]+", string.Empty);
            if (defaultName.Length > 8)
            {
                defaultName = defaultName.Substring(0, 8);
            }

            this.nickName = settings.NickName;
            if (string.IsNullOrEmpty(this.nickName))
            {
                if (defaultName[0].IsInt32())
                {
                    this.nickName = string.Format("_{0}", defaultName);
                }
                else
                {
                    this.nickName = defaultName;
                }
            }

            this.userName = settings.UserName;
            if (string.IsNullOrEmpty(this.userName))
            {
                this.userName = defaultName;
            }

            this.realName = settings.RealName;
            if (string.IsNullOrEmpty(this.realName))
            {
                this.realName = defaultName;
            }

            if (string.IsNullOrEmpty(settings.ThemeFileName) || !File.Exists(settings.ThemeFileName))
            {
                string rootPath = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                this.themeFileName = Path.Combine(rootPath, @"themes\default.css");
            }
            else
            {
                this.themeFileName = settings.ThemeFileName;
            }

            this.defaultMode = Yaircc.Properties.Settings.Default.Mode;
            if (string.IsNullOrEmpty(this.defaultMode))
            {
                this.defaultMode = "+ix";
            }

            this.groupUsersByMode = Yaircc.Properties.Settings.Default.GroupUsersByMode ? GlobalSettings.Boolean.Yes : GlobalSettings.Boolean.No;
            this.debugMode = Yaircc.Properties.Settings.Default.DebugMode ? GlobalSettings.Boolean.Yes : GlobalSettings.Boolean.No;
            this.checkForUpdateOnStart = Yaircc.Properties.Settings.Default.CheckForUpdateOnStart ? GlobalSettings.Boolean.Yes : GlobalSettings.Boolean.No;
            this.useEmoticons = Yaircc.Properties.Settings.Default.UseEmoticons ? GlobalSettings.Boolean.Yes : GlobalSettings.Boolean.No;
            this.usePrivateBrowsing = Yaircc.Properties.Settings.Default.UsePrivateBrowsing ? GlobalSettings.Boolean.Yes : GlobalSettings.Boolean.No;
            this.useEmbeddedMedia = Yaircc.Properties.Settings.Default.UseEmbeddedMedia ? GlobalSettings.Boolean.Yes : GlobalSettings.Boolean.No;
        }

        #endregion

        #region Enums

        /// <summary>
        /// A yes/no representation of a boolean value.
        /// </summary>
        public enum Boolean
        {
            /// <summary>
            /// Represents the boolean value true.
            /// </summary>
            Yes = 1,

            /// <summary>
            /// Represents the boolean value false.
            /// </summary>
            No = 0
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton instance of the class.
        /// </summary>
        public static GlobalSettings Instance
        {
            get { return GlobalSettings.SingletonInstance; }
        }

        /// <summary>
        /// Gets or sets the preferred nick name to use when connecting to a server.
        /// </summary>
        [Category("Connection"), DisplayName("Nick name"), Description("The default nick name to initially use when connecting to a server. Must be 2-9 characters long and may only contain letters, numbers or one of the following special characters: \"[\", \"]\", \"\\\", \"`\", \"_\", \"^\", \"{\", \"|\", \"}\", \"-\"")]
        public string NickName
        {
            get
            {
                return this.nickName;
            }

            set
            {
                string pattern = @"^[\[\]\\`_\^\{\|\}a-zA-Z]{1}[\[\]\\`_\^\{\|\}a-zA-Z0-9\-]+$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(value.ToString()))
                {
                    throw new ArgumentException("Nick names may only contain letters, numbers or one of the special characters below.\r\n\r\n" +
                                                "\"[\", \"]\", \"\\\", \"`\", \"_\", \"^\", \"{\", \"|\", \"}\", \"-\"\r\n\r\n" +
                                                "Names can also not start with a number or hyphen.");
                }
                else
                {
                    this.nickName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the preferred user name to use when connecting to a server.
        /// </summary>
        [Category("Connection"), DisplayName("User name"), Description("The default user name to register with when connecting to a server. May contain any character except spaces and \"@\".")]
        public string UserName
        {
            get
            {
                return this.userName;
            }

            set
            {
                if (value.Contains("@"))
                {
                    throw new ArgumentException("User names cannot contain the \"@\" character.");
                }
                else if (value.Trim().Contains(" "))
                {
                    throw new ArgumentException("User names cannot contain blank spaces.");
                }
                else
                {
                    this.userName = value.Trim();
                }
            }
        }

        /// <summary>
        /// Gets or sets the real name to use when connecting to a server.
        /// </summary>
        [Category("Connection"), DisplayName("Real name"), Description("Your real name. WARNING: this will be visible by other users when using the whois command.")]
        public string RealName
        {
            get { return this.realName; }
            set { this.realName = value; }
        }

        /// <summary>
        /// Gets or sets the default mode string to use when connecting to a server.
        /// </summary>
        [Category("Connection"), DisplayName("Mode"), Description("The mode to be set once a connection has been established. Example: +x, +i, +ix etc.")]
        public string Mode
        {
            get { return this.defaultMode; }
            set { this.defaultMode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to group users by their role in the users pane.
        /// </summary>
        [Category("User Interface"), DisplayName("Group users by role"), Description("Toggles whether or not to group the users by role in the users pane on the left hand side of the screen when joining a channel. The supported roles are: admin, founder, half-operator, normal, operator and voice.")]
        public GlobalSettings.Boolean GroupUsersByMode
        {
            get { return this.groupUsersByMode; }
            set { this.groupUsersByMode = value; }
        }

        /// <summary>
        /// Gets or sets the full path to the theme to use.
        /// </summary>
        [Browsable(false)]
        [Category("User Interface"), DisplayName("Theme"), Description("The theme file used to skin the chat windows (Requires restart).")]
        [Editor(typeof(ThemeFileNameEditor), typeof(System.Drawing.Design.UITypeEditor))]
        public string ThemeFileName
        {
            get
            {
                return this.themeFileName;
            }

            set
            {
                if (string.IsNullOrEmpty(value))
                {
                    throw new ArgumentException("A theme file must be specified.");
                }
                else
                {
                    this.themeFileName = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use emoticons.
        /// </summary>
        [Category("User Interface"), DisplayName("Use emoticons"), Description("Enable or disable the use of graphical emoticons when the textual representations are sent or received.")]
        public GlobalSettings.Boolean UseEmoticons
        {
            get { return this.useEmoticons; }
            set { this.useEmoticons = value; }
        }

        /// <summary>
        /// Gets or sets a value that indicates whether or not to use embedded media players when a user posts a media link.
        /// </summary>
        [Category("User Interface"), DisplayName("Use embedded media players"), Description("Enable or disable the use of embedded media players when a supported URI is sent or received.\r\nCurrently supported URIs: Spotify, YouTube.")]
        public GlobalSettings.Boolean UseEmbeddedMedia
        {
            get { return this.useEmbeddedMedia; }
            set { this.useEmbeddedMedia = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to enable the developer debugging features.
        /// </summary>
        [Category("Miscellaneous"), DisplayName("Enable debug mode"), Description("Enable the developer debug features. This may cause unexpected results, only enable this if you want to help in testing an issue.")]
        public GlobalSettings.Boolean DebugMode
        {
            get { return this.debugMode; }
            set { this.debugMode = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to check for updates to the application when it is started.
        /// </summary>
        [Category("Miscellaneous"), DisplayName("Automatically check for updates"), Description("Automatically check for updates when yaircc is started.")]
        public GlobalSettings.Boolean CheckForUpdateOnStart
        {
            get { return this.checkForUpdateOnStart; }
            set { this.checkForUpdateOnStart = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to open links in a private browser.
        /// </summary>
        [Category("Miscellaneous"), DisplayName("Open links in private browsing mode"), Description("If enabled, all links will be opened in private browsing mode and will not be recorded in your web browsing history.")]
        public GlobalSettings.Boolean UsePrivateBrowsing
        {
            get { return this.usePrivateBrowsing; }
            set { this.usePrivateBrowsing = value; }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Saves the current settings.
        /// </summary>
        /// <returns>True on successful save.</returns>
        public bool Save()
        {
            bool retval = true;

            try
            {
                Yaircc.Properties.Settings.Default.NickName = this.nickName;
                Yaircc.Properties.Settings.Default.UserName = this.userName;
                Yaircc.Properties.Settings.Default.Mode = this.defaultMode;
                Yaircc.Properties.Settings.Default.RealName = this.realName;
                Yaircc.Properties.Settings.Default.DebugMode = this.debugMode == GlobalSettings.Boolean.Yes;
                Yaircc.Properties.Settings.Default.GroupUsersByMode = this.groupUsersByMode == GlobalSettings.Boolean.Yes;
                Yaircc.Properties.Settings.Default.ThemeFileName = this.themeFileName;
                Yaircc.Properties.Settings.Default.CheckForUpdateOnStart = this.checkForUpdateOnStart == GlobalSettings.Boolean.Yes;
                Yaircc.Properties.Settings.Default.UseEmoticons = this.useEmoticons == GlobalSettings.Boolean.Yes;
                Yaircc.Properties.Settings.Default.UsePrivateBrowsing = this.usePrivateBrowsing == GlobalSettings.Boolean.Yes;
                Yaircc.Properties.Settings.Default.UseEmbeddedMedia = this.useEmbeddedMedia == GlobalSettings.Boolean.Yes;
                Yaircc.Properties.Settings.Default.Save();
            }
            catch
            {
                retval = false;
            }

            return retval;
        }

        #endregion
    }
}