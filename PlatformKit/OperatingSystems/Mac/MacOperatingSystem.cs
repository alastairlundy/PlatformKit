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
using PlatformKit.Internal.Localizations;
using PlatformKit.OperatingSystems.Abstractions;
#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems
{

    public class MacOperatingSystem : AbstractOperatingSystem
    {
        /// <summary>
        /// Detects the macOS version and returns it as a System.Version object.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
#endif
        public override Version GetOperatingSystemVersion()
        {
            if (OperatingSystem.IsMacOS())
            {
                return Version.Parse(GetMacSwVersInfo()[1].Replace("ProductVersion:", string.Empty)
                    .Replace(" ", string.Empty));
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }
        }

        // ReSharper disable once IdentifierTypo
        /// <summary>
        /// Gets info from sw_vers command on Mac.
        /// </summary>
        /// <returns></returns>
        private string[] GetMacSwVersInfo()
        {
            // ReSharper disable once StringLiteralTypo
            return CommandRunner.RunCommandOnMac("sw_vers").Split(Convert.ToChar(Environment.NewLine));
        }

        /// <summary>
        /// Detects macOS's XNU Kernel Version.
        /// </summary>
        /// <returns>The version</returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
#endif
        public override Version GetKernelVersion()
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }

            string[] array = RuntimeInformation.OSDescription.Split(' ');

            for (int index = 0; index < array.Length; index++)
            {
                if (array[index].ToLower().StartsWith("root:xnu-"))
                {
                    array[index] = array[index].Replace("root:xnu-", string.Empty)
                        .Replace("~", ".");

                    if (RuntimeInformation.OSArchitecture == Architecture.Arm64)
                    {
                        array[index] = array[index].Replace("/RELEASE_ARM64_T", string.Empty).Remove(array.Length - 4);
                    }
                    else
                    {
                        array[index] = array[index].Replace("/RELEASE_X86_64", string.Empty);
                    }

                    return Version.Parse(array[index]);
                }
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Detects the Build Number of the installed version of macOS.
        /// </summary>
        /// <returns>the build number of the installed version of macOS.</returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
#endif
        public override string GetOperatingSystemBuildNumber()
        {
            if (OperatingSystem.IsMacOS())
            {
                return GetMacSwVersInfo()[2].ToLower().Replace("BuildVersion:",
                    string.Empty).Replace(" ", string.Empty);
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }
        }
    }
}