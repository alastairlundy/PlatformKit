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

// See https://aka.ms/new-console-template for more information

using System.Runtime.InteropServices;

using PlatformKit;
using PlatformKit.Identification;


using PlatformKit.Linux;
using PlatformKit.Linux.Models;

using PlatformKit.Mac;

using PlatformKit.Windows;
using PlatformKit.Windows.Models;


var platformManager = new PlatformKitIdentification();
    
Console.WriteLine(".NET Detected RuntimeID: " + RuntimeInformation.RuntimeIdentifier);
    
            var title = $"{platformManager.GetProjectName()} v{platformManager.GetProjectVersion()}";
            Console.Title = title;
            //    Console.WriteLine(title) ;
          
            Console.WriteLine("Programmatically Generated RuntimeID (AnyGeneric): " + RuntimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.AnyGeneric));
            Console.WriteLine("Programmatically Generated RuntimeID (Generic): " + RuntimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic));
            Console.WriteLine("Programmatically Generated RuntimeID (Specific): " + RuntimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + RuntimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + RuntimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));

            foreach (var candidate in RuntimeIdentification.GetPossibleRuntimeIdentifierCandidates())
            {
                Console.WriteLine("Possible RID: " + candidate);
            }
          
            //processManager.OpenUrlInBrowser("duckduckgo.com");
            
            if (OperatingSystem.IsWindows())
            {
                Console.WriteLine("Windows Version Enum: " + WindowsAnalyzer.GetWindowsVersionToEnum());
                
               // var caption = windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem");
               // var name = windowsAnalyzer.GetWMIValue("Name", "Win32_OperatingSystem");
                
                //Console.WriteLine("Caption: " + caption);
                //Console.WriteLine("Name: " + name);
            
           //     Console.WriteLine("OS :" + windowsAnalyzer.GetWMIClass("Win32_OperatingSystem"));
                
                //Console.WriteLine("Caption: " + windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem"));

               // Console.WriteLine(processManager.RunPowerShellCommand("systeminfo"));
               Console.WriteLine("WindowsEdition: " + WindowsAnalyzer.GetWindowsEdition().ToString());
               
               Console.WriteLine("WinOS " +  WMISearcher.GetWMIClass("Win32_OperatingSystem"));

              WindowsSystemInformationModel sysinfo = WindowsAnalyzer.GetWindowsSystemInformation();
              Console.WriteLine(sysinfo.ToString());
            }

            if (OperatingSystem.IsMacOS())
            {
                Console.WriteLine("Is this AppleSilicon?: " + MacOsAnalyzer.IsAppleSiliconMac());
                Console.WriteLine("Mac Processor Type: " + MacOsAnalyzer.GetMacProcessorType());

                Console.WriteLine("macOS Version Enum: " +
                                  MacOsAnalyzer.GetMacOsVersionToEnum());
                
                Console.WriteLine("macOS Version Detected: " + MacOsAnalyzer.GetMacOsVersion().ToString());
                
                Console.WriteLine("Darwin Version: " + MacOsAnalyzer.GetDarwinVersion());
                Console.WriteLine("Xnu Version: " + MacOsAnalyzer.GetXnuVersion());
            }

            UrlRunner.OpenUrlInDefaultBrowser("duckduckgo.com");

            // Console.WriteLine("OsVersion: " + versionAnalyzer.DetectOSVersion());
    
            if (OperatingSystem.IsLinux())
            {
                LinuxOsReleaseModel osRelease = LinuxOsReleaseRetriever.GetLinuxOsRelease();
                
                Console.WriteLine("Linux Distro Name: " + osRelease.Name);
                Console.WriteLine("Linux Distro Version: " + LinuxAnalyzer.GetLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + LinuxAnalyzer.GetLinuxKernelVersion());
                
                Console.WriteLine("Is an LTS release? " + osRelease.IsLongTermSupportRelease);
            }

Console.WriteLine("PlatformKit Version: " + PlatformKitIdentification.GetPlatformKitVersion().ToString());

            Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);
            
            Console.WriteLine(RuntimeInformation.RuntimeIdentifier);
            Console.WriteLine(RuntimeInformation.FrameworkDescription);