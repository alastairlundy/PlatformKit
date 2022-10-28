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

using PlatformKit.Windows;

namespace PlatformKit.Hardware.Common;

public class DisplayDetection : BaseDetection
{

    public DisplayDetection()
    {
        _windowsAnalyzer = new WindowsAnalyzer();
    }

    protected override DisplayModel DetectWindows()
    {
        DisplayModel displayModel = new();

        displayModel.DisplayResolution = new DisplayResolutionModel()
        {
            VerticalResolutionPx = int.Parse(_windowsAnalyzer.GetWMIValue("CurrentVerticalResolution","Win32_VideoController")),
            HorizontalResolutionPx = int.Parse(_windowsAnalyzer.GetWMIValue("CurrentHorizontalResolution","Win32_VideoController")),
            IsHiDpi = displayModel.DisplayResolution.PixelsPerInch >= 200
        };

        displayModel.MaximumRefreshRateHz = int.Parse(_windowsAnalyzer.GetWMIValue("MaxRefreshRate", "Win32_VideoController"));
        displayModel.MinimumRefreshRateHz = int.Parse(_windowsAnalyzer.GetWMIValue("MinRefreshRate", "Win32_VideoController"));
        displayModel.CurrentRefreshRateHz = int.Parse(_windowsAnalyzer.GetWMIValue("CurrentRefreshRate", "Win32_VideoController"));

        return displayModel;
    }

}