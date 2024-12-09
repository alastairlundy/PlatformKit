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
using System.Threading.Tasks;
using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems.Linux.Extensions
{
    public static class LinuxOsInformationExtensions
    {
        /// <summary>
        /// Detects what base Linux Distribution a Distro is based off of.
        /// </summary>
        /// <returns></returns>
        public static async Task<LinuxDistroBase> GetDistroBase(this LinuxOperatingSystem linuxOperatingSystem)
        {
            LinuxOsReleaseModel osReleaseModel = await linuxOperatingSystem.GetLinuxOsReleaseAsync();
            return GetDistroBase(linuxOperatingSystem, osReleaseModel);
        }

        /// <summary>
        /// Detects what base Linux Distribution a Distro is based off of.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="System.PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        public static LinuxDistroBase GetDistroBase(this LinuxOperatingSystem linuxOperatingSystem, LinuxOsReleaseModel linuxOsRelease)
        {
            if (OperatingSystem.IsLinux() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }
            
            string identifierLike = linuxOsRelease.Identifier_Like.ToLower();
            
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

        /// <summary>
        /// Detects the Linux Distribution Version as read from /etc/os-release.
        /// Preserves the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns>the version of the linux distribution being run as a string.</returns>
        /// <exception cref="System.PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        public static string GetLinuxDistributionVersionAsString(this LinuxOperatingSystem linuxOperatingSystem, LinuxOsReleaseModel osReleaseModel)
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
    }
}