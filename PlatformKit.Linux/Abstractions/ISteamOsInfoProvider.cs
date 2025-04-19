/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

using System.Threading.Tasks;

// ReSharper disable InconsistentNaming

namespace PlatformKit.Linux.Abstractions;

/// <summary>
/// 
/// </summary>
public interface ISteamOsInfoProvider
{
    
    /// <summary>
    /// Retrieves the current SteamOS mode.
    /// </summary>
    /// <returns>The current SteamOS mode.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    Task<SteamOSMode> GetSteamOSModeAsync();
    
    /// <summary>
    /// Retrieves the current SteamOS mode, including whether to include HoloISO as Steam OS.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs">Whether HoloISO should be considered as Steam OS.</param>
    /// <returns>The current SteamOS mode.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    Task<SteamOSMode> GetSteamOSModeAsync(bool includeHoloIsoAsSteamOs);
    
    /// <summary>
    /// Checks whether the current OS is Steam OS.
    /// </summary>
    /// <returns>True if the current OS is Steam OS; false otherwise.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    Task<bool> IsSteamOSAsync();
    
    /// <summary>
    /// Checks whether the current system is running a Steam OS, including an option to include HoloISO as Steam OS.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs">Whether HoloISO should be considered as Steam OS.</param>
    /// <returns>A boolean indicating whether the system is running a Steam OS.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    Task<bool> IsSteamOSAsync(bool includeHoloIsoAsSteamOs);
}