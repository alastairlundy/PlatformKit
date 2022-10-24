using System;
using System.Runtime.InteropServices;

using PlatformKit.Hardware.Common;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Hardware.Common;

public class MemoryDetection
{
    protected MemoryModel DetectWindows()
    {
        MemoryModel memoryModel = new MemoryModel();

        WindowsAnalyzer windowsAnalyzer = new WindowsAnalyzer();

        var windowsSystemInfo = windowsAnalyzer.GetWindowsSystemInformation();
        
        memoryModel.TotalPhysicalRamMB = windowsSystemInfo.TotalPhysicalMemoryMB;
        memoryModel.AvailablePhysicalRamMB = windowsSystemInfo.AvailablePhysicalMemoryMB;
        memoryModel.AvailableVirtualRamMB = windowsSystemInfo.VirtualMemoryAvailableSizeMB;
        memoryModel.TotalVirtualRamMB = windowsSystemInfo.VirtualMemoryMaxSizeMB;

       //Get all other MemoryModel values from WMI
       memoryModel.MinVoltageMillivolts = int.Parse(windowsAnalyzer.GetWMIValue("MinVoltage", "Win32_PhysicalMemory"));
       memoryModel.MaxVoltageMillivolts = int.Parse(windowsAnalyzer.GetWMIValue("MaxVoltage", "Win32_PhysicalMemory"));
       memoryModel.MemorySpeedMHz = int.Parse(windowsAnalyzer.GetWMIValue("ConfiguredClockSpeed", "Win32_PhysicalMemory"));

       return memoryModel;
    }

    protected MemoryModel DetectMac()
    {
        MemoryModel memoryModel = new MemoryModel();

        MacOSAnalyzer macOsAnalyzer = new MacOSAnalyzer();

      //  memoryModel.AvailablePhysicalRamMB =
        //    macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "");

        return memoryModel;
    }

    protected MemoryModel DetectLinux()
    {
        throw new System.NotImplementedException();
    }

    public MemoryModel Detect()
    {
        if (OSAnalyzer.IsWindows())
        {
            return DetectWindows();
        }
        else if (OSAnalyzer.IsMac())
        {
            return DetectMac();
        }
        else if (OSAnalyzer.IsLinux())
        {
            return DetectLinux();
        }

        throw new PlatformNotSupportedException();
    }
}