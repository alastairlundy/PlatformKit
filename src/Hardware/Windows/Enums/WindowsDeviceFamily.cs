/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023
    Copyright (c) NeverSpyTech Limited 2022

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

namespace PlatformKit.Hardware.Windows;

/// <summary>
/// 
/// </summary>
public enum WindowsDeviceFamily
{
    /// <summary>
    /// Windows 10 or 11 [Home,Pro,Education,Enterprise,LTSC,Server]
    /// </summary>
    Desktop,
    /// <summary>
    /// Windows 10 or 11 based Xbox OS.
    /// </summary>
    Xbox,
    HoloLens,
    /// <summary>
    /// Windows 10 or 11 Teams OS.
    /// </summary>
    SurfaceHub,
}