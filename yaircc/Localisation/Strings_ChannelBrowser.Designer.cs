﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.5466
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Yaircc.Localisation {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Strings_ChannelBrowser {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Strings_ChannelBrowser() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("Yaircc.Localisation.Strings_ChannelBrowser", typeof(Strings_ChannelBrowser).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Displaying {0} of {1} channels..
        /// </summary>
        internal static string DisplayingChannels {
            get {
                return ResourceManager.GetString("DisplayingChannels", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Fetching channels....
        /// </summary>
        internal static string FetchingChannels {
            get {
                return ResourceManager.GetString("FetchingChannels", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Last refreshed {0:dd/MM/yyyy, HH:mm}.
        /// </summary>
        internal static string LastRefreshed {
            get {
                return ResourceManager.GetString("LastRefreshed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Populating list....
        /// </summary>
        internal static string PopulatingList {
            get {
                return ResourceManager.GetString("PopulatingList", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Channels on {0}.
        /// </summary>
        internal static string Title {
            get {
                return ResourceManager.GetString("Title", resourceCulture);
            }
        }
    }
}
