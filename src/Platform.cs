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

using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;

using HardwareKit.Components.Base.enums;
using HardwareKit.Components.Base.enums.Architecture;

namespace AluminiumTech.DevKit.PlatformKit{
    /// <summary>
    /// A class that helps get system information.
    /// </summary>
    public class Platform {

        /// <summary>
        /// Returns the OS Architecture To Enum
        /// </summary>
        /// <returns></returns>
        public CPUArchitectureFamily ToCPUArchitectureFamily() {
            var osArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture;

            try {
                if (osArchitecture.Equals(System.Runtime.InteropServices.Architecture.Arm))
                {
                    return CPUArchitectureFamily.ARM32;
                }
                else if (osArchitecture.Equals(System.Runtime.InteropServices.Architecture.Arm64))
                {
                    return CPUArchitectureFamily.ARM64;
                }
                else if (osArchitecture.Equals(System.Runtime.InteropServices.Architecture.X86))
                {
                    return CPUArchitectureFamily.i386;
                }
                else if (osArchitecture.Equals(System.Runtime.InteropServices.Architecture.X64))
                {
                    return CPUArchitectureFamily.X64;
                }

                return CPUArchitectureFamily.NotDetected;
            }
            catch (Exception ex) {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Determine what OS is being run
        /// Can only detect Windows, Mac, or Linux.
        /// </summary>
        /// <returns></returns>
        public System.Runtime.InteropServices.OSPlatform GetOSPlatform() {
            System.Runtime.InteropServices.OSPlatform osPlatform = System.Runtime.InteropServices.OSPlatform.Create("Other Platform");
            // Check if it's windows
            bool isWindows = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
            osPlatform = isWindows ? System.Runtime.InteropServices.OSPlatform.Windows : osPlatform;
            // Check if it's osx
            bool isMac = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
            osPlatform = isMac ? System.Runtime.InteropServices.OSPlatform.OSX : osPlatform;
            // Check if it's Linux
            bool isLinux = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
            osPlatform = isLinux ? System.Runtime.InteropServices.OSPlatform.Linux : osPlatform;

            return osPlatform;
        }

        /// <summary>
        /// Returns the OS Family as an Enum
        /// </summary>
        /// <returns></returns>
        public OperatingSystemFamily ToOperatingSystemFamily() {
            if (GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Windows)) {
                return OperatingSystemFamily.Windows;
            }
            else if (GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.OSX)) {
                return OperatingSystemFamily.macOS;
            }
            else if (GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Linux) ||
                ToString().ToLower().Equals("linux")) {
                return OperatingSystemFamily.Linux;
            }
            return OperatingSystemFamily.NotDetected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GetAppName()
        {
            return GetAssembly()?.GetName().Name;
        }

        /// <summary>
        /// Return's the executing app's assembly.
        /// </summary>
        /// <returns></returns>
        public Assembly GetAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// Return an app's version as a string.
        /// </summary>
        /// <returns></returns>
        public string GetAppVersionToString() {
            return GetAssembly()?.GetName().Version.ToString();
        }

        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        public System.Version GetAppVersionToVersion() {
            return Assembly.GetExecutingAssembly()?.GetName().Version;
        }

        /// <summary>
        /// Display license information in the Console from a Text File
        /// </summary>
        public void ShowLicenseInConsole(string pathToTextFile, int durationMilliSeconds) {
            Stopwatch licenseWatch = new Stopwatch();

            try {
                var lines = File.ReadAllLines(pathToTextFile);

                foreach (string line in lines) {
                    Console.WriteLine(line);
                }

                Console.WriteLine("                                                         ");
                Console.WriteLine("                                                         ");
            }
            catch (Exception ex){
                Console.WriteLine("Here are some details in case you need them:");
                Console.WriteLine(ex.ToString());
            }

            licenseWatch.Start();
            
            while (licenseWatch.ElapsedMilliseconds <= durationMilliSeconds) {
                //Do nothing to make sure everybody sees the license.
            }

            licenseWatch.Stop();
            licenseWatch.Reset();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string appName = "";

            try
            {
                appName = Assembly.GetEntryAssembly()?.GetName().ToString();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
            
            var appVersion = GetAppVersionToString();

            string bitness = Environment.Is64BitProcess == true ? "64 Bit" : "32 Bit";

            string app = "";
            
            if (appName.Length > 0)
            {
                app = "App " + appName + " v" + appVersion + " " + bitness;
            }

            //Ensure compatibility with .NET Core 3.1 and .NET Standard 2.0
            //Re-introduce RunTimeID usage when we switch to targetting .NET 5
            var runtime = System.Runtime.InteropServices.RuntimeInformation.OSDescription + " on " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

            var running = " running on RunTimeID: " + runtime;

            return app + running;
        }
    }
}