/* MIT License

Copyright (c) 2018-2021 AluminiumTech

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
using System.Runtime.Versioning;

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows;

namespace AluminiumTech.DevKit.PlatformKit
{
    // ReSharper disable once InconsistentNaming
    public class OSVersionAnalyzer
    {
        protected PlatformManager _platformManager;
        
        public OSVersionAnalyzer()
        {
            _platformManager = new PlatformManager();
        }
        
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]        
#endif
        public bool IsWindows10()
        {
            switch (GetWindowsVersionToEnum())
            {
                case WindowsVersion.Win10_v1507:
                    return true;
                case WindowsVersion.Win10_v1511:
                    return true;
                case WindowsVersion.Win10_v1607:
                    return true;
                case WindowsVersion.Win10_v1703:
                    return true;
                case WindowsVersion.Win10_v1709ForMobile:
                    return true;
                 case WindowsVersion.Win10_v1709:
                    return true;
                case WindowsVersion.Win10_v1803:
                    return true;
                case WindowsVersion.Win10_v1809:
                    return true;
                case WindowsVersion.Win10_v1903:
                    return true;
                case WindowsVersion.Win10_v1909:
                    return true;
                case WindowsVersion.Win10_v2004:
                    return true;
                case WindowsVersion.Win10_20H2:
                    return true;
                case WindowsVersion.Win10_21H1:
                    return true;
                default:
                    return false;
            }
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]        
#endif
        public bool IsWindows11()
        {
            switch (GetWindowsVersionToEnum())
            {
                case WindowsVersion.Win11_21H2:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]        
#endif
        public WindowsVersion GetWindowsVersionToEnum()
        {
            return GetWindowsVersionToEnum(DetectWindowsVersion());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]        
#endif
        public WindowsVersion GetWindowsVersionToEnum(Version input)
        {
            try
            {
                if (input == new Version(6,0,6000,0))
                    return WindowsVersion.WinVista;
                if (input == new Version(6,0,6001,0))
                    return WindowsVersion.WinVistaSP1;
                if (input == new Version(6,0,6002,0))
                    return WindowsVersion.WinVistaSP2;
                if (input == new Version(6,1,7600,0))
                    return WindowsVersion.Win7;
                if (input == new Version(6,1,7601,0))
                    return WindowsVersion.Win7SP1;
                if (input == new Version(6,2,9200,0))
                    return WindowsVersion.Win8;
                if (input == new Version(6,3,9600,0))
                    return WindowsVersion.Win8_1;
                if (input == new Version(10,0,10240,0))
                    return WindowsVersion.Win10_v1507;
                if (input == new Version(10,0,10586,0))
                    return WindowsVersion.Win10_v1511;
                if (input == new Version(10,0,14393,0))
                    return WindowsVersion.Win10_v1607;
                if (input == new Version(10,0,15063,0))
                    return WindowsVersion.Win10_v1703;
                if (input == new Version(10,0,15254,0))
                    return WindowsVersion.Win10_v1709ForMobile;
                if (input == new Version(10,0,16299,0))
                    return WindowsVersion.Win10_v1709;
                if (input == new Version(10,0,17134,0))
                    return WindowsVersion.Win10_v1803;
                if (input == new Version(10,0,17763,0))
                    return WindowsVersion.Win10_v1809;
                if (input == new Version(10,0,18362,0))
                    return WindowsVersion.Win10_v1903;
                if (input == new Version(10,0,18363,0))
                    return WindowsVersion.Win10_v1909;
                if (input == new Version(10,0,19041,0))
                    return WindowsVersion.Win10_v2004;
                if (input == new Version(10,0,19042,0))
                    return WindowsVersion.Win10_20H2;
                if (input == new Version(10,0,19043,0))
                    return WindowsVersion.Win10_21H1;
                if (input == new Version(10,0,22000,0))
                    return WindowsVersion.Win11_21H2;
                
                return WindowsVersion.NotDetected;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">Not yet implemented on Linux or macOS. Please do not run on Linux or macOS yet!</exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once InconsistentNaming
        public Version GetOSVersion()
        {
            try
            {
                var description = System.Runtime.InteropServices.RuntimeInformation.OSDescription;

                if (_platformManager.IsWindows())
                {
                    return DetectWindowsVersion();
                }

                if (_platformManager.IsLinux())
                {
                    throw new NotImplementedException();
                }

                if (_platformManager.IsMac())
                {
                    throw new NotImplementedException();
                }

#if NETCOREAPP3_0_OR_GREATER
                if (_platformManager.IsFreeBSD())
                {
                    throw new NotImplementedException();
                }
#endif
                throw new PlatformNotSupportedException();
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
        
        /// <summary>
        /// Detects Windows Version and returns it as a System.Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once MemberCanBePrivate.Global
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]        
#endif
        protected Version DetectWindowsVersion()
        {
            try
            {
                if (_platformManager.IsWindows())
                {
                    var description = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                    description = description.Replace("Microsoft Windows", string.Empty);

                    string[] descArray = description.Split(' ');

                    foreach (var d in descArray)
                    {
                        if (!d.Contains(string.Empty))
                        {
                            description = d;
                        }
                    }

                    return Version.Parse(description);
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception(exception.ToString());
            }
        }

        /*
       /// <summary>
       /// 
       /// </summary>
       /// <returns></returns>
       /// <exception cref="PlatformNotSupportedException">Throws an error if called on an OS besides macOS.</exception>
       /// <exception cref="Exception"></exception>
                public Version DetectMacOsVersion()
                {
                    try
                    {
                        if (_platformManager.IsMac())
                        {
                            var description = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                            description = description.Replace("", string.Empty);

                            string[] descArray = description.Split(' ');
                            
                            foreach (var d in descArray)
                            {
                                if (!d.Contains(string.Empty))
                                {
                                    description = d;
                                }
                            }
                        
                            return Version.Parse(description);
                        }
                        else
                        {
                            throw new PlatformNotSupportedException();
                        }
                    }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
        */

        /*
        public Version DetectLinuxDistributionVersion()
        {
            try
            {
                if (_platformManager.IsLinux())
                {
                    
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
        */

        /// <summary>
       /// Detects the linux kernel version to string.
       /// </summary>
       /// <returns></returns>
       /// <exception cref="PlatformNotSupportedException"></exception>
       //[SupportedOSPlatformAttribute("L")]
       public Version DetectLinuxKernelVersion()
       {
           if (_platformManager.IsLinux())
           {
               var description = Environment.OSVersion.ToString();
               description = description.Replace("Unix ", string.Empty);
               
               return Version.Parse(description);
           }
           else
           {
               throw new PlatformNotSupportedException();
           }
       }
       
        
       

    }
}