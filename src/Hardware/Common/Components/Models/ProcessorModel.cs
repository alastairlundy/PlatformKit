/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at
    Commercial License - https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt
    Non-Commercial License - https://neverspy.tech/platformkit-noncommercial-license or in the file PlatformKit_NonCommercial_License.txt
  
  To use PlatformKit under either a commercial or non-commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

namespace PlatformKit.Hardware.Common{
    /// <summary>
    /// A class to store Processor Information.
    /// </summary>
    public class ProcessorModel : HardwareComponentModel{
        
        public string? ProcessorName { get; set; }

        public string? Stepping { get; set; }
        public string? CPUFamily { get; set; }
        public string? Revision { get; set; }

        public bool? SupportsVirtualization { get; set; }
        
        public double? ThermalDesignPowerWatts { get; set; }
        
        public string? Socket { get; set; }
        public string? ProcessorFamilyDescription { get; set; }
        
        public string? NominalClockSpeed { get; set; }
        public string? NominalVoltage { get; set; }
        
        public int? L3CacheSizeMB { get; set; }
        public int? L2CacheSizeKB { get; set; }
        
        public string? FabricationProcess { get; set; }
        public string? ArchitectureName { get; set; }
        
        public int? BaseClockSpeedMHz { get; set; }
        public int? BoostClockSpeedMHz { get; set; }
        
        public int? CoreCount { get; set; }
        public int? ThreadCount { get; set; }
    }
}