/* MIT License

Copyright (c) 2018-2022 AluminiumTech

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
    */

// ReSharper disable InconsistentNaming

using System;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows
{
    public enum WindowsVersion
    {
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        WinVista,
        // ReSharper disable once InconsistentNaming
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        WinVistaSP1,
        // ReSharper disable once InconsistentNaming
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        WinVistaSP2,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        WinServer_2008,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        Win7,
        // ReSharper disable once InconsistentNaming
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        Win7SP1,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        WinServer_2008_R2,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        Win8,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        WinServer_2012,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        Win8_1,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
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
        Win10_InsiderPreview,
        Win11_InsiderPreview,
        NotDetected,
        Unsupported,
    }
}