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
using PlatformKit.Hardware.Common;

using PlatformKit.Mac;

namespace PlatformKit.Hardware.Mac;

/// <summary>
/// A class to represent Mac models
/// </summary>
public class MacModel
{
    //public ReleaseYearModel ReleaseYear { get; }

    MacDeviceFamily DeviceFamily
    {
        get
        {
            MacDeviceFamily macDeviceFamily;
        
            switch (MacHardware.MacDescription.ToLower().Replace(" ", String.Empty))
            {
                case "macbookair":
                    macDeviceFamily = MacDeviceFamily.MacBookAir;
                    break;
                case "macbookpro":
                    macDeviceFamily = MacDeviceFamily.MacBookPro;
                    break;
                case "macbook":
                    macDeviceFamily = MacDeviceFamily.MacBook;
                    break;
                case "macmini":
                    macDeviceFamily = MacDeviceFamily.MacMini;
                    break;
                case "macstudio":
                    macDeviceFamily = MacDeviceFamily.MacStudio;
                    break;
                case "macpro":
                    macDeviceFamily = MacDeviceFamily.MacPro;
                    break;
                case "imacpro":
                    macDeviceFamily = MacDeviceFamily.iMacPro;
                    break;
                case "imac":
                    macDeviceFamily = MacDeviceFamily.iMac;
                    break;
                case "macminiserver":
                    macDeviceFamily = MacDeviceFamily.MacMiniServer;
                    break;
                default:
                    macDeviceFamily = MacDeviceFamily.NotDetected;
                    break;
            }

            return macDeviceFamily;
        }
    }

    public MacHardwareModel MacHardware
    {
        get
        {
            return new MacHardwareModel()
            {
                MacDescription = macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Model Name"),
                ProcessorDescription = macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Processor Name"),
                SerialNumber = macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Serial Number (system)"),
                //GraphicsProcessorDescription = 
                //StartupDiskDescription = 
                //MacDisplayModel = 
            };
        }
    }
    
    public string MacIdentifier 
    {
        get
        {
            return macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Model Identifier");
        }
    }
    
    public MacOsSystemInformation InstalledOperatingSystem 
    {
        get
        {
            var macOsSystemInformation = new MacOsSystemInformation();
            macOsSystemInformation.Detect();
            return macOsSystemInformation;
        }
    }


    protected MacOSAnalyzer macOsAnalyzer;
    
    public MacModel()
    {
        macOsAnalyzer = new MacOSAnalyzer();
    }
}