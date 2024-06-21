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
using System.IO;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Linux;

public static class LinuxOsReleaseRetriever
{
    /// <summary>
    /// Detects Linux Os Release information and returns it.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
    public static LinuxOsRelease GetLinuxOsRelease()
    {
        LinuxOsRelease linuxDistributionInformation = new LinuxOsRelease();

        //Assign a default value.
        linuxDistributionInformation.IsLongTermSupportRelease = false;

        if (OperatingSystem.IsLinux())
        {
            string[] resultArray = File.ReadAllLines("/etc/os-release");

            resultArray = RemoveUnwantedCharacters(resultArray);

            for (int index = 0; index < resultArray.Length; index++)
            {
                string line = resultArray[index].ToUpper();

                if (line.Contains("NAME=") && !line.Contains("VERSION"))
                {
                    if (line.Contains("CODE"))
                    {

                    }

                    if (line.StartsWith("PRETTY_"))
                    {
                        linuxDistributionInformation.PrettyName =
                            resultArray[index].Replace("PRETTY_NAME=", string.Empty);
                    }

                    if (!line.Contains("PRETTY") && !line.Contains("CODE"))
                    {
                        linuxDistributionInformation.Name = resultArray[index].Replace("NAME=", string.Empty);
                    }
                }

                if (line.Contains("VERSION="))
                {
                    if (line.Contains("LTS"))
                    {
                        linuxDistributionInformation.IsLongTermSupportRelease = true;
                    }
                    else
                    {
                        linuxDistributionInformation.IsLongTermSupportRelease = false;
                    }

                    if (line.Contains("ID="))
                    {
                        linuxDistributionInformation.VersionId =
                            resultArray[index].Replace("VERSION_ID=", string.Empty);
                    }
                    else if (!line.Contains("ID=") && line.Contains("CODE"))
                    {
                        linuxDistributionInformation.VersionCodename =
                            resultArray[index].Replace("VERSION_CODENAME=", string.Empty);
                    }
                    else if (!line.Contains("ID=") && !line.Contains("CODE"))
                    {
                        linuxDistributionInformation.Version = resultArray[index].Replace("VERSION=", string.Empty)
                            .Replace("LTS", string.Empty);
                    }
                }

                if (line.Contains("ID"))
                {
                    if (line.Contains("ID_LIKE="))
                    {
                        linuxDistributionInformation.Identifier_Like =
                            resultArray[index].Replace("ID_LIKE=", string.Empty);

                        if (linuxDistributionInformation.Identifier_Like.ToLower().Contains("ubuntu") &&
                            linuxDistributionInformation.Identifier_Like.ToLower().Contains("debian"))
                        {
                            linuxDistributionInformation.Identifier_Like = "ubuntu";
                        }
                    }
                    else if (!line.Contains("VERSION"))
                    {
                        linuxDistributionInformation.Identifier = resultArray[index].Replace("ID=", string.Empty);
                    }
                }

                if (line.Contains("URL="))
                {
                    if (line.StartsWith("HOME_"))
                    {
                        linuxDistributionInformation.HomeUrl = resultArray[index].Replace("HOME_URL=", string.Empty);
                    }
                    else if (line.StartsWith("SUPPORT_"))
                    {
                        linuxDistributionInformation.SupportUrl =
                            resultArray[index].Replace("SUPPORT_URL=", string.Empty);
                    }
                    else if (line.StartsWith("BUG_"))
                    {
                        linuxDistributionInformation.BugReportUrl =
                            resultArray[index].Replace("BUG_REPORT_URL=", string.Empty);
                    }
                    else if (line.StartsWith("PRIVACY_"))
                    {
                        linuxDistributionInformation.PrivacyPolicyUrl =
                            resultArray[index].Replace("PRIVACY_POLICY_URL=", string.Empty);
                    }
                }
            }

            return linuxDistributionInformation;
        }

        throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static Version GetDistributionVersion()
    {
        if (OperatingSystem.IsLinux())
        {
            return GetDistributionVersion(GetLinuxOsRelease());
        }

        throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="osRelease"></param>
    /// <returns></returns>
    public static Version GetDistributionVersion(LinuxOsRelease osRelease)
    {
        return Version.Parse(osRelease.Version);
    }

internal static string[] RemoveUnwantedCharacters(string[] data)
    {
        char[] delimiter = [' ', '\t', '\n', '\r', '"'];

        for (int i = 0; i < data.Length; i++)
        {
            foreach (char c in delimiter)
            {
                data[i] = data[i].Replace(c.ToString(), string.Empty);
            }
        }

        return data;
    }
    
    
}