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
    /// 
    /// </summary>
    public class BatteryModel : HardwareComponentModel
    {

        public BatteryModel() : base()
        {
            
        }
        
        public string Name { get; set; }
        
        public string ExpectedBatteryLife { get; set; }
        public string FullCapacityCharge { get; set; }

        public string EstimatedRunTime { get; set; }
        
        public string MaxRechargeTime { get; set; }
        public string TimeToFullCharge { get; set; }
        
        public string Status { get; set; }

        protected override void DetectWindows()
        {
            ExpectedBatteryLife = _windowsAnalyzer.GetWMIValue("ExpectedBatteryLife", "Win32_Battery");
            FullCapacityCharge = _windowsAnalyzer.GetWMIValue("FullChargeCapacity", "Win32_Battery");
            EstimatedRunTime = _windowsAnalyzer.GetWMIValue("EstimatedRunTime", "Win32_Battery");
            TimeToFullCharge = _windowsAnalyzer.GetWMIValue("TimeToFullCharge", "Win32_Battery");
            MaxRechargeTime = _windowsAnalyzer.GetWMIValue("MaxRechargeTime", "Win32_Battery");
            Status = _windowsAnalyzer.GetWMIValue("Status", "Win32_Battery");
            Name = _windowsAnalyzer.GetWMIValue("Name", "Win32_Battery");
        }
    }
}