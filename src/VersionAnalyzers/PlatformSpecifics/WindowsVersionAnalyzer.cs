/* MIT License

Copyright (c) 2018-2022 AluminiumTech

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
using PlatformKit.Analyzers;
using PlatformKit.Internal.Exceptions;
using PlatformKit.Software.Windows;

//Move namespace in v3.
namespace PlatformKit.VersionAnalyzers.PlatformSpecifics;

/// <summary>
/// 
/// </summary>
public static class WindowsVersionAnalyzer
{

        /// <summary>
        /// 
        /// </summary>
        /// <param name="osVersionAnalyzer"></param>
        /// <returns></returns>
        public static bool IsWindows10(this OSVersionAnalyzer osVersionAnalyzer)
        {
           return IsWindows10(osVersionAnalyzer, GetWindowsVersionToEnum(osVersionAnalyzer));
        }
        
    /// <summary>
    /// 
    /// </summary>
    /// <param name="osVersionAnalyzer"></param>
    /// <param name="windowsVersion"></param>
    /// <returns></returns>
    /// <exception cref="OperatingSystemDetectionException"></exception>
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
                throw new OperatingSystemDetectionException();
            default:
                return false;
        }
    }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="osVersionAnalyzer"></param>
        /// <returns></returns>
        public static bool IsWindows11(this OSVersionAnalyzer osVersionAnalyzer)
        {
            return IsWindows11(osVersionAnalyzer, GetWindowsVersionToEnum(osVersionAnalyzer));
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="osVersionAnalyzer"></param>
    /// <param name="windowsVersion"></param>
    /// <returns></returns>
    /// <exception cref="OperatingSystemDetectionException"></exception>
    public static bool IsWindows11(this OSVersionAnalyzer osVersionAnalyzer, WindowsVersion windowsVersion)
    {
        switch (windowsVersion)
        {
            case WindowsVersion.Win11_21H2:
                return true;
            case WindowsVersion.Win11_InsiderPreview:
                return true;
            case WindowsVersion.NotDetected:
                throw new OperatingSystemDetectionException();
            default:
                return false;
        }
    }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static WindowsVersion GetWindowsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer)
        {
            return GetWindowsVersionToEnum(osVersionAnalyzer, DetectWindowsVersion(osVersionAnalyzer));
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public static WindowsVersion GetWindowsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer, Version input)
        {
            try
            {
                if (input.Major == 5)
                {
                    //We don't support Windows XP.
                    throw new PlatformNotSupportedException("Windows XP is not supported.");
                }

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
                            return WindowsVersion.NotDetected;
                        }
                }
            }
            catch (Exception exception)
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
        public static Version DetectWindowsVersion(this OSVersionAnalyzer osVersionAnalyzer)
        {
            try
            {
                if (new OSAnalyzer().IsWindows())
                {
                    string description = RuntimeInformation.OSDescription
                        .Replace("Microsoft Windows", string.Empty)
                        .Replace(" ", string.Empty);

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
                        throw new OperatingSystemDetectionException();
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
        /// <exception cref="OperatingSystemDetectionException"></exception>
        public static Version GetWindowsVersionFromEnum(this OSVersionAnalyzer osVersionAnalyzer, WindowsVersion windowsVersion)
        {
            return windowsVersion switch
            {
                WindowsVersion.WinVista => new Version(6, 0, 6000),
                WindowsVersion.WinVistaSP1 => new Version(6, 0, 6001),
                WindowsVersion.WinVistaSP2 => new Version(6, 0, 6002),
                WindowsVersion.WinServer_2008 => new Version(6, 0, 6003),
                WindowsVersion.Win7 => new Version(6, 1, 7600),
                WindowsVersion.Win7SP1 => new Version(6, 1, 7601),
                WindowsVersion.WinServer_2008_R2 => new Version(6, 1, 7600),
                WindowsVersion.Win8 => new Version(6, 2, 9200),
                WindowsVersion.WinServer_2012 => new Version(6, 2, 9200),
                WindowsVersion.Win8_1 => new Version(6, 3, 9600),
                WindowsVersion.WinServer_2012_R2 => new Version(6, 3, 9600),
                WindowsVersion.Win10_v1507 => new Version(10, 0, 10240),
                WindowsVersion.Win10_v1511 => new Version(10, 0, 10586),
                WindowsVersion.Win10_v1607 => new Version(10, 0, 14393),
                WindowsVersion.Win10_Server2016 => new Version(10, 0, 14393),
                WindowsVersion.Win10_v1703 => new Version(10, 0, 15063),
                WindowsVersion.Win10_v1709_Mobile => new Version(10, 0, 15254),
                WindowsVersion.Win10_v1709 => new Version(10, 0, 16299),
                WindowsVersion.Win10_Server_v1709 => new Version(10, 0, 16299),
                WindowsVersion.Win10_v1803 => new Version(10, 0, 17134),
                WindowsVersion.Win10_v1809 => new Version(10, 0, 17763),
                WindowsVersion.Win10_Server2019 => new Version(10, 0, 17763),
                WindowsVersion.Win10_v1903 => new Version(10, 0, 18362),
                WindowsVersion.Win10_v1909 => new Version(10, 0, 18363),
                WindowsVersion.Win10_v2004 => new Version(10, 0, 19041),
                WindowsVersion.Win10_20H2 => new Version(10, 0, 19042),
                WindowsVersion.Win10_21H1 => new Version(10, 0, 19043),
                WindowsVersion.Win10_21H2 => new Version(10, 0, 19044),
                WindowsVersion.Win10_Server2022 => new Version(10, 0, 20348),
                WindowsVersion.Win11_21H2 => new Version(10, 0, 22000),
                _ => throw new OperatingSystemDetectionException()
            };
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

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows11()
        {
            return IsWindows11(new OSVersionAnalyzer());
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public static bool IsWindows10()
        {
            return IsWindows10(new OSVersionAnalyzer());
        }
}