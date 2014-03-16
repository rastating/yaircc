//-----------------------------------------------------------------------
// <copyright file="FavouriteServers.cs" company="rastating">
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
    using System.Collections.Generic;
    using System.IO;
    using System.Xml.Serialization;

    /// <summary>
    /// Represents the collection of the user's favourite servers.
    /// </summary>
    [Serializable]
    public sealed class FavouriteServers : IAssimilable
    {
        #region Fields

        /// <summary>
        /// The singleton instance of the class.
        /// </summary>
        private static readonly FavouriteServers SingletonInstance = new FavouriteServers(true);

        /// <summary>
        /// The list of servers marked as favourites.
        /// </summary>
        private List<Server> servers;

        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="FavouriteServers"/> class.
        /// </summary>
        /// <param name="loadFromFile">A value indicating whether or not to deserialise the data from a file.</param>
        private FavouriteServers(bool loadFromFile)
        {
            this.servers = new List<Server>();

            if (!Directory.Exists(this.DataPath))
            {
                Directory.CreateDirectory(this.DataPath);
            }
            else if (File.Exists(this.FilePath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(FavouriteServers));
                using (Stream stream = File.OpenRead(this.FilePath))
                {
                    object savedServers = serializer.Deserialize(stream);
                    if (savedServers != null)
                    {
                        this.Assimilate(savedServers);
                    }
                }
            }
        }

        /// <summary>
        /// Prevents a default instance of the <see cref="FavouriteServers"/> class from being created.
        /// </summary>
        private FavouriteServers()
        {
            this.servers = new List<Server>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton instance of the class.
        /// </summary>
        public static FavouriteServers Instance
        {
            get { return FavouriteServers.SingletonInstance; }
        }

        /// <summary>
        /// Gets or sets the list of servers marked as favourites.
        /// </summary>
        public List<Server> Servers
        {
            get { return this.servers; }
            set { this.servers = value; }
        }

        /// <summary>
        /// Gets the path in which the file that stores the serialised favourites resides.
        /// </summary>
        private string DataPath
        {
            get
            {
                string roamingPath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
                return Path.Combine(roamingPath, "yaircc");
            }
        }

        /// <summary>
        /// Gets the full path to the file which stores the serialised favourites.
        /// </summary>
        private string FilePath
        {
            get
            {
                return Path.Combine(this.DataPath, "favourite-servers.xml");
            }
        }

        #endregion

        #region IAssimilable Members

        /// <summary>
        /// Assimilate all the data stored in <paramref name="obj"/>
        /// </summary>
        /// <param name="obj">The object in which to assimilate data from</param>
        public void Assimilate(object obj)
        {
            if (obj is FavouriteServers)
            {
                this.Servers = (obj as FavouriteServers).Servers;
            }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Creates a new server using the default values.
        /// </summary>
        /// <returns>The newly created server.</returns>
        public Server Create()
        {
            Server retval = new Server();
            string alias = "New Server";
            int count = 1;

            while (this.Servers.Find(i => i.Alias.Equals(alias, StringComparison.OrdinalIgnoreCase)) != null)
            {
                alias = string.Format("New Server ({0})", count);
                count++;
            }

            retval.Alias = alias;
            this.Servers.Add(retval);

            return retval;
        }

        /// <summary>
        /// Saves the class to file.
        /// </summary>
        /// <returns>true upon successful save.</returns>
        public bool Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(FavouriteServers));
            using (Stream stream = File.Create(this.FilePath))
            {
                serializer.Serialize(stream, this);
            }

            return true;
        }

        #endregion
    }
}