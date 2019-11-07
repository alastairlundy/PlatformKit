using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace AluminiumCoreLib.PlatformKit{
    /// <summary>
    /// 
    /// </summary>
    public class ProcessManager{
        Platform platform;

        /// <summary>
        /// 
        /// </summary>
        public ProcessManager(){
            platform = new Platform();
        }

        /// <summary>
        /// Print all the processes running to the Console.
        /// </summary>
        public void ListAllProcesses()
        {
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
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
            var plat = platform.GetPlatformAsString();

            if (plat.ToLower().Contains("win"))
            {
                RunProcessWindows(ProcessName);
            }
            else if (plat.ToLower().Contains("osx"))
            {
                RunProcessMac(ProcessName);
            }
            else if (plat.ToLower().Contains("linux"))
            {
                RunProcessLinux(ProcessName);
                //  throw new NotImplementedException();
            }
        }
        /// <summary>
        /// Run a Process
        /// </summary>
        /// <param name="ProcessName"></param>
        /// <param name="Arguments"></param>
        public void RunProcess(string ProcessName, string Arguments)
        {
            var plat = platform.GetPlatformAsString();

            if (plat.ToLower().Contains("win"))
            {
                RunProcessWindows(ProcessName, Arguments);
            }
            else if (plat.ToLower().Contains("osx"))
            {
                RunProcessMac(ProcessName, Arguments);
            }
            else if (plat.ToLower().Contains("linux"))
            {
                RunProcessLinux(ProcessName, Arguments);
            }
        }
        /// <summary>
        /// Run a process on Windows.
        /// </summary>
        /// <param name="ProcessName"></param>
        public void RunProcessWindows(string ProcessName)
        {
            Process process = new Process();
            process.StartInfo.FileName = ProcessName + ".exe";
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
        }
        public void RunProcessWindows(string ProcessName, string Arguments)
        {
            Process process = new Process();
            process.StartInfo.FileName = ProcessName + ".exe";
            process.StartInfo.Arguments = Arguments;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Normal;
            process.Start();
        }
        public void RunProcessWindows(string ProcessName, string Arguments, ProcessWindowStyle pws)
        {
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
        public void RunProcessMac(string ProcessName)
        {
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
        /// Run a Process on macOS
        /// </summary>
        /// <param name="ProcessName"></param>
        /// Courtesy of https://github.com/fontanaricardo/RunCommand
        public void RunProcessMac(string ProcessName, string ProcessArguments)
        {
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
        public void RunProcessLinux(string ProcessName)
        {
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
        public void RunProcessLinux(string ProcessName, string ProcessArguments)
        {
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
        public bool OpenURLInBrowser(string url)
        {
            var plat = platform.GetPlatformAsString();
            if (plat.ToLower().Contains("win"))
            {
                Process.Start(new ProcessStartInfo("cmd", $"/c start {url.Replace("&", "^&")}") { CreateNoWindow = true });
                return true;
            }
            if (plat.ToLower().Contains("linux"))
            {
                Process.Start("xdg-open", url);
                return true;
            }
            if (plat.ToLower().Contains("osx"))
            {
                Process.Start("open", url);
                return true;
            }
            return false;
        }

        public Process StringToProcessTranslation(string ProcessName)
        {
            try
            {
                Process process;

                if (IsProcessRunning(ProcessName))
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process p in processes)
                    {
                        if (p.ProcessName.Equals(ProcessName))
                        {
                            process = Process.GetProcessById(p.Id);
                            return process;
                        }
                    }
                }

                return null;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.ToString());
            }
        }

        public void TerminateProcess(string ProcessName)
        {
            if (IsProcessRunning(ProcessName))
            {
                Process process = StringToProcessTranslation(ProcessName);
                process.Kill();
            }
        }

        /// <summary>
        /// Check how many processes are currently running.
        /// </summary>
        /// <returns></returns>
        public int GetProcessCount()
        {
            int count = 0;
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                //Get whatever attribute for process
                count++;
            }
            return count;
        }
        /// <summary>
        /// Get the list of processes as an ObjectList containing Strings
        /// </summary>
        /// <returns></returns>
        public List<string> GetProcessListAsStrings()
        {
            var strList = new List<string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                strList.Add(process.ToString());
            }
            strList.TrimExcess();
            return strList;
        }
        /// <summary>
        /// Returns a List containing all the running processes.
        /// </summary>
        /// <returns></returns>
        public List<Process> GetProcessListAsProcesses()
        {
            var strList = new List<Process>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
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
        public Dictionary<int, string> ProcessListToDictionary()
        {
            var dictionary = new Dictionary<int, string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process process in processes)
            {
                dictionary.Add(process.Id, process.ProcessName);
            }
            return dictionary;
        }
    }
}