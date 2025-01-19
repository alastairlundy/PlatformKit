/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

using PlatformKit.Specifics;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#endif

namespace PlatformKit.Extensions.Platforms;

public static class WindowsPlatformIsVersionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static bool IsWindows10(this WindowsPlatform platform)
    {
        return OperatingSystem.IsWindowsVersionAtLeast(10, 0, 10240)
               && platform.OperatingSystemVersion < new Version(10, 0, 22000);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static bool IsWindows11(this WindowsPlatform platform)
    {
        return OperatingSystem.IsWindowsVersionAtLeast(10, 0, 22000)
               && platform.OperatingSystemVersion < new Version(10, 0, 29000);
    }
}