/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
<<<<<<< HEAD
 You can view the PlatformKit Licenses at https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt

 To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
=======
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
>>>>>>> main
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using LicensingKit.Models;

namespace PlatformKit.Internal.Licensing;

public class PlatformKitSettings
{

    public static AbstractLicense SelectedLicense { get; set; }
}