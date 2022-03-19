/* MIT License

Copyright (c) 2018-2022 AluminiumTech

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
using System.Threading.Tasks;

using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Deprecation;

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable InvalidXmlDocComment

namespace AluminiumTech.DevKit.PlatformKit
{
    /// <summary>
    ///     A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager
    {
        // ReSharper disable once InconsistentNaming
        protected readonly OSAnalyzer _osAnalyzer;

        public ProcessManager()
        {
            _osAnalyzer = new OSAnalyzer();
        }

        /// <summary>
        ///     Run a Process with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">(Optional) Arguments to be passed to the executable.</param>
        /// <param name="processStartInfo">(Optional) Process Start Information to be passed to the executable. This may have the unintended effect of overriding other optional or non-optional arguments.</param>
        public string RunProcess(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            if (_osAnalyzer.IsWindows()) return RunProcessWindows(executableLocation, executableName, arguments, processStartInfo);
            if (_osAnalyzer.IsLinux()) return RunProcessLinux(executableLocation, executableName, arguments, processStartInfo);
            if (_osAnalyzer.IsMac()) return RunProcessMac(executableLocation, executableName, arguments, processStartInfo);

#if NETCOREAPP3_0_OR_GREATER
            if (_osAnalyzer.IsFreeBSD()) throw new NotImplementedException();
#endif
            
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        ///     Run a process on Windows with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        /// <param name="windowStyle">Whether to open the window in full screen mode, normal size, minimized, or a different window mode.</param>
        /// <exception cref="Exception"></exception>
        public string RunProcessWindows(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null,
            bool runAsAdministrator = false, bool insertExeInExecutableNameIfMissing = true,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            try
            {
                Process process;
            
                if (processStartInfo != null)
                {
                    processStartInfo.WorkingDirectory = executableLocation;
                    processStartInfo.FileName = executableName;
                    processStartInfo.Arguments = arguments;
                    processStartInfo.RedirectStandardOutput = true;
                    process = new Process { StartInfo = processStartInfo};
                }
                else
                {
                    process = new Process();
                    
                    process.StartInfo.FileName = executableName;

                    if (!executableName.EndsWith(".exe") && insertExeInExecutableNameIfMissing)
                    {
                        process.StartInfo.FileName += ".exe";
                    }

                    process.StartInfo.WorkingDirectory = executableLocation;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.Arguments = arguments;
                    process.StartInfo.WindowStyle = windowStyle;
                }

                if (runAsAdministrator)
                {
                    process.StartInfo.Verb = "runas";
                }
                
                process.Start();

                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }

        /// <summary>
        ///     Run a Process on macOS
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        public string RunProcessMac(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            var procStartInfo = new ProcessStartInfo
            {
                WorkingDirectory = executableLocation,
                FileName = executableName,
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = false,
                Arguments = arguments
            };

            Process process;
            
            if (processStartInfo == null)
            {
                process = new Process { StartInfo = procStartInfo };
            }
            else
            {
                procStartInfo = processStartInfo;
                procStartInfo.WorkingDirectory = executableLocation;
                procStartInfo.FileName = executableName;
                procStartInfo.Arguments = arguments;
                procStartInfo.RedirectStandardOutput = true;
                process = new Process { StartInfo = processStartInfo};
            }
            
            process.Start();

            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string RunProcessLinux(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            try
            {
                ProcessStartInfo procStartInfo;
                
                if (processStartInfo == null)
                {
                     procStartInfo = new ProcessStartInfo
                    {
                        WorkingDirectory = executableLocation,
                        FileName = executableName,
                        RedirectStandardOutput = true,
                        UseShellExecute = false,
                        CreateNoWindow = false,
                        Arguments = arguments
                    };
                }
                else
                {
                    procStartInfo = processStartInfo;
                    procStartInfo.WorkingDirectory = executableLocation;
                    procStartInfo.FileName = executableName;
                    procStartInfo.Arguments = arguments;
                    procStartInfo.RedirectStandardOutput = true;
                }
                
                var process = new Process { StartInfo = procStartInfo };
                process.Start();

                process.WaitForExit();
                return process.StandardOutput.ReadToEnd();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw new Exception(ex.ToString());
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        [Obsolete(DeprecationMessages.DeprecationV3 + "\r\n" + 
                  "This will be removed in PlatformKit 3.0 and will not be replaced. Please use platform specific RunCommand methods instead.")]
        public string RunCommand(string command, ProcessStartInfo processStartInfo = null)
        {
            if (_osAnalyzer.IsWindows()) return RunCmdCommand(command, processStartInfo);
            if (_osAnalyzer.IsLinux()) return RunLinuxCommand(command, processStartInfo);
            if (_osAnalyzer.IsMac()) throw new NotImplementedException();
            if (_osAnalyzer.IsFreeBSD()) throw new NotImplementedException();
                //return RunCommandMac(command, processStartInfo);

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Runs commands in the Windows Cmd Command Prompt.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string RunCmdCommand(string command, ProcessStartInfo processStartInfo = null)
        {
            return RunProcessWindows(Environment.SystemDirectory, "cmd", command, processStartInfo);
        }

        /// <summary>
        /// Runs commands in Windows Powershell.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string RunPowerShellCommand(string command, ProcessStartInfo processStartInfo = null)
        {
            if (_osAnalyzer.IsWindows())
            {
                var location = Environment.SystemDirectory + Path.DirectorySeparatorChar + "WindowsPowerShell" +
                               Path.DirectorySeparatorChar + "v1.0";
                return RunProcessWindows(location, "powershell", command, processStartInfo);
   
            }
                
            throw new PlatformNotSupportedException();
        }

        /*
        public string RunMacCommand(string command)
        {
            
        }
        
        /* <summary>
        / 
         </summary>
         <param name="command"></param>
         <returns></returns>
        internal string RunMacCommand(string command)
        {
            if (_osAnalyzer.IsMac())
            {
      //          var processName = "zsh";
      //          var location = "/usr/bin/";

                var processArguments = "-c \" " + command + " \"";

                return RunProcessMac(location, processName, processArguments);
            }

            throw new PlatformNotSupportedException();
        }
        */

        /// <summary>
        /// Run a command or program as if inside a terminal on Linux.
        /// </summary>
        /// <param name="command"></param>
        /// <param name="processStartInfo"></param>
        /// <returns></returns>
        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use the RunLinuxCommand() method instead.")]
        public string RunCommandLinux(string command, ProcessStartInfo processStartInfo = null)
        {
            return RunLinuxCommand(command, processStartInfo);
        }
        
        /// <summary>
        /// Run a command or program as if inside a terminal on Linux.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string RunLinuxCommand(string command, ProcessStartInfo processStartInfo = null)
        {
            try
            {
                if (_osAnalyzer.IsLinux())
                {
                    var location = "/usr/bin/";

                    var args = command.Split(' ');
                    command = args[0];
                    var processArguments = "";

                    if (args.Length > 0)
                    {
                        for (int index = 1; index < args.Length; index++)
                        {
                            processArguments += args[index].Replace(command, String.Empty);
                        }
                    }

                    if (!Directory.Exists(location))
                    {
                        throw new DirectoryNotFoundException("Could not find directory '" + nameof(location) + "' with value: " + location);
                    }
                    
                    return RunProcessLinux(location, command, processArguments, processStartInfo);
                }

                throw new PlatformNotSupportedException();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
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
        [Obsolete(DeprecationMessages.DeprecationV3)]
        public void RunActionOn(Action windowsMethod = null, Action macMethod = null,
            Action linuxMethod = null, Action freeBsdMethod = null)
        {
            try
            {
                if (_osAnalyzer.IsWindows() && windowsMethod != null)
                {
                    windowsMethod.Invoke();
                }

                if (_osAnalyzer.IsLinux() && linuxMethod != null)
                {
                    linuxMethod.Invoke();
                }
                if (_osAnalyzer.IsMac() && macMethod != null)
                {
                    macMethod.Invoke();
                }

#if NETCOREAPP3_0_OR_GREATER
                if (_osAnalyzer.IsFreeBSD() && freeBsdMethod != null)
                {
                    freeBsdMethod.Invoke();
                }
#endif
                if (_osAnalyzer.IsMac() && macMethod == null ||
                    _osAnalyzer.IsLinux() && linuxMethod == null ||
                    _osAnalyzer.IsWindows() && windowsMethod == null)
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
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool OpenUrlInBrowser(string url, bool allowNonSecureHttp = false)
        {
            try
            {
                if (!url.StartsWith("https://") || !url.StartsWith("www."))
                {
                    if (allowNonSecureHttp)
                    {
                        url = "http://" + url;
                    }
                    else
                    {
                        url = "https://" + url;
                    }
                }
                else if (url.StartsWith("http://") && !allowNonSecureHttp)
                {
                    url = url.Replace("http://", "https://");
                }

                if (_osAnalyzer.IsWindows())
                {
                    RunCmdCommand($"/c start {url.Replace("&", "^&")}", new ProcessStartInfo { CreateNoWindow = true});
                    return true;
                }
                if (_osAnalyzer.IsLinux())
                {
                    RunLinuxCommand("xdg-open " + url);
                    return true;
                }
                if (_osAnalyzer.IsMac())
                {
                    var task = new Task(() =>
                        Process.Start("open", url));
                    task.Start();
                    return true;
                }
#if  NETCOREAPP3_1_OR_GREATER
                if (_osAnalyzer.IsFreeBSD())
                {
                    throw new NotImplementedException();
                }          
#endif

                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
    }
}