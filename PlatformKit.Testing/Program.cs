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

using AluminiumTech.DevKit.PlatformKit;

using System;
using System.Diagnostics;

namespace PlatformKit.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            PlatformManager platform = new PlatformManager();
            OSVersionAnalyzer versionAnalyzer = new OSVersionAnalyzer();
            
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            
            Console.WriteLine("Desc: " +  System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Console.WriteLine("TFM: " + System.Runtime.InteropServices.RuntimeInformation.RuntimeIdentifier);
            Console.WriteLine("Generated TFM (Generic): " + platform.GenerateTFM(true));
            Console.WriteLine("Generated TFM (Specific): " + platform.GenerateTFM(false));
            Console.WriteLine("Generated TFM (Specific) using Cheats: " + platform.GenerateTFM(false, true));
            Console.WriteLine("OsVersion: " + Environment.OSVersion);
            Console.WriteLine("Framework: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

            if (platform.IsLinux())
            {
                Console.WriteLine("Linux Kernel Version: " + versionAnalyzer.DetectLinuxKernelVersion().ToString());
            }
            
            //  processManager.SuspendProcess("Spotify");

            //  Console.WriteLine("Start suspending...");

            // PerformanceCounter

            while (stopwatch.ElapsedMilliseconds < (1 * 1000))
            {

            } 
            //   processManager.ResumeProcess("Spotify");

            //  Console.WriteLine("Start resuming...");

            Console.ReadLine();
        }
    }
}