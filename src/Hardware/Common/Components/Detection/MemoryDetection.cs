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

using System;
using System.Runtime.InteropServices;

using PlatformKit.Hardware.Common;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Hardware.Common;

public class MemoryDetection : BaseDetection
{
    public MemoryDetection()
    {
        _windowsAnalyzer = new WindowsAnalyzer();
        _macOsAnalyzer = new MacOSAnalyzer();
    }
    
    protected override MemoryModel DetectWindows()
    {
        MemoryModel memoryModel = new MemoryModel();
        
        var windowsSystemInfo = _windowsAnalyzer.GetWindowsSystemInformation();
        
        memoryModel.TotalPhysicalRamMB = windowsSystemInfo.TotalPhysicalMemoryMB;
        memoryModel.AvailablePhysicalRamMB = windowsSystemInfo.AvailablePhysicalMemoryMB;
        memoryModel.AvailableVirtualRamMB = windowsSystemInfo.VirtualMemoryAvailableSizeMB;
        memoryModel.TotalVirtualRamMB = windowsSystemInfo.VirtualMemoryMaxSizeMB;

       //Get all other MemoryModel values from WMI
       memoryModel.MinVoltageMillivolts = int.Parse(_windowsAnalyzer.GetWMIValue("MinVoltage", "Win32_PhysicalMemory"));
       memoryModel.MaxVoltageMillivolts = int.Parse(_windowsAnalyzer.GetWMIValue("MaxVoltage", "Win32_PhysicalMemory"));
       memoryModel.MemorySpeedMHz = int.Parse(_windowsAnalyzer.GetWMIValue("ConfiguredClockSpeed", "Win32_PhysicalMemory"));

       return memoryModel;
    }

    protected override MemoryModel DetectMac()
    {
        MemoryModel memoryModel = new MemoryModel();

        //  memoryModel.AvailablePhysicalRamMB =
        //    macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "");

        return memoryModel;
    }
}