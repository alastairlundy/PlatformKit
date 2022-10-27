using System;
using PlatformKit.Windows;

namespace PlatformKit.Hardware.Common;

public class ProcessorDetection
{
    protected ProcessorModel DetectWindows()
    {
        ProcessorModel processorModel = new ProcessorModel();

        WindowsAnalyzer windowsAnalyzer = new WindowsAnalyzer();

        processorModel.ProcessorName = windowsAnalyzer.GetWMIValue("Name", "Win32_Processor");
        processorModel.CoreCount = int.Parse(windowsAnalyzer.GetWMIValue("NumberOfCores", "Win32_Processor"));
        processorModel.ThreadCount = int.Parse(windowsAnalyzer.GetWMIValue("ThreadCount", "Win32_Processor"));
        //processorModel Nominal clockspeed
        
        processorModel.BoostClockSpeedMHz = int.Parse(windowsAnalyzer.GetWMIValue("MaxClockSpeed", "Win32_Processor"));

        processorModel.ArchitectureName = windowsAnalyzer.GetWMIValue("Architecture", "Win32_Processor");
        processorModel.CPUFamily = windowsAnalyzer.GetWMIValue("Family", "Win32_Processor");
        processorModel.ProcessorFamilyDescription = windowsAnalyzer.GetWMIValue("OtherFamilyDescription", "Win32_Processor");

        processorModel.L2CacheSizeKB = int.Parse(windowsAnalyzer.GetWMIValue("L2CacheSize", "Win32_Processor"));
        processorModel.L3CacheSizeMB = int.Parse(windowsAnalyzer.GetWMIValue("L3CacheSize", "Win32_Processor")) / 1024;

        processorModel.Socket = windowsAnalyzer.GetWMIValue("SocketDesignation", "Win32_Processor");
        processorModel.Revision = windowsAnalyzer.GetWMIValue("Revision", "Win32_Processor");
        
        processorModel.SupportsVirtualization = Boolean.Parse(windowsAnalyzer.GetWMIValue("VirtualizationFirmwareEnabled", "Win32_Processor"));

        return processorModel;
    }

    protected ProcessorModel DetectMac()
    {
        throw new NotImplementedException();
    }

    protected ProcessorModel DetectLinux()
    {
        throw new NotImplementedException();
    }

    public ProcessorModel Detect()
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