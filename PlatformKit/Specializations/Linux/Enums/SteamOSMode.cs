/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace PlatformKit.Specializations.Linux
{
    /// <summary>
    /// The mode that SteamOS 3.x and newer is running in.
    /// </summary>
// ReSharper disable once InconsistentNaming
    public enum SteamOSMode
    {
        /// <summary>
        /// The normal UI of the SteamDeck without a desktop environment running.
        /// </summary>
        GamingMode,
        /// <summary>
        /// The mode where the Manjaro desktop environment is running.
        /// </summary>
        DesktopMode,
        // ReSharper disable once InconsistentNaming
        NotSteamOS
    }
}