/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Runtime.CompilerServices;

namespace PlatformKit.Classic.OSDetection
{
    // ReSharper disable once InconsistentNaming
    public static class OSAnalyzerExtensions {
        
    /// <summary>
    /// Returns whether or not the current OS is Windows.
    /// </summary>
    /// <returns></returns>
    public static bool IsWindows(this OSAnalyzer osAnalyzer)
    {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
    }

    /// <summary>
    /// Returns whether or not the current OS is macOS.
    /// </summary>
    /// <returns></returns>
    // ReSharper disable once InconsistentNaming
    public static bool IsMacOS(this OSAnalyzer osAnalyzer)
    {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
    }
        
    /// <summary>
    /// Returns whether or not the current OS is Linux based.
    /// </summary>
    /// <returns></returns>
    public static bool IsLinux(this OSAnalyzer osAnalyzer)
    {
        return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
    }
    
#if NETCOREAPP3_0_OR_GREATER

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Returns whether or not the current OS is FreeBSD based.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an error if run on .NET Standard 2 or .NET Core 2.1 or earlier.</exception>
    public static bool IsFreeBSD(this OSAnalyzer osAnalyzer)
    {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD);
    }
#endif

    }
}