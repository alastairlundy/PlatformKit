/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AlastairLundy.Extensions.Processes;
using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using PlatformKit.Specifics;

using PlatformKit.Specifics.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class AndroidPlatformProvider : IAndroidPlatformProvider
    {
        private readonly ICliCommandRunner _commandRunner;

        public AndroidPlatformProvider(ICliCommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
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
                ICliCommandBuilder commandBuilder = new CliCommandBuilder("uname")
                        .WithArguments("-m");
                
                CliCommand command = commandBuilder.Build();
                
                BufferedProcessResult result = await _commandRunner.ExecuteBufferedAsync(command);

                return result.StandardOutput switch
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
                ICliCommandBuilder commandBuilder = new CliCommandBuilder("uname")
                        .WithArguments("-o");
                
                CliCommand command = commandBuilder.Build();
                
                BufferedProcessResult result = await _commandRunner.ExecuteBufferedAsync(command);

                return result.StandardOutput;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<Version> GetPlatformVersionAsync()
        {
            string version = await GetPropValueAsync("release");
            
            return Version.Parse(version);
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<Version> GetPlatformKernelVersionAsync()
        {
                ICliCommandBuilder commandBuilder = new CliCommandBuilder("uname")
                        .WithArguments("-r");
                
                CliCommand command = commandBuilder.Build();
                
                BufferedProcessResult result = await _commandRunner.ExecuteBufferedAsync(command);

                int indexOfDash = result.StandardOutput.IndexOf('-');

                string versionString;
                
                if (indexOfDash != -1)
                {
                        versionString = result.StandardOutput.Substring(indexOfDash, result.StandardOutput.Length - indexOfDash);
                }
                else
                { 
                        versionString = result.StandardOutput;
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
            ICliCommandBuilder commandBuilder = new CliCommandBuilder("getprop")
                      .WithArguments($"ro.build.version.{value}");
                
            CliCommand command = commandBuilder.Build();
                
            BufferedProcessResult result = await _commandRunner.ExecuteBufferedAsync(command);
            
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