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
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows;

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable InvalidXmlDocComment

namespace AluminiumTech.DevKit.PlatformKit
{
    /// <summary>
    /// A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager
    {
        protected PlatformManager _platformManager;

        public ProcessManager()
        {
            _platformManager = new PlatformManager();
        }

        /// <summary>
        /// Run a Process with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public void RunProcess(string processName, string arguments = "")
        {
            RunActionOn(() => RunProcessWindows(processName, arguments), () => RunProcessMac(processName, arguments),
                () => RunProcessLinux(processName, arguments));
        }

        /// <summary>
        ///  Run a process on Windows with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        /// <param name="runAsAdministrator"></param>
        /// <param name="pws"></param>
        /// <exception cref="Exception"></exception>
        public void RunProcessWindows(string processName, string arguments = "", bool runAsAdministrator = false,
            ProcessWindowStyle pws = ProcessWindowStyle.Normal)
        {
            try
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                Process process = new Process();

                if (processName.Contains(".exe") || processName.EndsWith(".exe"))
                {
                    process.StartInfo.FileName = processName;
                }
                else
                {
                    process.StartInfo.FileName = $"{processName}.exe";
                }

                if (runAsAdministrator)
                {
                    process.StartInfo.Verb = "runas";
                }
                
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WindowStyle = pws;
                process.Start();
            }
            catch (Exception ex)
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

            Process process = new Process { StartInfo = procStartInfo };
            process.Start();
        }

        // This won't be implemented for V2. This will be implemented in V2.1 or later
        /*
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
        /// TODO: Test, Fix, and Revamp this method as required to ensure it is working for V2.1 or later
        /// THIS WILL BE DISABLED FOR V2.
        /// 
        /// WARNING: Does not work on Windows or macOS.
        /// </summary>
        /// <param name="command"></param>
        /* internal void RunCommandLinux(string command)
        {
            try
            {
                if (GetPlatformOperatingSystemFamily() != (OperatingSystemFamily.Linux))
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
        */
        
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

                Process process = new Process { StartInfo = procStartInfo };
                process.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }

        }

        /// <summary>
        /// Run different actions or methods depending on the operating system.
        /// </summary>
        /// <param name="windowsMethod"></param>
        /// <param name="macMethod"></param>
        /// <param name="linuxMethod"></param>
        /// <param name="freeBsdMethod"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once MemberCanBePrivate.Global
        public void RunActionOn(Action windowsMethod = null, Action macMethod = null,
            Action linuxMethod = null, Action freeBsdMethod = null)
        {
            try
            {
                if (_platformManager.IsWindows() && windowsMethod != null)
                {
                    windowsMethod.Invoke();
                }

                if (_platformManager.IsLinux() && linuxMethod != null)
                {
                    linuxMethod.Invoke();
                }
                if (_platformManager.IsMac() && macMethod != null)
                {
                    macMethod.Invoke();
                }
                
#if NETCOREAPP3_0_OR_GREATER
                if (_platformManager.IsFreeBSD() && freeBsdMethod != null)
                {
                    freeBsdMethod.Invoke();
                }
#endif
                if (_platformManager.IsMac() && macMethod == null ||
                    _platformManager.IsLinux() && linuxMethod == null ||
                    _platformManager.IsWindows() && windowsMethod == null)
                {
                    throw new ArgumentNullException();
                }
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }
        
        /// <summary>
        /// Open a URL in the default browser.
        ///
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool OpenUrlInBrowser(string url)
        {
            try
            {
                if (!url.StartsWith("http://") || !url.StartsWith("https://") || !url.StartsWith("www."))
                {
                    url = "https://" + url;
                }
                
                if (_platformManager.IsWindows())
                {
                    Task task = new Task(() =>
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}")
                            { CreateNoWindow = true }));
                    task.Start();
                    return true;
                }
                else if (_platformManager.IsLinux())
                {
                    Task task = new Task(() =>
                        Process.Start("xdg-open", url));
                    task.Start();
                    return true;
                }
                else if (_platformManager.IsMac())
                {
                    Task task = new Task(()=> 
                        Process.Start("open", url));
                    task.Start();
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
        /// Determines whether a process (or the current process if unspecified) is running as an administrator.
        /// Currently only supports Windows. Running on macOS or Linux will return a PlatformNotSupportedException.
        /// WORK IN PROGRESS. 
        /// Fix for future 2.x release
        /// </summary>
        /// <param name="process"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Occurs when running on macOS or Linux as these are not currently supported.</exception>
        public bool IsRunningAsAdministrator(Process process = null)
        {
            try
            {
                if (process == null)
                {
                    process = Process.GetCurrentProcess();
                }
                
                if (_platformManager.IsWindows())
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
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }
    }
}