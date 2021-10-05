/* MIT License

Copyright (c) 2019-2021 AluminiumTech

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

using HardwareKit.Components.Base.enums;


namespace AluminiumTech.DevKit.PlatformKit
{
    /// <summary>
    /// A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager
    {
        protected OperatingSystemFamily cachedOSFamily;

        public ProcessManager()
        {
        }

        /// <summary>
        /// Allow ProcessManager.cs access to Platform.cs's OperatingSystemFamily detection.
        /// </summary>
        /// <returns></returns>
        internal OperatingSystemFamily GetPlatformOperatingSystemFamily()
        {
            if (cachedOSFamily.Equals(null))
            {
                cachedOSFamily = new Platform().ToOperatingSystemFamily();
            }

            return cachedOSFamily;
        }

        /// <summary>
        /// Run a Process with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcess(string processName, string arguments = "")
        {
            var plat = GetPlatformOperatingSystemFamily();
            
            RunActionOn(()=> RunProcessWindows(processName, arguments), ()=> RunProcessMac(processName, arguments),
                ()=> RunProcessLinux(processName, arguments));
        }

        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        /// <param name="pws"></param>
        public void RunProcessWindows(string processName, string arguments = "", ProcessWindowStyle pws = ProcessWindowStyle.Normal)
        {
            try
            {
                Process process = new Process();
                process.StartInfo.FileName = processName + ".exe";
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WindowStyle = pws;
                process.Start();
            }
            catch(Exception ex)
            {
               Console.WriteLine(ex.ToString());
               throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Run a Process on macOS
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcessMac(string processName, string arguments = "")
        {
            var procStartInfo = new ProcessStartInfo()
            {
                FileName = processName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = arguments
            };

            Process process = new Process {StartInfo = procStartInfo};
            process.Start();
        }

        /// This won't be implemented for V2. This will be implemented in V2.1
        /*
         * 
        public void RunConsoleCommand(string arguments)
        {
            var plat = new Platform().ToEnum();

            string programName = "";
            
            if (plat.Equals(OperatingSystemFamily.Windows))
            {
                programName = "cmd";
            }
            else if (plat.Equals(OperatingSystemFamily.macOS))
            {
           //     programName = "open -b com.apple.terminal " + arguments;
            }
            else if (plat.Equals(OperatingSystemFamily.Linux))
            {
                //programName
            }

            RunProcess(programName, arguments);
        }
         */

        /// <summary>
        /// TODO: Test, Fix, and Revamp this method as required to ensure it is working for V2.1
        /// THIS WILL BE DISABLED FOR V2.
        /// 
        /// WARNING: Does not work on Windows or macOS.
        /// </summary>
        /// <param name="command"></param>
        internal void RunCommandLinux(string command)
        {
            try
            {
                if (!(new Platform().ToOperatingSystemFamily().Equals(OperatingSystemFamily.Linux)))
                {
                    throw new PlatformNotSupportedException();
                }

                //https://askubuntu.com/questions/506985/c-opening-the-terminal-process-and-pass-commands
                string processName = "/usr/bin/bash";
                string processArguments = "-c \" " + command + " \"";

                RunProcessLinux(processName, processArguments);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="processArguments"></param>
        public void RunProcessLinux(string processName, string processArguments = "")
        {
            try
            {
                var procStartInfo = new ProcessStartInfo()
                    {
                        FileName = processName,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        Arguments = processArguments
                    };

                    Process process = new Process {StartInfo = procStartInfo};
                    process.Start();
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }
            
        }

        /// <summary>
        /// Run different actions or methods depending on the operating system.
        /// </summary>
        /// <param name="WindowsMethod"></param>
        /// <param name="MacMethod"></param>
        /// <param name="LinuxMethod"></param>
        /// <returns></returns>
        public bool RunActionOn(System.Action WindowsMethod = null, System.Action MacMethod = null, System.Action LinuxMethod = null)
        {
            try
            {
                if (GetPlatformOperatingSystemFamily().Equals(OperatingSystemFamily.Windows) && WindowsMethod != null)
                {
                    WindowsMethod.Invoke();
                    return true;
                }
                else if (GetPlatformOperatingSystemFamily().Equals(OperatingSystemFamily.Linux) && LinuxMethod != null)
                {
                    LinuxMethod.Invoke();
                    return true;
                }
                else if (GetPlatformOperatingSystemFamily().Equals(OperatingSystemFamily.macOS) && MacMethod != null)
                {
                    MacMethod.Invoke();
                    return true;
                }
                else if(GetPlatformOperatingSystemFamily().Equals(OperatingSystemFamily.macOS) && MacMethod == null ||
                        GetPlatformOperatingSystemFamily().Equals(OperatingSystemFamily.Linux) && LinuxMethod == null ||
                        GetPlatformOperatingSystemFamily().Equals(OperatingSystemFamily.Windows) && WindowsMethod == null)
                {
                    throw new ArgumentNullException();
                }

                return false;
            }
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Check to see if a process is running or not.
        /// </summary>
        public bool IsProcessRunning(string processName)
        {
            foreach (Process proc in Process.GetProcesses())
            {
                proc.ProcessName.Replace("System.Diagnostics.Process (", "");

                Console.WriteLine(proc.ProcessName);

                processName.Replace(".exe", "");

                if(proc.ProcessName == processName)
                {
                    return true;
                }
                if (proc.ProcessName.ToLower().Equals(processName.ToLower()))
                {
                    return true;
                }
                if (proc.ProcessName.ToLower().Contains(processName.ToLower()))
                {
                    return true;
                }
                if (proc.ProcessName.Contains(processName))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts a String to a Process
        /// </summary>
        /// <param name="processName"></param>
        /// <returns></returns>
        public Process ConvertStringToProcess(string processName)
        {
            try
            {
                processName.Replace(".exe", "");

                if (IsProcessRunning(processName))
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process p in processes)
                    {
                        if (p.ProcessName.Equals(processName))
                        {
                            return p;
                        }
                    }
                }

                return null;
              //  throw new Exception();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Get the list of processes as a String Array
        /// </summary>
        /// <returns></returns>
        public string[] GetProcessesToStringArray()
        {
            var strList = new List<string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                strList.Add(process.ProcessName);
            }

            strList.TrimExcess();
            return strList.ToArray();
        }


        /// <summary>
        /// Open a URL in the default browser.
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        public bool OpenUrlInBrowser(string url)
        {
            try
            {
                Platform platform = new Platform();

                if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Windows))
                {
                    Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}")
                    { CreateNoWindow = true });
                    return true;
                }
                else if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Linux))
                {
                    Process.Start("xdg-open", url);
                    return true;
                }
                else if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.OSX))
                {
                    Process.Start("open", url);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        /// Suspends a process using native or imported method calls.
        ///  WARNING: This is only implemented on Windows and will throw an exception if run on Linux or macOS.
        /// </summary>
        /// <param name="processName"></param>
        public void SuspendProcess(string processName)
        {
            Platform platform = new Platform();

            if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                PlatformSpecifics.WindowsProcessSpecifics.Suspend(ConvertStringToProcess(processName));
            }
            else if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                throw new PlatformNotSupportedException();
            }
            else if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Resumes a process using native or imported method calls.
        /// WARNING: This is only implemented on Windows and will throw an exception if run on Linux or macOS.
        /// </summary>
        /// <param name="processName"></param>
        public void ResumeProcess(string processName)
        {
            Platform platform = new Platform();

            if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Windows))
            {
                if (IsProcessRunning(processName))
                {
                    PlatformSpecifics.WindowsProcessSpecifics.Resume(ConvertStringToProcess(processName));
                }
            }
            else if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                throw new PlatformNotSupportedException();
            }
            else if (platform.GetOSPlatform().Equals(System.Runtime.InteropServices.OSPlatform.Linux))
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// Determines whether a process (or the current process if unspecified) is running as an admistrator.
        /// Currently only supports Windows. Running on macOS or Linux will return a PlatformNotSupportedException.
        /// WORK IN PROGRESS. 
        /// Fix for V2
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        internal bool IsRunningAsAdministrator(Process process = null)
        {
            if (process.Equals(null))
            {
                process = Process.GetCurrentProcess();
            }

            Platform platform = new Platform();

            if (platform.ToOperatingSystemFamily().Equals(AluminiumTech.HardwareKit.Components.Base.enums.OperatingSystemFamily.Windows))
            {

                if (process.StartInfo.Verb.Contains("runas"))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            /*       else if (platform.ToOperatingSystemFamily().Equals(OperatingSystemFamily.macOS))
                 {
                     return (Mono.Unix.Native.Syscall.geteuid() == 0);
                 }
                 else if (platform.ToOperatingSystemFamily().Equals(OperatingSystemFamily.Linux))
                 {
                     return (Mono.Unix.Native.Syscall.geteuid() == 0);
                 }

          */
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
    }
}