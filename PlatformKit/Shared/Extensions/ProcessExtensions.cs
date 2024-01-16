/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
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
        public static string[] ToStringArray(this Process process)
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
        /// Check to see if a specified process is running or not.
        /// </summary>
        /// <param name="process"></param>
        /// <param name="processName"></param>
        /// <returns></returns>
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