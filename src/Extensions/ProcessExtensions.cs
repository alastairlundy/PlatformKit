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

using AluminiumTech.DevKit.PlatformKit.Deprecation;

namespace AluminiumTech.DevKit.PlatformKit.Extensions
{
    public static class ProcessExtensions
    {
        /// <summary>
        /// Get the list of processes as a String Array
        /// </summary>
        /// <returns></returns>
         static  string[] ToStringArray(this Process process)
        {
            var strList = new List<string>();
            Process[] processes = Process.GetProcesses();

            foreach (Process proc in processes)
            {
                strList.Add(proc.ProcessName);
            }

            strList.TrimExcess();
            return strList.ToArray();
        }
        
        /// <summary>
        /// Check to see if a process is running or not.
        /// </summary>
        public static bool IsProcessRunning(this Process process, string processName)
        {
            foreach (Process proc in Process.GetProcesses())
            {
                var procName =  proc.ProcessName.Replace("System.Diagnostics.Process (", String.Empty);

                //Console.WriteLine(proc.ProcessName);

                processName = processName.Replace(".exe", String.Empty);
                
                if (procName.ToLower().Equals(processName.ToLower()) ||
                    procName.ToLower().Contains(processName.ToLower()))
                {
                    return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Converts a String to a Process
        /// </summary>
        /// <param name="process"></param>
        /// <param name="processName"></param>
        /// <returns></returns>
        public static Process ConvertStringToProcess(this Process process, string processName)
        {
            try
            {
                processName = processName.Replace(".exe", string.Empty);

                if (IsProcessRunning(process, processName) ||
                    IsProcessRunning(process, processName.ToLower()) ||
                    IsProcessRunning(process, processName.ToUpper())
                )
                {
                    Process[] processes = Process.GetProcesses();

                    foreach (Process p in processes)
                    {
                        if (p.ProcessName.ToLower().Equals(processName.ToLower()))
                        {
                            return p;
                        }
                    }
                }

                return null;
                //  throw new Exception();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
    }
}