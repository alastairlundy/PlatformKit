/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace PlatformKit.Specializations.Windows
{
    /// <summary>
    /// An enum representing different Windows Editions.
    /// </summary>
    public enum WindowsEdition
    {
        Home,
        Education,
        Professional,
        ProfessionalForEducation,
        ProfessionalForWorkstations,
        // ReSharper disable once InconsistentNaming
        EnterpriseLTSC,
        EnterpriseSemiAnnualChannel,
        IoTCore,
        IoTEnterprise,
        // ReSharper disable once InconsistentNaming
        IoTEnterpriseLTSC,
        Team,
        Server,
        /// <summary>
        /// Windows 11 or newer only
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SE,
    }
}