//-----------------------------------------------------------------------
// <copyright file="NickNameTypeConverter.cs" company="intninety">
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
