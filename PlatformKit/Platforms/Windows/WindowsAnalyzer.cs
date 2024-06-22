/*
        MIT License
       
       Copyright (c) 2020-2024 Alastair Lundy
       
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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

using AlastairLundy.Extensions.System.StringExtensions;
using AlastairLundy.Extensions.System.VersionExtensions;

using PlatformKit.Internal.Deprecation;
using PlatformKit.Internal.Exceptions;

#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Windows;

/// <summary>
/// A class to Detect Windows versions, Windows features, and find out more about a user's Windows installation.
/// </summary>
public class WindowsAnalyzer
{

    /// <summary>
    /// Detects the Edition of Windows being run.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="WindowsEditionDetectionException">Throws an exception if operating system detection fails.</exception>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't Windows.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static WindowsEdition GetWindowsEdition()
    {
        return GetWindowsEdition(GetWindowsSystemInformation());
    }

    /// <summary>
    /// Detects the Edition of Windows from specified WindowsSystemInformation.
    /// </summary>
    /// <param name="windowsSystemInformation"></param>
    /// <returns></returns>
    /// <exception cref="WindowsEditionDetectionException"></exception>
    /// <exception cref="PlatformNotSupportedException">Thrown when not running on Windows.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static WindowsEdition GetWindowsEdition(WindowsSystemInformationModel windowsSystemInformation)
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }
        
        string edition = windowsSystemInformation.OsName.ToLower();
            
        if (edition.Contains("home"))
        {
            return WindowsEdition.Home;
        }
        else if (edition.Contains("pro") && edition.Contains("workstation"))
        {
            return WindowsEdition.ProfessionalForWorkstations;
        }
        else if (edition.Contains("pro") && !edition.Contains("education"))
        {
            return WindowsEdition.Professional;
        }
        else if (edition.Contains("pro") && edition.Contains("education"))
        {
            return WindowsEdition.ProfessionalForEducation;
        }
        else if (!edition.Contains("pro") && edition.Contains("education"))
        {
            return WindowsEdition.Education;
        }
        else if (edition.Contains("server"))
        {
            return WindowsEdition.Server;
        }
        else if (edition.Contains("enterprise") && edition.Contains("ltsc") &&
                 !edition.Contains("iot"))
        {
            return WindowsEdition.EnterpriseLTSC;
        }
        else if (edition.Contains("enterprise") && !edition.Contains("ltsc") &&
                 !edition.Contains("iot"))
        {
            return WindowsEdition.EnterpriseSemiAnnualChannel;
        }
        else if (edition.Contains("enterprise") && edition.Contains("ltsc") &&
                 edition.Contains("iot"))
        {
            return WindowsEdition.IoTEnterpriseLTSC;
        }
        else if (edition.Contains("enterprise") && !edition.Contains("ltsc") &&
                 edition.Contains("iot"))
        {
            return WindowsEdition.IoTEnterprise;
        }
        else if (edition.Contains("iot") && edition.Contains("core"))
        {
            return WindowsEdition.IoTCore;
        }
        else if (edition.Contains("team"))
        {
            return WindowsEdition.Team;
        }

        if (IsWindows11())
        {
            if (edition.Contains("se"))
            {
                return WindowsEdition.SE;
            }
        }

        throw new WindowsEditionDetectionException();

    }

    protected static int GetNetworkCardPositionInWindowsSysInfo(List<NetworkCardModel> networkCards, NetworkCardModel lastNetworkCard)
    {
        for (int position = 0; position < networkCards.Count; position++)
        {
            if (Equals(networkCards[position], lastNetworkCard))
            {
                return position;
            }
        }

        throw new ArgumentException();
    }

    /// <summary>
    /// Detect WindowsSystemInformation
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown when not running on Windows.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static WindowsSystemInformationModel GetWindowsSystemInformation()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }
        
        WindowsSystemInformationModel windowsSystemInformation = new WindowsSystemInformationModel();
        HyperVRequirements hyperVRequirements = new HyperVRequirements();
        
        List<string> processors = new List<string>();
        List<NetworkCardModel> networkCards = new List<NetworkCardModel>();
        List<string> ipAddresses = new List<string>();
        
        NetworkCardModel lastNetworkCard = null;

        string desc = CommandRunner.RunPowerShellCommand("systeminfo");

#if NET5_0_OR_GREATER
        string[] array = desc.Split(Environment.NewLine);
#elif NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        string[] array = desc.Split(new[]
        {
            Environment.NewLine
        }, StringSplitOptions.None);
#endif

#region Manual Detection

bool wasLastLineProcLine = false;
bool wasLastLineNetworkLine = false;

int networkCardNumber = 0;

for (int index = 0; index < array.Length; index++)
{
    string nextLine = "";

    array[index] = array[index].Replace("  ", string.Empty);

    if (index < (array.Length - 1))
    {
        nextLine = array[index + 1].Replace("  ", string.Empty);
    }
    else
    {
        nextLine = array[index].Replace("  ", string.Empty);
    }
    
    if (nextLine.ToLower().Contains("host name:"))
    {
        windowsSystemInformation.HostName =
            nextLine.Replace("Host Name:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("os name:"))
    {
        windowsSystemInformation.OsName = nextLine.Replace("OS Name:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("os version:") && !nextLine.ToLower().Contains("bios"))
    {
        windowsSystemInformation.OsVersion = nextLine.Replace("OS Version:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("os manufacturer:"))
    {
        windowsSystemInformation.OsManufacturer = nextLine.Replace("OS Manufacturer:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("os configuration:"))
    {
        windowsSystemInformation.OsConfiguration = nextLine.Replace("OS Configuration:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("os build type:"))
    {
        windowsSystemInformation.OsBuildType = nextLine.Replace("OS Build Type:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("registered owner:"))
    {
        windowsSystemInformation.RegisteredOwner = nextLine.Replace("Registered Owner:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("registered organization:"))
    {
        windowsSystemInformation.RegisteredOrganization =
            nextLine.Replace("Registered Organization:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("product id:"))
    {
        windowsSystemInformation.ProductId = nextLine.Replace("Product ID:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("original install date:"))
    {
        nextLine = nextLine.Replace("Original Install Date:", string.Empty);

        string[] info = nextLine.Split(',');
        
        windowsSystemInformation.OriginalInstallDate = info[0].Contains("/") ? DateTime.Parse(info[0]) : DateParser(info);
    }
    else if (nextLine.ToLower().Contains("system boot time:"))
    {
        nextLine = nextLine.Replace("System Boot Time:", string.Empty);
        string[] info = nextLine.Split(',');
        
        windowsSystemInformation.SystemBootTime = info[0].Contains("/") ? DateTime.Parse(info[0]) : DateParser(info);
    }
    else if (nextLine.ToLower().Contains("system manufacturer:"))
    {
        windowsSystemInformation.SystemManufacturer = nextLine.Replace("System Manufacturer:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("system model:"))
    {
        windowsSystemInformation.SystemModel = nextLine.Replace("System Model:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("system type:"))
    {
        windowsSystemInformation.SystemType = nextLine.Replace("System Type:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("processor(s):"))
    {
        processors.Add(nextLine.Replace("Processor(s):", string.Empty));

        wasLastLineProcLine = true;
        wasLastLineNetworkLine = false;
    }
    else if (nextLine.ToLower().Contains("bios version:"))
    {
        windowsSystemInformation.BiosVersion = nextLine.Replace("BIOS Version:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("windows directory:"))
    {
        windowsSystemInformation.WindowsDirectory = nextLine.Replace("Windows Directory:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("system directory:"))
    {
        windowsSystemInformation.SystemDirectory = nextLine.Replace("System Directory:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("boot device:"))
    {
        windowsSystemInformation.BootDevice = nextLine.Replace("Boot Device:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("system locale:"))
    {
        windowsSystemInformation.SystemLocale = nextLine.Replace("System Locale:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("input locale:"))
    {
        windowsSystemInformation.InputLocale = nextLine.Replace("Input Locale:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("time zone:"))
    {
        windowsSystemInformation.TimeZone = TimeZoneInfo.Local;
    }
    else if (nextLine.ToLower().Contains("memory:"))
    {
        nextLine = nextLine.Replace(",", string.Empty).Replace(" MB", string.Empty);

        if (nextLine.ToLower().Contains("total physical memory:"))
        {
            nextLine = nextLine.Replace("Total Physical Memory:", string.Empty);
            windowsSystemInformation.TotalPhysicalMemoryMB = int.Parse(nextLine);
        }
        else if (nextLine.ToLower().Contains("available physical memory"))
        {
            nextLine = nextLine.Replace("Available Physical Memory:", string.Empty);
            windowsSystemInformation.AvailablePhysicalMemoryMB = int.Parse(nextLine);
        }
        if (nextLine.ToLower().Contains("virtual memory: max size:"))
        {
            nextLine = nextLine.Replace("Virtual Memory: Max Size:", string.Empty);
            windowsSystemInformation.VirtualMemoryMaxSizeMB = int.Parse(nextLine);
        }
        else if (nextLine.ToLower().Contains("virtual memory: available:"))
        {
            nextLine = nextLine.Replace("Virtual Memory: Available:", string.Empty);
            windowsSystemInformation.VirtualMemoryAvailableSizeMB = int.Parse(nextLine);
        }
        else if (nextLine.ToLower().Contains("virtual memory: in use:"))
        {
            nextLine = nextLine.Replace("Virtual Memory: In Use:", string.Empty);
            windowsSystemInformation.VirtualMemoryInUse = int.Parse(nextLine);
        }
    }
    else if (nextLine.ToLower().Contains("page file location(s):"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        List<string> locations =
        [
            nextLine.Replace("Page File Location(s):", string.Empty)
        ];

        int locationNumber = 1;
        
        while (!array[index + 1 + locationNumber].ToLower().Contains("domain"))
        {
            locations.Add(array[index + 1 + locationNumber]);
            locationNumber++;
        }

        windowsSystemInformation.PageFileLocations = locations.ToArray();
    }
    else if (nextLine.ToLower().Contains("domain:"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        windowsSystemInformation.Domain = nextLine.Replace("Domain:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("logon server:"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        windowsSystemInformation.LogonServer = nextLine.Replace("Logon Server:", string.Empty);
    }
    else if (nextLine.ToLower().Contains("hotfix(s):"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        List<string> hotfixes = new List<string>();
        
        int hotfixCount = 0;
        while (array[index + 2 + hotfixCount].Contains("[") && array[index + 2 + hotfixCount].Contains("]:"))
        {
            hotfixes.Add(array[index + 2 + hotfixCount].Replace("  ", string.Empty));
            hotfixCount++;
        }

        windowsSystemInformation.HotfixesInstalled = hotfixes.ToArray();
    }
    else if (nextLine.ToLower().Contains("network card(s):"))
    {
        if (networkCardNumber > 0)
        {
            if (lastNetworkCard != null)  networkCards[networkCardNumber - 1].IpAddresses = ipAddresses.ToArray();
            ipAddresses.Clear();
        }
        
        wasLastLineProcLine = false;
        
        NetworkCardModel networkCard = new NetworkCardModel
        {
            Name = array[index + 2].Replace("  ", string.Empty)
        };

        networkCards.Add(networkCard);
        lastNetworkCard = networkCard; 

        wasLastLineNetworkLine = true;
        networkCardNumber++;
    }
    else if (nextLine.ToLower().Contains("connection name:"))
    {
        wasLastLineProcLine = false;
        
        if (networkCards.Contains(lastNetworkCard))
        {
           int position = GetNetworkCardPositionInWindowsSysInfo(networkCards, lastNetworkCard);
           networkCards[position].ConnectionName = nextLine.Replace("Connection Name:", string.Empty).Replace("  ", string.Empty);
        }
    }
    else if (nextLine.ToLower().Contains("dhcp enabled:"))
    {
        wasLastLineProcLine = false;
        
        int position = GetNetworkCardPositionInWindowsSysInfo(networkCards, lastNetworkCard);
        networkCards[position].DhcpEnabled = array[index + 4].ToLower().Contains("yes");
    }
    else if (nextLine.ToLower().Contains("dhcp server:"))
    {
        int position = GetNetworkCardPositionInWindowsSysInfo(networkCards, lastNetworkCard);
        networkCards[position].DhcpServer = nextLine.Replace("DHCP Server:", string.Empty).Replace("  ", string.Empty);
    }
    else if (nextLine.ToLower().Contains("[") && nextLine.ToLower().Contains("]"))
    {
        string compare = nextLine.Replace("[", string.Empty).Replace("]:", string.Empty);

        int dotCounter = compare.CountDotsInString();

        if (dotCounter >= 3 && wasLastLineNetworkLine)
        {
            ipAddresses.Add(nextLine);
        }
        else if (wasLastLineProcLine)
        {
            processors.Add(nextLine);
        }
    }

    else if (nextLine.ToLower().Contains("hyper-v requirements:"))
    {
        hyperVRequirements.VmMonitorModeExtensions = nextLine.Replace("Hyper-V Requirements:", string.Empty)
            .Replace("VM Monitor Mode Extensions: ", string.Empty).Contains("Yes");
    }
    else if (nextLine.ToLower().Contains("virtualization enabled in firmware:"))
    {
        hyperVRequirements.VirtualizationEnabledInFirmware =
            nextLine.Replace("Virtualization Enabled In Firmware:", string.Empty).Contains("Yes");
    }
    else if (nextLine.ToLower().Contains("second level address translation:"))
    {
        hyperVRequirements.SecondLevelAddressTranslation = nextLine
            .Replace("Second Level Address Translation:", string.Empty).Contains("Yes");
    }
    else if (nextLine.ToLower().Contains("data execution prevention available:"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        hyperVRequirements.DataExecutionPreventionAvailable = nextLine
            .Replace("Data Execution Prevention Available:", string.Empty).Contains("Yes");
        break;
    }
}
        
        if (networkCardNumber == 1)
        {
            // ReSharper disable once ConditionIsAlwaysTrueOrFalse
            if (lastNetworkCard != null && ipAddresses != null && networkCards.Count > 0)
                networkCards[networkCardNumber - 1].IpAddresses = ipAddresses.ToArray();
            
            ipAddresses.Clear();
        }
        #endregion
        
        windowsSystemInformation.NetworkCards = networkCards.ToArray();
        windowsSystemInformation.Processors = processors.ToArray();
        windowsSystemInformation.HyperVRequirements = hyperVRequirements;
        return windowsSystemInformation;
    }

    private static DateTime DateParser(string[] info)
    {
        DateTime dt = new DateTime();
        
        info[1] = info[1].Replace(" ", string.Empty).Replace(":", string.Empty);

        string hours = info[1].Substring(0, 2);
        string minutes = info[1].Substring(2, 2);
        string seconds = info[1].Substring(4, 2);

        dt = dt.AddHours(double.Parse(hours));
        dt = dt.AddMinutes(double.Parse(minutes));
        dt = dt.AddSeconds(double.Parse(seconds));
            
        return dt;
    }

    /// <summary>
    /// Checks whether the detected version of Windows is Windows 10
    /// </summary>
    /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static bool IsWindows10()
    {
        return IsWindows10(GetWindowsVersionToEnum());
    }
        
    /// <summary>
    /// Returns whether a specified version of Windows is Windows 10.
    /// </summary>
    /// <param name="windowsVersion"></param>
    /// <returns></returns>
    public static bool IsWindows10(WindowsVersion windowsVersion)
    {
        return windowsVersion switch
        {
            WindowsVersion.Win10_v1507 => true,
            WindowsVersion.Win10_v1511 => true,
            WindowsVersion.Win10_v1607 or WindowsVersion.Win10_Server2016 => true,
            WindowsVersion.Win10_v1703 => true,
            WindowsVersion.Win10_v1709_Mobile => true,
            WindowsVersion.Win10_v1709 or WindowsVersion.Win10_Server_v1709 => true,
            WindowsVersion.Win10_v1803 => true,
            WindowsVersion.Win10_v1809 or WindowsVersion.Win10_Server2019 => true,
            WindowsVersion.Win10_v1903 => true,
            WindowsVersion.Win10_v1909 => true,
            WindowsVersion.Win10_v2004 => true,
            WindowsVersion.Win10_20H2 => true,
            WindowsVersion.Win10_21H1 => true,
            WindowsVersion.Win10_21H2 => true,
            WindowsVersion.Win10_22H2 => true,
            WindowsVersion.Win10_Server2022 => true,
            WindowsVersion.Win10_InsiderPreview => true,
            WindowsVersion.NotDetected => throw new WindowsVersionDetectionException(),
            _ => false,
        };
    }

    /// <summary>
    /// Returns whether the currently installed version of Windows is Windows 11.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static bool IsWindows11()
    { 
        return IsWindows11(GetWindowsVersionToEnum());
    }

        /// <summary>
        /// Returns whether the specified version of Windows is Windows 11.
        /// </summary>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        public static bool IsWindows11(WindowsVersion windowsVersion)
        {
            return windowsVersion switch
            {
                WindowsVersion.Win11_21H2 => true,
                WindowsVersion.Win11_22H2 => true,
                WindowsVersion.Win11_23H2 => true,
                WindowsVersion.Win11_24H2 => true,
                WindowsVersion.Win11_InsiderPreview => true,
                WindowsVersion.NotSupported => false,
                WindowsVersion.NotDetected => false,
                _ => false,
            };
        }

        /// <summary>
        /// Detects the installed version of Windows and returns it as an enum.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if not run on Windows.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public static WindowsVersion GetWindowsVersionToEnum()
        {
            if (OperatingSystem.IsWindows())
            {
                return GetWindowsVersionToEnum(GetWindowsVersion());
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Converts the specified version input to an enum corresponding to a Windows version.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static WindowsVersion GetWindowsVersionToEnum(Version input)
        {
            return input.Build switch
            {
                < 6000 => WindowsVersion.NotSupported,
                6000 => WindowsVersion.NotSupported,
                //return WindowsVersion.WinVista;
                6001 => WindowsVersion.NotSupported,
                //return WindowsVersion.WinVistaSP1;
                6002 => WindowsVersion.NotSupported,
                //return WindowsVersion.WinVistaSP2;
                6003 => WindowsVersion.NotSupported,
                //return WindowsVersion.WinServer_2008;
                //Technically Server 2008 also can be Build number 6001 or 6002 but this provides an easier way to identify it.
                7600 => WindowsVersion.NotSupported,
                7601 => WindowsVersion.NotSupported,
                9200 => WindowsVersion.Win8,
                9600 => WindowsVersion.Win8_1,
                10240 => WindowsVersion.Win10_v1507,
                10586 => WindowsVersion.Win10_v1511,
                14393 => WindowsVersion.Win10_v1607,
                15063 => WindowsVersion.Win10_v1703,
                15254 => WindowsVersion.Win10_v1709_Mobile,
                16299 => WindowsVersion.Win10_v1709,
                17134 => WindowsVersion.Win10_v1803,
                17763 => WindowsVersion.Win10_v1809,
                18362 => WindowsVersion.Win10_v1903,
                18363 => WindowsVersion.Win10_v1909,
                19041 => WindowsVersion.Win10_v2004,
                19042 => WindowsVersion.Win10_20H2,
                19043 => WindowsVersion.Win10_21H1,
                19044 => WindowsVersion.Win10_21H2,
                19045 => WindowsVersion.Win10_22H2,
                //Build number used exclusively by Windows Server and not by Windows 10 or 11
                20348 => WindowsVersion.Win10_Server2022,
                22000 => WindowsVersion.Win11_21H2,
                > 10240 and < 22000 => WindowsVersion.Win10_InsiderPreview,
                22621 => WindowsVersion.Win11_22H2,
                22631 => WindowsVersion.Win11_23H2,
                26100 => WindowsVersion.Win11_24H2,
                > 26100 => WindowsVersion.Win11_InsiderPreview,
                _ => WindowsVersion.NotDetected
            };
        }
        
        /// <summary>
        /// Detects Windows Version and returns it as a System.Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        // ReSharper disable once MemberCanBePrivate.Global
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public static Version GetWindowsVersion()
        {
            if (OperatingSystem.IsWindows())
            {
                return Version.Parse(RuntimeInformation.OSDescription
                    .Replace("Microsoft Windows", string.Empty)
                    .Replace(" ", string.Empty));
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return the version of Windows in the Version format based on the specified WindowsVersion enum.
        /// </summary>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        /// <exception cref="WindowsVersionDetectionException"></exception>
        public static Version GetWindowsVersionFromEnum(WindowsVersion windowsVersion)
        {
            return windowsVersion switch
            {
                WindowsVersion.Win8 or WindowsVersion.WinServer_2012 => new Version(6, 2, 9200),
                WindowsVersion.Win8_1 or WindowsVersion.WinServer_2012_R2 => new Version(6, 3, 9600),
                WindowsVersion.Win10_v1507 => new Version(10, 0, 10240),
                WindowsVersion.Win10_v1511 => new Version(10, 0, 10586),
                WindowsVersion.Win10_v1607 or WindowsVersion.Win10_Server2016 => new Version(10, 0, 14393),
                WindowsVersion.Win10_v1703 => new Version(10, 0, 15063),
                WindowsVersion.Win10_v1709_Mobile => new Version(10, 0, 15254),
                WindowsVersion.Win10_v1709 or WindowsVersion.Win10_Server_v1709 => new Version(10, 0, 16299),
                WindowsVersion.Win10_v1803 => new Version(10, 0, 17134),
                WindowsVersion.Win10_v1809 or WindowsVersion.Win10_Server2019 => new Version(10, 0, 17763),
                WindowsVersion.Win10_v1903 => new Version(10,0, 18362),
                WindowsVersion.Win10_v1909 => new Version(10,0, 18363),
                WindowsVersion.Win10_v2004 => new Version(10,0, 19041),
                WindowsVersion.Win10_20H2 => new Version(10,0, 19042),
                WindowsVersion.Win10_21H1 => new Version(10,0, 19043),
                WindowsVersion.Win10_21H2 => new Version(10,0, 19044),
                WindowsVersion.Win10_22H2 => new Version(10,0,19045),
                WindowsVersion.Win10_Server2022 => new Version(10, 0, 20348),
                WindowsVersion.Win11_21H2 => new Version(10, 0, 22000),
                WindowsVersion.Win11_22H2 => new Version(10,0,22621),
                WindowsVersion.Win11_23H2 => new Version(10,0,22631),
                WindowsVersion.Win11_24H2 => new Version(10,0,26100),
                _ => throw new WindowsVersionDetectionException()
            };
        }

        /// <summary>
        /// Checks to see whether the specified version of Windows is the same or newer than the installed version of Windows.
        /// </summary>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if not run on Windows.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public static bool IsAtLeastVersion(WindowsVersion windowsVersion)
        {
            if (OperatingSystem.IsWindows())
            {
                return GetWindowsVersion().IsAtLeast(GetWindowsVersionFromEnum(windowsVersion));
            }

            throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Checks to see whether the specified version of Windows is the same or newer than the installed version of Windows.
        /// </summary>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if not run on Windows.</exception>
        [Obsolete(DeprecationMessages.DeprecationV5)]
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public static bool IsAtLeastVersion(Version windowsVersion)
        {
            if (OperatingSystem.IsWindows())
            {
                return  GetWindowsVersion().IsAtLeast(windowsVersion);
            }

            throw new PlatformNotSupportedException();
        }
}