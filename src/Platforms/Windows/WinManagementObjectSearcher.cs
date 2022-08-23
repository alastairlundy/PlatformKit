
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

                throw new Exception(ex.ToString());
            }
        }
    }
}