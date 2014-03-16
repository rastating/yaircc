//-----------------------------------------------------------------------
// <copyright file="ModeMessage.cs" company="rastating">
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

namespace Yaircc.Net.IRC
{
    using System;
    using System.Collections.Generic;
    using System.Text.RegularExpressions;
    using Yaircc.Localisation;

    /// <summary>
    /// Represents a MODE command.
    /// </summary>
    public class ModeMessage : Message
    {
        #region Constructors

        /// <summary>
        /// Initialises a new instance of the <see cref="ModeMessage"/> class.
        /// </summary>
        public ModeMessage()
            : base("MODE")
        {
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ModeMessage"/> class.
        /// </summary>
        /// <param name="target">The nick name of the user to receive the mode of.</param>
        public ModeMessage(string target)
            : base("MODE", new string[] { target }) 
        { 
        }

        /// <summary>
        /// Initialises a new instance of the <see cref="ModeMessage"/> class.
        /// </summary>
        /// <param name="messageParameters">The parameters to pass to the mode command.</param>
        public ModeMessage(string[] messageParameters)
            : base(string.Empty, "MODE", messageParameters, string.Empty)
        {
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the source of the message.
        /// </summary>
        public override string Source
        {
            get
            {
                return Strings_General.NotificationSource;
            }
        }

        /// <summary>
        /// Gets the content of the message.
        /// </summary>
        public override string Content
        {
            get
            {
                return string.Format("Mode for {0} changed to {1} by {2}", this.Nickname, this.ModeString, base.Source);
            }
        }

        /// <summary>
        /// Gets the nick name of the user having their mode changed.
        /// </summary>
        public string Nickname
        {
            get
            {
                if (this.Parameters.Length > 2)
                {
                    return this.Parameters[2];
                }
                else
                {
                    return this.Parameters[0];
                }
            }
        }

        /// <summary>
        /// Gets the recipient the message was to be processed by. This can be either a channel name or a nick name.
        /// </summary>
        public override string Target
        {
            get { return this.Parameters[0]; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the message must be handled by its target.
        /// </summary>
        public override bool MustBeHandledByTarget
        {
            get
            {
                return this.Nickname.StartsWithEither("#", "&") || this.Parameters.Length > 2;
            }
        }

        /// <summary>
        /// Gets the new mode string.
        /// </summary>
        public string ModeString
        {
            get
            {
                if (this.Parameters.Length > 1)
                {
                    return this.Parameters[1];
                }
                else if (!string.IsNullOrEmpty(this.TrailingParameter))
                {
                    return this.TrailingParameter;
                }
                else
                {
                    return string.Empty;
                }
            }
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

            if (input.ToLower().StartsWithEither(new string[] { @"/ban", @"/unban" }))
            {
                retval = this.ParseBanCommand(input);
            }
            else if (input.ToLower().StartsWithEither(new string[] { @"/deop", @"/dehop", @"/op", @"/hop" }))
            {
                retval = this.ParseOpCommand(input);
            }
            else if (input.ToLower().StartsWithEither(new string[] { @"/except", @"/unexcept" }))
            {
                retval = this.ParseExceptCommand(input);
            }
            else if (input.StartsWith(@"/voice", StringComparison.OrdinalIgnoreCase))
            {
                retval = this.ParseVoiceCommand(input);
            }
            else
            {
                retval = this.ParseModeCommand(input);
            }

            return retval;
        }

        /// <summary>
        /// Parse a ban command into the message.
        /// </summary>
        /// <param name="payload">The raw ban command.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParseBanCommand(string payload)
        {
            ParseResult retval;
            string pattern = @"/(?<command>ban|unban)(\s+(?<mask>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(payload);

            if (match.Success)
            {
                if (string.IsNullOrEmpty(this.ChannelName))
                {
                    retval = new ParseResult(false, match.Groups["command"].Value.Equals("ban", StringComparison.OrdinalIgnoreCase) ? Strings_MessageParseResults.Ban_InvalidContext : Strings_MessageParseResults.Unban_InvalidContext);
                }
                else
                {
                    // If we have a mask then we need to ban or unban it, otherwise
                    // the user is requesting a list of bans on the channel
                    // that the message has been instantiated on
                    if (match.Groups["mask"].Success)
                    {
                        if (match.Groups["command"].Value.Equals("ban", StringComparison.OrdinalIgnoreCase))
                        {
                            this.Parameters = new string[] { this.ChannelName, "+b", match.Groups["mask"].Value };
                        }
                        else
                        {
                            this.Parameters = new string[] { this.ChannelName, "-b", match.Groups["mask"].Value };
                        }

                        retval = new ParseResult(true, string.Empty);
                    }
                    else
                    {
                        // The lack of a mask is only valid if we are using /ban
                        if (match.Groups["command"].Value.Equals("ban", StringComparison.OrdinalIgnoreCase))
                        {
                            this.Parameters = new string[] { this.ChannelName, "+b" };
                            retval = new ParseResult(true, string.Empty);
                        }
                        else
                        {
                            retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Nickname);
                        }
                    }
                }
            }
            else
            {
                retval = new ParseResult(false, string.Empty);
            }

            return retval;
        }

        /// <summary>
        /// Parse a deop/dehop/hop/op command into the message.
        /// </summary>
        /// <param name="payload">The raw deop/dehop/hop/op command.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParseOpCommand(string payload)
        {
            ParseResult retval;
            string pattern = @"/(?<command>deop|dehop|hop|op)\s+((?<channel>[#&!\+][^\x07\x2C\s]{1,199})\s+)?(?<nickname>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(payload);

            if (match.Success)
            {
                string modifier = match.Groups["command"].Value.StartsWith("de") ? "-" : "+";
                string mode = match.Groups["command"].Value.EndsWith("hop") ? "h" : "o";
                string channel = this.ChannelName;

                if (match.Groups["channel"].Success)
                {
                    channel = match.Groups["channel"].Value;
                }

                this.Parameters = new string[] { channel, modifier + mode, match.Groups["nickname"].Value };
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Nickname);
            }

            return retval;
        }

        /// <summary>
        /// Parse an except/unexcept command into the message.
        /// </summary>
        /// <param name="payload">The raw except/unexcept command.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParseExceptCommand(string payload)
        {
            ParseResult retval;
            string pattern = @"/(?<command>except|unexcept)(\s+(?<mask>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(payload);

            if (match.Success)
            {
                if (string.IsNullOrEmpty(this.ChannelName))
                {
                    string resultMessage = match.Groups["command"].Value.Equals("except", StringComparison.OrdinalIgnoreCase) ? 
                                                        Strings_MessageParseResults.Except_InvalidContext : 
                                                        Strings_MessageParseResults.Unexcept_InvalidContext;
                    retval = new ParseResult(false, resultMessage);
                }
                else
                {
                    // If we have a mask then we need to except it, otherwise
                    // the user is requesting a list of exceptions on the channel
                    // that the message has been instantiated on
                    if (match.Groups["mask"].Success)
                    {
                        if (match.Groups["command"].Value.Equals("except", StringComparison.OrdinalIgnoreCase))
                        {
                            this.Parameters = new string[] { this.ChannelName, "+e", match.Groups["mask"].Value };
                        }
                        else
                        {
                            this.Parameters = new string[] { this.ChannelName, "-e", match.Groups["mask"].Value };
                        }

                        retval = new ParseResult(true, string.Empty);
                    }
                    else
                    {
                        if (match.Groups["command"].Value.Equals("except", StringComparison.OrdinalIgnoreCase))
                        {
                            this.Parameters = new string[] { this.ChannelName, "+e" };
                            retval = new ParseResult(true, string.Empty);
                        }
                        else
                        {
                            retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Nickname);
                        }
                    }                    
                }
            }
            else
            {
                retval = new ParseResult(false, string.Empty);
            }

            return retval;
        }

        /// <summary>
        /// Parse a mode command into the message.
        /// </summary>
        /// <param name="payload">The raw mode command.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParseModeCommand(string payload)
        {
            ParseResult retval;
            string pattern = @"/mode(\s+(?<target>[^\s]+))?(\s+(?<modestr>[^\s]+))?(\s+(?<args>.+))?";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(payload);

            if (match.Success)
            {
                List<string> parameters = new List<string>();

                if (match.Groups["target"].Success)
                {
                    parameters.Add(match.Groups["target"].Value);

                    if (match.Groups["modestr"].Success)
                    {
                        parameters.Add(match.Groups["modestr"].Value);

                        if (match.Groups["args"].Success)
                        {
                            parameters.AddRange(match.Groups["args"].Value.Split(new string[] { " " }, StringSplitOptions.RemoveEmptyEntries));
                        }
                    }
                }

                this.Parameters = parameters.ToArray();
                retval = new ParseResult(true, string.Empty);
            }
            else
            {
                retval = new ParseResult(false, string.Empty);
            }

            return retval;
        }

        /// <summary>
        /// Parse a voice command into the message.
        /// </summary>
        /// <param name="payload">The raw voice command.</param>
        /// <returns>A <see cref="ParseResult"/> that contains the result of the operation.</returns>
        private ParseResult ParseVoiceCommand(string payload)
        {
            ParseResult retval;
            string pattern = @"/voice\s+(?<mask>.+)";
            Regex regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Match match = regex.Match(payload);

            if (match.Success)
            {
                if (string.IsNullOrEmpty(this.ChannelName))
                {
                    retval = new ParseResult(false, Strings_MessageParseResults.Voice_InvalidContext);
                }
                else
                {
                    this.Parameters = new string[] { this.ChannelName, "+v", match.Groups["mask"].Value };
                    retval = new ParseResult(true, string.Empty);
                }
            }
            else
            {
                retval = new ParseResult(false, Strings_MessageParseResults.MissingParameter_Nickname);
            }

            return retval;
        }

        #endregion
    }
}
