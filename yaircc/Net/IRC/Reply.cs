//-----------------------------------------------------------------------
// <copyright file="Reply.cs" company="rastating">
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

namespace Yaircc.Net.IRC
{
    /// <summary>
    /// Specifies the type of reply received from an IRC command.
    /// </summary>
    public enum Reply
    {
        #region RFC 2812

        /// <summary>
        /// The server sends Replies 001 to 004 to a user upon successful registration.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_WELCOME = 001,

        /// <summary>
        /// The server sends Replies 001 to 004 to a user upon successful registration.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_YOURHOST = 002,

        /// <summary>
        /// The server sends Replies 001 to 004 to a user upon successful registration.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_CREATED = 003,

        /// <summary>
        /// The server sends Replies 001 to 004 to a user upon successful registration.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "Server name: {3} | Version: {4} | User modes: {5} | Channel modes: {6}")]
        RPL_MYINFO = 004,

        /// <summary>
        /// For the purpose of yaircc, this will assume it is sending the ISUPPORT numeric, however this is also defined in RFC 2812 as something else. This could do with clearing up at some point. See http://www.irc.org/tech_docs/005.html
        /// </summary>
        [MessageTypeAttribute(MessageType.WarningMessage)]
        RPL_ISUPPORT = 005,

        /// <summary>
        /// The RPL_TRACE* are all returned by the server in response to the TRACE message.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACESERVICE = 207,

        /// <summary>
        /// The RPL_TRACE* are all returned by the server in response to the TRACE message.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACECLASS = 209,

        /// <summary>
        /// The RPL_TRACE* are all returned by the server in response to the TRACE message.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACERECONNECT = 210,

        /// <summary>
        /// A service entry in the service list
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_SERVLIST = 234,

        /// <summary>
        /// Termination of an RPL_SERVLIST list
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of service listing")]
        RPL_SERVLISTEND = 235,

        /// <summary>
        /// Reply to STATS
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSVLINE = 240,

        /// <summary>
        /// Reply to STATS
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSPING = 246,

        /// <summary>
        /// Reply to STATS
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSBLINE = 247,

        /// <summary>
        /// Reply to STATS
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSDLINE = 250,

        /// <summary>
        /// Used to terminate a list of RPL_TRACE* replies.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACEEND = 262,

        /// <summary>
        /// When a server drops a command without processing it, it MUST use this reply. Also known as RPL_LOAD_THROTTLED and RPL_LOAD2HI.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "An error occurred when processing the \"{3}\" command. Please wait and try again.")]
        RPL_TRYAGAIN = 263,

        /// <summary>
        /// RPL_UNIQOPIS 
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_UNIQOPIS = 325,

        /// <summary>
        /// An invite mask for the invite mask list.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_INVITELIST = 346,

        /// <summary>
        /// Termination of an RPL_INVITELIST list.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of channel invite list for {3}")]
        RPL_ENDOFINVITELIST = 347,

        /// <summary>
        /// An exception mask for the exception mask list. Also known as RPL_EXLIST (Unreal, Ultimate).
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_EXCEPTLIST = 348,

        /// <summary>
        /// Termination of an RPL_EXCEPTLIST list. Also known as RPL_ENDOFEXLIST (Unreal, Ultimate).
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of channel exception list for {3}")]
        RPL_ENDOFEXCEPTLIST = 349,

        /// <summary>
        /// Sent upon successful registration of a service.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_YOURESERVICE = 383,

        /// <summary>
        /// Returned to a client which is attempting to send an SQUERY (or other message) to a service which does not exist.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No such service: {3}")]
        ERR_NOSUCHSERVICE = 408,

        /// <summary>
        /// Used when a message is being sent to a mask with an invalid syntax.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Bad server/host mask")]
        ERR_BADMASK = 415,

        /// <summary>
        /// Return when the target is unable to be reached temporarily, e.g. a delay mechanism in play, or a service being offline.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Nick/channel is temporarily unavailable: {3}")]
        ERR_UNAVAILRESOURCE = 437,

        /// <summary>
        /// The given channel mask was invalid.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Bad channel mask")]
        ERR_BADCHANMASK = 476,

        /// <summary>
        /// Returned when attempting to set a mode on a channel which does not support channel modes, or channel mode changes. Also known as ERR_MODELESS.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Channel doesn't support modes")]
        ERR_NOCHANMODES = 477,

        /// <summary>
        /// Returned when a channel access list (i.e. ban list etc) is full and cannot be added to.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Channel list is full")]
        ERR_BANLISTFULL = 478,

        /// <summary>
        /// Sent by the server to a user upon connection to indicate the restricted nature of the connection (i.e. user mode +r).
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Your connection is restricted!")]
        ERR_RESTRICTED = 484,

