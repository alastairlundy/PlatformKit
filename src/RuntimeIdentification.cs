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

using System.Runtime.InteropServices;

using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Exceptions;
using AluminiumTech.DevKit.PlatformKit.Models;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Enums;

// ReSharper disable InconsistentNaming

namespace AluminiumTech.DevKit.PlatformKit
{
    public class RuntimeIdentification
    {
        protected OSAnalyzer osAnalyzer;
        protected PlatformManager platformManager;
        protected OSVersionAnalyzer versionAnalyzer;
        
        public RuntimeIdentification()
        {
            platformManager = new PlatformManager();
            versionAnalyzer = new OSVersionAnalyzer();
            osAnalyzer = new OSAnalyzer();
        }

        protected string GetArchitectureString()
        {
            string cpuArch = null;

            cpuArch = RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => "x64",
                Architecture.X86 => "x86",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => null
            };

            return cpuArch;
        }

        protected string GetOsNameString(bool useGenericTFM)
        {
            string osName = null;

            if (platformManager.IsWindows())
            {
                osName = "win";
            }
            if (platformManager.IsMac())
            {
                osName = "osx";
            }
            if (platformManager.IsLinux())
            {
                if (useGenericTFM)
                {
                    osName = "linux";
                }
                else
                {
                    osName = osAnalyzer.GetLinuxDistributionInformation().Name;
                }
            }

            return osName;
        }

        protected string GetOsVersionString()
        {
            string osVersion = null;

            if (platformManager.IsWindows())
            {
                if (versionAnalyzer.IsWindows10())
                {
                    osVersion = "10";
                }
                else if (versionAnalyzer.IsWindows11())
                {
                    osVersion = "11";
                }
                else if (!versionAnalyzer.IsWindows10() && !versionAnalyzer.IsWindows11())
                {
                    switch (versionAnalyzer.GetWindowsVersionToEnum())
                    {
                        case WindowsVersion.WinVista:
                            throw new PlatformNotSupportedException();
                        case WindowsVersion.WinVistaSP1:
                            throw new PlatformNotSupportedException();
                        case WindowsVersion.WinVistaSP2:
                            throw new PlatformNotSupportedException();
                        case WindowsVersion.WinServer_2008:
                            throw new PlatformNotSupportedException();
                        case WindowsVersion.WinServer_2008_R2:
                            osVersion = "7";
                            break;
                        case WindowsVersion.Win7:
                            osVersion = "7";
                            break;
                        case WindowsVersion.Win7SP1:
                            osVersion = "7";
                            break;
                        case WindowsVersion.Win8:
                            osVersion = "8";
                            break;
                        case WindowsVersion.WinServer_2012:
                            osVersion = "8";
                            break;
                        case WindowsVersion.Win8_1:
                            osVersion = "81";
                            break;
                        case WindowsVersion.WinServer_2012_R2:
                            osVersion = "81";
                            break;
                        case WindowsVersion.NotDetected:
                            throw new OperatingSystemVersionDetectionException();
                        default:
                            throw new OperatingSystemVersionDetectionException();
                    }
                }
            }
            if (platformManager.IsLinux())
            {
                osVersion = versionAnalyzer.DetectLinuxDistributionVersionAsString();
                //osVersion = version.GetFriendlyVersionToString(FriendlyVersionFormatStyle.MajorDotMinor);
            }
            if (platformManager.IsMac())
            {
                //For now we need to throw an error cos we don't currently have macOS Version detection built in.
                throw new PlatformNotSupportedException();

                /*
                switch (versionAnalyzer.)
                {
                    case MacOsVersion.v10_9_Mavericks:
                        osVersion = "10.9";
                        break;
                    case MacOsVersion.v10_10_Yosemite:
                        osVersion = "10.10";
                        break;
                    case MacOsVersion.v10_11_ElCapitan:
                        osVersion = "10.11";
                        break;
                    case MacOsVersion.v10_12_Sierra:
                        osVersion = "10.12";
                        break;
                    case MacOsVersion.v10_13_HighSierra:
                        osVersion = "10.13";
                        break;
                    case MacOsVersion.v10_14_Mojave:
                        osVersion = "10.14";
                        break;
                    case MacOsVersion.v10_15_Catalina:
                        osVersion = "10.15";
                        break;
                    case MacOsVersion.v11_BigSur:
                        osVersion = "11.0";
                        break;
                    case MacOsVersion.v12_Monterrey:
                        osVersion = "12.0";
                        break;
                    default:
                        throw new PlatformNotSupportedException();
                        break;
                }
                */
            }

#if DEBUG
            Console.WriteLine("OsVersion: " + osVersion);     
#endif
            
            return osVersion;
        }

        /// <summary>
        /// Generates a Generic Runtime ID that is OS version independent but not CPU or OS independent.
        /// Produces a Runtime ID in the format of [OperatingSystem]-[ProcessorArchitecture]
        /// </summary>
        /// <returns></returns>
        public string GenerateGenericRuntimeIdentifier()
        {
            try
            {
                var osName = GetOsNameString(true);
                var cpuArch = GetArchitectureString();

                return $"{osName}-{cpuArch}";
            }
            catch
            {
                throw new GenericRuntimeIdentifierGenerationFailedException();
            }
        }

        /// <summary>
        /// Generates the specific Runtime Identifier in the format of [OperatingSystemName].[OperatingSystemVersion]-[ProcessorArchitecture].
        /// On Windows the dot between OS Name and OS Version is omitted because that's how Microsoft creates/uses RuntimeIDs.
        /// </summary>
        /// <returns></returns>
        public string GenerateSpecificRuntimeIdentifier()
        {
            try
            {
                var osName = GetOsNameString(false);
                var osVersion = GetOsVersionString();
                var cpuArch = GetArchitectureString();

                if (platformManager.IsWindows())
                {
                    return $"{osName}{osVersion}-{cpuArch}";
                }
                else
                {
                    return $"{osName}.{osVersion}-{cpuArch}";
                }
            }
            catch
            {
                throw new SpecificRuntimeIdentifierGenerationException();
            }
        }

        /// <summary>
        /// Detects the TFM if running on .NET 5 or later and generates the generic TFM if running on .NET Standard 2.0 or later.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public string DetectRuntimeIdentifier()
        {
#if NET5_0_OR_GREATER
            return RuntimeInformation.RuntimeIdentifier;
#endif
            return GenerateGenericRuntimeIdentifier();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public RuntimeIdentifier GenerateRuntimeIdentifier()
        {
            var runtimeIdentifier = new RuntimeIdentifier();
            runtimeIdentifier.GeneratedGenericIdentifier = GenerateGenericRuntimeIdentifier();
            runtimeIdentifier.GeneratedSpecificIdentifier = GenerateSpecificRuntimeIdentifier();

            runtimeIdentifier.DotNetIdentifier = DetectRuntimeIdentifier();

            return runtimeIdentifier;
        }
    }
}