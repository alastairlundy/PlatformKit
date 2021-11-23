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
using System.Threading.Tasks;

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable InvalidXmlDocComment

namespace AluminiumTech.DevKit.PlatformKit
{
    /// <summary>
    ///     A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager
    {
        protected PlatformManager _platformManager;

        public ProcessManager()
        {
            _platformManager = new PlatformManager();
        }

        /// <summary>
        ///     Run a Process with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        public string RunProcess(string executableLocation, string executableName, string arguments = "")
        {
            if (_platformManager.IsWindows()) return RunProcessWindows(executableLocation, executableName, arguments);
            if (_platformManager.IsLinux()) return RunProcessLinux(executableLocation: executableLocation, executableName, arguments);
            if (_platformManager.IsMac()) return RunProcessMac(executableLocation, executableName, arguments);

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
        public string RunProcessWindows(string executableLocation, string executableName, string arguments = "",
            bool runAsAdministrator = false, bool insertExeInExecutableNameIfMissing = true,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            try
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                var process = new Process();

                process.StartInfo.FileName = executableName;

                if (!executableName.EndsWith(".exe") && insertExeInExecutableNameIfMissing)
                    process.StartInfo.FileName += ".exe";

                if (runAsAdministrator) process.StartInfo.Verb = "runas";

                process.StartInfo.WorkingDirectory = executableLocation;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.Arguments = arguments;
                process.StartInfo.WindowStyle = windowStyle;
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
        public string RunProcessMac(string executableLocation, string executableName, string arguments = "")
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

            var process = new Process { StartInfo = procStartInfo };
            process.Start();

            process.WaitForExit();
            return process.StandardOutput.ReadToEnd();
        }

        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string RunProcessLinux(string executableLocation, string executableName, string processArguments = "")
        {
            try
            {
                var procStartInfo = new ProcessStartInfo
                {
                    WorkingDirectory = executableLocation,
                    FileName = executableName,
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = false,
                    Arguments = processArguments
                };

                if (executableName.ToLower().StartsWith("/usr/bin/"))
                {
                    procStartInfo.WorkingDirectory = "/usr/bin/";
                    procStartInfo.FileName = executableName.Replace("/usr/bin/", string.Empty);
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

        // This won't be implemented for V2. This will be implemented in V2.1 or later
        /*
        public void RunConsoleCommand(string arguments)
        {
            else if (plat.Equals(OperatingSystemFamily.macOS))
            {
           //     programName = "open -b com.apple.terminal " + arguments;


            RunProcess(programName, arguments);
        }
         */

        public string RunCommand(string command)
        {
            if (_platformManager.IsWindows()) return RunCmdCommand(command);
            if (_platformManager.IsLinux()) return RunCommandLinux(command);
            if (_platformManager.IsMac()) return RunCommandMac(command);

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string RunCmdCommand(string command)
        {
            return RunProcessWindows(Environment.SystemDirectory, "cmd", command);
        }

        /// <summary>
        /// Runs commands in Windows Powershell
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string RunPowerShellCommand(string command)
        {
            try
            {
                var location = Environment.SystemDirectory + Path.DirectorySeparatorChar + "WindowsPowerShell" +
                               Path.DirectorySeparatorChar + "v1.0";
                return RunProcessWindows(location, "powershell", command);
            }
            catch
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public string RunCommandMac(string command)
        {
            try
            {
                var processName = "zsh";
                var location = "/usr/bin/";

                var processArguments = "-c \" " + command + " \"";

                return RunProcessMac(location, processName, processArguments);
            }
            catch
            {
                throw new PlatformNotSupportedException();
            }
        }
        
        /// <summary>
        /// Run a command or program as if inside a terminal on Linux.
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string RunCommandLinux(string command)
        {
            try
            {
                var executableName = "bash";
                var location = "/usr/bin/";

                if (!Directory.Exists(location + Path.DirectorySeparatorChar + executableName))
                {
                    executableName = "zsh";
                }
            
                if (!Directory.Exists(location))
                {
                    throw new ArgumentException("Cannot execute a command if there is no command to execute.");
                }

                var processArguments = "-c \" " + command + " \"";

                return RunProcessLinux(location, executableName, processArguments);
            }
            catch
            {
                throw new PlatformNotSupportedException();
            }
        }

        /// <summary>
        ///     Run different actions or methods depending on the operating system.
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
        ///     Open a URL in the default browser.
        ///     Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        public bool OpenUrlInBrowser(string url)
        {
            try
            {
                if (!url.StartsWith("https://") || !url.StartsWith("www."))
                {
                    url = "https://" + url;
                }
                else if (url.StartsWith("http://"))
                {
                    url = url.Replace("http://", "https://");
                }

                if (_platformManager.IsWindows())
                {
                    var task = new Task(() =>
                        Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}")
                            { CreateNoWindow = true }));
                    task.Start();
                    return true;
                }

                if (_platformManager.IsLinux())
                {
                    var task = new Task(() =>
                        Process.Start("xdg-open", url));
                    task.Start();
                    return true;
                }

                if (_platformManager.IsMac())
                {
                    var task = new Task(() =>
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
    }
}