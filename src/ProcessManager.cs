/* MIT License

Copyright (c) 2019-2020 AluminiumTech

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
using System.Diagnostics;

using AluminiumTech.PlatformKit.enums;

namespace AluminiumTech.PlatformKit{
    /// <summary>
    /// A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager{

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
        public void WarnProcessCount(int processWarnCount, string warningMessage){
            if (GetProcessCount() > processWarnCount)
            {
                Console.WriteLine(warningMessage);
            }
        }

        /// <summary>
        /// Check to see if a process is running or not.
        /// </summary>
        public bool IsProcessRunning(string processName){
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if(process.ToString().Equals(processName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Run a Process
        /// </summary>
        /// <param name="processName"></param>
        public void RunProcess(string processName){
            RunProcess(processName, "");
        }

        /// <summary>
        /// Run a Process with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcess(string processName, string arguments){
            Platform platform = new Platform();
            var plat = platform.ToEnum();

            if (plat.Equals(OperatingSystemFamily.Windows))
            {
                RunProcessWindows(processName, arguments);
            }
            else if (plat.Equals(OperatingSystemFamily.macOS))
            {
                RunProcessMac(processName, arguments);
            }
            else if (plat.Equals(OperatingSystemFamily.Linux))
            {
                RunProcessLinux(processName, arguments);
            }
        }
        
        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcessWindows(string processName, string arguments = ""){
            RunProcessWindows(processName,ProcessWindowStyle.Normal, arguments);
        }

        /// <summary>
        /// Run a process on Windows with Arguments and a Process Window Style
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        /// <param name="pws"></param>
        public void RunProcessWindows(string processName, ProcessWindowStyle pws,  string arguments = ""){
            Process process = new Process();
            process.StartInfo.FileName = processName + ".exe";
            process.StartInfo.Arguments = arguments;
            process.StartInfo.WindowStyle = pws;
            process.Start();
        }

        /// <summary>
        /// Run a Process on macOS
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcessMac(string processName, string arguments = ""){
            var procStartInfo = new ProcessStartInfo()
            {
                FileName = processName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = arguments
            };

            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
        }

        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="processArguments"></param>
        public void RunProcessLinux(string processName, string processArguments = ""){
            var procStartInfo = new ProcessStartInfo()
            {
                FileName = processName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = processArguments
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
        public bool OpenUrlInBrowser(string url){
            Platform platform = new Platform();
            var plat = platform.ToEnum();

            if (plat.Equals(OperatingSystemFamily.Windows))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });
                return true;
            }
            if (plat.Equals(OperatingSystemFamily.Linux))
            {
                Process.Start("xdg-open", url);
                return true;
            }
            if (plat.Equals(OperatingSystemFamily.macOS))
            {
                Process.Start("open", url);
                return true;
            }
            return false;
        }

        /// <summary>
        /// Converts a String to a Process
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public Process ConvertStringToProcess(string processName){
            try{
                Process process;

                if (IsProcessRunning(processName)){
                    Process[] processes = Process.GetProcesses();

                    foreach (Process p in processes){
                        if (p.ProcessName.Equals(processName)){
                            process = Process.GetProcessById(p.Id);
                            return process;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex){
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// End a process if it is currently running.
        /// </summary>
        /// <param name="processName"></param>
        public void TerminateProcess(string processName){
            if (IsProcessRunning(processName)){
                Process process = ConvertStringToProcess(processName);
                process.Kill();
            }
        }

        /// <summary>
        /// Check how many processes are currently running.
        /// </summary>
        /// <returns></returns>
        public int GetProcessCount(){
            return Process.GetProcesses().Length;
        }

        /// <summary>
        /// Get the list of processes as a List containing Strings
        /// </summary>
        /// <returns></returns>
        public List<string> GetProcessesToStringList(){
            var strList = new List<string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes){
                strList.Add(process.ToString());
            }
            strList.TrimExcess();
            return strList;
        }

        /// <summary>
        /// Returns a List containing all the running processes.
        /// </summary>
        /// <returns></returns>
        public List<Process> GetProcessesToProcessList(){
            var strList = new List<Process>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes){
                strList.Add(process);
            }

            strList.TrimExcess();
            return strList;
        }

        /// <summary>
        /// Returns a Dictionary containing all the running processes.
        /// <typeparam name="int"> The Process ID.</typeparam>
        /// <typeparam name="string"> The Process Name.</typeparam>
        /// </summary>
        /// <returns></returns>
        public Dictionary<int, string> GetProcessListToDictionary(){
            var dictionary = new Dictionary<int, string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes){
                dictionary.Add(process.Id, process.ProcessName);
            }

            return dictionary;
        }
    }
}