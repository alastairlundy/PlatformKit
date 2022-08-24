/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at
    Commercial License - https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt
    Non-Commercial License - https://neverspy.tech/platformkit-noncommercial-license or in the file PlatformKit_NonCommercial_License.txt
  
  To use PlatformKit under either a commercial or non-commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

// ReSharper disable All

using System;

namespace PlatformKit.Mac
{
    /// <summary>
    /// An enum representing macOS versions.
    /// </summary>
    public enum MacOsVersion
    {
        v10_9_Mavericks,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_10_Yosemite,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_11_ElCapitan,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_12_Sierra,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_13_HighSierra,
        v10_14_Mojave,
        v10_15_Catalina,
        /// <summary>
        /// First version of macOS to move away from Major version 10 in [Major].[Minor].[Update]
        /// First version to support Apple Silicon on M1 powered Macs and newer.
        /// </summary>
        v11_BigSur,
        v12_Monterey,
        v13_Ventura,
        NotDetected,
        NotSupported,
    }
}