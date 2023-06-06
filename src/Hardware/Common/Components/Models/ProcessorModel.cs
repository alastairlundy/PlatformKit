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

namespace PlatformKit.Hardware.Components{
    /// <summary>
    /// A class to store Processor Information.
    /// </summary>
    public class ProcessorModel : HardwareComponentModel{

        public ProcessorModel() : base()
        {
        }
        
        public string ProcessorName { get; set; }

        public string Stepping { get; set; }
        public string CPUFamily { get; set; }
        public string Revision { get; set; }

        public bool SupportsVirtualization { get; set; }
        
        public double ThermalDesignPowerWatts { get; set; }
        
        public string Socket { get; set; }
        public string ProcessorFamilyDescription { get; set; }
        
        public string NominalClockSpeed { get; set; }
        public string NominalVoltage { get; set; }
        
        public int L3CacheSizeMB { get; set; }
        public int L2CacheSizeKB { get; set; }
        
        public string FabricationProcess { get; set; }
        public string ArchitectureName { get; set; }
        
        public int BaseClockSpeedMHz { get; set; }
        public int BoostClockSpeedMHz { get; set; }
        
        public int CoreCount { get; set; }
        public int ThreadCount { get; set; }

        protected override void DetectWindows()
        {
            ProcessorName = _windowsAnalyzer.GetWMIValue("Name", "Win32_Processor");
            CoreCount = int.Parse(_windowsAnalyzer.GetWMIValue("NumberOfCores", "Win32_Processor"));
            ThreadCount = int.Parse(_windowsAnalyzer.GetWMIValue("ThreadCount", "Win32_Processor"));
            //processorModel Nominal clockspeed

            BoostClockSpeedMHz = int.Parse(_windowsAnalyzer.GetWMIValue("MaxClockSpeed", "Win32_Processor"));

            ArchitectureName = _windowsAnalyzer.GetWMIValue("Architecture", "Win32_Processor");
            CPUFamily = _windowsAnalyzer.GetWMIValue("Family", "Win32_Processor");
            ProcessorFamilyDescription = _windowsAnalyzer.GetWMIValue("OtherFamilyDescription", "Win32_Processor");

            L2CacheSizeKB = int.Parse(_windowsAnalyzer.GetWMIValue("L2CacheSize", "Win32_Processor"));
            L3CacheSizeMB = int.Parse(_windowsAnalyzer.GetWMIValue("L3CacheSize", "Win32_Processor")) / 1024;

            Socket = _windowsAnalyzer.GetWMIValue("SocketDesignation", "Win32_Processor");
            Revision = _windowsAnalyzer.GetWMIValue("Revision", "Win32_Processor");

            SupportsVirtualization =
                bool.Parse(_windowsAnalyzer.GetWMIValue("VirtualizationFirmwareEnabled", "Win32_Processor"));

        }
    }
}