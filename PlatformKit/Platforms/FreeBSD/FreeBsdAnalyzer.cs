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
using AlastairLundy.System.Extensions.StringExtensions;
using AlastairLundy.System.Extensions.VersionExtensions;
using PlatformKit.Internal.Deprecation;

#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.FreeBSD;

/// <summary>
/// A class to detect FreeBSD versions and features.
/// </summary>
public class FreeBsdAnalyzer
{

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Detects and Returns the Installed version of FreeBSD
    /// </summary>
    /// <returns></returns>
    public static Version GetFreeBSDVersion()
    {
        if (OperatingSystem.IsFreeBSD())
        {
            return Version.Parse(CommandRunner.RunCommandOnFreeBsd("uname -v").Replace("FreeBSD", String.Empty)
                .Split(' ')[0].Replace("-release",  string.Empty).AddMissingZeroes());
        }

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// Checks to see whether the specified version of FreeBSD is the same or newer than the installed version of FreeBSD.
    /// </summary>
    /// <param name="expectedVersion"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    [Obsolete(DeprecationMessages.DeprecationV5)]
    public static bool IsAtLeastVersion(Version expectedVersion)
    {
        if (OperatingSystem.IsFreeBSD())
        {
            return GetFreeBSDVersion().IsAtLeast((expectedVersion));
        }

        throw new PlatformNotSupportedException();
    }
}