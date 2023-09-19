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

namespace PlatformKit.Windows
{
    public class WinManagementObjectSearcher
    {
        protected ProcessManager processManager;

        public WinManagementObjectSearcher()
        {
            processManager = new ProcessManager();
        }

        /// <summary>
        ///
        /// WARNING: DO NOT RUN on NON-Windows platforms. This will result in errors.
        /// </summary>
        /// <param name="queryObjectsList"></param>
        /// <param name="wmiClass"></param>
        /// <returns></returns>
        public Dictionary<string, string> Get(List<string> queryObjectsList, string wmiClass)
        {
            Dictionary<string, string> queryObjectsDictionary = new Dictionary<string, string>();
            
            try
            {
                if (OSAnalyzer.IsWindows())
                {
                    var output = processManager.RunPowerShellCommand("Get-WmiObject -Class " + wmiClass + " | Select-Object *")
                        .Replace(wmiClass, string.Empty);

                    if (output == null)
                    {
                     //   PlatformKitAnalytics.ReportError(new ArgumentException(), nameof(Get));
                        throw new ArgumentNullException();
                    }
                    
                    foreach (string query in queryObjectsList)
                    {
                        if (query.Contains(output))
                        {
                            var value = output.Replace(query + "                         : ", string.Empty);
                            queryObjectsDictionary.Add(query, value);
                        }
                    }
                    
                    return queryObjectsDictionary;
                }
                else
                {
             //       PlatformKitAnalytics.ReportError(new PlatformNotSupportedException(), nameof(Get));
                    throw new PlatformNotSupportedException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);

                throw new Exception(ex.ToString());
            }
        }
    }
}