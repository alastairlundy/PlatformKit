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

using CliRunner.Abstractions;

using PlatformKit.Internal.Localizations;
using PlatformKit.Specializations.Windows;
using PlatformKit.Specializations.Windows.Abstractions;

using PlatformKit.Specifics;
using PlatformKit.Specifics.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
// ReSharper disable RedundantExplicitArrayCreation
#else
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class WindowsPlatformProvider : IWindowsPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;
        private readonly IWindowsSystemInfoProvider _windowsSystemInfoProvider;

        public WindowsPlatformProvider(ICommandRunner commandRunner, IWindowsSystemInfoProvider windowsSystemInfoProvider)
        {
            _commandRunner = commandRunner;
            _windowsSystemInfoProvider = windowsSystemInfoProvider;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
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

            Platform platform = new Platform(await GetOsNameAsync(),
                platformVersion,
                platformVersion,
                PlatformFamily.WindowsNT,
                buildNumber,
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