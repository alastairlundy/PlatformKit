/* MIT License

Copyright (c) 2018-2020 AluminiumTech

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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

using AluminiumTech.PlatformKit.enums;

namespace AluminiumTech.PlatformKit{
    /// <summary>
    /// A class that helps get system information.
    /// </summary>
    public class Platform {

        /// <summary>
        /// Returns the OS Architecture To Enum
        /// </summary>
        /// <returns></returns>
        private CPUArchitectureFamily GetOsArchitectureFamilyToEnum() {
            var osArchitecture = RuntimeInformation.OSArchitecture;

            try {
                if (osArchitecture.Equals(Architecture.Arm))
                {
                    return CPUArchitectureFamily.ARM32;
                }
                else if (osArchitecture.Equals(Architecture.Arm64))
                {
                    return CPUArchitectureFamily.ARM64;
                }
                else if (osArchitecture.Equals(Architecture.X86))
                {
                    return CPUArchitectureFamily.X86;
                }
                else if (osArchitecture.Equals(Architecture.X64))
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
        /// Returns the OS Architecture as a string.
        /// </summary>
        /// <returns></returns>
        public string GetOsArchitectureToString() {
            return RuntimeInformation.OSArchitecture.ToString();
        }

        /// <summary>
        /// Determine what OS is being run
        /// </summary>
        /// <returns></returns>
        public OSPlatform GetOsPlatform() {
            OSPlatform osPlatform = OSPlatform.Create("Other Platform");
            // Check if it's windows
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            osPlatform = isWindows ? OSPlatform.Windows : osPlatform;
            // Check if it's osx
            bool isOsx = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            osPlatform = isOsx ? OSPlatform.OSX : osPlatform;
            // Check if it's Linux
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            osPlatform = isLinux ? OSPlatform.Linux : osPlatform;
            
            bool isBsd = RuntimeInformation.IsOSPlatform(OSPlatform.Create("BSD"));
            osPlatform = isBsd ? OSPlatform.Create("BSD") : osPlatform; 
                
            return osPlatform;
        }

        /// <summary>
        /// Returns the OS platform as a String.
        /// </summary>
        /// <returns></returns>
        public override string ToString() {
            if (GetOsPlatform().ToString().ToLower().Equals("windows")) {
                return "Windows";
            }
            else if (GetOsPlatform().ToString().ToLower().Equals("osx") ||
                     GetOsPlatform().ToString().ToLower().Equals("mac") ||
                     GetOsPlatform().ToString().ToLower().Equals("macos") ||
                     GetOsPlatform().ToString().ToLower().Equals("darwin")) 
            {
                if (RuntimeInformation.OSDescription.ToLower().Contains("osx") ||
                    RuntimeInformation.OSDescription.ToLower().Contains("darwin")) {
                    return "macOS";
                }
            }
            else if (GetOsPlatform().ToString().ToLower().Equals("linux")) {
                return "Linux";
            }
            else if (GetOsPlatform().ToString().ToLower().Contains("bsd"))
            {
                return "BSD";
            }
            return null;
        }

        /// <summary>
        /// Returns whether or not the current OS is 64 Bit.
        /// </summary>
        /// <returns></returns>
        public bool Is64BitOs() {
            return Environment.Is64BitOperatingSystem;
        }

        /// <summary>
        /// Returns whether or not the App currently running is 64 Bit or not.
        /// </summary>
        /// <returns></returns>
        public bool Is64BitApp() {
            return Environment.Is64BitProcess;
        }

        /// <summary>
        /// Returns the OS Family as an Enum
        /// </summary>
        /// <returns></returns>
        public OperatingSystemFamily ToEnum() {
            if (ToString().ToLower().Equals("windows")) {
                return OperatingSystemFamily.Windows;
            }
            else if (ToString().ToLower().Equals("mac") ||
                     ToString().ToLower().Equals("osx") ||
                    ToString().ToLower().Equals("darwin")
                     ) {
                return OperatingSystemFamily.macOS;
            }
            else if (ToString().ToLower().Equals("linux")) {
                return OperatingSystemFamily.Linux;
            }
            else if (ToString().ToLower().Equals("unix")) {
                return OperatingSystemFamily.Unix;
            }
            else if (ToString().ToLower().Contains("bsd"))
            {
                return OperatingSystemFamily.BSD;
            }
            return OperatingSystemFamily.NotDetected;
        }

        /// <summary>
        /// Return an app's version as a string.
        /// </summary>
        /// <returns></returns>
        public string GetAppVersionToString() {
            return Assembly.GetEntryAssembly()?.GetName().Version.ToString();
        }

        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        public System.Version GetAppVersionToVersion() {
            return Assembly.GetEntryAssembly()?.GetName().Version;
        }

        /// <summary>
        /// Display license information in the Console from a Text File
        /// </summary>
        public void ShowLicenseInConsole(string pathToTextFile, int durationMilliSeconds) {
            Stopwatch licenseWatch = new Stopwatch();
            licenseWatch.Reset();
            licenseWatch.Start();
            string[] lines;

            try {
                lines = File.ReadAllLines(pathToTextFile);

                foreach (string line in lines) {
                    Console.WriteLine(line);
                }

                Console.WriteLine("                                                         ");
                Console.WriteLine("                                                         ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception ex){
                Console.WriteLine("Here are some details in case you need them:");
                Console.WriteLine(ex.ToString());
            }

            while (licenseWatch.ElapsedMilliseconds <= durationMilliSeconds) {
                //Do nothing to make sure everybody sees the license.
            }
        }

        /// <summary>
        /// Get's the OS's Version and returns it as a string.
        /// </summary>
        /// <returns></returns>
        public string GetOsVersionToString(){       
            if (ToEnum().Equals(OperatingSystemFamily.Windows)){
                return GetWindowsVersionToString();
            }
            else if (ToEnum().Equals(OperatingSystemFamily.macOS)){
                return GetmacOSKernelVersionToString();
            }
            else if (ToEnum().Equals(OperatingSystemFamily.Linux)){
                return GetLinuxKernelVersionToString();
            }
            else{
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Get the Windows Version returned as a string.
        /// </summary>
        /// <returns></returns>
        public string GetWindowsVersionToString() {
            string windowsKernel = RuntimeInformation.OSDescription;
            string[] words = windowsKernel.Split();

            foreach(string word in words) {
                if (word.ToLower().Contains("10.0") || word.ToLower().Contains("9.") || word.ToLower().Contains("8.")){
                    //Do not replace if it contains the Windows 10 build and version info.
                    return word;
                }
                else{
                    windowsKernel = windowsKernel.Replace(word, " ");
                }
            }
            return windowsKernel;
        }

        /// <summary>
        /// Get the macOS Version returned as a string.
        /// </summary>
        /// <returns></returns>
        public string GetmacOSKernelVersionToString(){
            string macOS = RuntimeInformation.OSDescription ?? throw new ArgumentNullException("RuntimeInformation.OSDescription");
            
           macOS = macOS.Replace("Darwin", " ");
           macOS = macOS.Replace("Kernel Version", " ");
           // macOSKernel.Replace("", "");
           macOS = macOS.Replace("root:xnu", "xnu");
           macOS = macOS.Replace("/RELEASE_X86_64", " ");

            string x = macOS;

            Console.WriteLine("New kernel string: " + x);
            return macOS;
        }

        /// <summary>
        /// Get the Linux Version returned as a string.
        /// </summary>
        /// <returns></returns>
        public string GetLinuxKernelVersionToString(){
            string linuxKernel = RuntimeInformation.OSDescription;
            string[] words = linuxKernel.Split();

            foreach(string word in words){
                if (word.ToLower().Contains("2.") || word.ToLower().Contains("3.") || word.ToLower().Contains("4.") || word.ToLower().Contains("5.")){
                    //Do not replace if it contains the Linux kernel version.
                }
                else{
                    linuxKernel = linuxKernel.Replace(word, " ");
                }         
            }
            return linuxKernel;
        }

        /// <summary>
        /// Gets the OS's Kernel Version and returns it as a System.Version object.
        /// </summary>
        /// <returns></returns>
        public Version GetOsKernelVersionToVersion(){
            if (ToEnum().Equals(OperatingSystemFamily.Windows)){
                return GetWindowsKernelVersionToVersion();
            }
            else if (ToEnum().Equals(OperatingSystemFamily.macOS)){
                return GetmacOSKernelVersionToVersion();
            }
            else if (ToEnum().Equals(OperatingSystemFamily.Linux)){
                return GetLinuxKernelVersionToVersion();
            }
            else{
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Get the Windows Version as a System.Version object.
        /// </summary>
        /// <returns></returns>
        public Version GetWindowsKernelVersionToVersion(){
            return new System.Version(GetWindowsVersionToString());
        }

        /// <summary>
        /// Get the macOS Kernel (XNU) Version as a System.Version object.
        /// </summary>
        /// <returns></returns>
        public Version GetmacOSKernelVersionToVersion(){
            return new System.Version(GetmacOSKernelVersionToString());
        }

        /// <summary>
        /// Get the Linux Kernel Version as a System.Version object.
        /// </summary>
        /// <returns></returns>
        public Version GetLinuxKernelVersionToVersion(){
            return new System.Version(GetLinuxKernelVersionToString());
        }
    }
}