/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;
using System.Reflection;

using LicensingKit;
using LicensingKit.Enums;
using PlatformKit.Internal.Licensing;

namespace PlatformKit.Identification;

/// <summary>
/// 
/// </summary>
public class PlatformIdentification
{
    public PlatformIdentification()
    {
        PlatformKitConstants.CheckLicenseState();
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static LicenseUsageState GetLicenseUsageState()
        {
           return LicenseManager.LicenseUsageState;
        }
}