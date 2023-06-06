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

using PlatformKit.Mac;

namespace PlatformKit.Hardware.Mac;

public class MacHardwareModel
{
    public string MacDescription { get; set; }

    public string SerialNumber { get; set; }
    
    public string ProcessorDescription { get; set; }
    
    public string MemoryDescription { get; set; }
    
    public string StartupDiskDescription { get; set; }
    
    public string GraphicsProcessorDescription { get; set; }
    
    public MacDisplayModel MacDisplayModel { get; set; }

    /// <summary>
    /// Detects the MacHardwareModel information from macOS System Profiler
    /// </summary>
    public void Detect()
    {
        MacOSAnalyzer macOsAnalyzer = new MacOSAnalyzer();

        MacDescription =
            macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Model Name");
        ProcessorDescription =
            macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Processor Name");
        SerialNumber =
            macOsAnalyzer.GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType,
                "Serial Number (system)");
        //GraphicsProcessorDescription = 
        //StartupDiskDescription = 
        //MacDisplayModel = 
    }
}