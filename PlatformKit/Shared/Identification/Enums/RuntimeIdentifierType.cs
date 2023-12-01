/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
     any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
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