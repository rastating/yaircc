//-----------------------------------------------------------------------
// <copyright file="NickNameTypeConverter.cs" company="intninety">
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
    using System.ComponentModel;
    using System.Text.RegularExpressions;

    /// <summary>
    /// Represents a type converter for IRC nick names.
    /// </summary>
    public class NickNameTypeConverter : TypeConverter
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="NickNameTypeConverter"/> class.
        /// </summary>
        public NickNameTypeConverter() 
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Returns whether this converter can convert an object of the given type to the type of this converter, using the specified context. 
        /// </summary>
        /// <param name="context">The format context.</param>
        /// <param name="sourceType">The type you want to convert from.</param>
        /// <returns>true if this converter can perform the conversion; false otherwise.</returns>
        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
        }

        /// <summary>
        /// Converts the given object to the type of this converter, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">The System.Globalization.CultureInfo to use as the current culture.</param>
        /// <param name="value">The System.Object to convert.</param>
        /// <returns>An System.Object that represents the converted value.</returns>
        public override object ConvertFrom(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value)
        {
            if (!(value is string))
            {
                return base.ConvertFrom(context, culture, value);
            }
            else
            {
                string pattern = @"^[\[\]\\`_\^\{\|\}a-zA-Z]{1}[\[\]\\`_\^\{\|\}a-zA-Z0-9\-]+$";
                Regex regex = new Regex(pattern);

                if (!regex.IsMatch(value.ToString()))
                {
                    throw new FormatException(@"Nick names may only contain letters, numbers or one of the special characters below. Names can also not start with a number or hyphen. 
                                                test");
                }

                return value;
            }
        }

        /// <summary>
        /// Converts the given value object to the specified type, using the specified context and culture information.
        /// </summary>
        /// <param name="context">An System.ComponentModel.ITypeDescriptorContext that provides a format context.</param>
        /// <param name="culture">A System.Globalization.CultureInfo. If null is passed, the current culture is assumed.</param>
        /// <param name="value">The System.Object to convert.</param>
        /// <param name="destinationType">The System.Type to convert the value parameter to.</param>
        /// <returns>An System.Object that represents the converted value.</returns>
        public override object ConvertTo(ITypeDescriptorContext context, System.Globalization.CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == null)
            {
                throw new ArgumentNullException("destinationType");
            }
            else if (destinationType == typeof(string))
            {
                return value;
            }
            else
            {
                return base.ConvertTo(context, culture, value, destinationType);
            }
        }

        #endregion
    }
}
