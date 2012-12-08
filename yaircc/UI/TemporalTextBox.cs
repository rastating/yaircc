//-----------------------------------------------------------------------
// <copyright file="TemporalTextBox.cs" company="intninety">
//     Copyright 2012 intninety
//
//     This file is part of TemporalTextBox.
//
//     TemporalTextBox is free software: you can redistribute it and/or modify
//     it under the terms of the GNU General Public License as published by
//     the Free Software Foundation, either version 3 of the License, or
//     (at your option) any later version.
//    
//     TemporalTextBox is distributed in the hope that it will be useful,
//     but WITHOUT ANY WARRANTY; without even the implied warranty of
//     MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//     GNU General Public License for more details.
//
//     You should have received a copy of the GNU General Public License
//     along with TemporalTextBox.  If not, see &lt;http://www.gnu.org/licenses/&gt;.
// </copyright>
//-----------------------------------------------------------------------

namespace Intninety
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Windows.Forms;

    /// <summary>
    /// Represents a Windows text box control capable of both undo and redo operations.
    /// </summary>
    public class TemporalTextBox : TextBox
    {
        #region Instance Members

        /// <summary>
        /// A stack containing the strings to be used in the redo operation.
        /// </summary>
        private Stack<string> redoStack;

        /// <summary>
        /// A stack containing the strings to be used in the undo operation.
        /// </summary>
        private Stack<string> undoStack;

        /// <summary>
        /// Indicates whether an undo or redo operation is in progress.
        /// </summary>
        private bool temporalOperationInProgress;

        /// <summary>
        /// Indicates whether the standard undo method will be used or the TemporalTextBox custom method.
        /// </summary>
        private bool useCustomUndoMethod;

        /// <summary>
        /// The text prior to the last text change.
        /// </summary>
        private string previousText;
        
        #endregion

        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="TemporalTextBox"/> class.
        /// </summary>
        public TemporalTextBox()
        {
            this.redoStack = new Stack<string>();
            this.undoStack = new Stack<string>();
            this.previousText = string.Empty;
            this.temporalOperationInProgress = false;
            this.useCustomUndoMethod = true;
            this.TextChanged += new EventHandler(this.TemporalTextBox_TextChanged);
            this.KeyDown += new KeyEventHandler(this.TemporalTextBox_KeyDown);
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets a value indicating whether the user can undo the previous operation in a text box control.
        /// </summary>
        public new bool CanUndo
        {
            get { return this.useCustomUndoMethod ? (this.undoStack.Count > 0 || this.Text.Length > 0) : base.CanUndo; }
        }

        /// <summary>
        /// Gets a value indicating whether the user can restore the text removed by the previous undo operation in a text box control.
        /// </summary>
        public bool CanRedo
        {
            get { return this.redoStack.Count > 0; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether or not to use the TemporalTextBox custom undo method or the default Microsoft method.
        /// </summary>
        public bool UseCustomUndoMethod
        {
            get { return this.useCustomUndoMethod; }
            set { this.useCustomUndoMethod = value; }
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Undoes the last edit operation in the text box.
        /// </summary>
        public new void Undo()
        {
            this.temporalOperationInProgress = true;
            this.redoStack.Push(this.Text);

            if (this.useCustomUndoMethod)
            {
                if (this.undoStack.Count == 0)
                {
                    this.Text = string.Empty;
                }
                else
                {
                    this.Text = this.undoStack.Pop();
                }
            }
            else
            {
                base.Undo();
            }

            this.temporalOperationInProgress = false;
        }

        /// <summary>
        /// Restores the text removed by the last undo operation in the text box.
        /// </summary>
        public void Redo()
        {
            this.temporalOperationInProgress = true;

            if (this.CanRedo)
            {
                if (this.useCustomUndoMethod)
                {
                    this.undoStack.Push(this.Text);
                }

                this.Text = this.redoStack.Pop();
            }

            this.temporalOperationInProgress = false;
        }

        /// <summary>
        /// Clears both the undo and redo stack.
        /// </summary>
        public void ClearStack()
        {
            this.undoStack.Clear();
            this.redoStack.Clear();
        }

        /// <summary>
        /// Transform the text box to be identical to <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The text box to transform into</param>
        /// <param name="hideSource">Hide the source text box after the transformation is complete</param>
        public void Transform(TextBox source, bool hideSource)
        {
            Type type = source.GetType();
            IEnumerator properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.CanWrite && !p.Name.Equals("WindowTarget")).GetEnumerator();
            while (properties.MoveNext())
            {
                PropertyInfo property = properties.Current as PropertyInfo;
                object value = property.GetValue(source, null);
                property.SetValue(this, value, null);
            }

            source.Visible = !hideSource;
            this.previousText = this.Text;
        }

        /// <summary>
        /// Transform the text box to be identical to <paramref name="source"/>.
        /// </summary>
        /// <param name="source">The text box to transform into</param>
        public void Transform(TextBox source)
        {
            this.Transform(source, false);
        }

        #endregion

        #region Event Handlers

        /// <summary>
        /// Occurs when a key is pressed while the control has focus.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void TemporalTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // If we are using the custom undo method and the user hits
            // the space key, the enter key, the backspace key or the 
            // delete key, then we need to push the current text onto 
            // the undo stack.
            if (this.useCustomUndoMethod && (e.KeyCode == Keys.Space || e.KeyCode == Keys.Enter || e.KeyCode == Keys.Back || e.KeyCode == Keys.Delete))
            {
                this.undoStack.Push(this.Text);
            }
        }

        /// <summary>
        /// Occurs when content changes in the text box.
        /// </summary>
        /// <param name="sender">The sender of the event</param>
        /// <param name="e">The event parameters</param>
        private void TemporalTextBox_TextChanged(object sender, EventArgs e)
        {
            // If the text has been changed by an operation other than an
            // undo or redo operation then we need to clear the redo stack.
            if (!this.temporalOperationInProgress)
            {
                this.redoStack.Clear();

                // If the text is not in an undo or redo operation and the
                // previous and current strings are more than one character
                // different in length, then the user has executed either a
                // cut or paste operation, and thus we have to push to the 
                // undo stack if the custom method is being used.
                if (this.useCustomUndoMethod)
                {
                    int difference = this.previousText.Length - this.Text.Length;
                    if ((difference < -1) || (difference > 1))
                    {
                        this.undoStack.Push(this.previousText);
                    }
                    else if ((difference == 0) && (!this.previousText.Equals(this.Text)))
                    {
                        this.undoStack.Push(this.previousText);
                    }
                }

                this.previousText = this.Text;
            }
        }

        #endregion
    }
}