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

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public WindowsSystemInformation GetWindowsSystemInformation()
    {
        WindowsSystemInformation windowsSystemInformation = new WindowsSystemInformation();
        
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

for (var index = 0; index < array.Length; index++)
{
    var line = array[index].Contains("  ") ? array[index].Replace("  ", String.Empty) : array[index];
    var nextLine = index < array.Length ? array[index + 1] : array[index];

    if (line.ToLower().Contains("host name:"))
    {
        windowsSystemInformation.HostName =
            line.Replace("Host Name:", String.Empty);
    }
    else if (line.ToLower().Contains("os name:"))
    {
        windowsSystemInformation.OsName = line.Replace("OS Name:", String.Empty);
    }
    else if (line.ToLower().Contains("os version:"))
    {
        windowsSystemInformation.OsVersion = line.Replace("OS Version:", String.Empty).Substring(0, 9);
    }
    else if (line.ToLower().Contains("os manufacturer:"))
    {
        windowsSystemInformation.OsManufacturer = line.Replace("OS Manufacturer:", String.Empty);
    }
    else if (line.ToLower().Contains("os configuration:"))
    {
        windowsSystemInformation.OsConfiguration = line.Replace("OS Configuration:", String.Empty);
    }
    else if (line.ToLower().Contains("os build type:"))
    {
        windowsSystemInformation.OsBuildType = line.Replace("OS Build Type:", String.Empty);
    }
    else if (line.ToLower().Contains("registered owner:"))
    {
        windowsSystemInformation.RegisteredOwner = line.Replace("Registered Owner:", String.Empty);
    }
    else if (line.ToLower().Contains("registered organization:"))
    {
        windowsSystemInformation.RegisteredOrganization =
            line.Replace("Registered Organization:", String.Empty);
    }
    else if (line.ToLower().Contains("product id:"))
    {
        windowsSystemInformation.ProductId = line.Replace("Product ID:", String.Empty);
    }
    else if (line.ToLower().Contains("original install date:"))
    {
        line = line.Replace("Original Install Date:", String.Empty);

        var info = line.Split(',');

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
    else if (line.ToLower().Contains("system boot time:"))
    {
        line = line.Replace("System Boot Time:", String.Empty);
        var info = line.Split(',');

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
    else if (line.ToLower().Contains("system manufacturer:"))
    {
        windowsSystemInformation.SystemManufacturer = line.Replace("System Manufacturer:", String.Empty);
    }
    else if (line.ToLower().Contains("system model:"))
    {
        windowsSystemInformation.SystemModel = line.Replace("System Model:", String.Empty);
    }
    else if (line.ToLower().Contains("system type:"))
    {
        windowsSystemInformation.SystemType = line.Replace("System Type:", String.Empty);
    }
    else if (line.ToLower().Contains("processor(s):"))
    {
        List<string> processors = new List<string>();
        
        processors.Add(line.Replace("Processor(s):", String.Empty));

        processors.Add(nextLine);

        int procCount = 2;
        while (array[index + procCount].Contains("[") && array[index + procCount].Contains("]:"))
        {
            processors.Add(array[index + procCount]);
            procCount++;
        }

        windowsSystemInformation.Processors = processors.ToArray();
    }
    else if (line.ToLower().Contains("bios version:"))
    {
        windowsSystemInformation.BiosVersion = line.Replace("BIOS Version:", String.Empty);
    }
    else if (line.ToLower().Contains("windows directory:"))
    {
        windowsSystemInformation.WindowsDirectory = line.Replace("Windows Directory:", String.Empty);
    }
    else if (line.ToLower().Contains("system directory:"))
    {
        windowsSystemInformation.SystemDirectory = line.Replace("System Directory:", String.Empty);
    }
    else if (line.ToLower().Contains("boot device:"))
    {
        windowsSystemInformation.BootDevice = line.Replace("Boot Device:", String.Empty);
    }
    else if (line.ToLower().Contains("system locale:"))
    {
        windowsSystemInformation.SystemLocale = line.Replace("System Locale:", String.Empty);
    }
    else if (line.ToLower().Contains("input locale:"))
    {
        windowsSystemInformation.SystemLocale = line.Replace("Input Locale:", String.Empty);
    }
    else if (line.ToLower().Contains("time zone:"))
    {
        windowsSystemInformation.TimeZone = TimeZoneInfo.Local;
    }
    else if (line.ToLower().Contains("memory:"))
    {
        line = line.Replace(",", String.Empty).Replace(" MB", String.Empty);

        if (line.ToLower().Contains("total physical memory:"))
        {
            line = line.Replace("Total Physical Memory:", String.Empty);
            windowsSystemInformation.TotalPhysicalMemoryMB = int.Parse(line);
        }
        else if (line.ToLower().Contains("available physical memory"))
        {
            line = line.Replace("Available Physical Memory:", String.Empty);
            windowsSystemInformation.AvailablePhysicalMemoryMB = int.Parse(line);
        }
        if (line.ToLower().Contains("virtual memory: max size:"))
        {
            line = line.Replace("Virtual Memory: Max Size:", String.Empty);
            windowsSystemInformation.VirtualMemoryMaxSizeMB = int.Parse(line);
        }
        else if (line.ToLower().Contains("virtual memory: available:"))
        {
            line = line.Replace("Virtual Memory: Available:", String.Empty);
            windowsSystemInformation.VirtualMemoryAvailableSizeMB = int.Parse(line);
        }
        else if (line.ToLower().Contains("virtual memory: in use:"))
        {
            line = line.Replace("Virtual Memory: In Use:", String.Empty);
            windowsSystemInformation.VirtualMemoryInUse = int.Parse(line);
        }
    }
    else if (line.ToLower().Contains("page file location(s):"))
    {
        List<string> locations = new List<string>();

        locations.Add(array[index].Replace("Page File Location(s):", String.Empty));

        int locationNumber = 1;
        
        while (!array[index + locationNumber].ToLower().Contains("domain"))
        {
            locations.Add(array[index + locationNumber]);
            locationNumber++;
        }

        windowsSystemInformation.PageFileLocations = locations.ToArray();
    }
    else if (line.ToLower().Contains("domain:"))
    {
        windowsSystemInformation.Domain = line.Replace("Domain:", String.Empty);
    }
    else if (line.ToLower().Contains("logon server:"))
    {
        windowsSystemInformation.LogonServer = line.Replace("Logon Server:", String.Empty);
    }
    else if (line.ToLower().Contains("hotfix(s):"))
    {
        List<string> hotfixes = new List<string>();
        hotfixes.Add(line.Replace("Hotfix(s):", String.Empty));

        int hotfixCount = 0;
        while (array[index + 1 + hotfixCount].Contains("[") && array[index + 1 + hotfixCount].Contains("]:"))
        {
            hotfixes.Add(array[index + 1 + hotfixCount]);
            hotfixCount++;
        }

        windowsSystemInformation.HotfixesInstalled = hotfixes.ToArray();
    }
    else if (line.ToLower().Contains("network card(s):"))
    {
        List<NetworkCard> networkCards = new List<NetworkCard>();

        while (true)
        {
            var networkCard = new NetworkCard
            {
                Name = line.Replace("Network Card(s):", String.Empty),
                ConnectionName = array[index + 1].Replace("Connection Name:", String.Empty)
            };

            array[index + 2] = array[index + 2].Replace("DHCP Enabled:   ", String.Empty);

            var yesOrNo = array[index + 2].ToLower().Equals("yes");

            networkCard.DhcpEnabled = yesOrNo;

            networkCard.DhcpServer = array[index + 3].Replace("DHCP Server:   ", String.Empty);

            List<string> ipAddresses = new List<string>();
        
            int ipNumber = 0;
            while (array[index + 4 + ipNumber].Contains("[") && array[index + 4 + ipNumber].Contains("]:"))
            {
                ipAddresses.Add(array[index + 4 + ipNumber]);
                ipNumber++;
            }

            networkCard.IpAddresses = ipAddresses.ToArray();
            networkCards.Add(networkCard);
            
            if (!nextLine.Contains("[") && nextLine.Contains("]:"))
            {
                break;
            }
        }

        windowsSystemInformation.NetworkCards = networkCards.ToArray();
    }
    else if (line.ToLower().Contains("hyper-v requirements:"))
    {
        HyperVRequirements hyperVRequirements = new HyperVRequirements();
        hyperVRequirements.VmMonitorModeExtensions = line.Replace("Hyper-V Requirements:", String.Empty)
            .Replace("VM Monitor Mode Extensions: ", String.Empty).Contains("Yes");

        hyperVRequirements.VirtualizationEnabledInFirmware =
            nextLine.Replace("Virtualization Enabled In Firmware:", String.Empty).Contains("Yes");

        hyperVRequirements.SecondLevelAddressTranslation = array[index + 2]
            .Replace("Second Level Address Translation:", String.Empty).Contains("Yes");
        
        hyperVRequirements.DataExecutionPreventionAvailable = array[index + 3]
            .Replace("Data Execution Prevention Available:", String.Empty).Contains("Yes");
    }
}
        
        #endregion

        return windowsSystemInformation;
    }
    

}