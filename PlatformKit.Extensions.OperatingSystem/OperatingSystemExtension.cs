using System;
using System.Runtime.InteropServices;

namespace PlatformKit.Classic.OSDetection
{
    public static class OperatingSystemExtension
    {
        public static bool IsWindows(this OperatingSystem operatingSystem)
        {
#if NET5_0_OR_GREATER
                  return OperatingSystem.IsWindows();
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        }

        // ReSharper disable once InconsistentNaming
        public static bool IsMacOS(this OperatingSystem operatingSystem)
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsMacOS();
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
        }

        public static bool IsLinux(this OperatingSystem operatingSystem)
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsLinux();
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
        }

        // ReSharper disable once InconsistentNaming
        public static bool IsFreeBSD(this OperatingSystem operatingSystem)
        {
            return System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToLower().Contains("freebsd");
        }
    }
}