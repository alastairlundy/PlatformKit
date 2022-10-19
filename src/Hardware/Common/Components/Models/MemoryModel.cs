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
    /// A class to store Memory Information.
    /// </summary>
    public class MemoryModel : HardwareComponentModel
    {
        public int AvailablePhysicalRamMB { get; set; }
        public int AvailablePhysicalRamGB => AvailablePhysicalRamMB / 1000;

        public int TotalPhysicalRamMB { get; set; }
        public int TotalPhysicalRamGB => TotalPhysicalRamMB / 1000;

        
        public int AvailableVirtualRamMB { get; set; }
        public int AvailableVirtualRamGB => AvailableVirtualRamMB / 1000;

        public int TotalVirtualRamMB { get; set; }
        public int TotalVirtualRamGB => TotalVirtualRamMB / 1000;
        
        public int VoltageMilliVolts { get; set; }
        public int MemorySpeedMHz { get; set; }

        public int MinVoltageMillivolts { get; set; }
        public int MaxVoltageMillivolts { get; set; }
    }
}