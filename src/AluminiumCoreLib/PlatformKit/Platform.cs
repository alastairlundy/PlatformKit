/*
MIT License

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
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Diagnostics;
using System.IO;

namespace AluminiumCoreLib.PlatformKit {
    /// <summary>
    /// A class that helps get system information.
    /// </summary>
    public class Platform {
        /// <summary>
        /// Returns the OS Architecture as a string.
        /// </summary>
        /// <returns></returns>
        private CPUArchitecture GetOSArchitectureAsEnum(){
            var x = GetPlatformAsString().ToLower();

            try{
                if (x.Contains("arm") && !x.Contains("64"))
                {
                    return CPUArchitecture.ARM;
                }
                else if (x.Contains("x86") && !x.Contains("-64"))
                {
                    return CPUArchitecture.X86;
                }
                else if (x.Contains("arm") && x.Contains("64"))
                {
                    return CPUArchitecture.ARM64;
                }
                else if ((x.Contains("x64")) || (x.Contains("x86") && x.Contains("-64")))
                {
                    return CPUArchitecture.X64;
                }

                return CPUArchitecture.NotDetected;
            }
            catch(Exception ex){
                throw new Exception(ex.ToString());
            }
        }
        /// <summary>
        /// Returns the OS Architecture as a string.
        /// </summary>
        /// <returns></returns>
        public string GetOSArchitectureToString(){
            return RuntimeInformation.OSArchitecture.ToString();
        }
        /// <summary>
        /// Determine what OS is being run
        /// </summary>
        /// <returns></returns>
        public OSPlatform GetOSPlatform() {
            OSPlatform osPlatform = OSPlatform.Create("Other Platform");
            // Check if it's windows
            bool isWindows = RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
            osPlatform = isWindows ? OSPlatform.Windows : osPlatform;
            // Check if it's osx
            bool isOSX = RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
            osPlatform = isOSX ? OSPlatform.OSX : osPlatform;
            // Check if it's Linux
            bool isLinux = RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
            osPlatform = isLinux ? OSPlatform.Linux : osPlatform;
            return osPlatform;
        }



        /// <summary>
        /// Gets the OS platform as a String.
        /// </summary>
        public string GetPlatformAsString() {
            if (GetOSPlatform().ToString().ToLower().Equals("windows")){
                if (RuntimeInformation.OSDescription.Contains("Windows 10")){
                    return "Windows 10";
                }
                else if (RuntimeInformation.OSDescription.Contains("Windows 7")){
                    return "Windows 7";
                }
                else if (RuntimeInformation.OSDescription.Contains("Windows 8.1")) {
                    return "Windows 8.1";
                }
            }
            else if (GetOSPlatform().ToString().ToLower().Equals("mac")){
                if (RuntimeInformation.OSDescription.Contains("OSX")){
                    return "macOS";
                }
            }
            else if (GetOSPlatform().ToString().ToLower().Equals("linux")){
                return "Linux";
            }
            return null;
        }
        /// <summary>
        /// Returns the OS Platform as an Enum
        /// </summary>
        /// <returns></returns>
        public OperatingSystem GetPlatformAsEnum(){
            if (GetOSPlatform().ToString().ToLower().Equals("windows")){
                if (RuntimeInformation.OSDescription.Contains("Windows 10") || RuntimeInformation.OSDescription.Contains("Windows 7") || RuntimeInformation.OSDescription.Contains("Windows 8.1")){
                        return OperatingSystem.Windows;
                }
            }
            else if (GetOSPlatform().ToString().ToLower().Equals("mac")){
                if (RuntimeInformation.OSDescription.Contains("OSX")){
                        return OperatingSystem.Mac;
                }
            }
            else if (GetOSPlatform().ToString().ToLower().Equals("linux")){
                    return OperatingSystem.Linux;
            }
            else if (GetOSPlatform().ToString().ToLower().Equals("unix")){
                return OperatingSystem.Unix;
            }
            return OperatingSystem.NotDetected;
        }

        /// <summary>
        /// Return an app's version as a string.
        /// </summary>
        /// <returns></returns>
        public string GetVersionString() {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        public System.Version GetVersion() {
            return Assembly.GetEntryAssembly().GetName().Version;
        }

        /// <summary>
        /// Display license information in the Console from a Text File
        /// </summary>
        public void ShowLicenseInConsole(string PathToTextFile, int durationMilliSeconds){
            Stopwatch licenseWatch = new Stopwatch();
            licenseWatch.Reset();
            licenseWatch.Start();
            string[] lines;

            try{
                lines = File.ReadAllLines(PathToTextFile);

                foreach (string line in lines){
                    Console.WriteLine(line);
                }

                Console.WriteLine("                                                         ");
                Console.WriteLine("                                                         ");
                Console.ForegroundColor = ConsoleColor.Gray;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Here are some details in case you need them:");
                Console.WriteLine(ex.ToString());
            }

            while (licenseWatch.ElapsedMilliseconds <= durationMilliSeconds){
            //Do nothing to make sure everybody sees the license.
            }
          }
  
    }
        /// <summary>
        /// An enum to help manage OS specific code.
        /// </summary>
        public enum OperatingSystem {
            Windows, Mac, Linux, Unix, Android, IOS, tvOS, watchOS, AndroidTV, wearOS, WindowsHoloGraphic, WindowsMixedReality, Tizen, NotDetected
        }
        public enum CPUArchitecture{
            X64, X86, ARM, ARM64, NotDetected
        }
}