/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable InvalidXmlDocComment

namespace PlatformKit
{
    /// <summary>
    ///  A class to manage processes on a device and/or start new processes.
    /// </summary>
    public class ProcessManager
    {

        /// <summary>
        /// Run a process on Windows with Arguments
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
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        ///  Run a Process on macOS
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        public string RunProcessMac(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
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
            catch(Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
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
                Console.WriteLine(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Run a Process on FreeBSD
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public string RunProcessFreeBsd(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            return RunProcessLinux(executableLocation, executableName, arguments, processStartInfo);
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
            if (PlatformAnalyzer.IsWindows())
            {
                var location = Environment.SystemDirectory + Path.DirectorySeparatorChar 
                                                           //+ "System32" +
                               + Path.DirectorySeparatorChar + "WindowsPowerShell" +
                               Path.DirectorySeparatorChar + "v1.0";
                return RunProcessWindows(location, "powershell", command, processStartInfo);
            }
                
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Open a URL in the default browser.
        /// Courtesy of https://github.com/dotnet/corefx/issues/10361
        /// </summary>
        /// <param name="url">The URL to be opened.</param>
        /// <param name="allowNonSecureHttp">Whether to allow non HTTPS links to be opened.</param>
        /// <returns></returns>
        public void OpenUrlInBrowser(string url, bool allowNonSecureHttp = false)
        {
            try
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

                if (PlatformAnalyzer.IsWindows())
                {
                    RunCmdCommand($"/c start {url.Replace("&", "^&")}", new ProcessStartInfo { CreateNoWindow = true});
                }
                if (PlatformAnalyzer.IsLinux())
                {
                    RunLinuxCommand($"xdg-open {url}");
                }
                if (PlatformAnalyzer.IsMac())
                {
                    var task = new Task(() => Process.Start("open", url));
                    task.Start();
                }
#if  NETCOREAPP3_0_OR_GREATER
                if (PlatformAnalyzer.IsFreeBSD())
                {
                    RunFreeBsdCommand($"xdg-open {url}");
                }          
#endif
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                throw;
            }
        }
    }
}