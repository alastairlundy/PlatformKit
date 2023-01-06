/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

namespace PlatformKit.Hardware.Common{
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