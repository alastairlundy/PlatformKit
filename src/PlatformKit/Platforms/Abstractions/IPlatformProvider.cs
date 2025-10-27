/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System.Threading.Tasks;

namespace PlatformKit.Abstractions
{
    /// <summary>
    /// An interface to allow for providing details on the current Platform.
    /// </summary>
    public interface IPlatformInfoProvider
    {
        public Task<Platform> GetCurrentPlatformAsync();
    }
}