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

using System;

namespace PlatformKit.Mac;

/// <summary>
/// A class to represent basic macOS System Information.
/// </summary>
public class MacOsSystemInformation
{
    public Version MacOsVersion { get; set; }
    
    public Version DarwinVersion { get; set; }
    
    public Version XnuVersion { get; set; } 
    
    public string MacOsBuildNumber { get; set; }
    
    public MacProcessorType ProcessorType { get; set; }
}