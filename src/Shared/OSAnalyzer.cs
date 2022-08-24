/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at
    Commercial License - https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt
    Non-Commercial License - https://neverspy.tech/platformkit-noncommercial-license or in the PlatformKit_NonCommercial_License.md
  
  To use PlatformKit under either a commercial or non-commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;

using PlatformKit.FreeBSD;

using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

//Move namespace in V3
namespace PlatformKit
{
    // ReSharper disable once InconsistentNaming
    public class OSAnalyzer
    {
        public OSAnalyzer()
        {
      
        }

        /// <summary>
        /// Returns whether or not the current OS is Windows.
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
        }

        /// <summary>
        /// Returns whether or not the current OS is macOS.
        /// </summary>
        /// <returns></returns>
        public static bool IsMac()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
        }
        
        /// <summary>
        /// Returns whether or not the current OS is Linux based.
        /// </summary>
        /// <returns></returns>
        public static bool IsLinux()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an error if run on .NET Standard 2 or .NET Core 2.1 or earlier.</exception>
        public static bool IsFreeBSD()
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD);
#else
            throw new PlatformNotSupportedException();
#endif
        }
        
        /// <summary>
        /// Determine what OS is being run
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public System.Runtime.InteropServices.OSPlatform GetOSPlatform() {
            System.Runtime.InteropServices.OSPlatform osPlatform = System.Runtime.InteropServices.OSPlatform.Create("Other Platform");
            // Check if it's windows
            osPlatform = IsWindows() ? System.Runtime.InteropServices.OSPlatform.Windows : osPlatform;
            // Check if it's osx
            osPlatform = IsMac() ? System.Runtime.InteropServices.OSPlatform.OSX : osPlatform;
            // Check if it's Linux
            osPlatform = IsLinux() ? System.Runtime.InteropServices.OSPlatform.Linux : osPlatform;
            
#if NETCOREAPP3_0_OR_GREATER
            // Check if it's FreeBSD
            osPlatform = IsFreeBSD() ? System.Runtime.InteropServices.OSPlatform.FreeBSD : osPlatform;
#endif

            return osPlatform;
        }
        
        /// <summary>
        /// Detect the OS version on Windows, macOS, or Linux.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once InconsistentNaming
        public Version DetectOSVersion()
        {
            try
            {
                if (IsWindows())
                {
                    return new WindowsAnalyzer().DetectWindowsVersion();
                }
                if (IsLinux())
                {
                    return new LinuxAnalyzer().DetectLinuxDistributionVersion();
                }
                if (IsMac())
                {
                    return new MacOSAnalyzer().DetectMacOsVersion();
                }

#if NETCOREAPP3_0_OR_GREATER
                if (IsFreeBSD())
                {
                    return new FreeBSDAnalyzer().DetectFreeBSDVersion();
                }
#endif
                throw new PlatformNotSupportedException();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
    }
}