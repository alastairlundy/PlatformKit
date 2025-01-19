/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

// ReSharper disable InconsistentNaming
namespace PlatformKit
{
    public enum PlatformFamily
    {
        WindowsNT,
        /// <summary>
        /// Darwin derived operating systems such as macOS, IOS, iPadOS, tvOS, watchOS, and visionOS are included in this.
        /// </summary>
        Darwin,
        Linux,
        BSD,
        Unix,
        /// <summary>
        /// Android based Operating Systems such as Android TV, wearOS, and FireOS are included in this.
        /// </summary>
        Android,
        Other
    }
}