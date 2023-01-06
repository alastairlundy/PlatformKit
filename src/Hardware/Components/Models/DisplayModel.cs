/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Hardware.Common{

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
/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Hardware.Common{

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