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

using PlatformKit.Specifics;

using PlatformKit.Specifics.Abstractions;

#if NET5_0_OR_GREATER
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
                PlatformFamily.Android);
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
                await GetCodeNameAsync());
        }

        private async Task<string> GetPlatformNameAsync()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("android")]
#endif
        private async Task<int> GetSdkLevelAsync()
        {
            string version = await GetPropValueAsync("sdk");
                
            return int.Parse(version);
        }

        private async Task<string> GetPropValueAsync(string value)
        {
            BufferedCommandResult result = await Cli.Run("getprop")
                .WithArguments($"ro.build.version.{value}")
                .ExecuteBufferedAsync(_commandRunner);

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