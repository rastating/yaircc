//-----------------------------------------------------------------------
// <copyright file="WhoMessage.cs" company="intninety">
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

namespace Yaircc.Net.IRC
{
    using System;
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a WHO command.
    /// </summary>
    public class WhoMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoMessage"/> class.
        /// </summary>
        public WhoMessage()
            : base("WHO") 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoMessage"/> class.
        /// </summary>
        /// <param name="name">The name of the user to get the who information of.</param>
        public WhoMessage(string name)
            : base("WHO", new string[] { name }) 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="WhoMessage"/> class.
        /// </summary>
        /// <param name="name">The name of the user to get the who information of.</param>
        /// <param name="operatorFlag">A flag to indicate whether or not to return information about only IRC operators.</param>
        public WhoMessage(string name, string operatorFlag)
            : base("WHO", new string[] { name, operatorFlag }) 
        {
        }

        #endregion

        #region Instance Methods

        /// <summary>
        /// Attempts to parse the input specified by the user into the Message.
        /// </summary>
        /// <param name="input">The input from the user.</param>
        /// <returns>True on success, false on failure.</returns>
        public override ParseResult TryParse(string input)
        {
            ParseResult retval;
            string pattern = @"/who\s+(?<mask>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(input);

            if (match.Success)
            {
                this.Parameters = match.Groups["mask"].Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries);

                if (this.Parameters.Length > 0)
                {
                    retval = new ParseResult(true, string.Empty);
                }
                else
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Mask);
                }
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Mask);
            }

            return retval;
        }

        #endregion
    }
}
