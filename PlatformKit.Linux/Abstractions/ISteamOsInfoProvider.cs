/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Runtime.Versioning;
using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace PlatformKit.Linux.Abstractions;

public interface ISteamOsInfoProvider
{
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public Task<SteamOSMode> GetSteamOSModeAsync();

    public Task<SteamOSMode> GetSteamOSModeAsync(bool includeHoloIsoAsSteamOs);
    
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public Task<bool> IsSteamOSAsync();
    public Task<bool> IsSteamOSAsync(bool includeHoloIsoAsSteamOs);

}