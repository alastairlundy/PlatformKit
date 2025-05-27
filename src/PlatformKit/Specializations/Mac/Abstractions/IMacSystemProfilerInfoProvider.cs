/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace PlatformKit.Specializations.Mac;

public interface IMacSystemProfilerInfoProvider
{
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    public Task<string> GetMacSystemProfilerValue(MacSystemProfilerDataType macSystemProfilerDataType, string key);

#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    public Task<bool> IsActivationLockEnabledAsync();
    
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    public Task<bool> IsSecureVirtualMemoryEnabledAsync();

}