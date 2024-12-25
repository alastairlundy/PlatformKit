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
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems.Mac.Extensions
{
    public static class MacOsInformationExtensions
    {
        /// <summary>
        /// Returns whether a Mac is Apple Silicon based.
        /// </summary>
        /// <param name="macOperatingSystem"></param>
        /// <returns>true if the currently running Mac uses Apple Silicon; false if running on an Intel Mac.</returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
#endif
        public static bool IsAppleSiliconMac(this MacOperatingSystem macOperatingSystem)
        {
#if NET5_0_OR_GREATER
            return RuntimeInformation.OSArchitecture switch
            {
                Architecture.Arm64 => true,
                Architecture.X64 => false,
                _ => false
            };        
#else
            switch (RuntimeInformation.OSArchitecture)
            {
                case Architecture.Arm64:
                    return true;
                case Architecture.X64:
                    return false;
                default:
                    return false;
            }
#endif
        }
        
        /// <summary>
        /// Detects the Darwin Version on macOS
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
#endif
        public static Version GetDarwinVersion(this MacOperatingSystem macOperatingSystem)
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }
            
            return Version.Parse(RuntimeInformation.OSDescription.Split(' ')[1]);
        }
        
        /// <summary>
        /// Detects macOS System Information.
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
#endif
        public static async Task<MacOsSystemInformationModel> GetMacSystemInformationModelAsync(this MacOperatingSystem macOperatingSystem)
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }

            return await Task.FromResult(new MacOsSystemInformationModel(
                await macOperatingSystem.GetOperatingSystemVersionAsync(),
                macOperatingSystem.GetDarwinVersion(),
                await macOperatingSystem.GetKernelVersionAsync(),
                await macOperatingSystem.GetOperatingSystemBuildNumberAsync(),
                macOperatingSystem.IsAppleSiliconMac()));
        }
    }
}