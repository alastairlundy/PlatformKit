/*
        MIT License
       
       Copyright (c) 2020-2024 Alastair Lundy
       
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

using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;
using PlatformKit.Internal.Exceptions.Windows;
using PlatformKit.Internal.Localizations;
// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems.Windows.Extensions
{
    public static class WindowsVersionEnumExtensions
    {
        /// <summary>
        /// Detects the installed version of Windows and returns it as an enum.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if not run on Windows.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public static async Task<WindowsVersion> GetWindowsVersionToEnumAsync(this WindowsOperatingSystem windowsOperatingSystem)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }
            
            return await Task.FromResult(GetWindowsVersionToEnum(windowsOperatingSystem, await windowsOperatingSystem.GetOperatingSystemVersionAsync()));
        }

        /// <summary>
        /// Converts the specified version input to an enum corresponding to a Windows version.
        /// </summary>
        /// <param name="windowsOperatingSystem"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static WindowsVersion GetWindowsVersionToEnum(this WindowsOperatingSystem windowsOperatingSystem, Version input)
        {
            return input.Build switch
            {
                < 10240 => WindowsVersion.NotSupported,
                10240 => WindowsVersion.Win10_v1507,
                10586 => WindowsVersion.Win10_v1511,
                14393 => WindowsVersion.Win10_v1607,
                15063 => WindowsVersion.Win10_v1703,
                16299 => WindowsVersion.Win10_v1709,
                17134 => WindowsVersion.Win10_v1803,
                17763 => WindowsVersion.Win10_v1809,
                18362 => WindowsVersion.Win10_v1903,
                18363 => WindowsVersion.Win10_v1909,
                19041 => WindowsVersion.Win10_v2004,
                19042 => WindowsVersion.Win10_20H2,
                19043 => WindowsVersion.Win10_21H1,
                19044 => WindowsVersion.Win10_21H2,
                19045 => WindowsVersion.Win10_22H2,
                //Build number used exclusively by Windows Server and not by Windows 10 or 11
                20348 => WindowsVersion.Win10_Server2022,
                22000 => WindowsVersion.Win11_21H2,
                > 10240 and < 22000 => WindowsVersion.Win10_InsiderPreview,
                22621 => WindowsVersion.Win11_22H2,
                22631 => WindowsVersion.Win11_23H2,
                26100 => WindowsVersion.Win11_24H2,
                > 26100 => WindowsVersion.Win11_InsiderPreview,
                _ => WindowsVersion.NotDetected
            };
        }

        /// <summary>
        /// Return the version of Windows in the Version format based on the specified WindowsVersion enum.
        /// </summary>
        /// <param name="windowsOperatingSystem"></param>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        /// <exception cref="WindowsVersionDetectionException"></exception>
        public static Version GetWindowsVersionFromEnum(this WindowsOperatingSystem windowsOperatingSystem, WindowsVersion windowsVersion)
        {
            return windowsVersion switch
            {
                WindowsVersion.Win10_v1507 => new Version(10, 0, 10240),
                WindowsVersion.Win10_v1511 => new Version(10, 0, 10586),
                WindowsVersion.Win10_v1607 or WindowsVersion.Win10_Server2016 => new Version(10, 0, 14393),
                WindowsVersion.Win10_v1703 => new Version(10, 0, 15063),
                WindowsVersion.Win10_v1709 or WindowsVersion.Win10_Server_v1709 => new Version(10, 0, 16299),
                WindowsVersion.Win10_v1803 => new Version(10, 0, 17134),
                WindowsVersion.Win10_v1809 or WindowsVersion.Win10_Server2019 => new Version(10, 0, 17763),
                WindowsVersion.Win10_v1903 => new Version(10,0, 18362),
                WindowsVersion.Win10_v1909 => new Version(10,0, 18363),
                WindowsVersion.Win10_v2004 => new Version(10,0, 19041),
                WindowsVersion.Win10_20H2 => new Version(10,0, 19042),
                WindowsVersion.Win10_21H1 => new Version(10,0, 19043),
                WindowsVersion.Win10_21H2 => new Version(10,0, 19044),
                WindowsVersion.Win10_22H2 => new Version(10,0,19045),
                WindowsVersion.Win10_Server2022 => new Version(10, 0, 20348),
                WindowsVersion.Win11_21H2 => new Version(10, 0, 22000),
                WindowsVersion.Win11_22H2 => new Version(10,0,22621),
                WindowsVersion.Win11_23H2 => new Version(10,0,22631),
                WindowsVersion.Win11_24H2 => new Version(10,0,26100),
                _ => throw new WindowsVersionDetectionException()
            };
        }
    }
}