/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.IO;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Extensions;

using PlatformKit.Abstractions;
using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#endif

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class LinuxPlatformProvider : IPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;

        public LinuxPlatformProvider(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
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
                await GetOsBuildNumber());

            return platform;
        }

        private async Task<string> GetOsBuildNumber()
        {
            return await GetOsReleasePropertyValueAsync("VERSION_ID");
        }

        private async Task<string> GetOsNameAsync()
        {
            throw new NotImplementedException();
        }

        private async Task<Version> GetOsVersionAsync()
        {
            if (!OperatingSystem.IsLinux())
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }

            string versionString = await GetOsReleasePropertyValueAsync("VERSION=");
            versionString = versionString.Replace("LTS", string.Empty);

            return Version.Parse(versionString);
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        private async Task<string> GetOsReleasePropertyValueAsync(string propertyName)
        {
            if (OperatingSystem.IsLinux() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }

            string output = string.Empty;

#if NET5_0_OR_GREATER
                string[] osReleaseInfo = await File.ReadAllLinesAsync("/etc/os-release");
#else
            string[] osReleaseInfo = await Task.Run(() => File.ReadAllLines("/etc/os-release"));
#endif

            foreach (string s in osReleaseInfo)
            {
                if (s.ToUpper().StartsWith(propertyName))
                {
                    output = s.Replace(propertyName, string.Empty);
                }
            }

            return output;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        private async Task<Version> GetKernelVersionAsync()
        {
            if (OperatingSystem.IsLinux())
            {
                BufferedCommandResult result = await Command.CreateInstance("/usr/bin/uname")
                    .WithArguments($"-v")
                    .ExecuteBufferedAsync(_commandRunner);

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