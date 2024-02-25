/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
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
                    throw new PlatformNotSupportedException();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }
        }
    }
}