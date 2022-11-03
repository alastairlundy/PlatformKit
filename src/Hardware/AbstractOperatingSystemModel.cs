/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;

namespace PlatformKit.Hardware.Common{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AbstractOperatingSystemModel : SoftwareComponentModel{

        public string EncryptionLevel { get; set; }
        public string Locale { get; set; }
        public string InstallDate { get; set; }
        
        public bool IsPaeEnabled { get; set; }
        
        public string SystemDrive { get; set; }
        public string SystemDirectory { get; set; }
        
        public System.Version OsKernelVersion { get; set; }

        public object Owner { get; set; }
        public string BootDevice { get; set; }
        
        public string CountryCode { get; set; }
        public string CurrentTimeZone { get; set; }

        public KernelSupportType OsKernelSupportType { get; set; }
        
        public System.Version OsVersion { get; set; }
    }
}