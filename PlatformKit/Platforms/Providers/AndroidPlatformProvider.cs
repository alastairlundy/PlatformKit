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

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
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
        private readonly ICommandRunner _commandRunner;

        public AndroidPlatformProvider(ICommandRunner commandRunner)
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
                ICommandBuilder commandBuilder = new CommandBuilder("uname")
                        .WithArguments("-m");
                
                Command command = commandBuilder.ToCommand();
                
                BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);

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

        private async Task<string> GetDeviceNameAsync()
        { 
                return await GetPropValueAsync("ro.product.model");
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<string> GetPlatformNameAsync()
        {
                ICommandBuilder commandBuilder = new CommandBuilder("uname")
                        .WithArguments("-o");
                
                Command command = commandBuilder.ToCommand();
                
                BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);

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
            ICommandBuilder commandBuilder = new CommandBuilder("getprop")
                      .WithArguments($"ro.build.version.{value}");
                
            Command command = commandBuilder.ToCommand();
                
            BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);
            
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