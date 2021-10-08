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

using AluminiumTech.DevKit.PlatformKit.consts;

using HardwareKit.Components.Base.enums;
using HardwareKit.Components.Base.enums.Architecture;

namespace AluminiumTech.DevKit.PlatformKit{
    /// <summary>
    /// A class that helps get system information.
    /// </summary>
    public class Platform {

        public Platform()
        {
            
        }
        
        /// <summary>
        /// Returns the OS Architecture To Enum
        /// </summary>
        /// <returns></returns>
        public ProcessorArchitectureFamily ToProcessorArchitectureFamily() {
            var osArchitecture = System.Runtime.InteropServices.RuntimeInformation.OSArchitecture;

            try {
                if (osArchitecture == (System.Runtime.InteropServices.Architecture.Arm))
                {
                    return ProcessorArchitectureFamily.ARM32;
                }
                else if (osArchitecture == (System.Runtime.InteropServices.Architecture.Arm64))
                {
                    return ProcessorArchitectureFamily.ARM64;
                }
                else if (osArchitecture == (System.Runtime.InteropServices.Architecture.X86))
                {
                    return ProcessorArchitectureFamily.X86;
                }
                else if (osArchitecture == (System.Runtime.InteropServices.Architecture.X64))
                {
                    return ProcessorArchitectureFamily.X64;
                }

                return ProcessorArchitectureFamily.NotDetected;
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
            // Check if it's FreeBSD
            //bool isFreeBsd = System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD);
            //osPlatform = isFreeBsd ? System.Runtime.InteropServices.OSPlatform.FreeBSD : osPlatform;
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
                ToString().ToLower().Contains("linux")) {
                return OperatingSystemFamily.Linux;
            }
            /* Enable once we're done supporting .NET Standard 2.0
             
             else if (GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.FreeBSD))
            {
                return OperatingSystemFamily.BSD;
            }
            */
            
            return OperatingSystemFamily.NotDetected;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public string GetAppName()
        {
            return GetAssembly()?.GetName().Name;
        }

        /// <summary>
        /// Return's the executing app's assembly.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public Assembly GetAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// Return an app's version as a string.
        /// </summary>
        /// <returns></returns>
        [Obsolete(DeprecationMessages.DeprecationV2RC2 + ". Use GetAppVersion().ToString(); instead")]
        public string GetAppVersionToString() {
            return GetAssembly()?.GetName()?.Version?.ToString();
        }

        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public Version GetAppVersion() {
            return Assembly.GetExecutingAssembly().GetName().Version;
        }

        /// <summary>
        /// Display license information in the Console from a Text File
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void ShowLicenseInConsole(string pathToTextFile, int durationMilliSeconds) {
            try {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                Stopwatch licenseWatch = new Stopwatch();
                
                var lines = File.ReadAllLines(pathToTextFile);

                foreach (string line in lines) {
                    Console.WriteLine(line);
                }

                Console.WriteLine("                                                         ");
                Console.WriteLine("                                                         ");
                
                licenseWatch.Start();
            
                while (licenseWatch.ElapsedMilliseconds <= durationMilliSeconds) {
                    //Do nothing to make sure everybody sees the license.
                }

                licenseWatch.Stop();
                licenseWatch.Reset();
            }
            catch (Exception exception){
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // ReSharper disable once RedundantAssignment
            string appName = "";

            try
            {
                appName = GetAppName();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
            
            var appVersion = GetAppVersion().ToString();

            // ReSharper disable once IdentifierTypo
            string bitness = Environment.Is64BitProcess ? "64 Bit" : "32 Bit";

            string app = "";
            
            if (appName?.Length > 0)
            {
                // ReSharper disable once HeapView.ObjectAllocation
                app = "App " + appName + " v" + appVersion + " " + bitness;
            }

            //Ensure compatibility with .NET Core 3.1 and .NET Standard 2.0
            //Re-introduce RunTimeID usage when we switch to targeting .NET 5
            // ReSharper disable once HeapView.ObjectAllocation
            var runtime = System.Runtime.InteropServices.RuntimeInformation.OSDescription + " on " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription;

            // ReSharper disable once HeapView.ObjectAllocation
            var running = " running on RunTime: " + runtime;

            // ReSharper disable once HeapView.ObjectAllocation
            return app + running;
        }
    }
}