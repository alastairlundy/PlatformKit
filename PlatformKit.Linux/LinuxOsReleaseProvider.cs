/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;

#else
using System.Runtime.Versioning;
#endif

using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

using AlastairLundy.Extensions.System.Strings;

using PlatformKit.Linux.Abstractions;

using PlatformKit.Linux.Internal.Localizations;

namespace PlatformKit.Linux;

public class LinuxOsReleaseProvider : ILinuxOsReleaseProvider
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public async Task<string> GetPropertyValueAsync(string propertyName)
    {
        LinuxOsReleaseInfo output = new LinuxOsReleaseInfo();
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
        
        string result = resultArray.First(x => x.Contains(propertyName));
        
        return result.Replace(propertyName, string.Empty)
            .Replace("=", string.Empty);
    }

#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
    public async Task<LinuxOsReleaseInfo> GetReleaseInfoAsync()
    {
        LinuxOsReleaseInfo output = new LinuxOsReleaseInfo();
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

        return await Task.FromResult(ParseOsReleaseInfo(resultArray));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]    
#endif
    public async Task<LinuxDistroBase> GetDistroBaseAsync()
    {
        if (OperatingSystem.IsLinux() == false)
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
        }
        
        LinuxOsReleaseInfo osReleaseInfo = await GetReleaseInfoAsync();
        
        string identifierLike = osReleaseInfo.Identifier_Like.ToLower();
            
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

    private LinuxOsReleaseInfo ParseOsReleaseInfo(string[] resultArray)
    {
        LinuxOsReleaseInfo linuxDistroInfo = new LinuxOsReleaseInfo();
            
            string[] releaseInfo = resultArray.Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();
            
            for (int index = 0; index < releaseInfo.Length; index++)
            {
                string line = releaseInfo[index].ToUpper();

                if (line.Contains("NAME=") && !line.Contains("VERSION"))
                {

                    if (line.StartsWith("PRETTY_"))
                    {
                        linuxDistroInfo.PrettyName =
                            releaseInfo[index].Replace("PRETTY_NAME=", string.Empty);
                    }

                    if (!line.Contains("PRETTY") && !line.Contains("CODE"))
                    {
                        linuxDistroInfo.Name = releaseInfo[index]
                            .Replace("NAME=", string.Empty);
                    }
                }

                if (line.Contains("VERSION="))
                {
                    if (line.Contains("LTS"))
                    {
                        linuxDistroInfo.IsLongTermSupportRelease = true;
                    }
                    else
                    {
                        linuxDistroInfo.IsLongTermSupportRelease = false;
                    }

                    if (line.Contains("ID="))
                    {
                        linuxDistroInfo.VersionId =
                            releaseInfo[index].Replace("VERSION_ID=", string.Empty);
                    }
                    else if (!line.Contains("ID=") && line.Contains("CODE"))
                    {
                        linuxDistroInfo.VersionCodename =
                            releaseInfo[index].Replace("VERSION_CODENAME=", string.Empty);
                    }
                    else if (!line.Contains("ID=") && !line.Contains("CODE"))
                    {
                        linuxDistroInfo.Version = releaseInfo[index].Replace("VERSION=", string.Empty)
                            .Replace("LTS", string.Empty);
                    }
                }

                if (line.Contains("ID"))
                {
                    if (line.Contains("ID_LIKE="))
                    {
                        linuxDistroInfo.Identifier_Like =
                            releaseInfo[index].Replace("ID_LIKE=", string.Empty);

                        if (linuxDistroInfo.Identifier_Like.ToLower().Contains("ubuntu") &&
                            linuxDistroInfo.Identifier_Like.ToLower().Contains("debian"))
                        {
                            linuxDistroInfo.Identifier_Like = "ubuntu";
                        }
                    }
                    else if (!line.Contains("VERSION"))
                    {
                        linuxDistroInfo.Identifier = releaseInfo[index].Replace("ID=", string.Empty);
                    }
                }

                if (line.Contains("URL="))
                {
                    if (line.StartsWith("HOME_"))
                    {
                        linuxDistroInfo.HomeUrl = releaseInfo[index].Replace("HOME_URL=", string.Empty);
                    }
                    else if (line.StartsWith("SUPPORT_"))
                    {
                        linuxDistroInfo.SupportUrl =
                            releaseInfo[index].Replace("SUPPORT_URL=", string.Empty);
                    }
                    else if (line.StartsWith("BUG_"))
                    {
                        linuxDistroInfo.BugReportUrl =
                            releaseInfo[index].Replace("BUG_REPORT_URL=", string.Empty);
                    }
                    else if (line.StartsWith("PRIVACY_"))
                    {
                        linuxDistroInfo.PrivacyPolicyUrl =
                            releaseInfo[index].Replace("PRIVACY_POLICY_URL=", string.Empty);
                    }
                }
            }

            return linuxDistroInfo;
    }

    private string[] RemoveUnwantedCharacters(string[] resultArray)
    {
        char[] delimiter = ['\t', '"'];

        resultArray = resultArray.Where(x => string.IsNullOrWhiteSpace(x) == false)
            .Select(x => x.RemoveEscapeCharacters()).ToArray();
            
        foreach (char c in delimiter)
        {
            resultArray = resultArray.Select(x => x.Replace(c.ToString(), string.Empty)).ToArray();
        }

        return resultArray;
    }
}