/*
        MIT License
       
       Copyright (c) 2020-2024 Alastair Lundy
       
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

#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable InvalidXmlDocComment

namespace PlatformKit
{
    /// <summary>
    ///  A class to manage processes on a device and/or start new processes.
    /// </summary>
    public static class ProcessRunner
    {

        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        /// <param name="windowStyle">Whether to open the window in full screen mode, normal size, minimized, or a different window mode.</param>
        /// <exception cref="Exception"></exception>
        public static string RunProcessOnWindows(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null,
            bool runAsAdministrator = false, bool insertExeInExecutableNameIfMissing = true,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
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

            Task task = new Task(() => process.Start());
            task.Start();

            task.Wait();
            var end = process.StandardOutput.ReadToEnd();

            if (end == null)
            {
                throw new NullReferenceException();
            }

            if (end.ToLower()
                .Contains("is not recognized as the name of a cmdlet, function, script file, or operable program"))
            {
                throw new Exception(end);
            }
            else if (end.ToLower()
                     .Contains(
                         "is not recognized as an internal or external command, operable program or batch file."))
            {
                throw new Exception(end);
            }

            return end;
        }

        /// <summary>
        ///  Run a Process on macOS
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        public static string RunProcessOnMac(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
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
        public static string RunProcessOnLinux(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
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

        /// <summary>
        /// Run a Process on FreeBSD
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static string RunProcessOnFreeBsd(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            return RunProcessOnLinux(executableLocation, executableName, arguments, processStartInfo);
        }

        /// <summary>
        /// Open a URL in the default browser.
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url">The URL to be opened.</param>
        /// <param name="allowNonSecureHttp">Whether to allow non HTTPS links to be opened.</param>
        /// <returns></returns>
        public static void OpenUrlInBrowser(string url, bool allowNonSecureHttp = false)
        {
            if ((!url.StartsWith("https://") || !url.StartsWith("www.")) && (!url.StartsWith("file://")))
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

            if (OperatingSystem.IsWindows())
            {
                CommandRunner.RunCmdCommand($"/c start {url.Replace("&", "^&")}", new ProcessStartInfo { CreateNoWindow = true});
            }
            if (OperatingSystem.IsLinux())
            {
                CommandRunner.RunCommandOnLinux($"xdg-open {url}");
            }
            if (OperatingSystem.IsMacOS())
            {
                var task = new Task(() => Process.Start("open", url));
                task.Start();
            }
#if  NETCOREAPP3_0_OR_GREATER
            if (OperatingSystem.IsFreeBSD())
            {
                CommandRunner.RunCommandOnFreeBsd($"xdg-open {url}");
            }          
#endif
        }
    }
}