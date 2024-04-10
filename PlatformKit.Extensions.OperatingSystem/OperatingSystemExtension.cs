using System;
using System.Runtime.InteropServices;

namespace PlatformKit.Extensions.OperatingSystem
{
    public static class OperatingSystemExtension
    {
        internal static System.OperatingSystem GetSystem(PlatformID platformId)
        {
            return new System.OperatingSystem(platformId, Environment.OSVersion.Version);
        }
        
        public static bool IsWindows()
        {
            return GetSystem(PlatformID.Win32NT).IsWindows();
        }
        
        public static bool IsWindows(this System.OperatingSystem operatingSystem)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        // ReSharper disable once InconsistentNaming
        public static bool IsMacOS()
        {
            return GetSystem(PlatformID.MacOSX).IsMacOS();
        }
        
        // ReSharper disable once InconsistentNaming
        public static bool IsMacOS(this System.OperatingSystem operatingSystem)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        public static bool IsLinux()
        {
            return GetSystem(PlatformID.Unix).IsLinux();
        }
        
        public static bool IsLinux(this System.OperatingSystem operatingSystem)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        // ReSharper disable once InconsistentNaming
        public static bool IsFreeBSD()
        {
            return GetSystem(PlatformID.Unix).IsFreeBSD();
        }
        
        // ReSharper disable once InconsistentNaming
        public static bool IsFreeBSD(this System.OperatingSystem operatingSystem)
        {
            return System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToLower().Contains("freebsd");
        }
    }
}