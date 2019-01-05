/*
MIT License

Copyright (c) 2018-2019 AluminiumTech

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

namespace AluminiumCoreLib.Utilities {
    /// <summary>
    /// A class that helps get system information.
    /// </summary>
    public class Platform {
        /// <summary>
        /// Returns the OS Architecture as a string.
        /// </summary>
        /// <returns></returns>
        public string ReturnOSArchitecture() {
            return RuntimeInformation.OSArchitecture.ToString();
        }
        /// <summary>
        /// Returns the OS Architecture as a string.
        /// </summary>
        /// <returns></returns>
        public CPUArchitecture ReturnOSArchitectureEnum(){
            var x = ReturnOSArchitecture().ToLower();

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
        /// Determine what OS is being run
        /// /// </summary>
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
        /// Print all the processes running to the Console.
        /// </summary>
        public void ListAllProcesses(){
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes){
                //Get whatever attribute for process
                Console.WriteLine("Process " + process.Id + ": " + process.ProcessName);
            }
        }

        /// <summary>
        /// Warn the user if the process count is extremely high.
        /// </summary>
        public void WarnProcessCount(int ProcessWarnCount, string WarningMessage){
            if(ReturnProcessCount() > ProcessWarnCount) {
              Console.WriteLine(WarningMessage);
            }
         }

        /// <summary>
        /// Check to see if a process is running or not.
        /// </summary>
        public bool IsProcessRunning(string ProcessName){
              Process[] processes = Process.GetProcesses();

            foreach (Process process in processes){
               if(process.Equals(ProcessName)){
                   return true;
               }
            }
            return false;
        }

        /// <summary>
        /// Gets the OS platform as a String.
        /// </summary>
        public string ReturnPlatform() {
            if (GetOSPlatform().ToString().ToLower() == "windows"){
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
            else if (GetOSPlatform().ToString().ToLower() == "mac"){
                if (RuntimeInformation.OSDescription.Contains("OSX")){
                    return "macOS";
                }
            }
            else if (GetOSPlatform().ToString().ToLower() == "linux"){
                return "Linux";
            }
            return null;
        }

        public OperatingSystem ReturnPlatformAsOSEnum(){
            if (GetOSPlatform().ToString().ToLower() == "windows"){
                if (RuntimeInformation.OSDescription.Contains("Windows 10")){
                        return OperatingSystem.Windows;
                }
                else if (RuntimeInformation.OSDescription.Contains("Windows 7")){
                        return OperatingSystem.Windows;
                    }
                else if (RuntimeInformation.OSDescription.Contains("Windows 8.1")){
                        return OperatingSystem.Windows;
                    }
            }
            else if (GetOSPlatform().ToString().ToLower() == "mac"){
                if (RuntimeInformation.OSDescription.Contains("OSX")){
                        return OperatingSystem.Mac;
                }
            }
            else if (GetOSPlatform().ToString().ToLower() == "linux"){
                    return OperatingSystem.Linux;
            }
               return OperatingSystem.Unix;
        }

        /// <summary>
        /// Return an app's version as a string.
        /// </summary>
        /// <returns></returns>
        public string ReturnVersionString() {
            return Assembly.GetEntryAssembly().GetName().Version.ToString();
        }
        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        public System.Version ReturnVersion() {
            return Assembly.GetEntryAssembly().GetName().Version;
        }
        /// <summary>
        /// Check how many processes are currently running.
        /// </summary>
        /// <returns></returns>
        public int ReturnProcessCount() {
            int count = 0;
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes) {
                //Get whatever attribute for process
                count++;
            }
            return count;
        }
        /// <summary>
        /// Get the list of processes as an ObjectList containing Strings
        /// </summary>
        /// <returns></returns>
        public List<string> ReturnProcessListAsString() {
            var strList = new List<string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes) {
                strList.Add(process.ToString());
            }
            strList.TrimExcess();
            return strList;
        }
        /// <summary>
        /// Returns an ObjectList containing all the running processes.
        /// </summary>
        /// <returns></returns>
        public List<Process> ProcessListToObjectList(){
                var strList = new List<Process>();
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes) {
                    strList.Add(process);
                }
                strList.TrimExcess();
                return strList;
            }

        /// <summary>
        /// Returns a HashMap containing all the running processes.
        /// <typeparam name="int"> The Process ID.</typeparam>
        /// <typeparam name="string"> The Process Name.</typeparam>
        /// </summary>
        /// <returns></returns>
        public HashMap<int, string> ProcessListToHashMap(){
                var hm = new HashMap<int, string>();
                Process[] processes = Process.GetProcesses();

                foreach (Process process in processes) {
                  hm.Put(process.Id, process.ProcessName);
                }
                return hm;
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

        /// <summary>
        /// Run a Process
        /// </summary>
        /// <param name="ProcessName"></param>
        public void RunProcess(string ProcessName){
            var plat = new Platform().ReturnPlatform();

            if (plat.ToLower().Contains("win")){
                RunProcessWindows(ProcessName);
            }
            else if (plat.ToLower().Contains("osx")){
                RunProcessMac(ProcessName);
            }
           else if (plat.ToLower().Contains("linux")){
                  RunProcessLinux(ProcessName);
              //  throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Run a Process
        /// </summary>
        /// <param name="ProcessName"></param>
        public void RunProcess(string ProcessName, string Arguments){
            var plat = new Platform().ReturnPlatform();

            if (plat.ToLower().Contains("win")){
                RunProcessWindows(ProcessName, Arguments);
            }
            else if (plat.ToLower().Contains("osx")){
                RunProcessMac(ProcessName, Arguments);
            }
            else if (plat.ToLower().Contains("linux")){
                 RunProcessLinux(ProcessName, Arguments);
            }
        }
        /// <summary>
        /// Run a process on Windows.
        /// </summary>
        /// <param name="ProcessName"></param>
        private void RunProcessWindows(string ProcessName){
            Process process = new Process();
            process.StartInfo.FileName = ProcessName + ".exe";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
        }
        private void RunProcessWindows(string ProcessName, string Arguments){
            Process process = new Process();
            process.StartInfo.FileName = ProcessName + ".exe";
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
        }
        private void RunProcessWindows(string ProcessName, string Arguments, ProcessWindowStyle pws){
            Process process = new Process();
            process.StartInfo.FileName = ProcessName + ".exe";
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.WindowStyle = pws;
            process.Start();
        }
        /// <summary>
        /// Run a Process on Mac
        /// </summary>
        /// <param name="ProcessName"></param>
        /// Courtesy of https://github.com/fontanaricardo/RunCommand
        private void RunProcessMac(string ProcessName){
            var procStartInfo = new ProcessStartInfo(ProcessName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
        }
        /// <summary>
        /// Run a Process on Mac
        /// </summary>
        /// <param name="ProcessName"></param>
        /// Courtesy of https://github.com/fontanaricardo/RunCommand
        private void RunProcessMac(string ProcessName, string ProcessArguments){
            var procStartInfo = new ProcessStartInfo(ProcessName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = ProcessArguments
            };

            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
        }
        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="ProcessName"></param>
        private void RunProcessLinux(string ProcessName){
            var procStartInfo = new ProcessStartInfo(ProcessName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
            };

            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
        }
        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="ProcessName"></param>
        private void RunProcessLinux(string ProcessName, string ProcessArguments){
            var procStartInfo = new ProcessStartInfo(ProcessName)
            {
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = ProcessArguments
            };

            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
        }
        /// <summary>
        /// Open a URL in the default browser.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        public bool OpenURLInBrowser(string url){
            var plat = new Platform().ReturnPlatform();
            if (plat.ToLower().Contains("win")){
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });
                return true;
            }
            if (plat.ToLower().Contains("linux")){
                Process.Start("xdg-open", url);
                return true;
            }
            if (plat.ToLower().Contains("osx")){
                Process.Start("open", url);
                return true;
            }
            return false;
        }

    }
            /// <summary>
        /// An enum to help manage OS specific code.
        /// </summary>
        public enum OperatingSystem {
            Windows, Mac, Linux, Unix, Android, IOS, tvOS, watchOS, AndroidTV, wearOS, WindowsHoloGraphic, WindowsMixedReality, Tizen
        }
        /// <summary>
        /// An enum to help manage CPU architecture specific code.
        /// </summary>
        public enum CPUArchitecture {
            X86, X64, ARM, ARM64, POWERPC, IA64, MIPS, NotDetected
        }
}