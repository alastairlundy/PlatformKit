/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

using CliRunner;
using CliRunner.Abstractions;

using PlatformKit.Abstractions;
using PlatformKit.Providers;
using PlatformKit.Specializations.Linux;
using PlatformKit.Specializations.Windows.Abstractions;

// ReSharper disable ConvertToPrimaryConstructor

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#endif

namespace PlatformKit
{
    public class DefaultPlatformProviderFactory : IPlatformProviderFactory
    {
        private readonly ICommandRunner _commandRunner;
        private readonly ILinuxOsReleaseProvider _linuxOsReleaseSearcher;
        private readonly IWindowsSystemInfoProvider _windowsSystemInfoProvider;

        public DefaultPlatformProviderFactory(ICommandRunner commandRunner,
            ILinuxOsReleaseProvider linuxOsReleaseSearcher,
            IWindowsSystemInfoProvider windowsSystemInfoProvider)
        {
            _commandRunner = commandRunner;
            _linuxOsReleaseSearcher = linuxOsReleaseSearcher;
            _windowsSystemInfoProvider = windowsSystemInfoProvider;
        }
        
        public static DefaultPlatformProviderFactory CreateFactory(ICommandRunner commandRunner,
            ILinuxOsReleaseProvider linuxOsReleaseSearcher, IWindowsSystemInfoProvider windowsSystemInfoProvider)
        {
            return new DefaultPlatformProviderFactory(commandRunner, linuxOsReleaseSearcher, windowsSystemInfoProvider);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public IPlatformProvider CreatePlatformProvider()
        {
            PlatformFamily platformFamily;
            
            if (OperatingSystem.IsWindows())
            {
                platformFamily = PlatformFamily.WindowsNT;
            }
            else if (OperatingSystem.IsLinux())
            {
                platformFamily = PlatformFamily.Linux;
            }
            else if (OperatingSystem.IsMacOS() ||
                     OperatingSystem.IsIOS() ||
                     OperatingSystem.IsTvOS() ||
                     OperatingSystem.IsWatchOS() ||
                     OperatingSystem.IsMacCatalyst())
            {
                platformFamily = PlatformFamily.Darwin;
            }
            else if (OperatingSystem.IsFreeBSD())
            {
                platformFamily = PlatformFamily.BSD;
            }
            else if (OperatingSystem.IsAndroid())
            {
                platformFamily = PlatformFamily.Android;
            }
            /*else if (OperatingSystem.IsTizen())
            {
                platformFamily = PlatformFamily.Linux;
            }*/
#if NET5_0_OR_GREATER
            else if (OperatingSystem.IsBrowser())
            {
                platformFamily = PlatformFamily.Other;
            }
#endif
            else
            {
                throw new PlatformNotSupportedException();
            }
            
            return CreatePlatformProvider(platformFamily);
        }

        public bool TryCreatePlatformProvider(out IPlatformProvider? platformProvider)
        {
            try
            {
                platformProvider = CreatePlatformProvider();
                return true;
            }
            catch
            {
                platformProvider = null;
                return false;
            }
        }

        public IPlatformProvider CreatePlatformProvider(PlatformFamily platformFamily)
        {
            return platformFamily switch
            {
                PlatformFamily.WindowsNT => new WindowsPlatformProvider(_commandRunner, _windowsSystemInfoProvider),
                PlatformFamily.Darwin => new DarwinPlatformProvider(_commandRunner),
                PlatformFamily.Linux => new LinuxPlatformProvider(_commandRunner, _linuxOsReleaseSearcher),
                PlatformFamily.BSD => new BSDPlatformProvider(_commandRunner),
                PlatformFamily.Android => new AndroidPlatformProvider(_commandRunner),
                PlatformFamily.Other => throw new PlatformNotSupportedException(),
                PlatformFamily.Unix => new UnixPlatformProvider(_commandRunner),
                _ => throw new ArgumentOutOfRangeException(nameof(platformFamily), platformFamily, null)
            };
        }

        public bool TryCreatePlatformProvider(PlatformFamily platformFamily, out IPlatformProvider? provider)
        {
            try
            {
                provider = CreatePlatformProvider(platformFamily);
                return true;
            }
            catch
            {
                provider = null;
                return false;
            }
        }
    }
}