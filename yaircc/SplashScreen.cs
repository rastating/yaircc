//-----------------------------------------------------------------------
// <copyright file="SplashScreen.cs" company="intninety">
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

namespace Yaircc
{
    using System;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Represents the splash screen.
    /// </summary>
    public partial class SplashScreen : Form
    {
        /// <summary>
        /// The owning form.
        /// </summary>
        private MainForm mainForm;

        /// <summary>
        /// Initialises a new instance of the <see cref="SplashScreen"/> class.
        /// </summary>
        /// <param name="mainForm">The owning form.</param>
        public SplashScreen(MainForm mainForm)
        {
            this.InitializeComponent();

            this.mainForm = mainForm;
            this.versionLabel.Text = string.Format("Version {0}", Assembly.GetExecutingAssembly().GetName().Version.ToString());
            this.timer.Enabled = true;
        }

        /// <summary>
        /// Handles the Tick event of System.Windows.Forms.Timer.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void Timer_Tick(object sender, EventArgs e)
        {
            this.mainForm.SetupForm();
            this.timer.Enabled = false;
        }
    }
}
