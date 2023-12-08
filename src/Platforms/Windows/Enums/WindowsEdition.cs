/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace PlatformKit.Windows;

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
    IoTEnterpriseLTSC,
    Team,
    Server,
    /// <summary>
    /// Windows 11 or newer only
    /// </summary>
    // ReSharper disable once InconsistentNaming
    SE,
}