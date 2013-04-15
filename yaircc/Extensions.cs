//-----------------------------------------------------------------------
// <copyright file="Extensions.cs" company="rastating">
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
    using System.Net.Sockets;
    using System.Windows.Forms;
    using Yaircc.Settings;
    using Yaircc.UI;

    /// <summary>
    /// Contains extension methods for a number of classes.
    /// </summary>
    public static class Extensions
    {
        /// <summary>
        /// Invokes the control if required and then invokes the action.
        /// </summary>
        /// <param name="control">The control to invoke.</param>
        /// <param name="action">The action to invoke.</param>
        public static void InvokeAction(this Control control, Action action)
        {
            if (control.InvokeRequired)
            {
                try
                {
                    control.Invoke((Delegate)action);
                }
                catch (ObjectDisposedException) 
                { 
                }
            }
            else
            {
                action();
            }
        }

        /// <summary>
        /// Splits the string into an array on each new line.
        /// </summary>
        /// <param name="source">The string to split.</param>
        /// <returns>An array of strings.</returns>
        public static string[] GetLines(this string source)
        {
            return source.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Removes all carriage returns from the string.
        /// </summary>
        /// <param name="source">The string to remove carriage returns from.</param>
        /// <returns>The string without any carriage returns.</returns>
        public static string RemoveCarriageReturns(this string source)
        {
            return source.TrimEnd('\r', '\n');
        }

        /// <summary>
        /// Checks to see if the string starts with one of the values specified.
        /// </summary>
        /// <param name="source">The string to check.</param>
        /// <param name="strings">The strings the string may start with.</param>
        /// <returns>True if <paramref name="source"/> starts with one of the strings passed in <paramref name="strings"/></returns>
        public static bool StartsWithEither(this string source, params string[] strings)
        {
            for (int i = 0; i < strings.GetLength(0); i++)
            {
                if (source.StartsWith(strings[i]))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Checks if the string is a valid 32 bit integer.
        /// </summary>
        /// <param name="source">The string to check.</param>
        /// <returns>True if <paramref name="source"/> is a valid 32 bit integer.</returns>
        public static bool IsNumeric(this string source)
        {
            int dummy;
            return int.TryParse(source, out dummy);
        }

        /// <summary>
        /// Checks whether or not the socket is connected in real time.
        /// </summary>
        /// <param name="client">The socket to check.</param>
        /// <returns>True if the socket is still connected.</returns>
        public static bool IsConnected(this Socket client)
        {
            try
            {
                bool blockingState = client.Blocking;
                try
                {
                    byte[] tmp = new byte[1];

                    client.Blocking = false;
                    client.Send(tmp, 0, 0);
                    return true;
                }
                catch (SocketException e)
                {
                    // 10035 == WSAEWOULDBLOCK
                    if (e.NativeErrorCode.Equals(10035))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                finally
                {
                    client.Blocking = blockingState;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Checks if the char is a valid 32 bit integer.
        /// </summary>
        /// <param name="source">The char to check.</param>
        /// <returns>True if <paramref name="source"/> is a valid 32 bit integer.</returns>
        public static bool IsInt32(this char source)
        {
            int dummy;
            return int.TryParse(source.ToString(), out dummy);
        }

        /// <summary>
        /// Gets a value indicating whether or not the TabControl has a tab connected to the specified server.
        /// </summary>
        /// <param name="source">The TabControl.</param>
        /// <param name="server">The server to search for.</param>
        /// <returns>true if present.</returns>
        public static bool ContainsTabConnectedToServer(this TabControl source, Server server)
        {
            for (int i = 0; i < source.TabPages.Count; i++)
            {
                if (source.TabPages[i] is IRCTabPage)
                {
                    IRCTabPage page = source.TabPages[i] as IRCTabPage;

                    if (page.Connection != null)
                    {
                        if (page.Connection.Server.Equals(server.Address, StringComparison.OrdinalIgnoreCase) &&
                            page.Connection.Port.Equals(server.Port))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Escapes backslashes and single quotation marks for use in a JavaScript function.
        /// </summary>
        /// <param name="source">The string.</param>
        /// <returns>The escaped string.</returns>
        public static string PrepareForJS(this string source)
        {
            return source.Replace(@"\", @"\\").Replace("'", @"\'");
        }
    }
}
