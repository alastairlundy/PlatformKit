/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023
    Copyright (c) NeverSpyTech Limited 2022

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
     any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
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