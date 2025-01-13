/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Threading.Tasks;
using CliRunner;
using CliRunner.Commands.Buffered;
using PlatformKit.Abstractions;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class DarwinPlatformProvider : IPlatformProvider
    {
        public async Task<Platform> GetCurrentPlatformAsync()
        {
            Platform platform = new Platform(await GetOsNameAsync(),
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                PlatformFamily.Darwin);

            return platform;
        }

        private async Task<string> GetOsNameAsync()
        {

        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
#endif
        private async Task<string> GetSwVersInfoAsync()
        {
            var result = await Cli.Run("/usr/bin/sw_vers")
                .ExecuteBufferedAsync();

            return result.StandardOutput;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
#endif
        private async Task<Version> GetOsVersionAsync()
        {
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
            {
                string result = await GetSwVersInfoAsync();

                string versionString = result.Split(Environment.NewLine)[1]
                    .Replace("ProductVersion:", string.Empty)
                    .Replace(" ", string.Empty);

                return Version.Parse(versionString);
            }
            else
            {

            }
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
#endif
        private async Task<Version> GetKernelVersionAsync()
        {
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
            {
                BufferedCommandResult result = await Cli.Run("/usr/bin/uname")
                    .WithArguments($"-v")
                    .ExecuteBufferedAsync();

                string versionString = result.StandardOutput
                    .Replace(" ", string.Empty);
                
                return Version.Parse(versionString);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}