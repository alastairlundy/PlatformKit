/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable RedundantExplicitArrayCreation

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
using AlastairLundy.CliInvoke.Core.Primitives;
using PlatformKit.Internal.Localizations;
using PlatformKit.Platforms.Specifics;
using PlatformKit.Specifics.Abstractions;
using PlatformKit.Windows;
using PlatformKit.Windows.Abstractions;

namespace PlatformKit.Providers
{
    public class WindowsPlatformProvider : IWindowsPlatformProvider
    {
        private readonly ICliCommandInvoker _cliCommandInvoker;
        private readonly IWindowsSystemInfoProvider _windowsSystemInfoProvider;

        public WindowsPlatformProvider(ICliCommandInvoker cliCommandInvoker, IWindowsSystemInfoProvider windowsSystemInfoProvider)
        {
            _cliCommandInvoker = cliCommandInvoker;
            _windowsSystemInfoProvider = windowsSystemInfoProvider;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
        {
            Version platformVersion = GetOsVersion();

            Platform platform = new Platform(await GetOsNameAsync(),
                platformVersion,
                platformVersion,
                PlatformFamily.WindowsNT,
                GetOsBuildNumber(),
                await GetProcessorArchitectureAsync());
            
            return platform;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        public async Task<WindowsPlatform> GetCurrentWindowsPlatformAsync()
        {
            Version platformVersion = GetOsVersion();

            string buildNumber;
            if (platformVersion.Revision == 0)
            {
                buildNumber = platformVersion.Build.ToString();
            }
            else
            {
                buildNumber = $"{platformVersion.Build}.{platformVersion.Revision}";
            }

            WindowsPlatform platform = new WindowsPlatform(await GetOsNameAsync(),
                platformVersion,
                platformVersion,
                buildNumber,
                await _windowsSystemInfoProvider.GetWindowsEditionAsync(),
                await GetProcessorArchitectureAsync());
            
            return platform;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        private async Task<Architecture> GetProcessorArchitectureAsync()
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }

            ICliCommandConfigurationBuilder commandBuilder = new CliCommandConfigurationBuilder("echo")
                .WithArguments("%PROCESSOR_ARCHITECTURE%");
            
            CliCommandConfiguration command = commandBuilder.Build();
            
            var result = await _cliCommandInvoker.ExecuteBufferedAsync(command);

            switch (result.StandardOutput.ToLower())
            {
                case "x86":
                    return Architecture.X86;
                case "amd64":
                    return Architecture.X64;
                case "arm64":
                    return Architecture.Arm64;
                case "arm":
                    return Architecture.Arm;
                default:
                    return Architecture.X64;
            }
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        private Version GetOsVersion()
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }
            
            return Version.Parse(RuntimeInformation.OSDescription
                .Replace("Microsoft Windows", string.Empty)
                .Replace(" ", string.Empty));
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        private async Task<string> GetOsNameAsync()
        {
            WindowsSystemInfo windowsSystemInfo =
                await _windowsSystemInfoProvider.GetWindowsSystemInfoAsync();
               return windowsSystemInfo.OsName;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        private string GetOsBuildNumber()
        {
            // ReSharper disable once RedundantAssignment
            string output = string.Empty;
            
            Version osVersion = GetOsVersion();

            if (osVersion.Revision != 0)
            {
                output = $"{osVersion.Build}.{osVersion.Revision}";
            }
            else
            {
                output = $"{osVersion.Build}";
            }

            return output;
        }
    }
}