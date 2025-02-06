/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Extensions;

using PlatformKit.Abstractions;
using PlatformKit.Internal.Localizations;
using PlatformKit.Specifics;
using PlatformKit.Specifics.Abstractions;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;

#endif

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
// ReSharper disable RedundantBoolCompare
#endif

namespace PlatformKit.Providers
{
    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    public class DarwinPlatformProvider : IDarwinPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;

        public DarwinPlatformProvider(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("watchos")]
        [SupportedOSPlatform("tvos")]
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
        {
            Platform platform = new Platform(await GetOsNameAsync(),
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                PlatformFamily.Darwin,
                await GetBuildNumberAsync(),
                await GetProcessorArchitectureAsync());

            return platform;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("watchos")]
        [SupportedOSPlatform("tvos")]
#endif
        public async Task<DarwinPlatform> GetCurrentDarwinPlatformAsync()
        {
            DarwinPlatform darwinPlatform = new DarwinPlatform(
                await GetOsNameAsync(), await GetDarwinVersionAsync(), 
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                await GetBuildNumberAsync(),
                await GetProcessorArchitectureAsync());
            
            return darwinPlatform;
        }

        private async Task<Version> GetDarwinVersionAsync()
        {
            if (OperatingSystem.IsMacOS() == true || OperatingSystem.IsMacCatalyst() == true)
            {
                
            }
            else
            {
                
            }
        }

        private async Task<Architecture> GetProcessorArchitectureAsync()
        {
            if (OperatingSystem.IsMacOS() == true || OperatingSystem.IsMacCatalyst() == true)
            {
                
            }
            else
            {
                
            }
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("watchos")]
        [SupportedOSPlatform("tvos")]
#endif
        private async Task<string> GetBuildNumberAsync()
        {
            if (OperatingSystem.IsMacOS() == false ||
                OperatingSystem.IsMacCatalyst() == false ||
                OperatingSystem.IsWatchOS() == false ||
                OperatingSystem.IsIOS() == false ||
                OperatingSystem.IsTvOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }

            if (OperatingSystem.IsMacOS() == true || OperatingSystem.IsMacCatalyst() == true)
            {
                string result = await GetSwVersInfoAsync();

                string[] resultArray = result.Split(Environment.NewLine.ToCharArray().First());
            
                return resultArray[2].ToLower().Replace("BuildVersion:",
                    string.Empty).Replace(" ", string.Empty);
            }
            else
            {
                return Environment.OSVersion.Version.Build.ToString();
            }
        }

        private async Task<string> GetOsNameAsync()
        {

        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("watchos")]
        [UnsupportedOSPlatform("tvos")]
#endif
        private async Task<string> GetSwVersInfoAsync()
        {
            ICommandBuilder commandBuilder = new CommandBuilder("/usr/bin/sw_vers");
            
            Command command = commandBuilder.ToCommand();
            
            BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);

            return result.StandardOutput;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("tvos")]
        [SupportedOSPlatform("watchos")]
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
                return Environment.OSVersion.Version;
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
                ICommandBuilder commandBuilder = new CommandBuilder("/usr/bin/uname")
                    .WithArguments($"-v");
                
                Command command = commandBuilder.ToCommand();
                
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