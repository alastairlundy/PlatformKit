/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable InconsistentNaming

using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using AlastairLundy.CliInvoke.Abstractions;


using PlatformKit.Abstractions;
using PlatformKit.Internal.Localizations;
using PlatformKit.Platforms.Providers;

// ReSharper disable ConvertToPrimaryConstructor


#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.InteropServices;
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Providers
{
    public class BSDPlatformProvider : UnixPlatformProvider
    {
        private readonly ICliCommandInvoker _cliCommandInvoker;

        public BSDPlatformProvider(ICliCommandInvoker cliCommandInvoker) : base(cliCommandInvoker)
        {
            _cliCommandInvoker = cliCommandInvoker;
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        public new async Task<Platform> GetCurrentPlatformAsync()
        {
            Platform platform = new Platform(
                await GetOsNameAsync(),
                await GetPlatformVersionAsync(),
                await GetKernelVersionAsync(),
                PlatformFamily.BSD,
                await GetBuildNumberAsync(),
                await GetPlatformArchitectureAsync());

            return platform;
        }

        private async Task<string> GetBuildNumberAsync()
        {
            
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
#endif
        private async Task<string> GetOsNameAsync()
        {
            if (OperatingSystem.IsFreeBSD())
            {
                try
                {
#if NET6_0_OR_GREATER
                    string[] lines = await File.ReadAllLinesAsync("/etc/freebsd-release");
#else
                    string[] lines = await FilePolyfill.ReadAllLinesAsync("/etc/freebsd-release");
#endif

                    string result = lines.First(x =>
                            x.Contains("name=", StringComparison.CurrentCultureIgnoreCase))
                        .Replace("Name=", string.Empty);

                    return result;
                }
                catch
                {
                    return "FreeBSD";
                }
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }
        }
    }
}