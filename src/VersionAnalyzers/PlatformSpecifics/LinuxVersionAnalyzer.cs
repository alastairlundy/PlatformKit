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
using PlatformKit.Software.Shared.Analyzers;
using PlatformKit.Software.Shared.Analyzers.PlatformSpecifics;

//Move namespace in v3.
namespace PlatformKit.Software.Shared.VersionAnalyzers.PlatformSpecifics;

/// <summary>
/// 
/// </summary>
public static class LinuxVersionAnalyzer
{

        /// <summary>
        /// Detects the Linux Distribution Version as read from cat /etc/os-release and reformats it into the format of System.Version object
        ///  WARNING: DOES NOT PRESERVE the version if the full version is in a Year.Month.Bugfix format.
        /// </summary>
        /// <returns></returns>
        public static Version DetectLinuxDistributionVersion(this OSVersionAnalyzer osVersionAnalyzer)
        {
            if (new OSAnalyzer().IsLinux())
            {
                var dotCounter = 0;

                var version = DetectLinuxDistributionVersionAsString(osVersionAnalyzer);

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
        public static string DetectLinuxDistributionVersionAsString(this OSVersionAnalyzer osVersionAnalyzer)
        {
            var osAnalyzer = new OSAnalyzer();

            if (osAnalyzer.IsLinux())
            {
                var linuxDistroInfo = osAnalyzer.GetLinuxDistributionInformation();

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
        public static Version DetectLinuxKernelVersion(this OSVersionAnalyzer osVersionAnalyzer)
        {
            if (new OSAnalyzer().IsLinux())
            {
                var description = Environment.OSVersion.ToString()
                    .Replace("Unix ", string.Empty);

                return Version.Parse(description);
            }

            throw new PlatformNotSupportedException();
        }
}