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

namespace PlatformKit.Hardware.Mac;

/// <summary>
/// The device family that a Mac belongs to.
/// </summary>
public enum MacDeviceFamily
{
    MacBook,
    MacBookAir,
    MacBookPro,
    MacMini,
    MacMiniServer,
    MacStudio,
    MacPro,
    // ReSharper disable once InconsistentNaming
    iMac,
    // ReSharper disable once InconsistentNaming
    iMacPro,
    NotDetected
}