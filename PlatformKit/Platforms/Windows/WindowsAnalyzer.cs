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
    /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static WindowsEdition GetWindowsEdition(WindowsSystemInformation windowsSystemInformation)
    {
        if (OperatingSystem.IsWindows())
        {
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

        throw new PlatformNotSupportedException();
    }

    protected static int GetNetworkCardPositionInWindowsSysInfo(List<NetworkCard> networkCards, NetworkCard lastNetworkCard)
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
    public static WindowsSystemInformation GetWindowsSystemInformation()
    {
        if (!OperatingSystem.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }
        
        WindowsSystemInformation windowsSystemInformation = new WindowsSystemInformation();

        HyperVRequirements hyperVRequirements = new HyperVRequirements();
        
        List<string> processors = new List<string>();
        List<NetworkCard> networkCards = new List<NetworkCard>();
        List<string> ipAddresses = new List<string>();
        
        NetworkCard lastNetworkCard = null;

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

    array[index] = array[index].Replace("  ", String.Empty);

    if (index < (array.Length - 1))
    {
        nextLine = array[index + 1].Replace("  ", String.Empty);
    }
    else
    {
        nextLine = array[index].Replace("  ", String.Empty);
    }
    
    if (nextLine.ToLower().Contains("host name:"))
    {
        windowsSystemInformation.HostName =
            nextLine.Replace("Host Name:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("os name:"))
    {
        windowsSystemInformation.OsName = nextLine.Replace("OS Name:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("os version:") && !nextLine.ToLower().Contains("bios"))
    {
        windowsSystemInformation.OsVersion = nextLine.Replace("OS Version:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("os manufacturer:"))
    {
        windowsSystemInformation.OsManufacturer = nextLine.Replace("OS Manufacturer:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("os configuration:"))
    {
        windowsSystemInformation.OsConfiguration = nextLine.Replace("OS Configuration:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("os build type:"))
    {
        windowsSystemInformation.OsBuildType = nextLine.Replace("OS Build Type:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("registered owner:"))
    {
        windowsSystemInformation.RegisteredOwner = nextLine.Replace("Registered Owner:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("registered organization:"))
    {
        windowsSystemInformation.RegisteredOrganization =
            nextLine.Replace("Registered Organization:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("product id:"))
    {
        windowsSystemInformation.ProductId = nextLine.Replace("Product ID:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("original install date:"))
    {
        nextLine = nextLine.Replace("Original Install Date:", String.Empty);

        string[] info = nextLine.Split(',');

        DateTime dt = new DateTime();

        if (info[0].Contains("/"))
        {
            dt = DateTime.Parse(info[0]);
        }

        if (info[1].Contains(" "))
        {
            info[1] = info[1].Replace(" ", String.Empty).Replace(":", String.Empty);

            string hours = info[1].Substring(0, 2);
            string mins = info[1].Substring(2, 2);
            string seconds = info[1].Substring(3, 2);

            dt = dt.AddHours(Double.Parse(hours));
            dt = dt.AddMinutes(Double.Parse(mins));
            dt = dt.AddSeconds(Double.Parse(seconds));
        }

        windowsSystemInformation.OriginalInstallDate = dt;
    }
    else if (nextLine.ToLower().Contains("system boot time:"))
    {
        nextLine = nextLine.Replace("System Boot Time:", String.Empty);
        string[] info = nextLine.Split(',');

        DateTime dt = new DateTime();

        if (info[0].Contains("/"))
        {
            dt = DateTime.Parse(info[0]);
        }

        if (info[1].Contains(" "))
        {
            info[1] = info[1].Replace(" ", String.Empty).Replace(":", String.Empty);

            string hours = info[1].Substring(0, 2);
            string mins = info[1].Substring(2, 2);
            string seconds = info[1].Substring(4, 2);

            dt = dt.AddHours(Double.Parse(hours));
            dt = dt.AddMinutes(Double.Parse(mins));
            dt = dt.AddSeconds(Double.Parse(seconds));
        }

        windowsSystemInformation.SystemBootTime = dt;
    }
    else if (nextLine.ToLower().Contains("system manufacturer:"))
    {
        windowsSystemInformation.SystemManufacturer = nextLine.Replace("System Manufacturer:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("system model:"))
    {
        windowsSystemInformation.SystemModel = nextLine.Replace("System Model:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("system type:"))
    {
        windowsSystemInformation.SystemType = nextLine.Replace("System Type:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("processor(s):"))
    {
        processors.Add(nextLine.Replace("Processor(s):", String.Empty));

        wasLastLineProcLine = true;
        wasLastLineNetworkLine = false;
    }
    else if (nextLine.ToLower().Contains("bios version:"))
    {
        windowsSystemInformation.BiosVersion = nextLine.Replace("BIOS Version:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("windows directory:"))
    {
        windowsSystemInformation.WindowsDirectory = nextLine.Replace("Windows Directory:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("system directory:"))
    {
        windowsSystemInformation.SystemDirectory = nextLine.Replace("System Directory:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("boot device:"))
    {
        windowsSystemInformation.BootDevice = nextLine.Replace("Boot Device:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("system locale:"))
    {
        windowsSystemInformation.SystemLocale = nextLine.Replace("System Locale:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("input locale:"))
    {
        windowsSystemInformation.InputLocale = nextLine.Replace("Input Locale:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("time zone:"))
    {
        windowsSystemInformation.TimeZone = TimeZoneInfo.Local;
    }
    else if (nextLine.ToLower().Contains("memory:"))
    {
        nextLine = nextLine.Replace(",", String.Empty).Replace(" MB", String.Empty);

        if (nextLine.ToLower().Contains("total physical memory:"))
        {
            nextLine = nextLine.Replace("Total Physical Memory:", String.Empty);
            windowsSystemInformation.TotalPhysicalMemoryMB = int.Parse(nextLine);
        }
        else if (nextLine.ToLower().Contains("available physical memory"))
        {
            nextLine = nextLine.Replace("Available Physical Memory:", String.Empty);
            windowsSystemInformation.AvailablePhysicalMemoryMB = int.Parse(nextLine);
        }
        if (nextLine.ToLower().Contains("virtual memory: max size:"))
        {
            nextLine = nextLine.Replace("Virtual Memory: Max Size:", String.Empty);
            windowsSystemInformation.VirtualMemoryMaxSizeMB = int.Parse(nextLine);
        }
        else if (nextLine.ToLower().Contains("virtual memory: available:"))
        {
            nextLine = nextLine.Replace("Virtual Memory: Available:", String.Empty);
            windowsSystemInformation.VirtualMemoryAvailableSizeMB = int.Parse(nextLine);
        }
        else if (nextLine.ToLower().Contains("virtual memory: in use:"))
        {
            nextLine = nextLine.Replace("Virtual Memory: In Use:", String.Empty);
            windowsSystemInformation.VirtualMemoryInUse = int.Parse(nextLine);
        }
    }
    else if (nextLine.ToLower().Contains("page file location(s):"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        List<string> locations = new List<string>();

        locations.Add(nextLine.Replace("Page File Location(s):", String.Empty));

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
        
        windowsSystemInformation.Domain = nextLine.Replace("Domain:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("logon server:"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        windowsSystemInformation.LogonServer = nextLine.Replace("Logon Server:", String.Empty);
    }
    else if (nextLine.ToLower().Contains("hotfix(s):"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        List<string> hotfixes = new List<string>();
        
        int hotfixCount = 0;
        while (array[index + 2 + hotfixCount].Contains("[") && array[index + 2 + hotfixCount].Contains("]:"))
        {
            hotfixes.Add(array[index + 2 + hotfixCount].Replace("  ", String.Empty));
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
        
        NetworkCard networkCard = new NetworkCard
        {
            Name = array[index + 2].Replace("  ", String.Empty)
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
           networkCards[position].ConnectionName = nextLine.Replace("Connection Name:", String.Empty).Replace("  ", String.Empty);
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
        networkCards[position].DhcpServer = nextLine.Replace("DHCP Server:", String.Empty).Replace("  ", String.Empty);
    }
    else if (nextLine.ToLower().Contains("[") && nextLine.ToLower().Contains("]"))
    {
        string compare = nextLine.Replace("[", String.Empty).Replace("]:", String.Empty);
        
        int dotCounter = 0;
        foreach (char c in compare)
        {
            if (c == '.' || c == ':')
            {
                dotCounter++;
            }
        }

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
        hyperVRequirements.VmMonitorModeExtensions = nextLine.Replace("Hyper-V Requirements:", String.Empty)
            .Replace("VM Monitor Mode Extensions: ", String.Empty).Contains("Yes");
    }
    else if (nextLine.ToLower().Contains("virtualization enabled in firmware:"))
    {
        hyperVRequirements.VirtualizationEnabledInFirmware =
            nextLine.Replace("Virtualization Enabled In Firmware:", String.Empty).Contains("Yes");
    }
    else if (nextLine.ToLower().Contains("second level address translation:"))
    {
        hyperVRequirements.SecondLevelAddressTranslation = nextLine
            .Replace("Second Level Address Translation:", String.Empty).Contains("Yes");
    }
    else if (nextLine.ToLower().Contains("data execution prevention available:"))
    {
        wasLastLineNetworkLine = false;
        wasLastLineProcLine = false;
        
        hyperVRequirements.DataExecutionPreventionAvailable = nextLine
            .Replace("Data Execution Prevention Available:", String.Empty).Contains("Yes");
        break;
    }
}
        
        
        if (networkCardNumber == 1)
        {
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
    /// <exception cref="OperatingSystemDetectionException"></exception>
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
            WindowsVersion.NotDetected => throw new WindowsVersionDetectionException(),
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
        /// <exception cref="Exception"></exception>
        public static WindowsVersion GetWindowsVersionToEnum(Version input)
        {
            if (input.Major == 5)
            {
                //We don't support Windows XP.
                return WindowsVersion.NotSupported;
            }

            switch (input.Build)
            {
                case 6000:
                    return WindowsVersion.NotSupported;
                //return WindowsVersion.WinVista;
                case 6001:
                    return WindowsVersion.NotSupported;
                //return WindowsVersion.WinVistaSP1;
                case 6002:
                    return WindowsVersion.NotSupported;
                //return WindowsVersion.WinVistaSP2;
                case 6003:
                    return WindowsVersion.NotSupported;
                //return WindowsVersion.WinServer_2008; //Technically Server 2008 also can be Build number 6001 or 6002 but this provides an easier way to identify it.
                case 7600:
                    return WindowsVersion.NotSupported;
                case 7601:
                    return WindowsVersion.NotSupported;
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
                case 19045:
                    return WindowsVersion.Win10_22H2;
                case 20348:
                    return WindowsVersion.Win10_Server2022; //Build number used exclusively by Windows Server and not by Windows 10 or 11.
                case 22000:
                    return WindowsVersion.Win11_21H2;
                case 22621:
                    return WindowsVersion.Win11_22H2;
                case 22631:
                    return WindowsVersion.Win11_23H2;
                case 26100:
                    return WindowsVersion.Win11_24H2;
                default:
                    //Assume any non enumerated value in between Windows 10 versions is an Insider preview for Windows 10.
                    if (input.Build is > 10240 and < 22000)
                    {
                        return WindowsVersion.Win10_InsiderPreview;
                    }
                    //Assume non enumerated values for Windows 11 are Insider Previews for Windows 11.

                    if(input.Build > 22631)
                    {
                        return WindowsVersion.Win11_InsiderPreview;
                    }

                    if (input.Build < 6000)
                    {
                        return WindowsVersion.NotSupported;
                    }

                    return WindowsVersion.NotDetected;
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
        [SupportedOSPlatform("windows")]
#endif
        public static Version GetWindowsVersion()
        {
            if (OperatingSystem.IsWindows())
            {
                return Version.Parse(RuntimeInformation.OSDescription
                    .Replace("Microsoft Windows", string.Empty)
                    .Replace(" ", string.Empty).AddMissingZeroes());
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Return the version of Windows in the Version format based on the specified WindowsVersion enum.
        /// </summary>
        /// <param name="windowsVersion"></param>
        /// <returns></returns>
        /// <exception cref="OperatingSystemDetectionException"></exception>
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