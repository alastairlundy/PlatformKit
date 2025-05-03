/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

using AlastairLundy.CliInvoke;
using AlastairLundy.CliInvoke.Abstractions;
using AlastairLundy.CliInvoke.Builders;
using AlastairLundy.CliInvoke.Builders.Abstractions;

using AlastairLundy.Extensions.Processes;
using PlatformKit.Providers;
using PlatformKit.Specifics;
using PlatformKit.Specifics.Abstractions;

namespace PlatformKit.Platforms.Providers
{
    public class AndroidPlatformProvider : UnixPlatformProvider, IAndroidPlatformProvider
    { 
            private readonly ICliCommandInvoker _cliCommandInvoker;

        public AndroidPlatformProvider(ICliCommandInvoker cliCommandInvoker) : base(cliCommandInvoker)
        {
            _cliCommandInvoker = cliCommandInvoker;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        public new async Task<Platform> GetCurrentPlatformAsync()
        {
            return new Platform(
                await GetPlatformNameAsync(),
                await GetPlatformVersionAsync(),
                await GetPlatformKernelVersionAsync(),
                PlatformFamily.Android,
                await GetBuildNumberAsync(),
                await GetProcessorArchitectureAsync());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        public async Task<AndroidPlatform> GetCurrentAndroidPlatformAsync()
        {
            return new AndroidPlatform(await GetPlatformNameAsync(),
                await GetPlatformVersionAsync(),
                await GetPlatformKernelVersionAsync(),
                await GetSdkLevelAsync(),
                await GetCodeNameAsync(),
                await GetBuildNumberAsync(),
                await GetDeviceNameAsync(),
                await GetProcessorArchitectureAsync());
        }
        
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
        [SupportedOSPlatform("linux")]
#endif
        private async Task<Architecture> GetProcessorArchitectureAsync()
        {
                string result = await GetUnameValueAsync("-m");
                
                return result switch
                {
                        "aarch64" => Architecture.Arm64,
                        "aarch32" => Architecture.Arm,
                        "x86_64" => Architecture.X64,
                        "x86" => Architecture.X86,
                        _ => RuntimeInformation.OSArchitecture
                };
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<string> GetBuildNumberAsync()
        {
                string descProp = await GetPropValueAsync("ro.build.description");
                
                string[] results = descProp.Split(' ');
                
                return results[3];
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<string> GetDeviceNameAsync()
        { 
                return await GetPropValueAsync("ro.product.model");
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<string> GetPlatformNameAsync()
        { 
                return await GetUnameValueAsync("-o");
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private new async Task<Version> GetPlatformVersionAsync()
        {
            string version = await GetPropValueAsync("release");
            
            return Version.Parse(version);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<Version> GetPlatformKernelVersionAsync()
        {
                string result = await GetUnameValueAsync("-r");

                int indexOfDash = result.IndexOf('-');

                string versionString;
                
                if (indexOfDash != -1)
                {
                        versionString = result.Substring(indexOfDash, result.Length - indexOfDash);
                }
                else
                { 
                        versionString = result;
                }
                
                return Version.Parse(versionString);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<int> GetSdkLevelAsync()
        {
            string version = await GetPropValueAsync("sdk");
                
            return int.Parse(version);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<string> GetPropValueAsync(string value)
        {
            ICliCommandConfigurationBuilder commandBuilder = new CliCommandConfigurationBuilder("getprop")
                      .WithArguments($"ro.build.version.{value}");
                
            CliCommandConfiguration command = commandBuilder.Build();
                
            BufferedProcessResult result = await _cliCommandInvoker.ExecuteBufferedAsync(command);
            
            return result.StandardOutput.Replace(" ", string.Empty);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<string> GetCodeNameAsync()
        {
            return await GetPropValueAsync("codename");
        }
    }
}