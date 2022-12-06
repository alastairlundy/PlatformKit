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
using PlatformKit.Internal.Analytics;
using PlatformKit.Internal.Licensing;


namespace PlatformKit.Windows
{
    public class WinManagementObjectSearcher
    {
        protected ProcessManager processManager;

        public WinManagementObjectSearcher()
        {
            processManager = new ProcessManager();

            PlatformKitConstants.CheckLicenseState();
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
                        PlatformKitAnalytics.ReportError(new ArgumentException(), nameof(Get));
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
                    PlatformKitAnalytics.ReportError(new PlatformNotSupportedException(), nameof(Get));
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