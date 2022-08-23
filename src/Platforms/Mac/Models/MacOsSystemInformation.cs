
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