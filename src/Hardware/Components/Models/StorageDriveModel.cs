/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

namespace PlatformKit.Hardware.Common{

    /// <summary>
    /// A class to help store Storage Drive Information.
    /// </summary>
    public class StorageDriveModel : HardwareComponentModel{
        public int? TotalTBWritten { get; set; }
        public int? TotalTBRead { get; set; }

        public StorageDriveType? DriveType { get; set; }
        public int? CapacityGB { get; set; }
        public int? CapacityTB => CapacityGB / 1000;
    }
}