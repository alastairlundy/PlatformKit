/* MIT License

Copyright (c) 2018-2022 AluminiumTech

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
using System.IO;

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Linux;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;

public static class LinuxAnalyzer
{
    /// <summary>
        /// Detects Linux Distribution information and returns it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static LinuxOsRelease GetLinuxDistributionInformation(this OSAnalyzer osAnalyzer)
        {
            var linuxDistributionInformation = new LinuxOsRelease();

            //Assign a default value.
            linuxDistributionInformation.IsLongTermSupportRelease = false;

            if (osAnalyzer.IsLinux())
            {
                char[] delimiter = { ' ', '\t', '\n', '\r', '"' };
                
                string[] resultArray = File.ReadAllLines("/etc/os-release");

                for (int index = 0; index < resultArray.Length; index++)
                {
                    foreach (var c in delimiter)
                    {
                        resultArray[index] = resultArray[index].Replace(c.ToString(), string.Empty);
                    }

                    if (resultArray[index].ToUpper().Contains("LTS"))
                    {
                        linuxDistributionInformation.IsLongTermSupportRelease = true;
                    }
                    else if (resultArray[index].ToUpper().Contains("NAME="))
                    {
                        resultArray[index] = resultArray[index].Replace("NAME=", string.Empty);
                        linuxDistributionInformation.Name = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("VERSION="))
                    {
                        resultArray[index] = resultArray[index].Replace("VERSION=", string.Empty);
                        linuxDistributionInformation.Version = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("ID="))
                    {
                        resultArray[index] = resultArray[index].Replace("ID=", string.Empty);
                        linuxDistributionInformation.Identifier = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("ID_LIKE="))
                    {
                        resultArray[index] = resultArray[index].Replace("ID_LIKE=", string.Empty);
                        linuxDistributionInformation.Identifier_Like = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("PRETTY_NAME="))
                    {
                        resultArray[index] = resultArray[index].Replace("PRETTY_NAME=", string.Empty);
                        linuxDistributionInformation.PrettyName = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("VERSION_ID="))
                    {
                        resultArray[index] = resultArray[index].Replace("VERSION_ID=", string.Empty);
                        linuxDistributionInformation.VersionId = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("HOME_URL="))
                    {
                        resultArray[index] = resultArray[index].Replace("HOME_URL=", string.Empty);
                        linuxDistributionInformation.HomeUrl = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("SUPPORT_URL="))
                    {
                        resultArray[index] = resultArray[index].Replace("SUPPORT_URL=", string.Empty);
                        linuxDistributionInformation.SupportUrl = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("BUG_REPORT_URL="))
                    {
                        resultArray[index] = resultArray[index].Replace("BUG_REPORT_URL=", string.Empty);
                        linuxDistributionInformation.BugReportUrl = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("PRIVACY_POLICY_URL="))
                    {
                        resultArray[index] = resultArray[index].Replace("PRIVACY_POLICY_URL=", string.Empty);
                        linuxDistributionInformation.PrivacyPolicyUrl = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("VERSION_CODENAME="))
                    {
                        resultArray[index] = resultArray[index].Replace("VERSION_CODENAME=", string.Empty);
                        linuxDistributionInformation.VersionCodename = resultArray[index];
                    }
                    
                    //Console.WriteLine("After: " + resultArray[index]);
                }
            
                return linuxDistributionInformation;
            }

            throw new PlatformNotSupportedException();
        }
}