/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt

 To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using PlatformKit.Internal.Licensing;

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
            PlatformKitConstants.CheckLicenseState();

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
            PlatformKitConstants.CheckLicenseState();


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
                  PlatformKitConstants.CheckLicenseState();

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
           //     PlatformKitAnalytics.ReportError(new Exception(exception.ToString()), nameof(ConvertStringToProcess));
                throw new Exception(exception.ToString());
            }
        }
    }
}