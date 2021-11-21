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
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Enums;

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
                if (input == new Version(6, 0, 6000, 0)) return WindowsVersion.WinVista;
                if (input == new Version(6, 0, 6001, 0)) return WindowsVersion.WinVistaSP1;
                if (input == new Version(6, 0, 6002, 0)) return WindowsVersion.WinVistaSP2;
                if (input == new Version(6, 0, 6003, 0)) return WindowsVersion.WinServer_2008; //Technically Server 2008 also can be Build number 6001 or 6002 but this provides an easier way to identify it.
                if (input == new Version(6, 1, 7600, 0)) return WindowsVersion.Win7;
                if (input == new Version(6, 1, 7601, 0)) return WindowsVersion.Win7SP1;
                if (input == new Version(6, 2, 9200, 0)) return WindowsVersion.Win8;
                if (input == new Version(6, 3, 9600, 0)) return WindowsVersion.Win8_1;
                if (input == new Version(10, 0, 10240, 0)) return WindowsVersion.Win10_v1507;
                if (input == new Version(10, 0, 10586, 0)) return WindowsVersion.Win10_v1511;
                if (input == new Version(10, 0, 14393, 0)) return WindowsVersion.Win10_v1607;
                if (input == new Version(10, 0, 15063, 0)) return WindowsVersion.Win10_v1703;
                if (input == new Version(10, 0, 15254, 0)) return WindowsVersion.Win10_v1709_Mobile; 
                if (input == new Version(10, 0, 16299, 0)) return WindowsVersion.Win10_v1709;
                if (input == new Version(10, 0, 17134, 0)) return WindowsVersion.Win10_v1803;
                if (input == new Version(10, 0, 17763, 0)) return WindowsVersion.Win10_v1809;
                if (input == new Version(10, 0, 18362, 0)) return WindowsVersion.Win10_v1903;
                if (input == new Version(10, 0, 18363, 0)) return WindowsVersion.Win10_v1909;
                if (input == new Version(10, 0, 19041, 0)) return WindowsVersion.Win10_v2004;
                if (input == new Version(10, 0, 19042, 0)) return WindowsVersion.Win10_20H2;
                if (input == new Version(10, 0, 19043, 0)) return WindowsVersion.Win10_21H1;
                if (input == new Version(10, 0, 19044, 0)) return WindowsVersion.Win10_21H2;
                if (input == new Version(10, 0, 20348, 0)) return WindowsVersion.Win10_Server2022; //Build number used exclusively by Windows Server and not by Windows 10 or 11.
                if (input == new Version(10, 0, 22000, 0)) return WindowsVersion.Win11_21H2;

                return WindowsVersion.NotDetected;
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">
        ///     Not yet implemented on macOS. Please do not run on macOS
        ///     yet!
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

                    for (int index = 0; index < description.Length; index++)
                    {
                        if (description[index].Equals('.'))
                        {
                            dotCounter++;
                        }
                    }
#if DEBUG
                    Console.WriteLine("Before DotCounter: " + dotCounter);
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
#if DEBUG
                    Console.WriteLine("After DotCounter: " + dotCounter);
                    Console.WriteLine("After DescV: " + description);
#endif

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