/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

using PlatformKit.Core;

// ReSharper disable once CheckNamespace
namespace PlatformKit.Windows
{
    public class WindowsSystemInfo
    {
        public string HostName { get; set; }
    
        public string OsName { get; set; }
        public string OsVersion { get; set; }
        public string OsManufacturer { get; set; }
        public string OsConfiguration { get; set; }
        public string OsBuildType { get; set; }
    
        public string RegisteredOwner { get; set; }
        public string RegisteredOrganization { get; set; }
    
        public string ProductId { get; set; }
    
        public DateTime OriginalInstallDate { get; set; }
        public DateTime SystemBootTime { get; set; }
    
        public string SystemManufacturer { get; set; }
        public string SystemModel { get; set; }
        public string SystemType { get; set; }
    
        public string[] Processors { get; set; }
    
        public string BiosVersion { get; set; }
    
    
        public string WindowsDirectory { get; set; }
        public string SystemDirectory { get; set; }
    
        public string BootDevice { get; set; }
    
    
        public string SystemLocale { get; set; }
        public string InputLocale { get; set; }
    
        public TimeZoneInfo TimeZone { get; set; }
    
        public int TotalPhysicalMemoryMegabyte { get; set; }
        public int AvailablePhysicalMemoryMegabyte { get; set; }
    
        public int VirtualMemoryMaxSizeMegabyte { get; set; }
        public int VirtualMemoryAvailableSizeMegabyte { get; set; }
        public int VirtualMemoryInUse { get; set; }
    
        public string Domain { get; set; }
    
        public string[] PageFileLocations { get; set; }
    
        public string LogonServer { get; set; }
    
        public string[] HotfixesInstalled { get; set; }
    
        public NetworkCardModel[] NetworkCards { get; set; }
    
        public HyperVRequirementsInfo HyperVRequirements { get; set; }
    }
}