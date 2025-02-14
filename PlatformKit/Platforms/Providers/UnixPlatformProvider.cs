/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;

using PlatformKit.Internal.Localizations;
using PlatformKit.Specifics.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class UnixPlatformProvider : IUnixPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;

        public UnixPlatformProvider(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
        {
           Platform platform = new Platform(await GetPlatformNameAsync(),
               await GetPlatformVersionAsync(),
               await GetKernelVersionAsync(), PlatformFamily.Unix,
               await GetPlatformBuildNumberAsync(),
               await GetPlatformArchitectureAsync());
           
           return platform;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        protected async Task<string> GetUnameValueAsync(string argument)
        {
            if (OperatingSystem.IsFreeBSD() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }

            ICommandBuilder commandBuilder = new CommandBuilder("uname")
                .WithArguments(argument)
                .WithWorkingDirectory(Environment.CurrentDirectory);
            
            Command command = commandBuilder.Build();
            
            BufferedProcessResult result = await _commandRunner.ExecuteBufferedAsync(command);
            
            return result.StandardOutput;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        protected async Task<Architecture> GetPlatformArchitectureAsync()
        {
            string result = await GetUnameValueAsync("-m");

            switch (result.ToLower())
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

        protected async Task<string> GetPlatformBuildNumberAsync()
        {
            
        }

        private async Task<string> GetPlatformNameAsync()
        {
            if (OperatingSystem.IsFreeBSD())
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
            if (OperatingSystem.IsLinux())
            {
#if NETSTANDARD2_0 || NETSTANDARD2_1
                    string[] lines = await Task.FromResult(File.ReadAllLines("/etc/os-release"));
#else
                string[] lines = await File.ReadAllLinesAsync("/etc/os-release");
#endif

                string result = lines.First(x =>
                        x.Contains("name=", StringComparison.CurrentCultureIgnoreCase))
                    .Replace("Name=", string.Empty);

                return result;
            }
            if (OperatingSystem.IsMacOS())
            {
                return "macOS";
            }

            return "Unix";
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        protected async Task<Version> GetKernelVersionAsync()
        {
            string result = await GetUnameValueAsync("-r");

            int indexOfDash = result.IndexOf('-');

            string versionString;
                
            if (indexOfDash != -1)
            {
                versionString = result.Substring(indexOfDash,
                    result.Length - indexOfDash);
            }
            else
            { 
                versionString = result;
            }
                
            return Version.Parse(versionString);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("linux")]
#endif
        protected async Task<Version> GetPlatformVersionAsync()
        {
            if (OperatingSystem.IsFreeBSD() == false &&
                OperatingSystem.IsLinux() == false &&
                OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }
            
            string result = await GetUnameValueAsync("-v");
            
            string versionString = result.Replace("FreeBSD", string.Empty)
                .Replace("BSD", string.Empty)
                .Replace("Unix", string.Empty)
                .Split(' ').First().Replace("-release", string.Empty);
            
            return Version.Parse(versionString);
        }
    }
}