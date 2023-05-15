/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023
    Copyright (c) NeverSpyTech Limited 2022

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
     any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using PlatformKit.Internal.Exceptions;

using PlatformKit.Windows;
using PlatformKit.Linux;
using PlatformKit.Mac;

// ReSharper disable InconsistentNaming

namespace PlatformKit.Identification
{
    /// <summary>
    /// A class to manage RuntimeId detection and programmatic generation.
    /// </summary>
    public class RuntimeIdentification
    {
        protected WindowsAnalyzer _windowsAnalyzer;
        protected MacOSAnalyzer _macOsAnalyzer;
        protected LinuxAnalyzer _linuxAnalyzer;

        /// <summary>
        /// 
        /// </summary>
        public RuntimeIdentification()
        {
            _windowsAnalyzer = new WindowsAnalyzer();
            _macOsAnalyzer = new MacOSAnalyzer();
            _linuxAnalyzer = new LinuxAnalyzer(); 
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected string GetArchitectureString()
        {
            string cpuArch = RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => "x64",
                Architecture.X86 => "x86",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
                _ => null
            };

            return cpuArch;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifierType"></param>
        /// <returns></returns>
        /// <exception cref="OperatingSystemDetectionException"></exception>
        protected string GetOsNameString(RuntimeIdentifierType identifierType)
        {
            string osName = null;

            if (identifierType == RuntimeIdentifierType.AnyGeneric)
            {
                return "any";
            }
            else
            {
                if (OSAnalyzer.IsWindows())
                {
                    osName = "win";
                }
                if (OSAnalyzer.IsMac())
                {
                    osName = "osx";
                }
                if (OSAnalyzer.IsLinux())
                {
                    if (identifierType == RuntimeIdentifierType.Generic)
                    {
                        return "linux";
                    }
                    else if (identifierType == RuntimeIdentifierType.Specific)
                    {
                        osName = _linuxAnalyzer.GetLinuxDistributionInformation().Identifier_Like.ToLower();
                    }
                    else if (identifierType == RuntimeIdentifierType.DistroSpecific || identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
                    {
                        osName = _linuxAnalyzer.GetLinuxDistributionInformation().Identifier.ToLower();
                    }
                    else
                    {
                      //  PlatformKitAnalytics.ReportError(new LinuxVersionDetectionException(), nameof(GetOsNameString));
                        throw new LinuxVersionDetectionException();
                    }
                }
            }

            return osName;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="WindowsVersionDetectionException"></exception>
        protected string GetOsVersionString()
        {
            string osVersion = null;

            if (OSAnalyzer.IsWindows())
            {
                if (_windowsAnalyzer.IsWindows10())
                {
                    osVersion = "10";
                }
                else if (_windowsAnalyzer.IsWindows11())
                {
                    osVersion = "11";
                }
                else if (!_windowsAnalyzer.IsWindows10() && !_windowsAnalyzer.IsWindows11())
                {
                    switch (_windowsAnalyzer.GetWindowsVersionToEnum())
                    {
                        case WindowsVersion.Win7:
                            osVersion = "7";
                            break;
                        case WindowsVersion.Win7SP1:
                            osVersion = "7";
                            break;
                        case WindowsVersion.WinServer_2008_R2:
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
                    //        PlatformKitAnalytics.ReportError(new WindowsVersionDetectionException(), nameof(GetOsVersionString));
                            throw new WindowsVersionDetectionException();
                        default:
                     //       PlatformKitAnalytics.ReportError(new WindowsVersionDetectionException(), nameof(GetOsVersionString));
                            throw new WindowsVersionDetectionException();
                    }
                }
            }
            if (OSAnalyzer.IsLinux())
            {
                osVersion = _linuxAnalyzer.DetectLinuxDistributionVersionAsString();

                int dotCounter = 0;
                
                foreach (var c in osVersion)
                {
                    if (c == '.')
                    {
                        dotCounter++;
                    }
                }

                if (dotCounter == 2)
                {
                    osVersion = osVersion.Remove(osVersion.Length - 2);
                }

                if (dotCounter == 3)
                {
                    osVersion = osVersion.Remove(osVersion.Length - 4);
                }
                
                //osVersion = version.GetFriendlyVersionToString(FriendlyVersionFormatStyle.MajorDotMinor);
            }
            if (OSAnalyzer.IsMac())
            {
                var version = _macOsAnalyzer.DetectMacOsVersion();

                if (version.Major == 10)
                {
                    if (version.Minor < 9)
                    {
                   //     PlatformKitAnalytics.ReportError(new PlatformNotSupportedException(), nameof(GetOsVersionString));
                        throw new PlatformNotSupportedException();
                    }
                    else
                    {
                        osVersion = $"{version.Major}.{version.Major}";
                    }
                }
                else if (version.Major > 10)
                {
                    osVersion = $"{version.Major}";
                }
            }

            return osVersion;
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="identifierType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GenerateRuntimeIdentifier(RuntimeIdentifierType identifierType)
        {
            if (identifierType == RuntimeIdentifierType.AnyGeneric)
            {
                return GenerateRuntimeIdentifier(identifierType, false, false);
            }
            else if (identifierType == RuntimeIdentifierType.Generic ||
                     identifierType == RuntimeIdentifierType.Specific && OSAnalyzer.IsLinux() ||
                     identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {
                return GenerateRuntimeIdentifier(identifierType, true, false);
            }
            else if (identifierType == RuntimeIdentifierType.Specific && !OSAnalyzer.IsLinux()
                     || identifierType == RuntimeIdentifierType.DistroSpecific)
            {
                return GenerateRuntimeIdentifier(identifierType, true, true);
            }
            else
            {
           //     PlatformKitAnalytics.ReportError(new ArgumentException(), nameof(GenerateRuntimeIdentifier));
                throw new ArgumentException();
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public string GenerateRuntimeIdentifier(RuntimeIdentifierType identifierType, bool includeOperatingSystemName, bool includeOperatingSystemVersion)
        {
            var osName = GetOsNameString(identifierType);
            var cpuArch = GetArchitectureString();
            
            if (identifierType == RuntimeIdentifierType.AnyGeneric ||
                identifierType == RuntimeIdentifierType.Generic && includeOperatingSystemName == false)
            {
                return $"any-{GetArchitectureString()}";
            }
            else if (identifierType == RuntimeIdentifierType.Generic && includeOperatingSystemName == true)
            {
                return $"{osName}-{cpuArch}";
            }
            else if (identifierType == RuntimeIdentifierType.Specific ||
                     OSAnalyzer.IsLinux() && identifierType == RuntimeIdentifierType.DistroSpecific ||
                     OSAnalyzer.IsLinux() && identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {
                var osVersion = GetOsVersionString();

                if (OSAnalyzer.IsWindows())
                {
                    if (includeOperatingSystemVersion)
                    {
                        return $"{osName}{osVersion}-{cpuArch}";
                    }
                    else
                    {
                        return $"{osName}-{cpuArch}";
                    }
                }

                if (OSAnalyzer.IsMac())
                {
                    if (includeOperatingSystemVersion)
                    {
                        return $"{osName}.{osVersion}-{cpuArch}";
                    }
                    else
                    {
                        return $"{osName}-{cpuArch}";
                    }
                }

                if ((OSAnalyzer.IsLinux() && 
                     (identifierType == RuntimeIdentifierType.DistroSpecific) || identifierType == RuntimeIdentifierType.VersionLessDistroSpecific))
                {
                    if (includeOperatingSystemVersion)
                    {
                        return $"{osName}.{osVersion}-{cpuArch}";
                    }
                    else
                    {
                        return $"{osName}-{cpuArch}";
                    }
                }
                if (((OSAnalyzer.IsLinux() && identifierType == RuntimeIdentifierType.Specific) && includeOperatingSystemVersion == false) ||
                    includeOperatingSystemVersion == false)
                {
                    return $"{osName}-{cpuArch}";
                }
            }
            else if(!OSAnalyzer.IsLinux() && identifierType is (RuntimeIdentifierType.DistroSpecific or RuntimeIdentifierType.VersionLessDistroSpecific))
            {
                Console.WriteLine("WARNING: Function not supported on Windows or macOS." +
                                  " Calling method using RuntimeIdentifierType.Specific instead.");
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific);
            }
            
       //     PlatformKitAnalytics.ReportError(new RuntimeIdentifierGenerationException(), nameof(GenerateRuntimeIdentifier));
            throw new RuntimeIdentifierGenerationException();
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
#else
            if (OSAnalyzer.IsLinux())
            {
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific);
            }
            else
            {
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific);
            }
#endif
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Dictionary<RuntimeIdentifierType, string> GetPossibleRuntimeIdentifierCandidates()
        {
            Dictionary<RuntimeIdentifierType, string> possibles = new Dictionary<RuntimeIdentifierType, string>
            {
                { RuntimeIdentifierType.AnyGeneric, GenerateRuntimeIdentifier(RuntimeIdentifierType.AnyGeneric) },
                { RuntimeIdentifierType.Generic, GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic) },
                { RuntimeIdentifierType.Specific, GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific) }
            };

            if (OSAnalyzer.IsLinux())
            {
                possibles.Add(RuntimeIdentifierType.VersionLessDistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
                possibles.Add(RuntimeIdentifierType.DistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));
            }

            return possibles;
        }
    }
}