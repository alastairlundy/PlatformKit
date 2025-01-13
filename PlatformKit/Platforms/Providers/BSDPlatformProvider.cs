/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable InconsistentNaming

using System;
using System.Linq;

#if NET5_0_OR_GREATER
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
#endif

using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Extensions;

using PlatformKit.Abstractions;
using PlatformKit.Internal.Localizations;

namespace PlatformKit.Providers
{
    public class BSDPlatformProvider : IPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;

        public BSDPlatformProvider(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
        {
            Platform platform = new Platform(
                await GetOsNameAsync(),
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                PlatformFamily.BSD);
            
            return platform;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<string> GetOsNameAsync()
        {
            
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<Version> GetOsVersionAsync()
        {
            if (OperatingSystem.IsFreeBSD() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }

            BufferedCommandResult result = await Cli.Run("/usr/bin/uname")
                .WithArguments("-v")
                .WithWorkingDirectory(Environment.CurrentDirectory)
                .ExecuteBufferedAsync(_commandRunner);
            
            string versionString = result.StandardOutput.Replace("FreeBSD", string.Empty)
                .Replace("BSD", string.Empty)
                .Split(' ').First().Replace("-release", string.Empty);
            
            return Version.Parse(versionString);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<Version> GetKernelVersionAsync()
        {
            
        }
    }
}