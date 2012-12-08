//-----------------------------------------------------------------------
// <copyright file="ThemeFileNameEditor.cs" company="intninety">
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

namespace Yaircc
{
    using System;
    using System.Drawing.Design;
    using System.IO;
    using System.Windows.Forms;

    /// <summary>
    /// Represents an open file dialog customised specifically for yaircc themes.
    /// </summary>
    public class ThemeFileNameEditor : UITypeEditor
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ThemeFileNameEditor"/> class.
        /// </summary>
        public ThemeFileNameEditor()
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Gets the editor style used by the EditValue method.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <returns>A UITypeEditorEditStyle value that indicates the style of editor used by the EditValue method. If the UITypeEditor does not support this method, then GetEditStyle will return None.</returns>
        public override UITypeEditorEditStyle GetEditStyle(System.ComponentModel.ITypeDescriptorContext context)
        {
            return UITypeEditorEditStyle.Modal;
        }

        /// <summary>
        /// Edits the specified object's value using the editor style indicated by the GetEditStyle method.
        /// </summary>
        /// <param name="context">An ITypeDescriptorContext that can be used to gain additional context information.</param>
        /// <param name="provider">An IServiceProvider that this editor can use to obtain services.</param>
        /// <param name="value">The object to edit.</param>
        /// <returns>The new value of the object. If the value of the object has not changed, this should return the same object it was passed.</returns>
        public override object EditValue(System.ComponentModel.ITypeDescriptorContext context, IServiceProvider provider, object value)
        {
            string retval = string.Empty;
            using (OpenFileDialog dialog = new OpenFileDialog())
            {
                dialog.Filter = "CSS files (*.css)|*.css";
                dialog.InitialDirectory = Path.GetDirectoryName(Application.ExecutablePath);
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    retval = dialog.FileName;
                }
            }

            return retval;
        }

        #endregion
    }
}
