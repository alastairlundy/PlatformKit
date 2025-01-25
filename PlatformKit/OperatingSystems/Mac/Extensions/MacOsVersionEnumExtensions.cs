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
using System.Threading.Tasks;
using PlatformKit.Internal.Exceptions.Mac;
using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#else
using System.Runtime.Versioning;
#endif

namespace PlatformKit.OperatingSystems.Mac.Extensions
{
    public static class MacOsVersionEnumExtensions
    {
        /// <summary>
        /// Returns macOS version as a macOS version enum.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
        /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
        public static async Task<MacOsVersion> GetMacOsVersionToEnumAsync(this MacOperatingSystem macOperatingSystem)
        {
            return GetMacOsVersionToEnum(await macOperatingSystem.GetOperatingSystemVersionAsync());
        }

        /// <summary>
        /// Converts a macOS version to a macOS version enum.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static MacOsVersion GetMacOsVersionToEnum(Version input)
        {
            return input.Major switch
            {
                10 => input.Minor switch
                {
                    < 15 => MacOsVersion.NotSupported,
                    15 => MacOsVersion.v10_15_Catalina,
                    //This is for compatibility reasons.
                    16 => MacOsVersion.v11_BigSur,
                    _ => MacOsVersion.NotDetected
                },
                11 => MacOsVersion.v11_BigSur,
                12 => MacOsVersion.v12_Monterey,
                13 => MacOsVersion.v13_Ventura,
                14 => MacOsVersion.v14_Sonoma,
                15 => MacOsVersion.v15_Sequoia,
                _ => MacOsVersion.NotDetected
            };
        }

        /// <summary>
        /// Converts a macOS version enum to a macOS version as a Version object.
        /// </summary>
        /// <param name="macOsVersion"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
        /// <exception cref="MacOsVersionDetectionException">Throws an exception if macOS version detection fails.</exception>
        public static Version GetMacOsVersionFromEnum(MacOsVersion macOsVersion)
        {
            return macOsVersion switch
            {
                MacOsVersion.v10_15_Catalina => new(10, 15),
                MacOsVersion.v11_BigSur => new(11, 0),
                MacOsVersion.v12_Monterey => new(12, 0),
                MacOsVersion.v13_Ventura => new(13, 0),
                MacOsVersion.v14_Sonoma => new Version(14, 0),
                MacOsVersion.v15_Sequoia => new Version(15,0),
                MacOsVersion.NotSupported => throw new PlatformNotSupportedException(),
                MacOsVersion.NotDetected => throw new MacOsVersionDetectionException(),
                _ => throw new ArgumentException(Resources.Exceptions_Arguments_InvalidMacOsVersionEnum),
            };
        }

        /// <summary>
        /// Checks to see whether the specified version of macOS is the same or newer than the installed version of macOS.
        /// </summary>
        /// <param name="macOperatingSystem"></param>
        /// <param name="macOsVersion">A MacOsVersion enum representing a major version of macOS.</param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
        // ReSharper disable once InconsistentNaming
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
        public static bool IsAtLeastVersion(this MacOperatingSystem macOperatingSystem, MacOsVersion macOsVersion)
        {
            if (OperatingSystem.IsMacOS())
            {
                var version = GetMacOsVersionFromEnum(macOsVersion);
                return OperatingSystem.IsMacOSVersionAtLeast(version.Major, version.Minor, version.Build);
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }
        }
    }
}