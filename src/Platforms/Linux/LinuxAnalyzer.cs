/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
<<<<<<< HEAD
 You can view the PlatformKit Licenses at https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt

 To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
=======
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
>>>>>>> main
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;
using System.IO;

using PlatformKit.Internal.Licensing;

namespace PlatformKit.Linux;

/// <summary>
/// 
/// </summary>
public class LinuxAnalyzer
{

    public LinuxAnalyzer()
    {
        PlatformKitConstants.CheckLicenseState();
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
                   
                 #if DEBUG   
                    Console.WriteLine("After: " + resultArray[index]);
                    #endif
                }
            
                return linuxDistributionInformation;
            }

            throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Detects the Linux Distribution Version as read from cat /etc/os-release and reformats it into the format of System.Version object
        ///  WARNING: DOES NOT PRESERVE the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns></returns>
        public Version DetectLinuxDistributionVersion()
        {
            if (OSAnalyzer.IsLinux())
            {
                var dotCounter = 0;

                var version = DetectLinuxDistributionVersionAsString();

                foreach (var c in version)
                {
                    if (c == '.')
                    {
                        dotCounter++;
                    }
                }

                if (dotCounter == 1)
                {
                    version += ".0";
                }
                else if (dotCounter == 2)
                {
                    version += ".0";
                }

                return Version.Parse(version);
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Detects the Linux Distribution Version as read from cat /etc/os-release.
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