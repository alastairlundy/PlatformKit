/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at
    Commercial License - https://neverspy.tech/platformkit-commercial-license or in the PlatformKit_Commercial_License.txt
    Non-Commercial License - https://neverspy.tech/platformkit-noncommercial-license or in the PlatformKit_NonCommercial_License.txt
  
  To use PlatformKit under either a commercial or non-commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

// See https://aka.ms/new-console-template for more information

using System;
using System.Runtime.InteropServices;
using System.Text;

using PlatformKit.Identification;

using PlatformKit;
using PlatformKit.Internal.Licensing;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

PlatformKitSettings.SelectedLicense = PlatformKitConstants.GPLv3_OrLater;

    var platformManager = new PlatformIdentification();
var osAnalyzer = new OSAnalyzer();
            var processManager = new ProcessManager();
var runtimeIdentification = new RuntimeIdentification();

var windowsAnalyzer = new WindowsAnalyzer();
var linuxAnalyzer = new LinuxAnalyzer();
var macAnalyzer = new MacOSAnalyzer();

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
            
            if (OSAnalyzer.IsWindows())
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

            if (OSAnalyzer.IsMac())
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
    
            if (OSAnalyzer.IsLinux())
            {  
                Console.WriteLine("Linux Distro Name: " + linuxAnalyzer.GetLinuxDistributionInformation().Name);
                Console.WriteLine("Linux Distro Version: " + linuxAnalyzer.DetectLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + linuxAnalyzer.DetectLinuxKernelVersion());
                
                Console.WriteLine("Is an LTS release? " + linuxAnalyzer.GetLinuxDistributionInformation().IsLongTermSupportRelease);
            }

Console.WriteLine("PlatformKit Version: " + PlatformIdentification.GetPlatformKitVersion().ToString());

            Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);