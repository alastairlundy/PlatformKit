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

// See https://aka.ms/new-console-template for more information

using System;
using System.Runtime.InteropServices;
using System.Text;
using AluminiumTech.DevKit.PlatformKit;
using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics.VersionAnalyzers;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Mac;
using AluminiumTech.DevKit.PlatformKit.Runtime;

            var platformManager = new PlatformIdentification();
            var osAnalyzer = new OSAnalyzer();
            var processManager = new ProcessManager();
            var versionAnalyzer = new OSVersionAnalyzer();
            var runtimeIdentification = new RuntimeIdentification();

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
            
            if (osAnalyzer.IsWindows())
            {
                Console.WriteLine("Windows Version Enum: " + versionAnalyzer.GetWindowsVersionToEnum());

             //   var res = processManager.RunPowerShellCommand("ping www.duckduckgo.com");
                
             //  Console.WriteLine(res);
            
                WindowsAnalyzer windowsAnalyzer = new WindowsAnalyzer();
               // var caption = windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem");
               // var name = windowsAnalyzer.GetWMIValue("Name", "Win32_OperatingSystem");
                
                //Console.WriteLine("Caption: " + caption);
                //Console.WriteLine("Name: " + name);
            
           //     Console.WriteLine("OS :" + windowsAnalyzer.GetWMIClass("Win32_OperatingSystem"));
                
                //Console.WriteLine("Caption: " + windowsAnalyzer.GetWMIValue("Caption", "Win32_OperatingSystem"));

               // Console.WriteLine(processManager.RunPowerShellCommand("systeminfo"));
               Console.WriteLine("WindowsEdition: " + windowsAnalyzer.DetectWindowsEdition().ToString());
               
               Console.WriteLine("WinOS " + windowsAnalyzer.GetWMIClass("Win32_OperatingSystem"));
               
               var desc = processManager.RunPowerShellCommand("systeminfo");

               List<string> strings = new List<string>();

               string currentString = "";

               char nextChar;
               char currentChar; 
               char previousChar;

               for (int charNumber = 0; charNumber < desc.Length; charNumber++)
               {
                   if (charNumber == 0)
                   {
                       previousChar = desc[0];
                       currentChar = desc[0];
                       nextChar = desc[1];
                   }
                   else
                   {
                       previousChar = desc[charNumber];
                       currentChar = desc[charNumber];

                       if (charNumber < desc.Length)
                       {
                           nextChar = desc[charNumber + 1];
                       }
                       else
                       {
                           nextChar = ' ';
                       }
                   }

                   if ((previousChar.Equals(' ') && currentChar.Equals(' ')))
                   {
                       if (currentString.Equals(String.Empty))
                       {
                           continue;
                       }
                       else
                       {
                           strings.Add(currentString);
                           currentString = "";
                       }
                       
                   }
                   else if (currentChar.Equals(' ') && !nextChar.Equals(' '))
                   {
                       strings.Add(currentString);
                       currentString = "";
                   }
                   else
                   {
                       currentString += desc[charNumber];
                   }
               }

               var arr = strings.ToArray();
                   //stringBuilder.ToString().Split(" ");

               for (int index = 0; index < arr.Length; index++)
               {
                    Console.WriteLine(index + " " + arr[index]);
               }
            }

            if (osAnalyzer.IsMac())
            {
                Console.WriteLine("Is this AppleSilicon?: " + osAnalyzer.IsAppleSiliconMac());
                Console.WriteLine("Mac Processor Type: " + osAnalyzer.GetMacProcessorType());

                Console.WriteLine("macOS Version Enum: " +
                                  MacOSVersionAnalyzer.DetectMacOsVersionEnum(new OSVersionAnalyzer()));
                
                Console.WriteLine("macOS Version Detected: " + new OSVersionAnalyzer().DetectOSVersion().ToString());
                
                Console.WriteLine("Darwin Version: " + MacOSVersionAnalyzer.DetectDarwinVersion());
                Console.WriteLine("Xnu Version: " + MacOSVersionAnalyzer.DetectXnuVersion());
            }
 
            // Console.WriteLine("OsVersion: " + versionAnalyzer.DetectOSVersion());
    
            if (osAnalyzer.IsLinux())
            {
                Console.WriteLine("Linux Distro Version: " + versionAnalyzer.DetectLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + versionAnalyzer.DetectLinuxKernelVersion());
            }
    
            Console.WriteLine("Current Directory: " + Environment.CurrentDirectory);