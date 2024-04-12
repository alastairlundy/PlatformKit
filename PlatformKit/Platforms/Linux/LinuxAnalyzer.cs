/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.IO;

using PlatformKit.Internal.Exceptions;
using PlatformKit.Linux.Enums;
using PlatformKit.Shared.Extensions;

namespace PlatformKit.Linux;

/// <summary>
/// A class to Detect Linux versions, Linux features, and find out more about a user's Linux installation.
/// </summary>
public class LinuxAnalyzer
{
    public LinuxAnalyzer()
    {
        
    }

    /// <summary>
    /// Detects what base Linux Distribution a Distro is based off of.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public LinuxDistroBase DetectDistroBase()
    {
        if (OSAnalyzer.IsLinux())
        {
            var osRel = GetLinuxDistributionInformation();

            if (osRel.Identifier_Like.ToLower().Contains("debian"))
            {
                return LinuxDistroBase.Debian;
            }
            if (osRel.Identifier_Like.ToLower().Contains("ubuntu"))
            {
                return LinuxDistroBase.Ubuntu;
            }
            if (osRel.Identifier_Like.ToLower().Contains("arch"))
            {
                return LinuxDistroBase.Arch;
            }
            if (osRel.Identifier_Like.ToLower().Contains("fedora"))
            {
                return LinuxDistroBase.Fedora;
            }

            throw new OperatingSystemDetectionException();
        }

        throw new PlatformNotSupportedException();
    }
    
    
        /// <summary>
        /// Detects Linux Distribution information and returns it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">This method only runs on Linux. Running it on any other platform will throw this exception.</exception>
        public LinuxOsRelease GetLinuxDistributionInformation()
        {
            var linuxDistributionInformation = new LinuxOsRelease();

            //Assign a default value.
            linuxDistributionInformation.IsLongTermSupportRelease = false;

            if (OSAnalyzer.IsLinux())
            {
                char[] delimiter = { ' ', '\t', '\n', '\r', '"' };
                
                string[] resultArray = File.ReadAllLines("/etc/os-release");

                for (int index = 0; index < resultArray.Length; index++)
                {
                    foreach (var c in delimiter)
                    {
                        resultArray[index] = resultArray[index].Replace(c.ToString(), string.Empty);
                    }
                    
                    if (resultArray[index].ToUpper().Contains("NAME=") 
                        && !resultArray[index].ToUpper().Contains("CODE") 
                        && !resultArray[index].ToUpper().Contains("PRETTY"))
                    {
                        resultArray[index] = resultArray[index].Replace("NAME=", string.Empty);
                        linuxDistributionInformation.Name = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("VERSION="))
                    {
                        if (resultArray[index].ToUpper().Contains("LTS"))
                        {
                            linuxDistributionInformation.IsLongTermSupportRelease = true;
                        }
                        
                        resultArray[index] = resultArray[index].Replace("VERSION=", string.Empty).Replace("LTS", String.Empty);
                        
                        linuxDistributionInformation.Version = resultArray[index];
                    }
                    else if (resultArray[index].ToUpper().Contains("ID=") && !resultArray[index].ToUpper().StartsWith("VERSION_"))
                    {
                        resultArray[index] = resultArray[index].Replace("ID=", string.Empty);
                        linuxDistributionInformation.Identifier = resultArray[index];
                        
                       // Console.WriteLine(linuxDistributionInformation.Identifier);
                    }
                    else if (resultArray[index].ToUpper().Contains("ID_LIKE="))
                    {
                        resultArray[index] = resultArray[index].Replace("ID_LIKE=", string.Empty);
                        linuxDistributionInformation.Identifier_Like = resultArray[index];

                        if (linuxDistributionInformation.Identifier_Like.ToLower().Contains("ubuntu") &&
                            linuxDistributionInformation.Identifier_Like.ToLower().Contains("debian"))
                        {
                            linuxDistributionInformation.Identifier_Like = "ubuntu";
                        }
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
                }
            
                return linuxDistributionInformation;
            }
            
            throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Detects the Linux Distribution Version as read from /etc/os-release and re-formats it into the format of System.Version object
        ///  WARNING: DOES NOT PRESERVE the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns></returns>
        public Version DetectLinuxDistributionVersion()
        {
            if (OSAnalyzer.IsLinux())
            {
                var version = DetectLinuxDistributionVersionAsString();

                return Version.Parse(version.AddMissingZeroes());
            }
            
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Detects the Linux Distribution Version as read from /etc/os-release.
        /// Preserves the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception when run on Windows or macOS.</exception>
        public string DetectLinuxDistributionVersionAsString()
        {
            if (OSAnalyzer.IsLinux())
            {
                var linuxDistroInfo = GetLinuxDistributionInformation();
                var osName = linuxDistroInfo.Name.ToLower();

                if (osName.ToLower().Contains("ubuntu") ||
                    osName.ToLower().Contains("pop") || osName.ToLower().Contains("buntu"))
                {
                    if (linuxDistroInfo.Version.Contains(".4.") || linuxDistroInfo.Version.EndsWith(".4"))
                    {
                        //Properly show Year.Month.minor version for Date base distribution versioning such as Pop!_OS and Ubuntu.
                        //This normally occurs with .04 being shown as .4
                        linuxDistroInfo.Version = linuxDistroInfo.Version.Replace(".4", ".04");
                    }
                }

                return linuxDistroInfo.Version;
            }
            
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Detects the linux kernel version to string.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">
        ///  Throws a platform not supported exception if run on Windows, macOS, or any platform that isn't Linux.
        /// </exception>
        public Version DetectLinuxKernelVersion()
        {
            if (OSAnalyzer.IsLinux())
            {
                var description = Environment.OSVersion.ToString()
                    .Replace("Unix ", string.Empty);

                return Version.Parse(description);
            }
            
            throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Returns whether the installed Linux Kernel version is equal to or newer than the Kernel Version provided as a parameter
        /// </summary>
        /// <param name="linuxKernelVersion">The Kernel Version to compare against.</param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public bool IsAtLeastKernelVersion(Version linuxKernelVersion)
        {
            if (OSAnalyzer.IsLinux())
            {
                var detected = DetectLinuxKernelVersion();
                var expected = linuxKernelVersion;

                if (detected.Major > expected.Major)
                {
                    return true;
                }
                else if(detected.Major == expected.Major)
                {
                    if (detected.Minor > expected.Minor)
                    {
                        return true;
                    }
                    else if (detected.Minor == expected.Minor)
                    {
                        if (detected.Build > expected.Build)
                        {
                            return true;
                        }
                        else if (detected.Build == expected.Build)
                        {
                            if (detected.Revision > expected.Revision)
                            {
                                return true;
                            }
                            else if (detected.Revision == expected.Revision)
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            throw new PlatformNotSupportedException();
        }
}