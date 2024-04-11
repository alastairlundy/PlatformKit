/*
        MIT License
       
       Copyright (c) 2024 Alastair Lundy
       
       Permission is hereby granted, free of charge, to any person obtaining a copy
       of this software and associated documentation files (the "Software"), to deal
       in the Software without restriction, including without limitation the rights
       to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
       copies of the Software, and to permit persons to whom the Software is
       furnished to do so, subject to the following conditions:
       
       The above copyright notice and this permission notice shall be included in all
       copies or substantial portions of the Software.
       
       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.
   */

using System;
using System.Runtime.InteropServices;
using AlastairLundy.System.Extensions.StringExtensions;
using AlastairLundy.System.Extensions.VersionExtensions;

namespace PlatformKit.Extensions.OperatingSystem
{
    public static class OperatingSystemExtension
    {
        internal static System.OperatingSystem GetSystem(PlatformID platformId)
        {
            return new System.OperatingSystem(platformId, Environment.OSVersion.Version);
        }

        /// <summary>
        /// Returns whether the operating system that is running is Windows.
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows()
        {
            return GetSystem(PlatformID.Win32NT).IsWindows();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        public static bool IsWindows(this System.OperatingSystem operatingSystem)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
        }

        /// <summary>
        /// Returns whether the operating system that is running is macOS.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsMacOS()
        {
            return GetSystem(PlatformID.MacOSX).IsMacOS();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsMacOS(this System.OperatingSystem operatingSystem)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
        }

        /// <summary>
        /// Returns whether the operating system that is running is Linux.
        /// </summary>
        /// <returns></returns>
        public static bool IsLinux()
        {
            return GetSystem(PlatformID.Unix).IsLinux();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        public static bool IsLinux(this System.OperatingSystem operatingSystem)
        {
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        }

        /// <summary>
        ///  Returns whether the operating system that is running is FreeBSD.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsFreeBSD()
        {
            return GetSystem(PlatformID.Unix).IsFreeBSD();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="operatingSystem"></param>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static bool IsFreeBSD(this System.OperatingSystem operatingSystem)
        {
            return System.Runtime.InteropServices.RuntimeInformation.OSDescription.ToLower().Contains("freebsd");
        }

        /// <summary>
        /// Checks to see whether the specified version of Windows is the same or newer than the installed version of Windows.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static bool IsWindowsVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
        {
            if (IsWindows())
            {
                return GetSystem(PlatformID.Win32NT).Version.IsAtLeast(new Version(major, minor, build, revision));
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Checks to see whether the specified version of macOS is the same or newer than the installed version of Windows.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        // ReSharper disable once InconsistentNaming
        public static bool IsMacOSVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
        {
            if (IsMacOS())
            {
                return GetSystem(PlatformID.MacOSX).Version.IsAtLeast(new Version(major, minor, build, revision));
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Checks to see whether the specified version of Linux is the same or newer than the installed version of Windows.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static bool IsLinuxVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
        {
            if (IsLinux())
            {
                return GetSystem(PlatformID.Unix).Version.IsAtLeast(new Version(major, minor, build, revision));
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Checks to see whether the specified version of FreeBSD is the same or newer than the installed version of Windows.
        /// </summary>
        /// <param name="major"></param>
        /// <param name="minor"></param>
        /// <param name="build"></param>
        /// <param name="revision"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        // ReSharper disable once InconsistentNaming
        public static bool IsFreeBSDVersionAtLeast(int major, int minor = 0, int build = 0, int revision = 0)
        {
            if (IsFreeBSD())
            {
                return GetSystem(PlatformID.Unix).Version.IsAtLeast(new Version(major, minor, build, revision));
            }

            throw new PlatformNotSupportedException();
        }
    }
}