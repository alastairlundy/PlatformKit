/* MIT License

Copyright (c) 2018-2021 AluminiumTech

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

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Linux;

// ReSharper disable InconsistentNaming

namespace AluminiumTech.DevKit.PlatformKit.Analyzers
{
    public class OSAnalyzer
    {
        public OSAnalyzer()
        {
   
        }

                /// <summary>
        /// Returns whether or not the current OS is Windows.
        /// </summary>
        /// <returns></returns>
        public bool IsWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
        }

        /// <summary>
        /// Returns whether or not the current OS is macOS.
        /// </summary>
        /// <returns></returns>
        public bool IsMac()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
        }
        
        /// <summary>
        /// Returns whether or not the current OS is Linux based.
        /// </summary>
        /// <returns></returns>
        public bool IsLinux()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an error if run on .NET Standard 2 or .NET Core 2.1 or earlier.</exception>
        public bool IsFreeBSD()
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD);
#else
            throw new PlatformNotSupportedException();
#endif
        }
        
        /// <summary>
        /// Determine what OS is being run
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public System.Runtime.InteropServices.OSPlatform GetOSPlatform() {
            System.Runtime.InteropServices.OSPlatform osPlatform = System.Runtime.InteropServices.OSPlatform.Create("Other Platform");
            // Check if it's windows
            osPlatform = IsWindows() ? System.Runtime.InteropServices.OSPlatform.Windows : osPlatform;
            // Check if it's osx
            osPlatform = IsMac() ? System.Runtime.InteropServices.OSPlatform.OSX : osPlatform;
            // Check if it's Linux
            osPlatform = IsLinux() ? System.Runtime.InteropServices.OSPlatform.Linux : osPlatform;
            
#if NETCOREAPP3_0_OR_GREATER
            // Check if it's FreeBSD
            osPlatform = IsFreeBSD() ? System.Runtime.InteropServices.OSPlatform.FreeBSD : osPlatform;
#endif

            return osPlatform;
        }
        
        /// <summary>
        /// Detects Linux Distribution information and returns it.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public LinuxOsRelease GetLinuxDistributionInformation()
        {
            var linuxDistributionInformation = new LinuxOsRelease();

            //Assign a default value.
            linuxDistributionInformation.IsLongTermSupportRelease = false;

            if (IsLinux())
            {
                var procManager = new ProcessManager();
                var result = procManager.RunCommandLinux("cat /etc/os-release");

                char[] delimiter = { ' ', '=', '\t', '\n', '\r' };
                var resultArray = result.Split(delimiter);

                for (var index = 0; index < resultArray.Length; index++)
                {
                    /*
#if DEBUG
                    Console.WriteLine("Now: " + resultArray[index]);
                    
                    if ((index + 1) < resultArray.Length)
                    {
                        Console.WriteLine("Next: " + resultArray[index + 1]);
                    }
#endif
*/
                    var isVersionLine = false;

                    for (var i = 0; i < 10; i++)
                    {
                        if (index > 0)
                        {
                            if (resultArray[index].Contains(i.ToString()) && resultArray[index].Contains(".") &&
                                resultArray[index - 1].Equals("VERSION") && !resultArray[index - 1].Contains("ID"))
                            {
                                isVersionLine = true;
                            }
                        }
                    }

                    if (isVersionLine)
                    {
                        var versionLine = resultArray[index];

                        versionLine = versionLine.Replace('"', ' ');
                        versionLine = versionLine.Replace(" ", string.Empty);
 /*                       
#if DEBUG
                        Console.WriteLine("LinuxDistroVersion: " + versionLine);
#endif
*/                     
                        linuxDistributionInformation.Version = versionLine;
                    }
                    else if (index < resultArray.Length)
                    {
                        if (resultArray[index].Equals("NAME"))
                        {
                            var name = resultArray[index + 1];
                            name = name.Replace('"', ' ');
                            name = name.Replace(" ", string.Empty);

                            linuxDistributionInformation.Name = name;
                        }
                        else if (resultArray[index].Equals("LTS"))
                        {
                            linuxDistributionInformation.IsLongTermSupportRelease = true;
                        }
                        else if (resultArray[index].Equals("ID"))
                        {
                            var id = resultArray[index + 1];
                            linuxDistributionInformation.Identifier = id;
                        }
                        else if (resultArray[index].Equals("ID_LIKE"))
                        {
                            var id_like = resultArray[index + 1];
                            linuxDistributionInformation.Identifier_Like = id_like;
                        }
                        else if (resultArray[index].Equals("PRETTY_NAME"))
                        {
                            var pretty_name = "";

                            var line1 = resultArray[index + 1];
                            var line2 = resultArray[index + 2];
                            var line3 = resultArray[index + 3];

                            if (!line1.Equals("VERSION_ID"))
                            {
                                line1 = line1.Replace('"', ' ');
                                line1 = line1.Replace(" ", string.Empty);

                                pretty_name += line1;
                            }

                            if (!line2.Equals("VERSION_ID"))
                            {
                                line2 = line2.Replace('"', ' ');
                                line2 = line2.Replace(" ", string.Empty);
                            }

                            if (!line3.Equals("VERSION_ID"))
                            {
                                line3 = line3.Replace('"', ' ');
                                line3 = line3.Replace(" ", string.Empty);

                                pretty_name += line3;
                            }

                            linuxDistributionInformation.PrettyName = pretty_name;
                        }
                        else if (resultArray[index].Equals("VERSION_ID"))
                        {
                            var versionId = resultArray[index + 1];
                            versionId = versionId.Replace('"', ' ');
                            versionId = versionId.Replace(" ", string.Empty);

                            linuxDistributionInformation.VersionId = versionId;
                        }
                        else if (resultArray[index].Contains("URL"))
                        {
                            var line = resultArray[index + 1];
                            line = line.Replace('"', ' ');
                            line = line.Replace(" ", string.Empty);

                            if (resultArray[index].Equals("BUG_REPORT_URL"))
                                linuxDistributionInformation.BugReportUrl = line;
                            else if (resultArray[index].Equals("HOME_URL"))
                                linuxDistributionInformation.HomeUrl = line;
                            else if (resultArray[index].Equals("SUPPORT_URL"))
                                linuxDistributionInformation.SupportUrl = line;
                            else if (resultArray[index].Equals("PRIVACY_POLICY_URL"))
                                linuxDistributionInformation.PrivacyPolicyUrl = line;
                        }
                        else if (resultArray[index].Equals("VERSION_CODENAME"))
                        {
                            var codename = resultArray[index + 1];

                            //codename = codename.Replace(" ", String.Empty);

                            linuxDistributionInformation.VersionCodename = codename;
                        }
                    }
                }

                return linuxDistributionInformation;
            }

            throw new PlatformNotSupportedException();
        }
    }
}