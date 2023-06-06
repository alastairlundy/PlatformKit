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
    /// A class to help store Display Information.
    /// </summary>
    public class DisplayModel : HardwareComponentModel
    {

        public DisplayModel() : base()
        {
        }
        
        public int CurrentRefreshRateHz { get; set; }
        
        public int MaximumRefreshRateHz { get; set; }
        public int MinimumRefreshRateHz { get; set; }

        public DisplayResolutionModel DisplayResolution { get; set; }

        public bool SupportsVariableRefreshRate { get; set; }

        protected override void DetectWindows()
        {
            DisplayResolution.VerticalResolutionPx = int.Parse(_windowsAnalyzer.GetWMIValue("CurrentVerticalResolution", "Win32_VideoController"));
            DisplayResolution.HorizontalResolutionPx = int.Parse(_windowsAnalyzer.GetWMIValue("CurrentHorizontalResolution", "Win32_VideoController"));
            DisplayResolution.IsHiDpi = DisplayResolution.PixelsPerInch >= 200;
            
            MaximumRefreshRateHz = int.Parse(_windowsAnalyzer.GetWMIValue("MaxRefreshRate", "Win32_VideoController"));
            MinimumRefreshRateHz = int.Parse(_windowsAnalyzer.GetWMIValue("MinRefreshRate", "Win32_VideoController"));
            CurrentRefreshRateHz = int.Parse(_windowsAnalyzer.GetWMIValue("CurrentRefreshRate", "Win32_VideoController"));
        }
    }
}