/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using CliRunner.Abstractions;

using PlatformKit.Internal.Localizations;
using PlatformKit.Platforms.Providers;
using PlatformKit.Specializations.Linux;
using PlatformKit.Specializations.Linux.Abstractions;
using PlatformKit.Specifics.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;

#endif

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class LinuxPlatformProvider : UnixPlatformProvider, ILinuxPlatformProvider
    {
        private readonly ICliCommandRunner _commandRunner;
        private readonly ILinuxOsReleaseProvider _linuxOsReleaseSearcher;
        
        public LinuxPlatformProvider(ICliCommandRunner commandRunner,
            ILinuxOsReleaseProvider linuxOsReleaseSearcher)
            : base(commandRunner)
        {
            _commandRunner = commandRunner;
            _linuxOsReleaseSearcher = linuxOsReleaseSearcher;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        public new async Task<Platform> GetCurrentPlatformAsync()
        {
            Platform platform = new Platform(
                await GetOsNameAsync(),
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                PlatformFamily.Linux,
                await GetOsBuildNumber(),
                await GetPlatformArchitectureAsync());

            return platform;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]        
#endif
        private async Task<string> GetOsBuildNumber()
        {
            return await _linuxOsReleaseSearcher.GetPropertyValueAsync("VERSION_ID");
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]        
#endif
        private async Task<string> GetOsNameAsync()
        {
            LinuxOsReleaseInfo releaseInfo = await _linuxOsReleaseSearcher.GetReleaseInfoAsync();

            return releaseInfo.PrettyName;
        }

        private async Task<Version> GetOsVersionAsync()
        {
            if (OperatingSystem.IsLinux() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }
            
            LinuxOsReleaseInfo releaseInfo = await _linuxOsReleaseSearcher.GetReleaseInfoAsync();

            string versionString = releaseInfo.Version;
            versionString = versionString.Replace("LTS", string.Empty);

            return Version.Parse(versionString);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        private new async Task<Version> GetKernelVersionAsync()
        {
            if (OperatingSystem.IsLinux())
            {
                string result = await GetUnameValueAsync("-v");

                string versionString = result.Replace(" ", string.Empty);
                
                return Version.Parse(versionString);
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}