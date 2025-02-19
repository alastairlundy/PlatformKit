/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace PlatformKit.Linux
{
    /// <summary>
    /// 
    /// </summary>
    public enum LinuxDistroBase
    {
        Debian,
        Ubuntu,
        Arch,
        Manjaro,
        Fedora,
        // ReSharper disable once InconsistentNaming
        RHEL,
        // ReSharper disable once InconsistentNaming
        SUSE,
        NotDetected,
        NotSupported,
    }
}