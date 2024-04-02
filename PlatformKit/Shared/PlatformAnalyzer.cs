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
        
        public PlatformAnalyzer()
        {
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
    }