/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023
    
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
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