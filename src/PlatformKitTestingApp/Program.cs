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

            var platform = new PlatformManager();
            var versionAnalyzer = new OSVersionAnalyzer();
            var runtimeIdentification = new RuntimeIdentification();

            Console.WriteLine(".NET Detected RuntimeID: " + RuntimeInformation.RuntimeIdentifier);
            
            Console.WriteLine("Programmatically Generated RuntimeID (Generic): " + runtimeIdentification.GenerateGenericRuntimeIdentifier());
            Console.WriteLine("Programmatically Generated RuntimeID (Specific): " + runtimeIdentification.GenerateSpecificRuntimeIdentifier());
            
            if (platform.IsWindows())
            {
                Console.WriteLine("Windows Version Enum: " + versionAnalyzer.GetWindowsVersionToEnum());
                
                Console.WriteLine(Environment.SystemDirectory);
            }
            
            Console.WriteLine("OsVersion: " + versionAnalyzer.DetectOSVersion());
            if (platform.IsLinux())
            {
                Console.WriteLine("Linux Distro Version: " + versionAnalyzer.DetectLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + versionAnalyzer.DetectLinuxKernelVersion());
            }

            //  Console.WriteLine("Start resuming...");

            //Console.ReadLine();