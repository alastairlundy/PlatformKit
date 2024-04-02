/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;

using PlatformKit.Internal.Deprecation;

using PlatformKit.FreeBSD;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit
{
    // ReSharper disable once InconsistentNaming
    public class OSAnalyzer
    {
        private readonly WindowsAnalyzer _windowsAnalyzer;
        private readonly MacOsAnalyzer _macOsAnalyzer;
        private readonly LinuxAnalyzer _linuxAnalyzer;
        private readonly FreeBsdAnalyzer _freeBsdAnalyzer;
        
        public OSAnalyzer()
        {
            _windowsAnalyzer = new WindowsAnalyzer();
            _macOsAnalyzer = new MacOsAnalyzer();
            _linuxAnalyzer = new LinuxAnalyzer();
            _freeBsdAnalyzer = new FreeBsdAnalyzer();
        }

        /// <summary>
        /// Returns whether or not the current OS is Windows.
        /// </summary>
        /// <returns></returns>
        [Obsolete(DeprecationMessages.DeprecationV4)]
        public static bool IsWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
        }

        /// <summary>
        /// Returns whether or not the current OS is macOS.
        /// </summary>
        /// <returns></returns>
        [Obsolete(DeprecationMessages.DeprecationV4)]
        public static bool IsMac()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
        }
        
        /// <summary>
        /// Returns whether or not the current OS is Linux based.
        /// </summary>
        /// <returns></returns>
        [Obsolete(DeprecationMessages.DeprecationV4)]
        public static bool IsLinux()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Returns whether or not the current OS is FreeBSD based.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an error if run on .NET Standard 2 or .NET Core 2.1 or earlier.</exception>
        [Obsolete(DeprecationMessages.DeprecationV4)]
        public static bool IsFreeBSD()
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD);
#else
            throw new PlatformNotSupportedException();
#endif
        }
        
        /// <summary>
        /// Determine what OS is being run
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        [Obsolete(DeprecationMessages.DeprecationV4)]
        public System.Runtime.InteropServices.OSPlatform GetOSPlatform() {
            System.Runtime.InteropServices.OSPlatform osPlatform = System.Runtime.InteropServices.OSPlatform.Create("Other Platform");
            // Check if it's windows
            osPlatform = IsWindows() ? System.Runtime.InteropServices.OSPlatform.Windows : osPlatform;
            // Check if it's osx
            osPlatform = IsMac() ? System.Runtime.InteropServices.OSPlatform.OSX : osPlatform;
            // Check if it's Linux
            osPlatform = IsLinux() ? System.Runtime.InteropServices.OSPlatform.Linux : osPlatform;
            
#if NETCOREAPP3_0_OR_GREATER
            // Check if it's FreeBSD
            osPlatform = IsFreeBSD() ? System.Runtime.InteropServices.OSPlatform.FreeBSD : osPlatform;
#endif

            return osPlatform;
        }
        
        /// <summary>
        /// Detect the OS version on Windows, macOS, or Linux.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once InconsistentNaming
        public Version DetectOSVersion()
        {
            try
            {
                if (IsWindows())
                {
                    return _windowsAnalyzer.DetectWindowsVersion();
                }
                if (IsLinux())
                {
                    return _linuxAnalyzer.DetectLinuxDistributionVersion();
                }
                if (IsMac())
                {
                    return _macOsAnalyzer.DetectMacOsVersion();
                }

#if NETCOREAPP3_0_OR_GREATER
                if (OperatingSystem.IsFreeBSD())
                {
                    return _freeBsdAnalyzer.DetectFreeBSDVersion();
                }
#endif
                throw new PlatformNotSupportedException();
            }
            catch (Exception exception)
            {
                throw;

            }
        }
    }
}