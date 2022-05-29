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
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics.VersionAnalyzers;
using AluminiumTech.DevKit.PlatformKit.Exceptions;
using AluminiumTech.DevKit.PlatformKit.Hardware.Shared.Models;
using AluminiumTech.DevKit.PlatformKit.Software.Windows;
using AluminiumTech.DevKit.PlatformKit.Software.Windows.Models;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;

/// <summary>
/// 
/// </summary>
public class WindowsAnalyzer
{
    protected readonly ProcessManager _processManager;
    protected readonly OSAnalyzer _osAnalyzer;
    
    public WindowsAnalyzer()
    {
        _processManager = new ProcessManager();
        _osAnalyzer = new OSAnalyzer();
    }

    /// <summary>
    /// Detects the Edition of Windows being run.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperatingSystemDetectionException"></exception>
    public WindowsEdition DetectWindowsEdition()
    {
        try
        {
            if (_osAnalyzer.IsWindows())
            {
                var desc = _processManager.RunPowerShellCommand("systeminfo");

                desc = desc.Replace("  ", String.Empty);
                
                var arr = desc.Replace(":", String.Empty).Split(' ');
                
                var edition = arr[41].Replace("OS", String.Empty);

                //var edition = GetWMIValue("Name", "Win32_OperatingSystem");

                //var edition = GetWindowsRegistryValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
                //   "EditionID");

                if (edition.ToLower().Contains("home"))
                {
                    return WindowsEdition.Home;
                }
                else if (edition.ToLower().Contains("pro") && edition.ToLower().Contains("workstation"))
                {
                    return WindowsEdition.ProfessionalForWorkstations;
                }
                else if (edition.ToLower().Contains("pro") && !edition.ToLower().Contains("education"))
                {
                    return WindowsEdition.Professional;
                }
                else if (edition.ToLower().Contains("pro") && edition.ToLower().Contains("education"))
                {
                    return WindowsEdition.ProfessionalForEducation;
                }
                else if (!edition.ToLower().Contains("pro") && edition.ToLower().Contains("education"))
                {
                    return WindowsEdition.Education;
                }
                else if (edition.ToLower().Contains("server"))
                {
                    return WindowsEdition.Server;
                }
                else if (edition.ToLower().Contains("enterprise") && edition.ToLower().Contains("ltsc") &&
                         !edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.EnterpriseLTSC;
                }
                else if (edition.ToLower().Contains("enterprise") && !edition.ToLower().Contains("ltsc") &&
                         !edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.EnterpriseSemiAnnualChannel;
                }
                else if (edition.ToLower().Contains("enterprise") && edition.ToLower().Contains("ltsc") &&
                         edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.IoTEnterpriseLTSC;
                }
                else if (edition.ToLower().Contains("enterprise") && !edition.ToLower().Contains("ltsc") &&
                         edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.IoTEnterprise;
                }

                if (WindowsVersionAnalyzer.IsWindows11())
                {
                    if (edition.ToLower().Contains("se"))
                    {
                        return WindowsEdition.SE;
                    }
                }

                throw new OperatingSystemDetectionException();
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.ToString());
            throw new Exception(exception.ToString());
        }
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Gets information from a WMI class in WMI.
    /// </summary>
    /// <param name="wmiClass"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public string GetWMIClass(string wmiClass)
    {
        if (_osAnalyzer.IsWindows())
        {
            var result = _processManager.RunPowerShellCommand("Get-WmiObject -Class " + wmiClass + " | Select-Object *");
            // var result = _processManager.RunPowerShellCommand("Get-CimInstance -Class " + wmiClass);
            
//#if DEBUG
  //          Console.WriteLine(result);              
//#endif

            return result;
        }

        throw new PlatformNotSupportedException();
    }
    
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Gets a property/value in a WMI Class from WMI.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="wmiClass"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public string GetWMIValue(string property, string wmiClass)
    {
        if (_osAnalyzer.IsWindows())
        {
            var result = _processManager.RunPowerShellCommand("Get-CimInstance -Class " 
                                                              + wmiClass + " -Property " + property);

            #if DEBUG
           //     Console.WriteLine(result);
            #endif
            
            var arr = result.Split(Convert.ToChar("\r\n"));
            
           foreach (var str in arr)
           {
               Console.WriteLine(str);
               
               if (str.ToLower().StartsWith(property.ToLower()))
               {
                   return str
                       .Replace(" : ", String.Empty)
                       .Replace(property, String.Empty)
                       .Replace(" ", String.Empty);
                       
               }
           }
           
           throw new ArgumentException();
        } 
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    /// <summary>
    ///  Gets the value of a registry key in the Windows registry.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
    public string GetWindowsRegistryValue(string query, string value){
        if (_osAnalyzer.IsWindows())
        {
            var result = _processManager.RunCmdCommand("REG QUERY " + query + " /v " + value);
                    
                if (result != null)
                {
                    return result.Replace(value, String.Empty)
                        .Replace("REG_SZ", String.Empty)
                        .Replace(" ", String.Empty);
                }
                else
                {
                    throw new ArgumentNullException();
                }
        }

        throw new PlatformNotSupportedException();
    }

    protected int GetNetworkCardPositionInWindowsSysInfo(List<NetworkCard> networkCards, NetworkCard lastNetworkCard)
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
    public WindowsSystemInformation GetWindowsSystemInformation()
    {
        if (!_osAnalyzer.IsWindows())
        {
            throw new PlatformNotSupportedException();
        }
        
        WindowsSystemInformation windowsSystemInformation = new WindowsSystemInformation();

        HyperVRequirements hyperVRequirements = new HyperVRequirements();
        
        List<string> processors = new List<string>();
        List<NetworkCard> networkCards = new List<NetworkCard>();
        List<string> ipAddresses = new List<string>();
        
        NetworkCard lastNetworkCard = null;
        string lastProcessor = "";
        
        var desc = _processManager.RunPowerShellCommand("systeminfo");

#if NET5_0_OR_GREATER
        var array = desc.Split(Environment.NewLine);
#elif NETSTANDARD2_0_OR_GREATER || NETCOREAPP3_0_OR_GREATER
        var array = desc.Split(new[]
        {
            Environment.NewLine
        }, StringSplitOptions.None);
#endif

#region Manual Detection

bool wasLastLineProcLine = false;
bool wasLastLineNetworkLine = false;

int networkCardNumber = 0;

for (var index = 0; index < array.Length; index++)
{
    var nextLine = "";

    array[index] = array[index].Replace("  ", String.Empty);

    if (index < (array.Length - 1))
    {
        nextLine = array[index + 1].Replace("  ", String.Empty);
    }
    else
    {
        nextLine = array[index].Replace("  ", String.Empty);
    }

    #if DEBUG
    Console.WriteLine("NextLine: " + nextLine);
    #endif
    
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

        var info = nextLine.Split(',');

        DateTime dt = new DateTime();

        if (info[0].Contains("/"))
        {
            dt = DateTime.Parse(info[0]);
        }

        if (info[1].Contains(" "))
        {
            info[1] = info[1].Replace(" ", String.Empty).Replace(":", String.Empty);

            var hours = info[1].Substring(0, 2);
            var mins = info[1].Substring(2, 2);
            var seconds = info[1].Substring(3, 2);

            dt = dt.AddHours(Double.Parse(hours));
            dt = dt.AddMinutes(Double.Parse(mins));
            dt = dt.AddSeconds(Double.Parse(seconds));
        }

        windowsSystemInformation.OriginalInstallDate = dt;
    }
    else if (nextLine.ToLower().Contains("system boot time:"))
    {
        nextLine = nextLine.Replace("System Boot Time:", String.Empty);
        var info = nextLine.Split(',');

        DateTime dt = new DateTime();

        if (info[0].Contains("/"))
        {
            dt = DateTime.Parse(info[0]);
        }

        if (info[1].Contains(" "))
        {
            info[1] = info[1].Replace(" ", String.Empty).Replace(":", String.Empty);

            var hours = info[1].Substring(0, 2);
            var mins = info[1].Substring(2, 2);
            var seconds = info[1].Substring(4, 2);

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
      //  processors.Add(nextLine.Replace("Processor(s):", String.Empty));

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
        
        NetworkCard networkCard = new NetworkCard();
        
        int start = 0;
        int finish = 0;
        
        for (int index1 = 0; index1 < array[index + 2].Length; index1++)
        {
            var c = array[index + 2][index1];
            
            if (c == '[')
            {
                start = index1;
            }
            else if (c == ':')
            {
                finish = index1;
            }
        }

        array[index + 2] = array[index + 2].Remove(start, Math.Abs(finish - start) + 1);
        
        networkCard.Name = array[index + 2].Replace("  ", String.Empty);

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
           var position = GetNetworkCardPositionInWindowsSysInfo(networkCards, lastNetworkCard);
           networkCards[position].ConnectionName = nextLine.Replace("Connection Name:", String.Empty).Replace("  ", String.Empty);
        }
    }
    else if (nextLine.ToLower().Contains("dhcp enabled:"))
    {
        wasLastLineProcLine = false;
        
        var position = GetNetworkCardPositionInWindowsSysInfo(networkCards, lastNetworkCard);
        networkCards[position].DhcpEnabled = array[index + 4].ToLower().Contains("yes");
    }
    else if (nextLine.ToLower().Contains("dhcp server:"))
    {
        var position = GetNetworkCardPositionInWindowsSysInfo(networkCards, lastNetworkCard);
        networkCards[position].DhcpServer = nextLine.Replace("DHCP Server:", String.Empty).Replace("  ", String.Empty);
    }
    else if (nextLine.ToLower().Contains("[") && nextLine.ToLower().Contains("]"))
    {
        var compare = nextLine.Replace("[", String.Empty).Replace("]:", String.Empty);
        
        int dotCounter = 0;
        foreach (char c in compare)
        {
            if (c == '.' || c == ':')
            {
                dotCounter++;
            }
        }
/*
        int start = 0;
        int finish = 0;
        
        for (int index1 = 0; index1 < nextLine.Length; index1++)
        {
            var c = nextLine[index1];
            
            if (c == '[')
            {
                start = index1;
            }
            else if (c == ':')
            {
                finish = index1;
            }
        }

        nextLine = nextLine.Remove(start, Math.Abs(finish - start) + 1);
  */      
        //nextLine = nextLine.Replace("[", String.Empty).Replace("]", String.Empty);

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
    else
    {
        
    }
}
        
        #endregion
        
        if (networkCardNumber == 1)
        {
            if (lastNetworkCard != null && ipAddresses != null && networkCards.Count > 0)
                networkCards[networkCardNumber - 1].IpAddresses = ipAddresses.ToArray();
            
            ipAddresses.Clear();
        }
        
        windowsSystemInformation.NetworkCards = networkCards.ToArray();
        windowsSystemInformation.Processors = processors.ToArray();
        windowsSystemInformation.HyperVRequirements = hyperVRequirements;
        return windowsSystemInformation;
    }


}