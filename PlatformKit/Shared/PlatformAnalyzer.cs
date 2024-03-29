/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Runtime.InteropServices;
using PlatformKit.FreeBSD;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit;

    // ReSharper disable once InconsistentNaming
    public class PlatformAnalyzer
    {
        private readonly WindowsAnalyzer _windowsAnalyzer;
        private readonly MacOsAnalyzer _macOsAnalyzer;
        private readonly LinuxAnalyzer _linuxAnalyzer;
        private readonly FreeBsdAnalyzer _freeBsdAnalyzer;
        
        public PlatformAnalyzer()
        {
            _windowsAnalyzer = new WindowsAnalyzer();
            _macOsAnalyzer = new MacOsAnalyzer();
            _linuxAnalyzer = new LinuxAnalyzer();
            _freeBsdAnalyzer = new FreeBsdAnalyzer();
        }

        internal static bool IsWindows()
        {
#if NET5_0_OR_GREATER
                  return OperatingSystem.IsWindows();
#else
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        }
        
        internal static bool IsMac()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsMacOS();
#else
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
        }
        
        internal static bool IsLinux()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsLinux();
#else
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
        }
        
        // ReSharper disable once InconsistentNaming
        internal static bool IsFreeBSD()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsFreeBSD();
#else
        #if NETCOREAPP3_0_OR_GREATER
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
        #else
            throw new PlatformNotSupportedException();
        #endif
#endif
        }
        
        /// <summary>
        /// Detect the OS version on Windows, macOS, or Linux.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once InconsistentNaming
        public Version GetOSVersion()
        {
            if (IsWindows())
            {
                return _windowsAnalyzer.GetWindowsVersion();
            }
            if (IsLinux())
            {
                return _linuxAnalyzer.GetLinuxDistributionVersion();
            }
            if (IsMac())
            {
                return _macOsAnalyzer.GetMacOsVersion();
            }

#if NETCOREAPP3_0_OR_GREATER
                if (IsFreeBSD())
                {
                    return _freeBsdAnalyzer.GetFreeBSDVersion();
                }
#endif
            throw new PlatformNotSupportedException();
        }
    }