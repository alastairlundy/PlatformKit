/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using PlatformKit.Hardware.Common;
using PlatformKit.Windows;

namespace PlatformKit.Hardware.Windows;

public class WindowsOperatingSystemModel : AbstractOperatingSystemModel
{
    public new bool IsProprietary => true;

    public WindowsEdition Edition { get; set; }
    public string ReleaseName { get; set; }
    
    public string ReleaseChannel { get; set; }
    public int BuildNumber { get; set; }
    public string ProductId { get; set; }
}