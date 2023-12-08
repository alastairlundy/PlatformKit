/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Reflection;


namespace PlatformKit.Identification;

/// <summary>
/// 
/// </summary>
public class PlatformIdentification
{
    public PlatformIdentification()
    {
        
    }
    
        /// <summary>
        /// Return's the executing app's assembly.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public Assembly GetAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// Gets the running App's name as reported by the Assembly.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public string GetAppName()
        {
            return GetAssembly().GetName().Name;
        }
        
        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public Version GetAppVersion()
        {
            return GetAssembly().GetName().Version;
        }

        /// <summary>
        /// Return the version of PlatformKit being run.
        /// </summary>
        /// <returns></returns>
        public static Version GetPlatformKitVersion()
        {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }
}