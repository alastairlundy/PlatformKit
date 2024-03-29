/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

// ReSharper disable InconsistentNaming

using System;
using PlatformKit.Internal.Deprecation;

namespace PlatformKit.Windows
{
    /// <summary>
    /// An enum representing different versions of Windows.
    /// </summary>
    public enum WindowsVersion
    {
        [Obsolete(DeprecationMessages.DeprecationV5)]
        Win7 = 7600,
        // ReSharper disable once InconsistentNaming
        [Obsolete(DeprecationMessages.DeprecationV5)]
        Win7SP1 = 7601,
        [Obsolete(DeprecationMessages.DeprecationV5)]
        WinServer_2008_R2 = 7601,
        [Obsolete(DeprecationMessages.DeprecationV5)]
        Win8 = 9200,
        [Obsolete(DeprecationMessages.DeprecationV5)]
        WinServer_2012 = 9200,
        [Obsolete(DeprecationMessages.DeprecationV5)]
        Win8_1 = 9600,
        [Obsolete(DeprecationMessages.DeprecationV5)]
        WinServer_2012_R2 = 9600,
        /// <summary>
        /// Initial release version of Windows 10 (Build 10240).
        /// </summary>
        Win10_v1507 = 10240,
        /// <summary>
        /// Initial release version of Windows 10 for Mobile and Xbox One.
        /// Windows 10 Build Number 10586
        /// </summary>
        Win10_v1511 = 10586,
        Win10_v1607 = 14393,
        /// <summary>
        /// Windows Server 2016 based on Windows 10 Build 14393 (Win10v1607)
        /// </summary>
        Win10_Server2016 = 14393,
        Win10_v1703 = 15063,
        Win10_v1709 = 16299,
        /// <summary>
        /// Windows Server v1709 based on Windows 10 Build 16299 (Win10v1709).
        /// Replaced by Windows Server 2019 (v1809)
        /// </summary>
        Win10_Server_v1709 = 16299,
        /// <summary>
        /// This is effectively a 2nd revision of v1703 for mobile (Identifies as Windows 10 Build 15254) but was publicly branded as V1709 for Mobile.
        /// As such UWP API Compatibility is Windows 10 Build 15063 (Win10v1703) or earlier.
        /// </summary>
        Win10_v1709_Mobile = 15254,
        Win10_v1803 = 17134,
        Win10_v1809 = 17763,
        /// <summary>
        /// Windows Server 2019 based on Windows 10 Build 17763 (Win10v1809)
        /// </summary>
        Win10_Server2019 = 17763,
        Win10_v1903 = 18362,
        Win10_v1909 = 18363,
        Win10_v2004 = 19041,
        Win10_20H2 = 19042,
        Win10_21H1 = 19043,
        Win10_21H2 = 19044,
        Win10_22H2 = 19045,
        /// <summary>
        /// Windows Server 2022 based on Windows 10 HOWEVER this does not share a build number with a Windows 10 release.
        /// Server 2022 identifies as Windows 10 Build 20348 which is unique to Server 2022.
        /// </summary>
        Win10_Server2022 = 20348,
        /// <summary>
        /// Initial launch version of Windows 11 - a new Windows 10 like OS likely based on Windows 10.
        /// Windows 11 identifies as Windows 11 Build Number 22000 HOWEVER the numerical representation of this shows it as if it is Windows 10.
        /// The version string is 10.0.22000.x . This may cause problems if you're looking for "10.0.x.x" in your application.
        /// We've created a ``IsWindows11()`` method to detect if a device is running on Windows 11 for your (and our) convenience.
        /// </summary>
        Win11_21H2 = 22000,
        Win11_22H2 = 22621,
        Win11_23H2 = 22631,
        Win10_InsiderPreview,
        Win11_InsiderPreview,
        NotDetected,
        NotSupported,
    }
}