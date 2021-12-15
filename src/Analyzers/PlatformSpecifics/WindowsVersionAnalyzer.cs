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
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;

public static class WindowsVersionAnalyzer
{

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]
#endif
        public static bool IsWindows10(this OSVersionAnalyzer osVersionAnalyzer)
        {
           return IsWindows10(osVersionAnalyzer, GetWindowsVersionToEnum(osVersionAnalyzer));
        }
        
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("Windows")]
#endif
    public static bool IsWindows10(this OSVersionAnalyzer osVersionAnalyzer, WindowsVersion windowsVersion)
    {
        switch (windowsVersion)
        {
            case WindowsVersion.Win10_v1507:
                return true;
            case WindowsVersion.Win10_v1511:
                return true;
            case WindowsVersion.Win10_v1607:
                return true;
            case WindowsVersion.Win10_Server2016:
                return true;
            case WindowsVersion.Win10_v1703:
                return true;
            case WindowsVersion.Win10_v1709_Mobile:
                return true;
            case WindowsVersion.Win10_Server_v1709:
                return true;
            case WindowsVersion.Win10_v1709:
                return true;
            case WindowsVersion.Win10_v1803:
                return true;
            case WindowsVersion.Win10_v1809:
                return true;
            case WindowsVersion.Win10_Server2019:
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
            case WindowsVersion.Win10_Server2022:
                return true;
            case WindowsVersion.Win10_InsiderPreview:
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
        public static bool IsWindows11(this OSVersionAnalyzer osVersionAnalyzer)
        {
            return IsWindows11(osVersionAnalyzer, GetWindowsVersionToEnum(osVersionAnalyzer));
        }

#if NET5_0_OR_GREATER
    [SupportedOSPlatform("Windows")]
#endif
    public static bool IsWindows11(this OSVersionAnalyzer osVersionAnalyzer, WindowsVersion windowsVersion)
    {
        switch (windowsVersion)
        {
            case WindowsVersion.Win11_21H2:
                return true;
            case WindowsVersion.Win11_InsiderPreview:
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
        public static WindowsVersion GetWindowsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer)
        {
            return GetWindowsVersionToEnum(osVersionAnalyzer, DetectWindowsVersion(osVersionAnalyzer));
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]
#endif
        public static WindowsVersion GetWindowsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer, Version input)
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
        ///     Detects Windows Version and returns it as a System.Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once MemberCanBePrivate.Global
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("Windows")]
#endif
        public static Version DetectWindowsVersion(this OSVersionAnalyzer osVersionAnalyzer)
        {
            try
            {
                if (new PlatformManager().IsWindows())
                {
                    string description = RuntimeInformation.OSDescription;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="osVersionAnalyzer"></param>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        /// <exception cref="OperatingSystemVersionDetectionException"></exception>
        public static Version GetWindowsVersionFromEnum(this OSVersionAnalyzer osVersionAnalyzer, WindowsVersion windowsVersion)
        {
            switch (windowsVersion)
            {
                case WindowsVersion.WinVista:
                    return new Version(6, 0, 6000);
                case WindowsVersion.WinVistaSP1:
                    return new Version(6, 0, 6001);
                case WindowsVersion.WinVistaSP2:
                    return new Version(6, 0, 6002);
                case WindowsVersion.WinServer_2008:
                    return new Version(6, 0, 6003);
                case WindowsVersion.Win7:
                    return new Version(6, 1, 7600);
                case WindowsVersion.Win7SP1:
                    return new Version(6, 1, 7601);
                case WindowsVersion.WinServer_2008_R2:
                    return new Version(6, 1, 7600);
                case WindowsVersion.Win8:
                    return new Version(6, 2, 9200);
                case WindowsVersion.WinServer_2012:
                    return new Version(6, 2, 9200);
                case WindowsVersion.Win8_1:
                    return new Version(6, 3, 9600);
                case WindowsVersion.WinServer_2012_R2:
                    return new Version(6, 3, 9600);
                case WindowsVersion.Win10_v1507:
                    return new Version(10, 0, 10240);
                case WindowsVersion.Win10_v1511:
                    return new Version(10, 0, 10586);
                case WindowsVersion.Win10_v1607:
                    return new Version(10, 0, 14393);
                case WindowsVersion.Win10_Server2016:
                    return new Version(10, 0, 14393);
                case WindowsVersion.Win10_v1703:
                    return new Version(10, 0, 15063);
                case WindowsVersion.Win10_v1709_Mobile:
                    return new Version(10, 0, 15254);
                case WindowsVersion.Win10_v1709:
                    return new Version(10, 0, 16299);
                case WindowsVersion.Win10_Server_v1709:
                    return new Version(10, 0, 16299);
                case WindowsVersion.Win10_v1803:
                    return new Version(10, 0, 17134);
                case WindowsVersion.Win10_v1809:
                    return new Version(10, 0, 17763);
                case WindowsVersion.Win10_Server2019:
                    return new Version(10, 0, 17763);
                case WindowsVersion.Win10_v1903:
                    return new Version(10, 0, 18362);
                case WindowsVersion.Win10_v1909:
                    return new Version(10, 0, 18363);
                case WindowsVersion.Win10_v2004:
                    return new Version(10, 0, 19041);
                case WindowsVersion.Win10_20H2:
                    return new Version(10, 0, 19042);
                case WindowsVersion.Win10_21H1:
                    return new Version(10, 0, 19043);
                case WindowsVersion.Win10_21H2:
                    return new Version(10, 0, 19044);
                case WindowsVersion.Win10_Server2022:
                    return new Version(10, 0, 20348);
                case WindowsVersion.Win11_21H2:
                    return new Version(10, 0, 22000);
                default:
                    throw new OperatingSystemVersionDetectionException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="versionAnalyzer"></param>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static bool IsAtLeastWindowsVersion(this OSVersionAnalyzer versionAnalyzer, WindowsVersion windowsVersion)
        {
            var detected = versionAnalyzer.DetectOSVersion();

            var expected = versionAnalyzer.GetWindowsVersionFromEnum(windowsVersion);
            
            return (detected.Build >= expected.Build);
        }
}