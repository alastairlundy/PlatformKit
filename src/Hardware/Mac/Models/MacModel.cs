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

using System;
using PlatformKit.Mac;

namespace PlatformKit.Hardware.Mac;

/// <summary>
/// A class to represent Mac models
/// </summary>
public class MacModel
{
    
    public MacHardwareModel HardwareModel { get; set; }
    
    public ReleaseYearTimeframe ReleaseTimeFrame { get; set; }
    public ReleaseYearModel ReleaseYear { get; }

    public MacDeviceFamily DeviceFamily
    {
        get
        {
            MacDeviceFamily macDeviceFamily;
            
            var hardware = new MacHardwareModel();
            hardware.Detect();

            switch (hardware.MacDescription.ToLower().Replace(" ", String.Empty))
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
            return new MacOsSystemInformation();
        }
    }


    protected MacOSAnalyzer macOsAnalyzer;
    
    public MacModel()
    {
        macOsAnalyzer = new MacOSAnalyzer();
    }
}