/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
     any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace PlatformKit.Shared.Extensions
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
    }
}