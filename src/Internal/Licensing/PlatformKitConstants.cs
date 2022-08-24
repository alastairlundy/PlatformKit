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

using LicensingKit;
using LicensingKit.Enums;
using LicensingKit.Models;

namespace PlatformKit.Internal.Licensing;

public class PlatformKitConstants
{
    public static readonly OpenSourceLicense GPLv3_OrLater =
        new("GPLv3_OrLater", LicenseType.OpenSource, CommonOpenSourceLicenses.GPLv3_OR_LATER);

    public static readonly ProprietaryLicense PlatformKitCommercialLicense = new("PlatformKit Commercial License",
        LicenseType.ClosedSourceProprietary,
        new LicensePermissionsModel(true,
            false,
            true,
            true,
            true,
            false,
            false,
            true));
    
    public static readonly ProprietaryLicense PlatformKitNonCommercialLicense = new("PlatformKit Non-Commercial License",
        LicenseType.ClosedSourceProprietary,
        new LicensePermissionsModel(true,
            false,
            true,
            true,
            false,
            false,
            false,
            true));

    internal static void CheckLicenseState()
    {
        LicenseManager.RegisterLicense(PlatformKitConstants.GPLv3_OrLater);
        LicenseManager.RegisterLicense(PlatformKitConstants.PlatformKitCommercialLicense);
        LicenseManager.RegisterLicense(PlatformKitNonCommercialLicense);
        LicenseManager.SelectedLicense = PlatformKitSettings.SelectedLicense;
        LicenseManager.CheckLicenseStatus();
    }
}