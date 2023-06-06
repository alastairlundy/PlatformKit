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
    /// A class to store gpu information.
    /// </summary>
    public class GraphicsProcessorModel : HardwareComponentModel{
        public int NumberOfStreamProcessors { get; set; }
        public int NumberOfTextureMappingUnits { get; set; }
        public int NumberOfRasterOperatingUnits { get; set; }
        
        public int NumberOfComputeUnits { get; set; }
        
        //public GraphicsApiSupport GraphicsApiSupport { get; set; }
        
        public string ArchitectureName { get; set; }
        
        // ReSharper disable once InconsistentNaming
        public int L2CacheSizeMB { get; set; }
        // ReSharper disable once InconsistentNaming
        public int L1CacheSizeKB { get; set; }

        public string GraphicsProcessorModelName { get; set; }
        
        public SoftwareComponentModel Driver { get; set; }
        
        public string MaxRefreshRate { get; set; }
        public string MinRefreshRate { get; set; }

        public MemoryModel VideoMemory { get; set; }
        
        public string VideoModeDescription { get; set; }
        
        public string VideoMode { get; set; }
        public string VideoArchitecture { get; set; }
        
        public int BaseClockSpeedMHz { get; set; }
        public int BoostClockSpeedMHz { get; set; }
        
        public string FabricationProcess { get; set; }
    }
}