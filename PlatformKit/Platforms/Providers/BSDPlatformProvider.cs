/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable InconsistentNaming

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;

using PlatformKit.Abstractions;
using PlatformKit.Internal.Localizations;

// ReSharper disable ConvertToPrimaryConstructor


#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
#endif

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
                PlatformFamily.BSD,
                await GetBuildNumberAsync(),
                await GetProcessorArchitectureAsync());
            
            return platform;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<string> GetUnameValueAsync(string argument)
        {
            if (OperatingSystem.IsFreeBSD() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }

            ICommandBuilder commandBuilder = new CommandBuilder("/usr/bin/uname")
                .WithArguments(argument)
                .WithWorkingDirectory(Environment.CurrentDirectory);
            
            Command command = commandBuilder.Build();
            
            BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);
            
            return result.StandardOutput;
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
                case "x86_64":
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

        private async Task<string> GetBuildNumberAsync()
        {
           
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<string> GetOsNameAsync()
        {
            if (OperatingSystem.IsFreeBSD())
            {
                try
                {
#if NETSTANDARD2_0 || NETSTANDARD2_1
                    string[] lines = await Task.FromResult(File.ReadAllLines("/etc/freebsd-release"));
#else
                    string[] lines = await File.ReadAllLinesAsync("/etc/freebsd-release");
#endif
                    
                    string result = lines.First(x => 
                            x.Contains("name=", StringComparison.CurrentCultureIgnoreCase))
                        .Replace("Name=", string.Empty);

                    return result;
                }
                catch
                {
                    return "FreeBSD";
                }
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }   
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<Version> GetOsVersionAsync()
        {
            string result = await GetUnameValueAsync("-v");
            
            string versionString = result.Replace("FreeBSD", string.Empty)
                .Replace("BSD", string.Empty)
                .Split(' ').First().Replace("-release", string.Empty);
            
            return Version.Parse(versionString);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<Version> GetKernelVersionAsync()
        {
            string result = await GetUnameValueAsync("-k");
            
            string versionString = result.Replace("FreeBSD", string.Empty)
                .Replace("BSD", string.Empty)
                .Split(' ').First().Replace("-release", string.Empty);
            
            return Version.Parse(versionString);
        }
    }
}