/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

namespace PlatformKit.Identification;

/// <summary>
/// The type of RuntimeIdentifier generated or detected.
/// </summary>
public enum RuntimeIdentifierType
{
    AnyGeneric,
    Generic,
    Specific,
    /// <summary>
    /// This is meant for Linux use only. DO NOT USE ON WINDOWS or MAC.
    /// </summary>
    DistroSpecific,
    /// <summary>
    /// This is meant for Linux use only. DO NOT USE ON WINDOWS or MAC.
    /// </summary>
    VersionLessDistroSpecific
}