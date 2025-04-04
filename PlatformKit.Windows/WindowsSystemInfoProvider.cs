﻿/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AlastairLundy.CliInvoke;
using AlastairLundy.CliInvoke.Abstractions;
using AlastairLundy.CliInvoke.Builders;
using AlastairLundy.CliInvoke.Builders.Abstractions;
using AlastairLundy.CliInvoke.Specializations.Configurations;
using AlastairLundy.Extensions.Processes;


using PlatformKit.Core;
using PlatformKit.Windows.Abstractions;
using PlatformKit.Windows.Exceptions;
using PlatformKit.Windows.Internal;

namespace PlatformKit.Windows;

public class WindowsSystemInfoProvider : IWindowsSystemInfoProvider
{
    private readonly ICliCommandInvoker _cliCommandInvoker;

    public WindowsSystemInfoProvider(ICliCommandInvoker cliCommandInvoker)
    {
       _cliCommandInvoker = cliCommandInvoker;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]    
#endif
    public async Task<WindowsSystemInfo> GetWindowsSystemInfoAsync()
    {
          if (OperatingSystem.IsWindows() == false)
          {
              throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
          }
        
          WindowsSystemInfo windowsSystemInformation = new WindowsSystemInfo();
          HyperVRequirementsInfo hyperVRequirements = new HyperVRequirementsInfo();
        
          List<string> processors = new List<string>();
          List<NetworkCardModel> networkCards = new List<NetworkCardModel>();
          List<string> ipAddresses = new List<string>();
        
          NetworkCardModel lastNetworkCard = null;

          ICliCommandConfigurationBuilder commandBuilder = new CliCommandConfigurationBuilder(new CmdCommandConfiguration())
              .WithArguments("systeminfo")
              .WithWorkingDirectory(Environment.SystemDirectory);

          CliCommandConfiguration command = commandBuilder.Build();
              
          BufferedProcessResult descResult = await _cliCommandInvoker.ExecuteBufferedAsync(command);
          
          string desc = descResult.StandardOutput;
            
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
                      windowsSystemInformation.TotalPhysicalMemoryMegabyte = int.Parse(nextLine);
                  }
                  else if (nextLine.ToLower().Contains("available physical memory"))
                  {
                      nextLine = nextLine.Replace("Available Physical Memory:", string.Empty);
                      windowsSystemInformation.AvailablePhysicalMemoryMegabyte = int.Parse(nextLine);
                  }
                  if (nextLine.ToLower().Contains("virtual memory: max size:"))
                  {
                      nextLine = nextLine.Replace("Virtual Memory: Max Size:", string.Empty);
                      windowsSystemInformation.VirtualMemoryMaxSizeMegabyte = int.Parse(nextLine);
                  }
                  else if (nextLine.ToLower().Contains("virtual memory: available:"))
                  {
                      nextLine = nextLine.Replace("Virtual Memory: Available:", string.Empty);
                      windowsSystemInformation.VirtualMemoryAvailableSizeMegabyte = int.Parse(nextLine);
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

                  NetworkCardModel networkCard = new NetworkCardModel(
                      name: array[index + 2].Replace("  ", string.Empty),
                  connectionName: "",
                      dhcpServer: "",
                      ipAddresses: ipAddresses.ToArray(),
                      isDhcpEnabled: false);
                    

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
                  networkCards[position].IsDhcpEnabled = array[index + 4].ToLower().Contains("yes");
              }
              else if (nextLine.ToLower().Contains("dhcp server:"))
              {
                  int position = GetNetworkCardPositionInWindowsSysInfo(networkCards, lastNetworkCard);
                  networkCards[position].DhcpServer = nextLine.Replace("DHCP Server:", string.Empty).Replace("  ", string.Empty);
              }
              else if (nextLine.ToLower().Contains("[") && nextLine.ToLower().Contains("]"))
              {
                  string compare = nextLine.Replace("[", string.Empty).Replace("]:", string.Empty);

                  int dotCounter = compare.Count(x => x.Equals('.'));

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

    private int GetNetworkCardPositionInWindowsSysInfo(List<NetworkCardModel> networkCards, NetworkCardModel lastNetworkCard)
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

    private DateTime DateParser(string[] info)
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
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    /// <exception cref="WindowsEditionDetectionException"></exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]    
#endif
    public async Task<WindowsEdition> GetWindowsEditionAsync()
    {
         if (OperatingSystem.IsWindows() == false)
         {
             throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
         }

         WindowsSystemInfo systemInfoModel = await GetWindowsSystemInfoAsync();
         string edition = systemInfoModel.OsName.ToLower();
                
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
         else if (edition.Contains("se") && systemInfoModel.OsName.Contains("11"))
         {
             return WindowsEdition.SE;
         }

         throw new WindowsEditionDetectionException();
    }
}