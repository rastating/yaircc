//-----------------------------------------------------------------------
// <copyright file="Themes.cs" company="rastating">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Linq;

    /// <summary>
    /// Represents the collection of installed themes on the system.
    /// </summary>
    public class Themes
    {
        #region Fields

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly Themes SingletonInstance = new Themes();

        /// <summary>
        /// The installed themes.
        /// </summary>
        private List<Theme> themes;

        #endregion

        #region Constructors

        /// <summary>
        /// Prevents a default instance of the <see cref="Themes" /> class from being created.
        /// </summary>
        private Themes()
        {
            if (!Directory.Exists(Directory.GetParent(this.CustomThemesPath).FullName))
            {
                Directory.CreateDirectory(Directory.GetParent(this.CustomThemesPath).FullName);
            }

            if (!Directory.Exists(this.CustomThemesPath))
            {
                Directory.CreateDirectory(this.CustomThemesPath);
            }

            this.themes = new List<Theme>();
            this.PopulateThemes();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton instance of the class.
        /// </summary>
        public static Themes Instance
        {
            get { return Themes.SingletonInstance; }
        }

        /// <summary>
        /// Gets the installed themes.
        /// </summary>
        public List<Theme> InstalledThemes
        {
            get { return this.themes; }
        }

        /// <summary>
        /// Gets the path to the official themes directory.
        /// </summary>
        private string OfficialThemesPath
        {
            get
            {
                return Path.Combine(Path.GetDirectoryName(System.Windows.Forms.Application.ExecutablePath), @"themes");
            }
        }

        /// <summary>
        /// Gets the path to the custom themes directory.
        /// </summary>
        private string CustomThemesPath
        {
            get
            {
                string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(roamingPath, @"yaircc\themes");
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Installs the theme at the specified path.
        /// </summary>
        /// <param name="path">The path of the theme to install.</param>
        /// <returns>The newly installed theme, or null if it failed to install.</returns>
        public Theme InstallTheme(string path)
        {
            Theme retval = this.CreateThemeFromFile(path, false);
            if (retval != null)
            {
                string fileName = Path.Combine(this.CustomThemesPath, retval.ID.ToString() + ".css");
                File.Copy(path, fileName, true);

                // If a theme with the same Guid already exists, replace it with the new one if the
                // existing theme is not an official theme.
                Theme existingTheme = this.themes.Find(t => t.ID.Equals(retval.ID));
                if (existingTheme == null)
                {
                    this.themes.Add(retval);
                }
                else if (!existingTheme.IsOfficial)
                {
                    this.themes.Remove(existingTheme);
                    this.themes.Add(retval);
                }
                else
                {
                    retval = null;
                }
            }

            return retval;
        }

        /// <summary>
        /// Uninstalls the specified theme from the system.
        /// </summary>
        /// <param name="theme">The theme to uninstall.</param>
        public void UninstallTheme(Theme theme)
        {
            File.Delete(theme.Path);
            this.themes.Remove(theme);
        }

        /// <summary>
        /// Populates the collection of themes.
        /// </summary>
        private void PopulateThemes()
        {
            this.themes.Clear();
            Action<string, bool> loadThemes = (path, isDefault) =>
                {
                    DirectoryInfo info = new DirectoryInfo(path);
                    FileInfo[] themes = info.GetFiles("*.css", SearchOption.TopDirectoryOnly);
                    for (int i = 0; i < themes.Length; i++)
                    {
                        Theme theme = this.CreateThemeFromFile(themes[i].FullName, isDefault);
                        if (theme != null)
                        {
                            this.themes.Add(theme);
                        }
                    }
                };

            loadThemes.Invoke(this.OfficialThemesPath, true);
            loadThemes.Invoke(this.CustomThemesPath, false);            
        }

        /// <summary>
        /// Get the XML portion of the specified theme.
        /// </summary>
        /// <param name="fileName">The theme to retrieve the XML header from.</param>
        /// <returns>An XElement encapsulating the XML from the theme.</returns>
        private XElement GetThemeXML(string fileName)
        {
            XElement retval = null;
            string[] lines = File.ReadAllLines(fileName);
            StringBuilder sb = new StringBuilder();

            // Valid yaircc themes must begin with a comment, the XML which defines
            // the theme is found between this opening asterisk and the closing one.
            if (lines[0].StartsWith("/*", StringComparison.OrdinalIgnoreCase))
            {
                // Build the theme XML
                int i = 1;
                while (!lines[i].StartsWith("*/", StringComparison.OrdinalIgnoreCase))
                {
                    sb.AppendLine(lines[i]);
                    i++;
                }

                // Attempt to parse the XML string, should an exception occur we should
                // consider this an invalid yaircc theme and return null.
                try
                {
                    retval = XElement.Parse(sb.ToString());
                }
                catch
                {
                }
            }

            return retval;
        }

        /// <summary>
        /// Initialise a new instance of the <see cref="Theme" /> class using the specified file.
        /// </summary>
        /// <param name="fileName">The file to create a theme from.</param>
        /// <param name="isOfficial">A value indicating whether or not the theme is an official theme.</param>
        /// <returns>The instantiated theme.</returns>
        private Theme CreateThemeFromFile(string fileName, bool isOfficial)
        {
            Theme retval = null;
            XElement root = this.GetThemeXML(fileName);
            if (root != null)
            {
                XElement author = root.Descendants("author").First();
                XElement description = root.Descendants("description").First();

                Guid themeId = new Guid(root.Attribute("id").Value);
                string themeName = root.Attribute("name").Value;
                string authorWebsite = author.Attribute("website").Value;
                string authorName = author.Value;

                retval = new Theme(themeId, themeName, authorName, authorWebsite, description.Value, isOfficial, fileName);
            }

            return retval;
        }

        #endregion
    }
}
