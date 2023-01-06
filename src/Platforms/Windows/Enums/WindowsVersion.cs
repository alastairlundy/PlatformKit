/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
<<<<<<< HEAD
 You can view the PlatformKit Licenses at https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt

 To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
=======
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
>>>>>>> main
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

// ReSharper disable InconsistentNaming

using System;

namespace PlatformKit.Windows
{
    /// <summary>
    /// An enum representing different versions of Windows.
    /// </summary>
    public enum WindowsVersion
    {
        //[Obsolete(Internal.Deprecation.DeprecationMessages.DeprecationV4)]
        Win7,
        // ReSharper disable once InconsistentNaming
        //[Obsolete(Internal.Deprecation.DeprecationMessages.DeprecationV4)]
        Win7SP1,
        //[Obsolete(Internal.Deprecation.DeprecationMessages.DeprecationV4)]
        WinServer_2008_R2,
        //[Obsolete(Internal.Deprecation.DeprecationMessages.DeprecationV4)]
        Win8,
        //[Obsolete(Internal.Deprecation.DeprecationMessages.DeprecationV4)]
        WinServer_2012,
        //[Obsolete(Internal.Deprecation.DeprecationMessages.DeprecationV4)]
        Win8_1,
        //[Obsolete(Internal.Deprecation.DeprecationMessages.DeprecationV4)]
        WinServer_2012_R2,
        /// <summary>
        /// Initial release version of Windows 10 (Build 10240).
        /// </summary>
        Win10_v1507,
        /// <summary>
        /// Initial release version of Windows 10 for Mobile and Xbox One.
        /// Windows 10 Build Number 10586
        /// </summary>
        Win10_v1511,
        Win10_v1607,
        /// <summary>
        /// Windows Server 2016 based on Windows 10 Build 14393 (Win10v1607)
        /// </summary>
        Win10_Server2016,
        Win10_v1703,
        Win10_v1709,
        /// <summary>
        /// Windows Server v1709 based on Windows 10 Build 16299 (Win10v1709).
        /// Replaced by Windows Server 2019 (v1809)
        /// </summary>
        Win10_Server_v1709,
        /// <summary>
        /// This is effectively a 2nd revision of v1703 for mobile (Identifies as Windows 10 Build 15254) but was publicly branded as V1709 for Mobile.
        /// As such UWP API Compatibility is Windows 10 Build 15063 (Win10v1703) or earlier.
        /// </summary>
        Win10_v1709_Mobile,
        Win10_v1803,
        Win10_v1809,
        /// <summary>
        /// Windows Server 2019 based on Windows 10 Build 17763 (Win10v1809)
        /// </summary>
        Win10_Server2019,
        Win10_v1903,
        Win10_v1909,
        Win10_v2004,
        Win10_20H2,
        Win10_21H1,
        Win10_21H2,
        Win10_22H2,
        /// <summary>
        /// Windows Server 2022 based on Windows 10 HOWEVER this does not share a build number with a Windows 10 release.
        /// Server 2022 identifies as Windows 10 Build 20348 which is unique to Server 2022.
        /// </summary>
        Win10_Server2022,
        /// <summary>
        /// Initial launch version of Windows 11 - a new Windows 10 like OS likely based on Windows 10.
        /// Windows 11 identifies as Windows 11 Build Number 22000 HOWEVER the numerical representation of this shows it as if it is Windows 10.
        /// The version string is 10.0.22000.x . This may cause problems if you're looking for "10.0.x.x" in your application.
        /// We've created a ``IsWindows11()`` method to detect if a device is running on Windows 11 for your (and our) convenience.
        /// </summary>
        Win11_21H2,
        Win11_22H2,
        Win10_InsiderPreview,
        Win11_InsiderPreview,
        NotDetected,
        NotSupported,
    }
}