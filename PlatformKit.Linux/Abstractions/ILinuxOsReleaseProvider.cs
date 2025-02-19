/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace PlatformKit.Linux.Abstractions;

public interface ILinuxOsReleaseProvider
{
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public Task<string> GetPropertyValueAsync(string propertyName);
    
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public Task<LinuxOsReleaseInfo> GetReleaseInfoAsync();

#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public Task<LinuxDistroBase> GetDistroBaseAsync();
}