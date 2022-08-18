using PlatformKit.Internal.Exceptions;

namespace PlatformKit.Internal.Licensing;

public class LicenseManager
{
    private static Licensing.PlatformKitLicenseContext _licenseContext;
    
    public static Licensing.PlatformKitLicenseContext LicenseContext
    {
        get
        {
            try
            {
                return _licenseContext;
            }
            catch
            {
                _licenseContext = PlatformKitLicenseContext.NotSet;
                return _licenseContext;
            }
        }
        set
        {
            if (value == PlatformKitLicenseContext.NotSet)
            {
                IsLicenseSet = false;
            }
            else if (value == PlatformKitLicenseContext.NonCommercial || value == PlatformKitLicenseContext.Commercial)
            {
                IsLicenseSet = true;
            }
            
            _licenseContext = value;
        }
    }

    private static bool IsLicenseSet { get; set; }
    
    public LicenseManager()
    {
        LicenseContext = PlatformKitLicenseContext.NotSet;
    }

    internal static void CheckLicenseStatus()
    {
        if (!IsLicenseSet)
        {
            throw new PlatformKitLicensingException();
        }
    }
}