/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;

using PlatformKit.Identification;

using PlatformKit;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;


    var platformManager = new PlatformIdentification();
var osAnalyzer = new OSAnalyzer();
            var processManager = new ProcessManager();
var runtimeIdentification = new RuntimeIdentification();

var windowsAnalyzer = new WindowsAnalyzer();
var linuxAnalyzer = new LinuxAnalyzer();
var macAnalyzer = new MacOsAnalyzer();

Console.WriteLine(".NET Detected RuntimeID: " + RuntimeInformation.RuntimeIdentifier);

            var title = $"{platformManager.GetAppName()} v{platformManager.GetAppVersion()}";
            Console.Title = title;
            //    Console.WriteLine(title) ;
          
            Console.WriteLine("Programmatically Generated RuntimeID (AnyGeneric): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.AnyGeneric));
            Console.WriteLine("Programmatically Generated RuntimeID (Generic): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic));
            Console.WriteLine("Programmatically Generated RuntimeID (Specific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));

            foreach (var candidate in runtimeIdentification.GetPossibleRuntimeIdentifierCandidates())
            {
                Console.WriteLine("Possible RID: " + candidate);
            }
          
            //processManager.OpenUrlInBrowser("duckduckgo.com");
            
            if (OperatingSystem.IsWindows())
            {
                Console.WriteLine("Windows Version Enum: " + windowsAnalyzer.GetWindowsVersionToEnum());

             //   var res = processManager.RunPowerShellCommand("ping www.duckduckgo.com");
                
             //  Console.WriteLine(res);
             
               // var caption = windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem");
               // var name = windowsAnalyzer.GetWMIValue("Name", "Win32_OperatingSystem");
                
                //Console.WriteLine("Caption: " + caption);
                //Console.WriteLine("Name: " + name);
            
           //     Console.WriteLine("OS :" + windowsAnalyzer.GetWMIClass("Win32_OperatingSystem"));
                
                //Console.WriteLine("Caption: " + windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem"));

               // Console.WriteLine(processManager.RunPowerShellCommand("systeminfo"));
               Console.WriteLine("WindowsEdition: " + windowsAnalyzer.DetectWindowsEdition().ToString());
               
               Console.WriteLine("WinOS " + windowsAnalyzer.GetWMIClass("Win32_OperatingSystem"));
               
              /* var desc = processManager.RunPowerShellCommand("systeminfo");

               var arr = desc.Split(Environment.NewLine);
               
                   //stringBuilder.ToString().Split(" ");

               for (int index = 0; index < arr.Length; index++)
               {
                    Console.WriteLine(index + " " + arr[index]);
               }
               */

              var sysinfo = windowsAnalyzer.GetWindowsSystemInformation();
              sysinfo.ToConsoleWriteLine();
            }

            if (OperatingSystem.IsMacOS())
            {
                Console.WriteLine("Is this AppleSilicon?: " + macAnalyzer.IsAppleSiliconMac());
                Console.WriteLine("Mac Processor Type: " + macAnalyzer.GetMacProcessorType());

                Console.WriteLine("macOS Version Enum: " +
                                  macAnalyzer.GetMacOsVersionToEnum());
                
                Console.WriteLine("macOS Version Detected: " + osAnalyzer.DetectOSVersion().ToString());
                
                Console.WriteLine("Darwin Version: " + macAnalyzer.DetectDarwinVersion());
                Console.WriteLine("Xnu Version: " + macAnalyzer.DetectXnuVersion());
            }

            processManager.OpenUrlInBrowser("duckduckgo.com");

            // Console.WriteLine("OsVersion: " + versionAnalyzer.DetectOSVersion());
    
            if (OperatingSystem.IsLinux())
            {  
                Console.WriteLine("Linux Distro Name: " + linuxAnalyzer.GetLinuxDistributionInformation().Name);
                Console.WriteLine("Linux Distro Version: " + linuxAnalyzer.DetectLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + linuxAnalyzer.DetectLinuxKernelVersion());
                
                Console.WriteLine("Is an LTS release? " + linuxAnalyzer.GetLinuxDistributionInformation().IsLongTermSupportRelease);
            }

Console.WriteLine("PlatformKit Version: " + PlatformIdentification.GetPlatformKitVersion().ToString());

            Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);
            
            Console.WriteLine(RuntimeInformation.RuntimeIdentifier);
            Console.WriteLine(RuntimeInformation.FrameworkDescription);