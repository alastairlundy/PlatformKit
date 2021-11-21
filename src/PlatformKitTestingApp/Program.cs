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
            var processManager = new ProcessManager();

            var versionAnalyzer = new OSVersionAnalyzer();

            var runtimeIdentification = new RuntimeIdentification();

//processManager.RunProcess("brave");
            //processManager.OpenUrlInBrowser("youtube.com");

            //processManager.OpenUrlInBrowser("google.com");
            //processManager.OpenUrlInBrowser("bing.com");
            //processManager.OpenUrlInBrowser("duckduckgo.com");

            Console.WriteLine("Desc: " + RuntimeInformation.OSDescription);
            Console.WriteLine("TFM: " + RuntimeInformation.RuntimeIdentifier);
            //Console.WriteLine("Generated TFM (Specific) using Cheats: " + platform.GenerateTFM(false, true));

            Console.WriteLine("Generated TFM (Generic): " + runtimeIdentification.GenerateGenericTFM());
            Console.WriteLine("Generated TFM (Specific): " + runtimeIdentification.GenerateSpecificTFM());
            //Console.WriteLine("Generated TFM (Generic): " + runtimeIdentification.GenerateGenericTFM());

            Console.WriteLine("Enum: " + versionAnalyzer.GetWindowsVersionToEnum());
            Console.WriteLine("OsVersion: " + versionAnalyzer.DetectWindowsVersion());
            // Console.WriteLine("Framework: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

            if (platform.IsLinux())
            {
                Console.WriteLine("Linux Distro Version: " + versionAnalyzer.DetectLinuxDistributionVersionAsString());
                Console.WriteLine("Linux Kernel Version: " + versionAnalyzer.DetectLinuxKernelVersion());
            }

            //  processManager.SuspendProcess("Spotify");

            //  Console.WriteLine("Start suspending...");

            // PerformanceCounter

            //   processManager.ResumeProcess("Spotify");

            //  Console.WriteLine("Start resuming...");

            //Console.ReadLine();