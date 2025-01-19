/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Linq;
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

namespace PlatformKit.Providers
{
    public class UnixPlatformProvider : IPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;

        public UnixPlatformProvider(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
        public async Task<Platform> GetCurrentPlatformAsync()
        {
            throw new System.NotImplementedException();
        }
        
        public async Task<Version> GetPlatformVersionAsync()
        {
            if (OperatingSystem.IsFreeBSD() == false && OperatingSystem.IsLinux() == false && OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }

            BufferedCommandResult result = await Command.CreateInstance("/usr/bin/uname")
                .WithArguments("-v")
                .WithWorkingDirectory(Environment.CurrentDirectory)
                .ExecuteBufferedAsync(_commandRunner);
            
            string versionString = result.StandardOutput.Replace("FreeBSD", string.Empty)
                .Replace("BSD", string.Empty)
                .Replace("Unix", string.Empty)
                .Split(' ').First().Replace("-release", string.Empty);
            
            return Version.Parse(versionString);
        }
    }
}