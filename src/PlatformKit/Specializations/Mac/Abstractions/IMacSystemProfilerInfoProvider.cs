/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Runtime.Versioning;
using System.Threading.Tasks;
using PlatformKit.OperatingSystems.Mac;

namespace PlatformKit.Specializations.Mac.Abstractions;

public interface IMacSystemProfilerInfoProvider
{
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    Task<string> GetMacSystemProfilerValue(MacSystemProfilerDataType macSystemProfilerDataType, string key);

#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    Task<bool> IsActivationLockEnabledAsync();
    
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("macos")]
    [SupportedOSPlatform("maccatalyst")]
#endif
    Task<bool> IsSecureVirtualMemoryEnabledAsync();

}