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

using AluminiumTech.DevKit.PlatformKit;
using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Runtime;

            var platformManager = new PlatformManager();
            var processManager = new ProcessManager();
            var versionAnalyzer = new OSVersionAnalyzer();
            var runtimeIdentification = new RuntimeIdentification();

            Console.WriteLine(".NET Detected RuntimeID: " + RuntimeInformation.RuntimeIdentifier);

            var title = $"{platformManager.GetAppName()} v{platformManager.GetAppVersion()}";
            Console.Title = title;
            Console.WriteLine(title) ;
            
            Console.WriteLine("Programmatically Generated RuntimeID (AnyGeneric): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.AnyGeneric));
            Console.WriteLine("Programmatically Generated RuntimeID (Generic): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic));
            Console.WriteLine("Programmatically Generated RuntimeID (Specific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
            Console.WriteLine("Programmatically Generated RuntimeID (DistroSpecific): " + runtimeIdentification.GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));
            
            if (platformManager.IsWindows())
            {
                Console.WriteLine("Windows Version Enum: " + versionAnalyzer.GetWindowsVersionToEnum());
                
            //    Console.WriteLine(processManager.RunPowerShellCommand("ping www.duckduckgo.com"));
            }

            if (platformManager.IsMac())
            {
                Console.WriteLine("Is this AppleSilicon?: " + platformManager.IsAppleSiliconMac());
                Console.WriteLine("Mac Processor Type: " + platformManager.GetMacProcessorType());
            }
            
            Console.WriteLine("OsVersion: " + versionAnalyzer.DetectOSVersion());
            
            if (platformManager.IsLinux())
            {
                Console.WriteLine("Linux Distro Version: " + versionAnalyzer.DetectLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + versionAnalyzer.DetectLinuxKernelVersion());
            }