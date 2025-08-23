/*
        MIT License
       
       Copyright (c) 2020-2025 Alastair Lundy
       
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
using CliWrap;
using CliWrap.Buffered;
using PlatformKit.Internal.Deprecation;
using PlatformKit.Internal.Helpers;
using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#endif

namespace PlatformKit.FreeBSD;

/// <summary>
/// A class to detect FreeBSD versions and features.
/// </summary>
[Obsolete(DeprecationMessages.DeprecationV5)]
public class FreeBsdAnalyzer
{

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Detects and Returns the Installed version of FreeBSD
    /// </summary>
    /// <returns></returns>
    [Obsolete(DeprecationMessages.DeprecationV5)]
    public static Version GetFreeBSDVersion()
    {
        if (OperatingSystem.IsFreeBSD())
        {
#if NETSTANDARD2_0 || NETSTANDARD2_1
            return Environment.OSVersion.Version;
#else
            BufferedCommandResult result = Cli.Wrap("/usr/bin/uname")
                .WithArguments("-v")
                .ExecuteBufferedSync();
            
            string standardOutput= result.StandardOutput.Replace("FreeBSD", string.Empty)
                .Split(' ')[0].Replace("-release",  string.Empty);
            
            return Version.Parse(standardOutput);
#endif
        }

        throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
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
#if NETSTANDARD2_0 || NETSTANDARD2_1
            return OperatingSystem.IsFreeBSDVersionAtLeast(expectedVersion.Major, expectedVersion.Minor, expectedVersion.Build, expectedVersion.Revision);
#else
            return GetFreeBSDVersion() >= (expectedVersion);
#endif
        }

        throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
    }
}