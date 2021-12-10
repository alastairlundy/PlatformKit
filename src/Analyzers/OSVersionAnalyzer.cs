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

using AluminiumTech.DevKit.PlatformKit.Exceptions;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Mac;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers
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
                case WindowsVersion.Win10_v1709_Mobile:
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
                case WindowsVersion.Win10_21H2:
                    return true;
                case WindowsVersion.Win10_Server2016:
                    return true;
                case WindowsVersion.Win10_Server2019:
                    return true;
                case WindowsVersion.Win10_Server_v1709:
                    return true;
                case WindowsVersion.Win10_Server2022:
                    return true;
                case WindowsVersion.NotDetected:
                    throw new OperatingSystemVersionDetectionException();
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
                case WindowsVersion.NotDetected:
                    throw new OperatingSystemVersionDetectionException();
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
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]
#endif
        public WindowsVersion GetWindowsVersionToEnum(Version input)
        {
            try
            {
                if (input.Major == 5)
                {
                    //We don't support Windows XP.
                    throw new PlatformNotSupportedException("Windows XP is not supported.");
                }
                if (input.Major == 6)
                {
                    switch (input.Build)
                    {
                            case 6000:
                                return WindowsVersion.WinVista;
                            case 6001:
                                return WindowsVersion.WinVistaSP1;
                            case 6002:
                                return WindowsVersion.WinVistaSP2;
                            case 6003:
                                return WindowsVersion.WinServer_2008; //Technically Server 2008 also can be Build number 6001 or 6002 but this provides an easier way to identify it.
                            case 7600:
                                return WindowsVersion.Win7;
                            case 7601:
                                return WindowsVersion.Win7SP1;
                            case 9200:
                                return WindowsVersion.Win8;
                            case 9600:
                                return WindowsVersion.Win8_1;
                    }   
                }
                if (input.Major == 10)
                {
                    switch (input.Build)
                    {
                        case 10240:
                            return WindowsVersion.Win10_v1507;
                        case 10586:
                            return WindowsVersion.Win10_v1511;
                        case 14393:
                            return WindowsVersion.Win10_v1607;
                        case 15063:
                            return WindowsVersion.Win10_v1703; 
                        case 15254:
                            return WindowsVersion.Win10_v1709_Mobile;
                        case 16299:
                            return WindowsVersion.Win10_v1709;
                        case 17134:
                            return WindowsVersion.Win10_v1803;
                        case 17763:
                            return WindowsVersion.Win10_v1809; 
                        case 18362:
                            return WindowsVersion.Win10_v1903;
                        case 18363:
                            return WindowsVersion.Win10_v1909;
                        case 19041:
                            return WindowsVersion.Win10_v2004;
                        case 19042: 
                            return WindowsVersion.Win10_20H2;
                        case 19043:
                            return WindowsVersion.Win10_21H1;
                        case 19044:
                            return WindowsVersion.Win10_21H2;
                        case 20348:
                            return WindowsVersion.Win10_Server2022; //Build number used exclusively by Windows Server and not by Windows 10 or 11.
                        case 22000:
                            return WindowsVersion.Win11_21H2;
                        default:
                            //Assume any non enumerated value in between Windows 10 versions is an Insider preview for Windows 10.
                            if (input.Build is > 10240 and < 22000)
                            {
                                return WindowsVersion.Win10_InsiderPreview;
                            }
                            //Assume non enumerated values for Windows 11 are Insider Previews for Windows 11.
                            else if(input.Build > 22000)
                            {
                                return WindowsVersion.Win11_InsiderPreview;
                            }
                            else
                            {
                                throw new OperatingSystemVersionDetectionException();
                            }
                    }
                }

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
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="OperatingSystemVersionDetectionException"></exception>
        public MacOsVersion GetMacOsVersionToEnum(Version input)
        {
            try
            {
                if (input.Major == 10)
                {
                    switch (input.Minor)
                    {
                            case 0:
                                return MacOsVersion.v10_0_Cheetah;
                            case 1:
                                return MacOsVersion.v10_1_Puma;
                            case 2:
                                return MacOsVersion.v10_2_Jaguar;
                            case 3:
                                return MacOsVersion.v10_3_Panther;
                            case 4:
                                return MacOsVersion.v10_4_Tiger;
                            case 5:
                                return MacOsVersion.v10_5_Leopard;
                            case 6:
                                return MacOsVersion.v10_6_SnowLeopard;
                            case 7:
                                return MacOsVersion.v10_7_Lion;
                            case 8:
                                return MacOsVersion.v10_8_MountainLion;
                            case 9:
                                return MacOsVersion.v10_9_Mavericks;
                            case 10:
                                return MacOsVersion.v10_10_Yosemite;
                            case 11:
                                return MacOsVersion.v10_11_ElCapitan;
                            case 12:
                                return MacOsVersion.v10_12_Sierra;
                            case 13:
                                return MacOsVersion.v10_13_HighSierra;
                            case 14:
                                return MacOsVersion.v10_14_Mojave;
                            case 15:
                                return MacOsVersion.v10_15_Catalina;
                    }
                }
                
                if (input.Major == 11 && input.Minor is >= 0 and <= 6) return MacOsVersion.v11_BigSur;
                if (input.Major == 12 && input.Minor is >= 0 and <= 6) return MacOsVersion.v12_Monterrey;

                throw new OperatingSystemVersionDetectionException();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">
        ///     Not yet implemented on macOS. Please do not run on macOS
        ///     yet!
        ///
        ///     Not yet implemented on FreeBSD either! Please do not run on FreeBSD.
        /// </exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once InconsistentNaming
        public Version DetectOSVersion()
        {
            try
            {
                if (_platformManager.IsWindows())
                {
                    return DetectWindowsVersion();
                }
                if (_platformManager.IsLinux())
                {
                    return DetectLinuxDistributionVersion();
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
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        ///     Detects Windows Version and returns it as a System.Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once MemberCanBePrivate.Global
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]
#endif
        public Version DetectWindowsVersion()
        {
            try
            {
                if (_platformManager.IsWindows())
                {
                    var description = RuntimeInformation.OSDescription;
                    description = description.Replace("Microsoft Windows", string.Empty);
                    description = description.Replace(" ", String.Empty);

                    var dotCounter = 0;

                    foreach (var t in description)
                    {
                        if (t.Equals('.'))
                        {
                            dotCounter++;
                        }
                    }
#if DEBUG
                    Console.WriteLine("Before DescV: " + description);
#endif
                    if (dotCounter == 1)
                    {
                        dotCounter++;
                        description += ".0";
                    }

                    if (dotCounter == 2)
                    {
                        dotCounter++;
                        description += ".0";
                    }

                    if (dotCounter > 3)
                    {
                        throw new OperatingSystemVersionDetectionException();
                    }

                    return Version.Parse(description);
                }

                throw new PlatformNotSupportedException();
            }
            catch (Exception exception)
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

        /// <summary>
        ///     Detects the Linux Distribution Version as read from cat /etc/os-release and reformats it into the format of
        ///     Version.
        ///     WARNING: DOES NOT PRESERVE the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns></returns>
        public Version DetectLinuxDistributionVersion()
        {
            var dotCounter = 0;

            var version = DetectLinuxDistributionVersionAsString();
            version = version.Replace(" ", string.Empty);

            foreach (var c in version)
            {
                if (c == '.')
                {
                    dotCounter++;
                }
            }

            if (dotCounter == 1)
            {
                version += ".0";
            }
            else if (dotCounter == 2)
            {
                version += ".0";
            }
#if DEBUG
            Console.WriteLine("Version: " + version);
#endif
            return Version.Parse(version);
        }

        /// <summary>
        ///     Detects the Linux Distribution Version as read from cat /etc/os-release.
        ///     Preserves the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public string DetectLinuxDistributionVersionAsString()
        {
            var osAnalyzer = new OSAnalyzer();

            var linuxDistroInfo = osAnalyzer.GetLinuxDistributionInformation();
            
#if DEBUG
            Console.WriteLine("LinuxDistroVersion Before: " + linuxDistroInfo.Version);
#endif
            
            var osName = osAnalyzer.GetLinuxDistributionInformation().Name.ToLower();

            if (osName.Contains("ubuntu") ||
                osName.Contains("pop!_os"))
            {
                if (linuxDistroInfo.Version.Contains(".4.") || linuxDistroInfo.Version.EndsWith(".4"))
                {
                    //Properly show Year.Month.minor version for Date base distribution versioning such as Pop!_OS and Ubuntu.
                    //This normally occurs with .04 being shown as .4
                    linuxDistroInfo.Version = linuxDistroInfo.Version.Replace(".4", ".04");
                }
            }
            
#if DEBUG
            Console.WriteLine("LinuxDistroVersion After: " + linuxDistroInfo.Version);
#endif
            
            return linuxDistroInfo.Version;
        }

        /// <summary>
        ///     Detects the linux kernel version to string.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">
        ///     Throws a platform not supported exception if run on Windows, macOS, or
        ///     any platform that isn't Linux.
        /// </exception>
        public Version DetectLinuxKernelVersion()
        {
            if (_platformManager.IsLinux())
            {
                var description = Environment.OSVersion.ToString();
                description = description.Replace("Unix ", string.Empty);

                return Version.Parse(description);
            }

            throw new PlatformNotSupportedException();
        }
    }
}