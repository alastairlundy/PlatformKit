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
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        public string RunProcess(string executableLocation, string executableName, string arguments = "")
        {
            if (_platformManager.IsWindows()) return RunProcessWindows(executableLocation, executableName, arguments);
            if (_platformManager.IsLinux()) return RunProcessLinux(executableLocation, executableName, arguments);
            if (_platformManager.IsMac()) return RunProcessMac(executableLocation, executableName, arguments);

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        ///     Run a process on Windows with Arguments
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
        /// <param name="runAsAdministrator"></param>
        /// <param name="pws"></param>
        /// <exception cref="Exception"></exception>
        public string RunProcessWindows(string executableLocation, string executableName, string arguments = "",
            bool runAsAdministrator = false, bool insertExeInExecutableNameIfMissing = true,
            ProcessWindowStyle pws = ProcessWindowStyle.Normal)
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
                process.StartInfo.WindowStyle = pws;
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
        /// <param name="processName"></param>
        /// <param name="arguments"></param>
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
        ///     Run a Process on Linux
        /// </summary>
        /// <param name="processName"></param>
        /// <param name="processArguments"></param>
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

        public string RunCmdCommand(string command)
        {
            return RunProcessLinux("cmd", command);
        }

        public string RunPowerShellCommand(string command)
        {
            return RunProcessLinux("powershell", command);
        }

        public string RunCommandMac(string command)
        {
            var processName = "zsh";
            var location = "/usr/bin/";

            var processArguments = "-c \" " + command + " \"";

            return RunProcessLinux(location, processName, processArguments);
        }

        public string RunCommandLinux(string command)
        {
            var processName = "bash";
            var location = "/usr/bin/";

            /*
            if (!Directory.Exists(location))
            {
                
            }
            */

            var processArguments = "-c \" " + command + " \"";

            return RunProcessLinux(location, processName, processArguments);
        }

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
                
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw new Exception(ex.ToString());
            }
        }
        */

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
                if (_platformManager.IsWindows() && windowsMethod != null) windowsMethod.Invoke();

                if (_platformManager.IsLinux() && linuxMethod != null) linuxMethod.Invoke();
                if (_platformManager.IsMac() && macMethod != null) macMethod.Invoke();

#if NETCOREAPP3_0_OR_GREATER
                if (_platformManager.IsFreeBSD() && freeBsdMethod != null) freeBsdMethod.Invoke();
#endif
                if (_platformManager.IsMac() && macMethod == null ||
                    _platformManager.IsLinux() && linuxMethod == null ||
                    _platformManager.IsWindows() && windowsMethod == null)
                    throw new ArgumentNullException();
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
                    url = "https://" + url;
                else if (url.StartsWith("http://")) url = url.Replace("http://", "https://");

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