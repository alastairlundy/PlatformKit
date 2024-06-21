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

using AlastairLundy.Extensions.System.StringExtensions;
using AlastairLundy.Extensions.System.VersionExtensions;

using PlatformKit.Internal.Deprecation;
using PlatformKit.Linux.Enums;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Linux;

/// <summary>
/// A class to Detect Linux versions, Linux features, and find out more about a user's Linux installation.
/// </summary>
public class LinuxAnalyzer
{

        /// <summary>
        /// Detects what base Linux Distribution a Distro is based off of.
        /// </summary>
        /// <returns>the distro base of the currently running Linux Distribution</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        public static LinuxDistroBase GetDistroBase()
        {
            if (OperatingSystem.IsLinux())
            {
                return GetDistroBase(GetLinuxDistributionInformation());
            }

            throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Detects what base Linux Distribution a Distro is based off of.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        public static LinuxDistroBase GetDistroBase(LinuxOsRelease linuxOsRelease)
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

            throw new PlatformNotSupportedException();
        }
    
        /// <summary>
        /// Detects Linux Distribution information and returns it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static LinuxOsRelease GetLinuxDistributionInformation()
        {
            if (OperatingSystem.IsLinux())
            {
                return LinuxOsReleaseRetriever.GetLinuxOsRelease();
            }
            
            throw new PlatformNotSupportedException();
        }
        
 
        /// <summary>
        /// Detects the Linux Distribution Version as read from /etc/os-release and re-formats it into the format of System.Version object
        ///  WARNING: DOES NOT PRESERVE the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns>the version of the linux distribution being run.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        public static Version GetLinuxDistributionVersion()
        {
            if (OperatingSystem.IsLinux())
            {
                return Version.Parse(GetLinuxDistributionVersionAsString().AddMissingZeroes());
            }
            
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Detects the Linux Distribution Version as read from /etc/os-release.
        /// Preserves the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns>the version of the linux distribution being run as a string.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        public static string GetLinuxDistributionVersionAsString()
        {
            if (OperatingSystem.IsLinux())
            {
                LinuxOsRelease linuxDistroInfo = GetLinuxDistributionInformation();

                string osName = linuxDistroInfo.Name.ToLower();

                if ((osName.Contains("ubuntu") || osName.Contains("pop") || osName.Contains("buntu")) &&
                (linuxDistroInfo.Version.Contains(".4.") || linuxDistroInfo.Version.EndsWith(".4")))
                {
                    //Properly show Year.Month.minor version for Date base distribution versioning such as Pop!_OS and Ubuntu.
                    //This normally occurs with .04 being shown as .4
                    linuxDistroInfo.Version = linuxDistroInfo.Version.Replace(".4", ".04");
                }
              

                return linuxDistroInfo.Version;
            }
            
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Detects the linux kernel version to string.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        public static Version GetLinuxKernelVersion()
        {
            if (OperatingSystem.IsLinux())
            {
                return Version.Parse(Environment.OSVersion.ToString()
                    .Replace("Unix ", string.Empty)); 
            }
            
            throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Returns whether the installed Linux Kernel version is equal to or newer than the Kernel Version provided as a parameter
        /// </summary>
        /// <param name="linuxKernelVersion">The Kernel Version to compare against.</param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static bool IsAtLeastKernelVersion(Version linuxKernelVersion)
        {
            if (OperatingSystem.IsLinux())
            {
                return GetLinuxKernelVersion().IsAtLeast(linuxKernelVersion);
            }

            throw new PlatformNotSupportedException();
        }
}