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

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using PlatformKit.Abstractions;
using PlatformKit.Internal.Localizations;
using PlatformKit.Specializations.Linux;
using PlatformKit.Specifics.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;

#endif

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class LinuxPlatformProvider : ILinuxPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;
        private readonly ILinuxOsReleaseProvider _linuxOsReleaseSearcher;
        
        public LinuxPlatformProvider(ICommandRunner commandRunner, ILinuxOsReleaseProvider linuxOsReleaseSearcher)
        {
            _commandRunner = commandRunner;
            _linuxOsReleaseSearcher = linuxOsReleaseSearcher;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
        {
            Platform platform = new Platform(
                await GetOsNameAsync(),
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                PlatformFamily.Linux,
                await GetOsBuildNumber(),
                await GetProcessorArchitectureAsync());

            return platform;
        }

        private async Task<Architecture> GetProcessorArchitectureAsync()
        {
            ICommandBuilder commandBuilder = new CommandBuilder("/usr/bin/uname")
                .WithArguments($"-m");
                
            Command command = commandBuilder.Build();
                
            BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);

            switch (result.StandardOutput.ToLower())
            {
                case "x86":
                    return Architecture.X86;
                case "x86-64":
                    return Architecture.X64;
                case "aarch64":
                    return Architecture.Arm64;
                case "aarch32" or "aarch":
                    return Architecture.Arm;
#if NET8_0_OR_GREATER
                case "s390x":
                    return Architecture.S390x;
                case "ppc64le":
                    return Architecture.Ppc64le;
#endif
                default:
                    return Architecture.X64;
            }
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
            if (!OperatingSystem.IsLinux())
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
        private async Task<Version> GetKernelVersionAsync()
        {
            if (OperatingSystem.IsLinux())
            {
                ICommandBuilder commandBuilder = new CommandBuilder("/usr/bin/uname")
                    .WithArguments($"-v");

                Command command = commandBuilder.Build();
                
                BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);

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