        /// <summary>
        /// Any mode requiring 'channel creator' privileges returns this error if the client is attempting to use it while not a channel creator on the given channel.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "You're not the original channel operator")]
        ERR_UNIQOPRIVSNEEDED = 485,

        #endregion

        #region RFC 1459

        /// <summary>
        /// &quot;&lt;nickname&gt; :No such nick/channel&quot; - Used to indicate the nickname parameter supplied to a command is currently unused.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No nickname or channel named \"{3}\" exists")]
        ERR_NOSUCHNICK = 401,

        /// <summary>
        /// &quot;&lt;server name&gt; :No such server&quot; - Used to indicate the server name given currently doesn't exist.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No such server \"{3}\"")]
        ERR_NOSUCHSERVER = 402,

        /// <summary>
        /// &quot;&lt;channel name&gt; :No such channel&quot; - Used to indicate the given channel name is invalid.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No channel named \"{3}\" exists")]
        ERR_NOSUCHCHANNEL = 403,

        /// <summary>
        /// &quot;&lt;channel name&gt; :Cannot send to channel&quot; - Sent to a user who is either (a) not on a channel which is mode +n or (b) not a chanop (or mode +v) on a channel which has mode +m set and is trying to send a PRIVMSG message to that channel.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Cannot send to channel: {3}")]
        ERR_CANNOTSENDTOCHAN = 404,

        /// <summary>
        /// &quot;&lt;channel name&gt; :You have joined too many channels&quot; - Sent to a user when they have joined the maximum number of allowed channels and they try to join another channel.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Could not join {3} - You have joined too many channels")]
        ERR_TOOMANYCHANNELS = 405,

        /// <summary>
        /// &quot;&lt;nickname&gt; :There was no such nickname&quot; - Returned by WHOWAS to indicate there is no history information for that nickname.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "There was no such nickname - {3}")]
        ERR_WASNOSUCHNICK = 406,

        /// <summary>
        /// &quot;&lt;target&gt; :Duplicate recipients. No message delivered&quot; - Returned to a client which is attempting to send PRIVMSG/NOTICE using the user@host destination format and for a user@host which has several occurrences.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Too many recipients were specified: {1}")]
        ERR_TOOMANYTARGETS = 407,

        /// <summary>
        /// &quot;:No origin specified&quot; - PING or PONG message missing the originator parameter which is required since these commands must work without valid prefixes.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No origin specified")]
        ERR_NOORIGIN = 409,

        /// <summary>
        /// &quot;:No recipient given (&lt;command&gt;)&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No recipient given")]
        ERR_NORECIPIENT = 411,

        /// <summary>
        /// &quot;:No text to send&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No text to send")]
        ERR_NOTEXTTOSEND = 412,

        /// <summary>
        /// &quot;&lt;mask&gt; :No toplevel domain specified&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No toplevel domain specified: {0}", " ")]
        ERR_NOTOPLEVEL = 413,

        /// <summary>
        /// &quot;&lt;mask&gt; :Wildcard in toplevel domain&quot; - 412 - 414 are returned by PRIVMSG to indicate that the message wasn't delivered for some reason. ERR_NOTOPLEVEL and ERR_WILDTOPLEVEL are errors that are returned when an invalid use of &quot;PRIVMSG $&lt;server&gt;&quot; or &quot;PRIVMSG #&lt;host&gt;&quot; is attempted.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Wildcard in toplevel domain: {0}", " ")]
        ERR_WILDTOPLEVEL = 414,

        /// <summary>
        /// &quot;&lt;command&gt; :Unknown command&quot; - Returned to a registered client to indicate that the command sent is unknown by the server.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "The command \"{3}\" is not supported by this server.")]
        ERR_UNKNOWNCOMMAND = 421,

        /// <summary>
        /// &quot;:MOTD File is missing&quot; - Server's MOTD file could not be opened by the server.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "MOTD file is missing")]
        ERR_NOMOTD = 422,

        /// <summary>
        /// &quot;&lt;server&gt; :No administrative info available&quot; - Returned by a server in response to an ADMIN message when there is an error in finding the appropriate information.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No administrative info available")]
        ERR_NOADMININFO = 423,

        /// <summary>
        /// &quot;:File error doing &lt;file op&gt; on &lt;file&gt;&quot; - Generic error message used to report a success file operation during the processing of a message.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "File error: {1}")]
        ERR_FILEERROR = 424,

        /// <summary>
        /// &quot;:No nickname given&quot; - Returned when a nickname parameter expected for a command and isn't found.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No nickname given")]
        ERR_NONICKNAMEGIVEN = 431,

        /// <summary>
        /// &quot;&lt;nick&gt; :Erroneus nickname&quot; - Returned after receiving a NICK message which contains characters which do not fall in the defined set. See section x.x.x for details on valid nicknames.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Erroneous nickname - {3}")]
        ERR_ERRONEUSNICKNAME = 432,

