/*
MIT License

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

namespace AluminiumCoreLib.PlatformKit{
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
        public void WarnProcessCount(int ProcessWarnCount, string WarningMessage)
        {
            if (GetProcessCount() > ProcessWarnCount)
            {
                Console.WriteLine(WarningMessage);
            }
        }

        /// <summary>
        /// Check to see if a process is running or not.
        /// </summary>
        public bool IsProcessRunning(string ProcessName)
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                if (process.Equals(ProcessName))
                {
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Run a Process
        /// </summary>
        /// <param name="ProcessName"></param>
        public void RunProcess(string ProcessName)
        {
            RunProcess(ProcessName, "");
        }
        /// <summary>
        /// Run a Process with Arguments
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="Arguments"></param>
        public void RunProcess(string ProcessName, string Arguments){
            Platform platform = new Platform();
            var plat = platform.ToEnum();

            if (plat.Equals(OperatingSystemFamily.Windows))
            {
                RunProcessWindows(ProcessName, Arguments);
            }
            else if (plat.Equals(OperatingSystemFamily.macOS))
            {
                RunProcessMac(ProcessName, Arguments);
            }
            else if (plat.Equals(OperatingSystemFamily.Linux))
            {
                RunProcessLinux(ProcessName, Arguments);
            }
        }
        /// <summary>
        /// Run a process on Windows.
        /// </summary>
        /// <param name="ProcessName"></param>
        public void RunProcessWindows(string ProcessName){
            RunProcessWindows(ProcessName + ".exe", "");
        }
        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="Arguments"></param>
        public void RunProcessWindows(string ProcessName, string Arguments){
            RunProcessWindows(ProcessName, Arguments, ProcessWindowStyle.Normal);
        }
        /// <summary>
        /// Run a process on Windows with Arguments and a Process Window Style
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="Arguments"></param>
        /// <param name="pws"></param>
        public void RunProcessWindows(string ProcessName, string Arguments, ProcessWindowStyle pws){
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
        public void RunProcessMac(string ProcessName){
            RunProcessMac(ProcessName, "");
        }
        /// <summary>
        /// Run a Process on macOS
        /// </summary>
        /// <param name="ProcessName"></param>
        /// Courtesy of https://github.com/fontanaricardo/RunCommand
        public void RunProcessMac(string ProcessName, string ProcessArguments){
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
        public void RunProcessLinux(string ProcessName){
            RunProcessLinux(ProcessName, "");
        }
        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="ProcessName"></param>
        public void RunProcessLinux(string ProcessName, string ProcessArguments){
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
        /// <param name="ProcessName"></param>
        /// <returns></returns>
        public Process ConvertStringToProcess(string ProcessName){
            try{
                Process process;

                if (IsProcessRunning(ProcessName)){
                    Process[] processes = Process.GetProcesses();

                    foreach (Process p in processes){
                        if (p.ProcessName.Equals(ProcessName)){
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
        /// <param name="ProcessName"></param>
        public void TerminateProcess(string ProcessName){
            if (IsProcessRunning(ProcessName)){
                Process process = ConvertStringToProcess(ProcessName);
                process.Kill();
            }
        }

        /// <summary>
        /// Check how many processes are currently running.
        /// </summary>
        /// <returns></returns>
        public int GetProcessCount(){
            int count = 0;
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes){
                //Get whatever attribute for process
                count++;
            }
            return count;
        }
        /// <summary>
        /// Get the list of processes as a List containing Strings
        /// </summary>
        /// <returns></returns>
        public List<string> GetProcessesAsStringList(){
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
        public List<Process> GetProcessesAsProcessList(){
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
        public Dictionary<int, string> ProcessListToDictionary(){
            var dictionary = new Dictionary<int, string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes){
                dictionary.Add(process.Id, process.ProcessName);
            }

            return dictionary;
        }
    }
}