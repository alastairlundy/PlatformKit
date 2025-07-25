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

using AlastairLundy.CliInvoke;
using AlastairLundy.CliInvoke.Abstractions;
using AlastairLundy.CliInvoke.Builders;
using AlastairLundy.CliInvoke.Builders.Abstractions;
using AlastairLundy.CliInvoke.Core.Primitives;
using AlastairLundy.CliInvoke.Core.Primitives.Results;

using PlatformKit.Internal.Localizations;
using PlatformKit.Platforms.Providers;
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
    public class DarwinPlatformProvider : UnixPlatformProvider, IDarwinPlatformProvider
    {
        private readonly ICliCommandInvoker _cliCommandInvoker;

        public DarwinPlatformProvider(ICliCommandInvoker cliCommandInvoker) : base(cliCommandInvoker)
        {
            _cliCommandInvoker = cliCommandInvoker;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [SupportedOSPlatform("maccatalyst")]
        [SupportedOSPlatform("ios")]
        [SupportedOSPlatform("watchos")]
        [SupportedOSPlatform("tvos")]
#endif
        public new async Task<Platform> GetCurrentPlatformAsync()
        {
            Platform platform = new Platform(GetOsName(),
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                PlatformFamily.Darwin,
                await GetBuildNumberAsync(),
                await GetPlatformArchitectureAsync());

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
                 GetOsName(), await GetDarwinVersionAsync(), 
                await GetOsVersionAsync(),
                await GetKernelVersionAsync(),
                await GetBuildNumberAsync(),
                await GetPlatformArchitectureAsync());
            
            return darwinPlatform;
        }

        private async Task<Version> GetDarwinVersionAsync()
        {
            if (OperatingSystem.IsMacOS() == true || OperatingSystem.IsMacCatalyst() == true)
            {
                return Version.Parse(RuntimeInformation.OSDescription.Split(' ')[1]);
            }
            else
            {
                return Version.Parse(RuntimeInformation.OSDescription.Split(' ')[1]);
            }
        }

        protected new async Task<Architecture> GetPlatformArchitectureAsync()
        {
            if (OperatingSystem.IsMacOS() == true || OperatingSystem.IsMacCatalyst() == true)
            {
                string result = await GetUnameValueAsync("-m");

                switch (result.ToLower())
                {
                    case "x86_64":
                        return Architecture.X64;
                    case "aarch64":
                        return Architecture.Arm64;
                    case "aarch32" or "aarch":
                        return Architecture.Arm;
                    default:
                        return Architecture.X64;
                }
            }
            else
            {
                return RuntimeInformation.OSArchitecture;
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

        private string GetOsName()
        {
            return RuntimeInformation.OSDescription.Split(' ').First();
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
            ICliCommandConfigurationBuilder commandBuilder = new CliCommandConfigurationBuilder("/usr/bin/sw_vers");
            
            CliCommandConfiguration command = commandBuilder.Build();
            
            BufferedProcessResult result = await _cliCommandInvoker.ExecuteBufferedAsync(command);

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
                
                string versionString = result.Split(Environment.NewLine.ToCharArray())[1]
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
        private new async Task<Version> GetKernelVersionAsync()
        {
            if (OperatingSystem.IsMacOS() || OperatingSystem.IsMacCatalyst())
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