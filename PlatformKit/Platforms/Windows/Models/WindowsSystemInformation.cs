/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023

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

// ReSharper disable InconsistentNaming

namespace PlatformKit.Windows;

/// <summary>
/// 
/// </summary>
public class WindowsSystemInformation
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
    
    public int TotalPhysicalMemoryMB { get; set; }
    public int AvailablePhysicalMemoryMB { get; set; }
    
    public int VirtualMemoryMaxSizeMB { get; set; }
    public int VirtualMemoryAvailableSizeMB { get; set; }
    public int VirtualMemoryInUse { get; set; }
    
    public string Domain { get; set; }
    
    public string[] PageFileLocations { get; set; }
    
    public string LogonServer { get; set; }
    
    public string[] HotfixesInstalled { get; set; }
    
    public NetworkCard[] NetworkCards { get; set; }
    
    public HyperVRequirements HyperVRequirements { get; set; }

    public void ToConsoleWriteLine()
    {
        Console.WriteLine(nameof(HostName) + ": " + HostName);
        Console.WriteLine(nameof(OsName) + ": " + OsName);
        Console.WriteLine(nameof(OsVersion) + ": " + OsVersion);
        Console.WriteLine(nameof(OsManufacturer) + ": " + OsManufacturer);
        Console.WriteLine(nameof(OsConfiguration) + ": " + OsConfiguration);
        Console.WriteLine(nameof(OsBuildType) + ": " + OsBuildType);
        Console.WriteLine(nameof(RegisteredOwner) + ": " + RegisteredOwner);
        Console.WriteLine(nameof(RegisteredOrganization) + ": " + RegisteredOrganization);
        Console.WriteLine(nameof(ProductId) + ": " + ProductId);
        Console.WriteLine(nameof(OriginalInstallDate) + ": " + OriginalInstallDate);
        Console.WriteLine(nameof(SystemBootTime) + ": " + SystemBootTime);
        Console.WriteLine(nameof(SystemManufacturer) + ": " + SystemManufacturer);
        Console.WriteLine(nameof(SystemModel) + ": " + SystemModel);

        foreach (var str in Processors)
        {
            Console.WriteLine(nameof(Processors) + ": " + str);
        }
        
        Console.WriteLine(nameof(BiosVersion) + ": " + BiosVersion);
        Console.WriteLine(nameof(WindowsDirectory) + ": " + WindowsDirectory);
        Console.WriteLine(nameof(SystemDirectory) + ": " + SystemDirectory);
        Console.WriteLine(nameof(BootDevice) + ": " + BootDevice);;
        Console.WriteLine(nameof(SystemLocale) + ": " + SystemLocale);
        Console.WriteLine(nameof(InputLocale) + ": " + InputLocale);
        Console.WriteLine(nameof(TimeZone) + ": " + TimeZone);
        Console.WriteLine(nameof(TotalPhysicalMemoryMB) + ": " + TotalPhysicalMemoryMB);
        Console.WriteLine(nameof(AvailablePhysicalMemoryMB) + ": " + AvailablePhysicalMemoryMB);
        Console.WriteLine(nameof(VirtualMemoryMaxSizeMB) + ": " + VirtualMemoryMaxSizeMB);
        Console.WriteLine(nameof(VirtualMemoryAvailableSizeMB) + ": " + VirtualMemoryAvailableSizeMB);
        Console.WriteLine(nameof(VirtualMemoryInUse) + ": " + VirtualMemoryInUse);
        
        foreach (var str in PageFileLocations)
        {
            Console.WriteLine(nameof(PageFileLocations) + ": " + str);
        }
        
        Console.WriteLine(nameof(Domain) + ": " + Domain);
        Console.WriteLine(nameof(LogonServer) + ": " + LogonServer);

        foreach (var str in HotfixesInstalled)
        {
            Console.WriteLine(nameof(HotfixesInstalled) + ": " + str);
        }

        foreach (var networkCard in NetworkCards)
        {
            Console.WriteLine(nameof(networkCard) + "." + nameof(networkCard.Name) + ": " + networkCard.Name);
            Console.WriteLine(nameof(networkCard) + "." + nameof(networkCard.ConnectionName) + ": " + networkCard.ConnectionName);
            Console.WriteLine(nameof(networkCard) + "." + nameof(networkCard.DhcpEnabled) + ": " + networkCard.DhcpEnabled);
            Console.WriteLine(nameof(networkCard) + "." + nameof(networkCard.DhcpServer) + ": " + networkCard.DhcpServer);
            

            foreach (var ipAddress in networkCard.IpAddresses)
            {
                Console.WriteLine(nameof(networkCard.IpAddresses) + ": " + ipAddress);
            }
        }
          
        Console.WriteLine(nameof(HyperVRequirements.DataExecutionPreventionAvailable) + ": " + HyperVRequirements.DataExecutionPreventionAvailable);
        Console.WriteLine(nameof(HyperVRequirements.SecondLevelAddressTranslation) + ": " + HyperVRequirements.SecondLevelAddressTranslation);
        Console.WriteLine(nameof(HyperVRequirements.VirtualizationEnabledInFirmware) + ": " + HyperVRequirements.VirtualizationEnabledInFirmware);
        Console.WriteLine(nameof(HyperVRequirements.VmMonitorModeExtensions) + ": " + HyperVRequirements.VmMonitorModeExtensions);
        
    }
}