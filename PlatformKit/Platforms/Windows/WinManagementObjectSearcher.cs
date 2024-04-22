/*
        MIT License
       
       Copyright (c) 2020-2024 Alastair Lundy
       
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
using System.Runtime.Versioning;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Windows
{
    public class WinManagementObjectSearcher
    {

        /// <summary>
        /// Returns a Dictionary of Query objects and their associated WMI values.
        /// WARNING: DO NOT RUN on NON-Windows platforms. This will result in errors.
        /// </summary>
        /// <param name="queryObjectsList"></param>
        /// <param name="wmiClass"></param>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public static Dictionary<string, string> Get(List<string> queryObjectsList, string wmiClass)
        {
            Dictionary<string, string> queryObjectsDictionary = new Dictionary<string, string>();
            
                if (OperatingSystem.IsWindows())
                {
                    string output = CommandRunner.RunPowerShellCommand("Get-WmiObject -Class " + wmiClass + " | Select-Object *")
                        .Replace(wmiClass, string.Empty);

                    if (output == null)
                    {
                        throw new ArgumentNullException();
                    }
                    
                    foreach (string query in queryObjectsList)
                    {
                        if (query.Contains(output))
                        {
                            string value = output.Replace(query + "                         : ", string.Empty);
                            queryObjectsDictionary.Add(query, value);
                        }
                    }
                    
                    return queryObjectsDictionary;
                }

                throw new PlatformNotSupportedException();
        }
    }
}