        /// <summary>
        /// &quot;&lt;nick&gt; :Nickname is already in use&quot; - Returned when a NICK message is processed that results in an attempt to change to a currently existing nickname.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Nickname is already in use - {3}")]
        ERR_NICKNAMEINUSE = 433,

        /// <summary>
        /// &quot;&lt;nick&gt; :Nickname collision KILL&quot; - Returned by a server to a client when it detects a nickname collision (registered of a NICK that already exists by another server).
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "The nick {3} is already in use by another user")]
        ERR_NICKCOLLISION = 436,

        /// <summary>
        /// &quot;&lt;nick&gt; &lt;channel&gt; :They aren't on that channel&quot; - Returned by the server to indicate that the target user of the command is not on the given channel.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "{3} is not on that channel ({4})")]
        ERR_USERNOTINCHANNEL = 441,

        /// <summary>
        /// &quot;&lt;channel&gt; :You're not on that channel&quot; - Returned by the server whenever a client tries to perform a channel effecting command for which the client isn't a member.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "You're not on that channel ({3})")]
        ERR_NOTONCHANNEL = 442,

        /// <summary>
        /// &quot;&lt;user&gt; &lt;channel&gt; :is already on channel&quot; - Returned when a client tries to invite a user to a channel they are already on.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "{3} is already on the channel ({4})")]
        ERR_USERONCHANNEL = 443,

        /// <summary>
        /// &quot;&lt;user&gt; :User not logged in&quot; - Returned by the summon after a SUMMON command for a user was unable to be performed since they were not logged in.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "{3}: User not logged in")]
        ERR_NOLOGIN = 444,

        /// <summary>
        /// &quot;:SUMMON has been disabled&quot; - Returned as a response to the SUMMON command. Must be returned by any server which does not implement it.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "SUMMON has been disabled")]
        ERR_SUMMONDISABLED = 445,

        /// <summary>
        /// &quot;:USERS has been disabled&quot; - Returned as a response to the USERS command. Must be returned by any server which does not implement it.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "USERS has been disabled")]
        ERR_USERSDISABLED = 446,

        /// <summary>
        /// &quot;:You have not registered&quot; - Returned by the server to indicate that the client must be registered before the server will allow it to be parsed in detail.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "You have not registered")]
        ERR_NOTREGISTERED = 451,

        /// <summary>
        /// &quot;&lt;command&gt; :Not enough parameters&quot; - Returned by the server by numerous commands to indicate to the client that it didn't supply enough parameters.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Not enough parameters specified for \"{3}\"")]
        ERR_NEEDMOREPARAMS = 461,

        /// <summary>
        /// &quot;:You may not reregister&quot; - Returned by the server to any link which tries to change part of the registered details (such as password or user details from second USER message).
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "You may not register")]
        ERR_ALREADYREGISTRED = 462,

        /// <summary>
        /// &quot;:Your host isn't among the privileged&quot; - Returned to a client which attempts to register with a server which does not been setup to allow connections from the host the attempted connection is tried.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Your host isn't among the privileged")]
        ERR_NOPERMFORHOST = 463,

        /// <summary>
        /// &quot;:Password incorrect&quot; - Returned to indicate a success attempt at registering a connection for which a password was required and was either not given or incorrect.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Password incorrect")]
        ERR_PASSWDMISMATCH = 464,

        /// <summary>
        /// &quot;:You are banned from this server&quot; - Returned after an attempt to connect and register yourself with a server which has been setup to explicitly deny connections to you.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "You are banned from this server")]
        ERR_YOUREBANNEDCREEP = 465,

        /// <summary>
        /// &quot;&lt;channel&gt; :Channel key already set&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Channel key already set for {3}")]
        ERR_KEYSET = 467,

        /// <summary>
        /// &quot;&lt;channel&gt; :Cannot join channel (+l)&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Cannot join channel {3} - channel is full")]
        ERR_CHANNELISFULL = 471,

        /// <summary>
        /// &quot;&lt;char&gt; :is unknown mode char to me&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "{3} is an unknown mode char")]
        ERR_UNKNOWNMODE = 472,

        /// <summary>
        /// &quot;&lt;channel&gt; :Cannot join channel (+i)&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Cannot join channel {3} - this channel is invite only")]
        ERR_INVITEONLYCHAN = 473,

        /// <summary>
        /// &quot;&lt;channel&gt; :Cannot join channel (+b)&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Cannot join channel {3} - you are banned")]
        ERR_BANNEDFROMCHAN = 474,

        /// <summary>
        /// &quot;&lt;channel&gt; :Cannot join channel (+k)&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Cannot join channel {3} - bad channel key")]
        ERR_BADCHANNELKEY = 475,

        /// <summary>
        /// &quot;:Permission Denied- You're not an IRC operator&quot; - Any command requiring operator privileges to operate must return this error to indicate the attempt was unsuccessful.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Permission denied - You're not an IRC operator")]
        ERR_NOPRIVILEGES = 481,

        /// <summary>
        /// &quot;&lt;channel&gt; :You're not channel operator&quot; - Any command requiring 'chanop' privileges (such as MODE messages) must return this error if the client making the attempt is not a chanop on the specified channel.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "You're not a channel operator on {3}")]
        ERR_CHANOPRIVSNEEDED = 482,

        /// <summary>
        /// &quot;:You cant kill a server!&quot; - Any attempts to use the KILL command on a server are to be refused and this error returned directly to the client.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "You can't kill a server!")]
        ERR_CANTKILLSERVER = 483,

        /// <summary>
        /// &quot;:No O-lines for your host&quot; - If a client sends an OPER message and the server has not been configured to allow connections from the client's host as an operator, this error must be returned.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "No O-lines for your host")]
        ERR_NOOPERHOST = 491,

        /// <summary>
        /// &quot;:Unknown MODE flag&quot; - Returned by the server to indicate that a MODE message was sent with a nickname parameter and that the a mode flag sent was not recognized.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Unknown MODE flag")]
        ERR_UMODEUNKNOWNFLAG = 501,

        /// <summary>
        /// &quot;:Cant change mode for other users&quot; - Error sent to any user trying to view or change the user mode for a user other than themselves.
        /// </summary>
        [MessageTypeAttribute(MessageType.ErrorMessage, "Cannot change mode for other users")]
        ERR_USERSDONTMATCH = 502,

        /// <summary>
        /// Dummy reply number. Not used.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_NONE = 300,

        /// <summary>
        /// &quot;:[&lt;reply&gt;{&lt;space&gt;&lt;reply&gt;}]&quot; - Reply format used by USERHOST to list replies to the query list. The reply string is composed as follows: &lt;reply&gt; ::= &lt;nick&gt;['*'] '=' &lt;'+'|'-'&gt;&lt;hostname&gt; The '*' indicates whether the client has registered as an Operator. The '-' or '+' characters represent whether the client has set an AWAY message or not respectively.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_USERHOST = 302,

        /// <summary>
        /// &quot;:[&lt;nick&gt; {&lt;space&gt;&lt;nick&gt;}]&quot; - Reply format used by ISON to list replies to the query list.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "Online: {1}")]
        RPL_ISON = 303,

        /// <summary>
        /// &quot;&lt;nick&gt; :&lt;away message&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3}: away with message \"{1}\"")]
        RPL_AWAY = 301,

        /// <summary>
        /// &quot;:You are no longer marked as being away&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.NotificationMessage, "You are no longer marked as away.", " ")]
        RPL_UNAWAY = 305,

        /// <summary>
        /// &quot;:You have been marked as being away&quot; - These replies are used with the AWAY command (if allowed). RPL_AWAY is sent to any client sending a PRIVMSG to a client which is away. RPL_AWAY is only sent by the server to which the client is connected. Replies RPL_UNAWAY and RPL_NOWAWAY are sent when the client removes and sets an AWAY message.
        /// </summary>
        [MessageTypeAttribute(MessageType.NotificationMessage, "You have been marked as being away.", " ")]
        RPL_NOWAWAY = 306,

        /// <summary>
        /// &quot;&lt;nick&gt; &lt;user&gt; &lt;host&gt; * :&lt;real name&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3} <{4}@{5}> \"{7}\"")]
        RPL_WHOISUSER = 311,

        /// <summary>
        /// &quot;&lt;nick&gt; &lt;server&gt; :&lt;server info&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3}: attached to {4} \"{1}\"")]
        RPL_WHOISSERVER = 312,

        /// <summary>
        /// &quot;&lt;nick&gt; :is an IRC operator&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3} is an IRC operator")]
        RPL_WHOISOPERATOR = 313,

        /// <summary>
        /// &quot;&lt;nick&gt; &lt;integer&gt; :seconds idle&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3}: idle for {4:n} seconds")]
        RPL_WHOISIDLE = 317,

        /// <summary>
        /// &quot;&lt;nick&gt; :End of /WHOIS list&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of WHOIS information for {3}")]
        RPL_ENDOFWHOIS = 318,

        /// <summary>
        /// &quot;&lt;nick&gt; :{[@|+]&lt;channel&gt;&lt;space&gt;}&quot; - Replies 311 - 313, 317 - 319 are all replies generated in response to a WHOIS message. Given that there are enough parameters present, the answering server must either formulate a reply out of the above numerics (if the query nick is found) or return an error reply. The '*' in RPL_WHOISUSER is there as the literal character and not as a wild card. For each reply set, only RPL_WHOISCHANNELS may appear more than once (for long lists of channel names). The '@' and '+' characters next to the channel name indicate whether a client is a channel operator or has been granted permission to speak on a moderated channel. The RPL_ENDOFWHOIS reply is used to mark the end of processing a WHOIS message.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3}: member of {1}")]
        RPL_WHOISCHANNELS = 319,

        /// <summary>
        /// &quot;&lt;nick&gt; &lt;user&gt; &lt;host&gt; * :&lt;real name&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3} <{4}@{5}> \"{7}\"")]
        RPL_WHOWASUSER = 314,

        /// <summary>
        /// &quot;&lt;nick&gt; :End of WHOWAS&quot; - When replying to a WHOWAS message, a server must use the replies RPL_WHOWASUSER, RPL_WHOISSERVER or ERR_WASNOSUCHNICK for each nickname in the presented list. At the end of all reply batches, there must be RPL_ENDOFWHOWAS (even if there was only one reply and it was an error).
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of WHOWAS information for {3}")]
        RPL_ENDOFWHOWAS = 369,

        /// <summary>
        /// &quot;Channel :Users Name&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_LISTSTART = 321,

        /// <summary>
        /// &quot;&lt;channel&gt; &lt;# visible&gt; :&lt;topic&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3}\t{4:n} users\t{1}")]
        RPL_LIST = 322,

        /// <summary>
        /// &quot;:End of /LIST&quot; - Replies RPL_LISTSTART, RPL_LIST, RPL_LISTEND mark the start, actual replies with data and end of the server's response to a LIST command. If there are no channels available to return, only the start and end reply must be sent.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of /LIST")]
        RPL_LISTEND = 323,

        /// <summary>
        /// &quot;&lt;channel&gt; &lt;mode&gt; &lt;mode params&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "Mode for {3} is {4}{5}")]
        RPL_CHANNELMODEIS = 324,

        /// <summary>
        /// &quot;&lt;channel&gt; :No topic is set&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "No topic has been set for {3}")]
        RPL_NOTOPIC = 331,

        /// <summary>
        /// &quot;&lt;channel&gt; :&lt;topic&gt;&quot; - When sending a TOPIC message to determine the channel topic, one of two replies is sent. If the topic is set, RPL_TOPIC is sent back else RPL_NOTOPIC.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "Topic for {3} is: \"{1}\"")]
        RPL_TOPIC = 332,

        /// <summary>
        /// &quot;&lt;channel&gt; &lt;nick&gt;&quot; - Returned by the server to indicate that the attempted INVITE message was successful and is being passed onto the end client.
        /// </summary>
        [MessageTypeAttribute(MessageType.NotificationMessage, "{3} has been invited to {4}")]
        RPL_INVITING = 341,

        /// <summary>
        /// &quot;&lt;user&gt; :Summoning user to IRC&quot; - Returned by a server answering a SUMMON message to indicate that it is summoning that user.
        /// </summary>
        [MessageTypeAttribute(MessageType.NotificationMessage, "Summoning {3}")]
        RPL_SUMMONING = 342,

        /// <summary>
        /// &quot;&lt;version&gt;.&lt;debuglevel&gt; &lt;server&gt; :&lt;comments&gt;&quot; - Reply by the server showing its version details. The &lt;version&gt; is the version of the software being used (including any patchlevel revisions) and the &lt;debuglevel&gt; is used to indicate if the server is running in &quot;debug mode&quot;. The &quot;comments&quot; field may contain any comments about the version or further version details.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_VERSION = 351,

        /// <summary>
        /// &quot;&lt;channel&gt; &lt;user&gt; &lt;host&gt; &lt;server&gt; &lt;nick&gt; &lt;H|G&gt;[*][@|+] :&lt;hopcount&gt; &lt;real name&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "User {7} ({4}@{5}), member of {3}, is connected to {6}")]
        RPL_WHOREPLY = 352,

        /// <summary>
        /// &quot;&lt;name&gt; :End of /WHO list&quot; - The RPL_WHOREPLY and RPL_ENDOFWHO pair are used to answer a WHO message. The RPL_WHOREPLY is only sent if there is an appropriate match to the WHO query. If there is a list of parameters supplied with a WHO message, a RPL_ENDOFWHO must be sent after processing each list item with &lt;name&gt; being the item.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of WHO results for \"{3}\"")]
        RPL_ENDOFWHO = 315,

        /// <summary>
        /// &quot;&lt;channel&gt; :[[@|+]&lt;nick&gt; [[@|+]&lt;nick&gt; [...]]]&quot;
        /// "@" is used for secret channels, "*" for private channels, and "=" for others (public channels).
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{4}: {1}")]
        RPL_NAMREPLY = 353,

        /// <summary>
        /// &quot;&lt;channel&gt; :End of /NAMES list&quot; - To reply to a NAMES message, a reply pair consisting of RPL_NAMREPLY and RPL_ENDOFNAMES is sent by the server back to the client. If there is no channel found as in the query, then only RPL_ENDOFNAMES is returned. The exception to this is when a NAMES message is sent with no parameters and all visible channels and contents are sent back in a series of RPL_NAMEREPLY messages with a RPL_ENDOFNAMES to mark the end.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of /NAMES list")]
        RPL_ENDOFNAMES = 366,

        /// <summary>
        /// &quot;&lt;mask&gt; &lt;server&gt; :&lt;hopcount&gt; &lt;server info&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_LINKS = 364,

        /// <summary>
        /// &quot;&lt;mask&gt; :End of /LINKS list&quot; - In replying to the LINKS message, a server must send replies back using the RPL_LINKS numeric and mark the end of the list using an RPL_ENDOFLINKS reply.v
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of /LINKS list")]
        RPL_ENDOFLINKS = 365,

        /// <summary>
        /// &quot;&lt;channel&gt; &lt;banid&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_BANLIST = 367,

        /// <summary>
        /// &quot;&lt;channel&gt; :End of channel ban list&quot; - When listing the active 'bans' for a given channel, a server is required to send the list back using the RPL_BANLIST and RPL_ENDOFBANLIST messages. A separate RPL_BANLIST is sent for each active banid. After the banids have been listed (or if none present) a RPL_ENDOFBANLIST must be sent.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of channel ban list for {3}")]
        RPL_ENDOFBANLIST = 368,

        /// <summary>
        /// &quot;:&lt;string&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_INFO = 371,

        /// <summary>
        /// &quot;:End of /INFO list&quot; - A server responding to an INFO message is required to send all its 'info' in a series of RPL_INFO messages with a RPL_ENDOFINFO reply to indicate the end of the replies.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "End of /INFO list")]
        RPL_ENDOFINFO = 374,

        /// <summary>
        /// &quot;:- &lt;server&gt; Message of the day - &quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_MOTDSTART = 375,

        /// <summary>
        /// &quot;:- &lt;text&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_MOTD = 372,

        /// <summary>
        /// &quot;:End of /MOTD command&quot; - When responding to the MOTD message and the MOTD file is found, the file is displayed line by line, with each line no longer than 80 characters, using RPL_MOTD format replies. These should be surrounded by a RPL_MOTDSTART (before the RPL_MOTDs) and an RPL_ENDOFMOTD (after).
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_ENDOFMOTD = 376,

        /// <summary>
        /// &quot;:You are now an IRC operator&quot; - RPL_YOUREOPER is sent back to a client which has just successfully issued an OPER message and gained operator status.
        /// </summary>
        [MessageTypeAttribute(MessageType.NotificationMessage, "You are now an IRC operator")]
        RPL_YOUREOPER = 381,

        /// <summary>
        /// &quot;&lt;config file&gt; :Rehashing&quot; - If the REHASH option is used and an operator sends a REHASH message, an RPL_REHASHING is sent back to the operator.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_REHASHING = 382,

        /// <summary>
        /// &quot;&lt;server&gt; :&lt;string showing server's local time&gt;&quot; - When replying to the TIME message, a server must send the reply using the RPL_TIME format above. The string showing the time need only contain the correct day and time there. There is no further requirement for the time string.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TIME = 391,

        /// <summary>
        /// &quot;:UserID Terminal Host&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_USERSSTART = 392,

        /// <summary>
        /// &quot;:%-8s %-9s %-8s&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_USERS = 393,

        /// <summary>
        /// &quot;:End of users&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_ENDOFUSERS = 394,

        /// <summary>
        /// &quot;:Nobody logged in&quot; - If the USERS message is handled by a server, the replies RPL_USERSTART, RPL_USERS, RPL_ENDOFUSERS and RPL_NOUSERS are used. RPL_USERSSTART must be sent first, following by either a sequence of RPL_USERS or a single RPL_NOUSER. Following this is RPL_ENDOFUSERS.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_NOUSERS = 395,

        /// <summary>
        /// &quot;Link &lt;version &amp; debug level&gt; &lt;destination&gt; &lt;next server&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACELINK = 200,

        /// <summary>
        /// &quot;Try. &lt;class&gt; &lt;server&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACECONNECTING = 201,

        /// <summary>
        /// &quot;H.S. &lt;class&gt; &lt;server&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACEHANDSHAKE = 202,

        /// <summary>
        /// &quot;???? &lt;class&gt; [&lt;client IP address in dot form&gt;]&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACEUNKNOWN = 203,

        /// <summary>
        /// &quot;Oper &lt;class&gt; &lt;nick&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACEOPERATOR = 204,

        /// <summary>
        /// &quot;User &lt;class&gt; &lt;nick&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACEUSER = 205,

        /// <summary>
        /// &quot;Serv &lt;class&gt; &lt;int&gt;S &lt;int&gt;C &lt;server&gt; &lt;nick!user|*!*&gt;@&lt;host|server&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACESERVER = 206,

        /// <summary>
        /// &quot;&lt;newtype&gt; 0 &lt;client name&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACENEWTYPE = 208,

        /// <summary>
        /// &quot;File &lt;logfile&gt; &lt;debug level&gt;&quot; - The RPL_TRACE* are all returned by the server in response to the TRACE message. How many are returned is dependent on the the TRACE message and whether it was sent by an operator or not. There is no predefined order for which occurs first. Replies RPL_TRACEUNKNOWN, RPL_TRACECONNECTING and RPL_TRACEHANDSHAKE are all used for connections which have not been fully established and are either unknown, still attempting to connect or in the process of completing the 'server handshake'. RPL_TRACELINK is sent by any server which handles a TRACE message and has to pass it on to another server. The list of RPL_TRACELINKs sent in response to a TRACE command traversing the IRC network should reflect the actual connectivity of the servers themselves along that path. RPL_TRACENEWTYPE is to be used for any connection which does not fit in the other categories but is being displayed anyway.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_TRACELOG = 261,

        /// <summary>
        /// &quot;&lt;linkname&gt; &lt;sendq&gt; &lt;sent messages&gt; &lt;sent bytes&gt; &lt;received messages&gt; &lt;received bytes&gt; &lt;time open&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSLINKINFO = 211,

        /// <summary>
        /// &quot;&lt;command&gt; &lt;count&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSCOMMANDS = 212,

        /// <summary>
        /// &quot;C &lt;host&gt; * &lt;name&gt; &lt;port&gt; &lt;class&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSCLINE = 213,

        /// <summary>
        /// &quot;N &lt;host&gt; * &lt;name&gt; &lt;port&gt; &lt;class&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSNLINE = 214,

        /// <summary>
        /// &quot;I &lt;host&gt; * &lt;host&gt; &lt;port&gt; &lt;class&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSILINE = 215,

        /// <summary>
        /// &quot;K &lt;host&gt; * &lt;username&gt; &lt;port&gt; &lt;class&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSKLINE = 216,

        /// <summary>
        /// &quot;Y &lt;class&gt; &lt;ping frequency&gt; &lt;connect frequency&gt; &lt;max sendq&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSYLINE = 218,

        /// <summary>
        /// &quot;&lt;stats letter&gt; :End of /STATS report&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_ENDOFSTATS = 219,

        /// <summary>
        /// &quot;L &lt;hostmask&gt; * &lt;servername&gt; &lt;maxdepth&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSLLINE = 241,

        /// <summary>
        /// &quot;:Server Up %d days %d:%02d:%02d&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSUPTIME = 242,

        /// <summary>
        /// &quot;O &lt;hostmask&gt; * &lt;name&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSOLINE = 243,

        /// <summary>
        /// &quot;H &lt;hostmask&gt; * &lt;servername&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_STATSHLINE = 244,

        /// <summary>
        /// &quot;&lt;user mode string&gt;&quot; - To answer a query about a client's own mode, RPL_UMODEIS is sent back.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "User mode is: {3}")]
        RPL_UMODEIS = 221,

        /// <summary>
        /// &quot;:There are &lt;integer&gt; users and &lt;integer&gt; invisible on &lt;integer&gt; servers&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_LUSERCLIENT = 251,

        /// <summary>
        /// &quot;&lt;integer&gt; :operator(s) online&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_LUSEROP = 252,

        /// <summary>
        /// &quot;&lt;integer&gt; :unknown connection(s)&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_LUSERUNKNOWN = 253,

        /// <summary>
        /// &quot;&lt;integer&gt; :channels formed&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_LUSERCHANNELS = 254,

        /// <summary>
        /// &quot;:I have &lt;integer&gt; clients and &lt;integer&gt; servers&quot; - In processing an LUSERS message, the server sends a set of replies from RPL_LUSERCLIENT, RPL_LUSEROP, RPL_USERUNKNOWN, RPL_LUSERCHANNELS and RPL_LUSERME. When replying, a server must send back RPL_LUSERCLIENT and RPL_LUSERME. The other replies are only sent back if a non-zero count is found for them.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_LUSERME = 255,

        /// <summary>
        /// &quot;&lt;server&gt; :Administrative info&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_ADMINME = 256,

        /// <summary>
        /// &quot;:&lt;admin info&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_ADMINLOC1 = 257,

        /// <summary>
        /// &quot;:&lt;admin info&gt;&quot;
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_ADMINLOC2 = 258,

        /// <summary>
        /// &quot;:&lt;admin info&gt;&quot; - When replying to an ADMIN message, a server is expected to use replies RLP_ADMINME through to RPL_ADMINEMAIL and provide a text message with each. For RPL_ADMINLOC1 a description of what city, state and country the server is in is expected, followed by details of the university and department (RPL_ADMINLOC2) and finally the administrative contact for the server (an email address here is required) in RPL_ADMINEMAIL.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage)]
        RPL_ADMINEMAIL = 259,

        #endregion

        #region Unreal
        /// <summary>
        /// RPL_WHOISREGNICK
        /// </summary>
        [MessageType(MessageType.ServerMessage)]
        RPL_WHOISREGNICK = 307,

        /// <summary>
        /// RPL_WHOISHOST
        /// </summary>
        [MessageType(MessageType.ServerMessage)]
        RPL_WHOISHOST = 378,

        /// <summary>
        /// RPL_MAP
        /// </summary>
        RPL_MAP = 006,

        /// <summary>
        /// RPLMAPEND
        /// </summary>
        RPLMAPEND = 007,

        /// <summary>
        /// Used to announce when services are not available.
        /// </summary>
        [MessageType(MessageType.WarningMessage, "ATTENTION: {0}", " ")]
        ERR_SERVICESDOWN = 440,

        #endregion

        #region ircu

        /// <summary>
        /// Server notice mask (hex)
        /// </summary>
        RPL_SNOMASK = 008,

        /// <summary>
        /// RPL_STATMEMTOT
        /// </summary>
        RPL_STATMEMTOT = 009,    

        /// <summary>
        /// RPL_WHOISACCOUNT
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3} is logged in as {4}")]
        RPL_WHOISACCOUNT = 330,

        /// <summary>
        /// RPL_TOPICWHOTIME
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, typeof(ReplyDelegates), "TransformTopicWhoTimeReply")]
        RPL_TOPICWHOTIME = 333,

        /// <summary>
        /// Sent by the server in response to a WHOIS message, shows the actual host address.
        /// </summary>
        [MessageTypeAttribute(MessageType.ServerMessage, "{3}: Actual user@host: {4}, Actual IP: {5}")]
        RPL_WHOISACTUALLY = 338,

        #endregion

        #region KinelIRCd

        /// <summary>
        /// Reply to WHOIS command - Returned if the target is connected securely, e.g. type may be TLSv1, or SSLv2 etc. If the type is unknown, a '*' may be used.
        /// </summary>
        [MessageType(MessageType.ServerMessage)]
        RPL_WHOISSECURE = 671,

        #endregion

        #region Unknown Origin

        /// <summary>
        /// Sent by the server to a user to suggest an alternative server, sometimes used when the connection is refused because the server is already full. Also known as RPL_SLINE (AustHex), and RPL_REDIR.
        /// </summary>
        [MessageType(MessageType.WarningMessage, "{0} {1}", " ")]
        RPL_BOUNCE = 010,

        /// <summary>
        /// Sent by the server when the input was too long.
        /// </summary>
        [MessageType(MessageType.ErrorMessage, "{1}")]
        RPL_417 = 417,

        #endregion

        #region Hybrid

        /// <summary>
        /// RPL_YOURCOOKIE
        /// </summary>
        RPL_YOURCOOKIE = 014,

        /// <summary>
        /// Also known as RPL_CURRENT_LOCAL.
        /// </summary>
        [MessageType(MessageType.ServerMessage, "{1}")]
        RPL_LOCALUSERS = 265,

        /// <summary>
        /// Also known as RPL_CURRENT_GLOBAL
        /// </summary>
        [MessageType(MessageType.ServerMessage, "{1}")]
        RPL_GLOBALUSERS = 266,

        #endregion

        #region IRCnet

        /// <summary>
        /// RPL_YOURID
        /// </summary>
        RPL_YOURID = 042,

        /// <summary>
        /// Sent to the client when their nickname was forced to change due to a collision.
        /// </summary>
        [MessageType(MessageType.WarningMessage)]
        RPL_SAVENICK = 043,

        #endregion

        #region aircd

        /// <summary>
        /// RPL_ATTEMPTINGJUNC
        /// </summary>
        RPL_ATTEMPTINGJUNC = 050,

        /// <summary>
        /// RPL_ATTEMPTINGREROUTE
        /// </summary>
        RPL_ATTEMPTINGREROUTE = 051,

        #endregion

        #region Undernet

        /// <summary>
        /// Reply to a user when user mode +x (host masking) was set successfully
        /// </summary>
        [MessageType(MessageType.NotificationMessage, "{3} is now your hidden host", " ")]
        RPL_HOSTHIDDEN = 396,

        #endregion

        #region Bahamut

        /// <summary>
        /// Contains a URL that the channel relates to that is sent upon joining.
        /// </summary>
        [MessageType(MessageType.ServerMessage, "{1}")]
        RPL_CHANNEL_URL = 328

        #endregion
    }
}