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

namespace PlatformKit.Extensions.OperatingSystem
{
    public static class OperatingSystemExtension
    {
        internal static System.OperatingSystem GetSystem(PlatformID platformId)
        {
            return new System.OperatingSystem(platformId, Environment.OSVersion.Version);
        }
        
        /// <summary>
        /// 
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
        /// 
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
        /// 
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
        /// 
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
    }
}