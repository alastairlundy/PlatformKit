/*
        MIT License

       Copyright (c) 2020-2025 Alastair Lundy

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

using AlastairLundy.Extensions.System.Strings;

using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

namespace PlatformKit.OperatingSystems.Linux.Extensions
{
    public static class LinuxOsReleaseExtensibilityExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="linuxOperatingSystem"></param>
        /// <returns></returns>
        public static async Task<LinuxOsReleaseModel> GetLinuxOsReleaseAsync(this LinuxOperatingSystem linuxOperatingSystem)
        {
            return await GetLinuxOsReleaseAsync();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
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

            return await Task.FromResult(ParseOsReleaseInfo(resultArray));
        }

        private static LinuxOsReleaseModel ParseOsReleaseInfo(IEnumerable<string> osReleaseInfo)
        {
            LinuxOsReleaseModel linuxDistroInfo = new LinuxOsReleaseModel();
            
            string[] releaseInfo = osReleaseInfo.Where(x => string.IsNullOrWhiteSpace(x) == false).ToArray();
            
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

        private static string[] RemoveUnwantedCharacters(string[] data)
        {
            char[] delimiter = ['\t', '\n', '\r', '"'];

            data = data.Where(x => string.IsNullOrWhiteSpace(x) == false)
                .Select(x => x.RemoveEscapeCharacters()).ToArray();
            
                foreach (char c in delimiter)
                {
                    data = data.Select(x => x.Replace(c.ToString(), string.Empty)).ToArray();
                }

            return data;
        }
    }
}