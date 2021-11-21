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