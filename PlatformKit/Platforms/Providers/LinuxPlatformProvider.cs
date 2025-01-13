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
using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Extensions;

using PlatformKit.Abstractions;

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
            throw new System.NotImplementedException();
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        private async Task<Version> GetKernelVersionAsync()
        {
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
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