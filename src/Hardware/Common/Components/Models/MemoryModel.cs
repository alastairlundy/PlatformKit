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
    /// A class to store Memory Information.
    /// </summary>
    public class MemoryModel : HardwareComponentModel
    {
        public MemoryModel() : base()
        {
            
        }
        
        public int AvailablePhysicalRamMB { get; set; }
        public int AvailablePhysicalRamGB => AvailablePhysicalRamMB / 1000;

        public int TotalPhysicalRamMB { get; set; }
        public int TotalPhysicalRamGB => TotalPhysicalRamMB / 1000;

        
        public int AvailableVirtualRamMB { get; set; }
        public int AvailableVirtualRamGB => AvailableVirtualRamMB / 1000;

        public int TotalVirtualRamMB { get; set; }
        public int TotalVirtualRamGB => TotalVirtualRamMB / 1000;
        
        public int MemorySpeedMHz { get; set; }

        public int MinVoltageMillivolts { get; set; }
        public int MaxVoltageMillivolts { get; set; }

        protected override void DetectWindows()
        {
            var windowsSystemInfo = _windowsAnalyzer.GetWindowsSystemInformation();
            
            TotalPhysicalRamMB = windowsSystemInfo.TotalPhysicalMemoryMB;
            AvailablePhysicalRamMB = windowsSystemInfo.AvailablePhysicalMemoryMB;
            AvailableVirtualRamMB = windowsSystemInfo.VirtualMemoryAvailableSizeMB;
            TotalVirtualRamMB = windowsSystemInfo.VirtualMemoryMaxSizeMB;
            
            //Get all other MemoryModel values from WMI
            MinVoltageMillivolts = int.Parse(_windowsAnalyzer.GetWMIValue("MinVoltage", "Win32_PhysicalMemory"));
            MaxVoltageMillivolts = int.Parse(_windowsAnalyzer.GetWMIValue("MaxVoltage", "Win32_PhysicalMemory"));
            MemorySpeedMHz = int.Parse(_windowsAnalyzer.GetWMIValue("ConfiguredClockSpeed", "Win32_PhysicalMemory"));
        }
    }
}