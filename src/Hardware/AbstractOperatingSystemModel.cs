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
namespace PlatformKit.Hardware{
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