/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

using CliRunner;
using CliRunner.Abstractions;

using PlatformKit.Abstractions;
using PlatformKit.Providers;

namespace PlatformKit
{
    public class DefaultPlatformProviderFactory : IPlatformProviderFactory
    {
        private readonly ICommandRunner _commandRunner;

        public DefaultPlatformProviderFactory(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
        public static DefaultPlatformProviderFactory CreateFactory()
        {
            return new DefaultPlatformProviderFactory(new CommandRunner(new CommandPipeHandler()));
        }

        public static DefaultPlatformProviderFactory CreateFactory(ICommandRunner commandRunner)
        {
            return new DefaultPlatformProviderFactory(commandRunner);
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
            else if (OperatingSystem.IsBrowser())
            {
                platformFamily = PlatformFamily.Other;
            }
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
                PlatformFamily.WindowsNT => new WindowsPlatformProvider(_commandRunner),
                PlatformFamily.Darwin => new DarwinPlatformProvider(_commandRunner),
                PlatformFamily.Linux => new LinuxPlatformProvider(_commandRunner),
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