/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt

 To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System.Reflection;
using LicensingKit;
using LicensingKit.Enums;
using LicensingKit.Models;

using PlatformKit.Internal.Exceptions;
using SKM.V3;
using SKM.V3.Methods;

namespace PlatformKit.Internal.Licensing;

public class PlatformKitConstants
{
    // ReSharper disable once InconsistentNaming
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

    internal static void CheckLicenseState()
    {
        LicenseManager.RegisterLicense(GPLv3_OrLater);
        LicenseManager.RegisterLicense(PlatformKitCommercialLicense);
        LicenseManager.SelectedLicense = PlatformKitSettings.SelectedLicense;
        LicenseManager.CheckLicenseStatus();
        
        if (PlatformKitSettings.SelectedLicense == PlatformKitCommercialLicense)
        {
            VerifyKey();
            //VerifyProjectCount();
        }
    }

    protected static void VerifyKey()
    {
        var rsa =
        
       var license = SKM.V3.Methods.Helpers.VerifySDKLicenseCertificate(rsa);

       if (license == null)
       {
           throw new AssemblyNotSignedException();
       }
    }

    protected static void VerifyProjectCount()
    {
     //   string projectName = Assembly.GetExecutingAssembly().GetName().Name;
    }
}