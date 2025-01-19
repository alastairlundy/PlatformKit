/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Linq;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Buffered;
using CliRunner.Extensions;
using CliRunner.Specializations;

using PlatformKit.Abstractions;
using PlatformKit.Internal.Localizations;
using PlatformKit.Specifics;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
// ReSharper disable RedundantExplicitArrayCreation
#endif

namespace PlatformKit.Providers
{
    public class WindowsPlatformProvider : IPlatformProvider
    {
        private readonly ICommandRunner _commandRunner;

        public WindowsPlatformProvider(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        public async Task<Platform> GetCurrentPlatformAsync()
        {
            Version platformVersion = GetOsVersion();

            string buildNumber = "";
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
                buildNumber);
            
            return platform;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]        
#endif
        public async Task<WindowsPlatform> GetCurrentWindowsPlatformAsync()
        {
            Version platformVersion = GetOsVersion();

            WindowsPlatform platform = new WindowsPlatform(await GetOsNameAsync(),
                platformVersion,
                platformVersion,
                GetOsBuildNumber());
            
            return platform;
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
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }

            BufferedCommandResult result = await CmdCommand.CreateInstance(_commandRunner)
                .WithArguments("systeminfo")
                .ExecuteBufferedAsync(_commandRunner);
            
            var lines = result.StandardOutput.Split(Environment.NewLine);
            
            return lines.First(x => x.Contains("OS Name:"))
                .Replace("OS Name:", string.Empty);
        }
        
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