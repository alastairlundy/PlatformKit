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

using CliRunner;

using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems.Mac
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
        public override async Task<Version> GetOperatingSystemVersionAsync()
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }

            string[] task = await GetMacSwVersInfoAsync();

            return await Task.FromResult(Version.Parse(task[1].Replace("ProductVersion:", string.Empty)
                .Replace(" ", string.Empty)));
        }

        // ReSharper disable once IdentifierTypo
        /// <summary>
        /// Gets info from sw_vers command on Mac.
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
#endif
        private async Task<string[]> GetMacSwVersInfoAsync()
        {
            var result = await Cli.Run("/usr/bin/sw_vers")
                .ExecuteBufferedAsync();
            
            // ReSharper disable once StringLiteralTypo
            return result.StandardOutput.Split(Convert.ToChar(Environment.NewLine));
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
        public override Task<Version> GetKernelVersionAsync()
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

                    return Task.FromResult(Version.Parse(array[index]));
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
        public override async Task<string> GetOperatingSystemBuildNumberAsync()
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }

            string[] task = await GetMacSwVersInfoAsync();

            return task[2].ToLower().Replace("BuildVersion:",
                string.Empty).Replace(" ", string.Empty);
        }
    }
}