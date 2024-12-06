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
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.Extensions.System.Strings.EscapeCharacters;

using PlatformKit.Internal.Localizations;
using PlatformKit.Linux.Enums;
using PlatformKit.Linux.Models;

#if NET5_0_OR_GREATER
using System.Runtime.Versioning;
#endif

#if NETSTANDARD2_0
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.Linux;

public static class LinuxOsReleaseRetriever
{
    
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static async Task<LinuxOsReleaseModel> GetLinuxOsReleaseAsync()
    {
        LinuxOsReleaseModel output = new LinuxOsReleaseModel();
        //Assign a default value.

        output.IsLongTermSupportRelease = false;
        
        if (OperatingSystem.IsLinux() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        }

#if NET6_0_OR_GREATER
        string[] resultArray = await File.ReadAllLinesAsync("/etc/os-release");
#else
        string[] resultArray = await Task.Run(() => File.ReadAllLines("/etc/os-release"));
#endif
        
        resultArray = RemoveUnwantedCharacters(resultArray);

        return await Task.Run(()=> ParseOsReleaseInfo(resultArray));
    }

#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    private static LinuxOsReleaseModel ParseOsReleaseInfo(IEnumerable<string> osReleaseInfo)
    {
        LinuxOsReleaseModel linuxDistributionInformation = new LinuxOsReleaseModel();

        var releaseInfo = osReleaseInfo as string[] ?? osReleaseInfo.ToArray();
        for (int index = 0; index < releaseInfo.Count(); index++)
        {
            string line = releaseInfo[index].ToUpper();

            if (line.Contains("NAME=") && !line.Contains("VERSION"))
            {

                if (line.StartsWith("PRETTY_"))
                {
                    linuxDistributionInformation.PrettyName =
                        releaseInfo[index].Replace("PRETTY_NAME=", string.Empty);
                }

                if (!line.Contains("PRETTY") && !line.Contains("CODE"))
                {
                    linuxDistributionInformation.Name = releaseInfo[index]
                        .Replace("NAME=", string.Empty);
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
                        releaseInfo[index].Replace("VERSION_ID=", string.Empty);
                }
                else if (!line.Contains("ID=") && line.Contains("CODE"))
                {
                    linuxDistributionInformation.VersionCodename =
                        releaseInfo[index].Replace("VERSION_CODENAME=", string.Empty);
                }
                else if (!line.Contains("ID=") && !line.Contains("CODE"))
                {
                    linuxDistributionInformation.Version = releaseInfo[index].Replace("VERSION=", string.Empty)
                        .Replace("LTS", string.Empty);
                }
            }

            if (line.Contains("ID"))
            {
                if (line.Contains("ID_LIKE="))
                {
                    linuxDistributionInformation.Identifier_Like =
                        releaseInfo[index].Replace("ID_LIKE=", string.Empty);

                    if (linuxDistributionInformation.Identifier_Like.ToLower().Contains("ubuntu") &&
                        linuxDistributionInformation.Identifier_Like.ToLower().Contains("debian"))
                    {
                        linuxDistributionInformation.Identifier_Like = "ubuntu";
                    }
                }
                else if (!line.Contains("VERSION"))
                {
                    linuxDistributionInformation.Identifier = releaseInfo[index].Replace("ID=", string.Empty);
                }
            }

            if (line.Contains("URL="))
            {
                if (line.StartsWith("HOME_"))
                {
                    linuxDistributionInformation.HomeUrl = releaseInfo[index].Replace("HOME_URL=", string.Empty);
                }
                else if (line.StartsWith("SUPPORT_"))
                {
                    linuxDistributionInformation.SupportUrl =
                        releaseInfo[index].Replace("SUPPORT_URL=", string.Empty);
                }
                else if (line.StartsWith("BUG_"))
                {
                    linuxDistributionInformation.BugReportUrl =
                        releaseInfo[index].Replace("BUG_REPORT_URL=", string.Empty);
                }
                else if (line.StartsWith("PRIVACY_"))
                {
                    linuxDistributionInformation.PrivacyPolicyUrl =
                        releaseInfo[index].Replace("PRIVACY_POLICY_URL=", string.Empty);
                }
            }
        }

        return linuxDistributionInformation;
    }
    
    
    /// <summary>
    /// Detects Linux Os Release information and returns it.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static LinuxOsReleaseModel GetLinuxOsRelease()
    {
        LinuxOsReleaseModel linuxDistributionInformation = new LinuxOsReleaseModel();

        //Assign a default value.
        linuxDistributionInformation.IsLongTermSupportRelease = false;

        if (OperatingSystem.IsLinux() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        }
        
        string[] resultArray = File.ReadAllLines("/etc/os-release");
        resultArray = RemoveUnwantedCharacters(resultArray);

        for (int index = 0; index < resultArray.Length; index++)
        {
            string line = resultArray[index].ToUpper();

            if (line.Contains("NAME=") && !line.Contains("VERSION"))
            {

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

    /// <summary>
    /// Detects the Linux Distribution Version as read from /etc/os-release.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based operating system.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static Version GetDistributionVersion()
    {
        if (OperatingSystem.IsLinux())
        {
            return GetDistributionVersion(GetLinuxOsRelease());
        }

        throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
    }

    /// <summary>
    /// Detects the Linux Distribution Version from the LinuxOsReleaseModel object.
    /// </summary>
    /// <param name="osRelease"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static Version GetDistributionVersion(LinuxOsReleaseModel osRelease)
    {
        return Version.Parse(osRelease.Version);
    }

    /// <summary>
    /// Detects what base Linux Distribution a Distro is based off of.
    /// </summary>
    /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static LinuxDistroBase GetDistroBase()
    {
        return GetDistroBase(GetLinuxOsRelease());
    }
    
    /// <summary>
    /// Detects what base Linux Distribution a Distro is based off of.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static LinuxDistroBase GetDistroBase(LinuxOsReleaseModel linuxOsRelease)
    {
        string identifierLike = linuxOsRelease.Identifier_Like.ToLower();
            
        if (OperatingSystem.IsLinux())
        {
            return identifierLike switch
            {
                "debian" => LinuxDistroBase.Debian,
                "ubuntu" => LinuxDistroBase.Ubuntu,
                "arch" => LinuxDistroBase.Arch,
                "manjaro" => LinuxDistroBase.Manjaro,
                "fedora" => LinuxDistroBase.Fedora,
                "rhel" or "oracle" or "centos" => LinuxDistroBase.RHEL,
                "suse" => LinuxDistroBase.SUSE,
                _ => LinuxDistroBase.NotDetected
            };
        }

        throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
    }

    /// <summary>
    /// Detects the Linux Distribution Version as read from /etc/os-release.
    /// Preserves the version if the full version is in a Year.Month.Bugfix format.
    /// </summary>
    /// <returns>the version of the linux distribution being run as a string.</returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static string GetLinuxDistributionVersionAsString()
    {
        return GetLinuxDistributionVersionAsString(GetLinuxOsRelease());
    }
    
    /// <summary>
    /// Detects the Linux Distribution Version as read from /etc/os-release.
    /// Preserves the version if the full version is in a Year.Month.Bugfix format.
    /// </summary>
    /// <returns>the version of the linux distribution being run as a string.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public static string GetLinuxDistributionVersionAsString(LinuxOsReleaseModel osReleaseModel)
    {
        if (OperatingSystem.IsLinux() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        }

        string osName = osReleaseModel.Name.ToLower();

        if ((osName.Contains("ubuntu") || osName.Contains("pop") || osName.Contains("buntu")) &&
            (osReleaseModel.Version.Contains(".4.") || osReleaseModel.Version.EndsWith(".4")))
        {
            //Properly show Year.Month.minor version for Date base distribution versioning such as Pop!_OS and Ubuntu.
            //This normally occurs with .04 being shown as .4
            osReleaseModel.Version = osReleaseModel.Version.Replace(".4", ".04");
        }
                
        return osReleaseModel.Version;
    }
    
    internal static string[] RemoveUnwantedCharacters(string[] data)
    {
        char[] delimiter = [' ', '\t', '\n', '\r', '"'];

        data = data.Where(x => x.Contains(" ")  == false)
            .Select(x => x.RemoveEscapeCharacters()).ToArray();
        
        foreach (char c in delimiter)
        {
            data = data.Select(x => x.Replace(c.ToString(), string.Empty)).ToArray();
        }

        return data;
    }
    
    
}