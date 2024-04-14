/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AlastairLundy.System.Extensions.StringExtensions;
using PlatformKit.FreeBSD;
using PlatformKit.Internal.Exceptions;

using PlatformKit.Windows;
using PlatformKit.Linux;
using PlatformKit.Mac;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

// ReSharper disable InconsistentNaming

namespace PlatformKit.Identification
{
    /// <summary>
    /// A class to manage RuntimeId detection and programmatic generation.
    /// </summary>
    public class RuntimeIdentification
    {
        private readonly WindowsAnalyzer _windowsAnalyzer;
        private readonly MacOsAnalyzer _macOsAnalyzer;
        private readonly LinuxAnalyzer _linuxAnalyzer;
        private readonly FreeBsdAnalyzer _freeBsdAnalyzer;

        /// <summary>
        /// 
        /// </summary>
        public RuntimeIdentification()
        {
            _windowsAnalyzer = new WindowsAnalyzer();
            _macOsAnalyzer = new MacOsAnalyzer();
            _linuxAnalyzer = new LinuxAnalyzer();
            _freeBsdAnalyzer = new FreeBsdAnalyzer();
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
                if (OperatingSystem.IsWindows())
                {
                    osName = "win";
                }
                if (OperatingSystem.IsMacOS())
                {
                    osName = "osx";
                }
                if (OperatingSystem.IsFreeBSD())
                {
                    osName = "freebsd";
                }
                if (OperatingSystem.IsLinux())
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

            if (OperatingSystem.IsWindows())
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
                        case WindowsVersion.Win7SP1 or WindowsVersion.WinServer_2008_R2:
                            osVersion = "7";
                            break;
                        case WindowsVersion.Win8 or WindowsVersion.WinServer_2012:
                            osVersion = "8";
                            break;
                        case WindowsVersion.Win8_1 or WindowsVersion.WinServer_2012_R2:
                            osVersion = "81";
                            break;
                        case WindowsVersion.NotDetected:
                            throw new WindowsVersionDetectionException();
                        default:
                            throw new WindowsVersionDetectionException();
                    }
                }
            }
            if (OperatingSystem.IsLinux())
            {
                osVersion = _linuxAnalyzer.DetectLinuxDistributionVersionAsString();

                int dotCounter = osVersion.CountDotsInString();

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
            if (OperatingSystem.IsFreeBSD())
            {
                osVersion = _freeBsdAnalyzer.DetectFreeBSDVersion().ToString();

                int dotCounter = osVersion.CountDotsInString();

                if (dotCounter == 2)
                {
                    osVersion = osVersion.Remove(osVersion.Length - 2, 2);
                }

                if (dotCounter == 3)
                {
                    osVersion = osVersion.Remove(osVersion.Length - 4, 4);
                }
                
                switch (dotCounter)
                {
                    case 3:
                        osVersion = osVersion.Remove(osVersion.Length, 4);
                        break;
                    case 2:
                        osVersion = osVersion.Remove(osVersion.Length, 2);
                        break;
                    case 1:
                        break;
                }
            }
            if (OperatingSystem.IsMacOS())
            {
                Version version = _macOsAnalyzer.DetectMacOsVersion();

                if (version.Major == 10)
                {
                    if (version.Minor < 9)
                    {
                        throw new PlatformNotSupportedException();
                    }
                    
                    osVersion = $"{version.Major}.{version.Major}";
                }
                else if (version.Major > 10)
                {
                    osVersion = $"{version.Major}";
                }
            }

            return osVersion;
        }

        /// <summary>
        /// Programmatically generates a .NET Runtime Identifier based on the system calling the method.
        /// Note: Microsoft advises against programmatically creating Runtime IDs but this may be necessary in some instances.
        /// For More Information Visit: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
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
                     identifierType == RuntimeIdentifierType.Specific && (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD()) ||
                     identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {
                return GenerateRuntimeIdentifier(identifierType, true, false);
            }
            else if (identifierType == RuntimeIdentifierType.Specific && (!OperatingSystem.IsLinux() && !OperatingSystem.IsFreeBSD())
                     || identifierType == RuntimeIdentifierType.DistroSpecific)
            {
                return GenerateRuntimeIdentifier(identifierType, true, true);
            }
            else
            {
                throw new ArgumentException();
            }
        }

        /// <summary>
        /// Programmatically generates a .NET Runtime Identifier based on the system calling the method.
        /// Note: Microsoft advises against programmatically creating Runtime IDs but this may be necessary in some instances.
        /// For More Information Visit: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        /// </summary>
        /// <param name="identifierType"></param>
        /// <param name="includeOperatingSystemName"></param>
        /// <param name="includeOperatingSystemVersion"></param>
        /// <returns></returns>
        /// <exception cref="RuntimeIdentifierGenerationException"></exception>
        public string GenerateRuntimeIdentifier(RuntimeIdentifierType identifierType, bool includeOperatingSystemName, bool includeOperatingSystemVersion)
        {
            string osVersion = GetOsVersionString();
            string osName = GetOsNameString(identifierType);
            string cpuArch = GetArchitectureString();
            
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
                     (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD()) && identifierType == RuntimeIdentifierType.DistroSpecific ||
                     (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD()) && identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {

                if (OperatingSystem.IsWindows())
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

                if (OperatingSystem.IsMacOS())
                {
                    if (includeOperatingSystemVersion)
                    {
                        return $"{osName}.{osVersion}-{cpuArch}";
                    }
                    
                    return $"{osName}-{cpuArch}";
                }

                if (((OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD()) && 
                     (identifierType == RuntimeIdentifierType.DistroSpecific) || identifierType == RuntimeIdentifierType.VersionLessDistroSpecific))
                {
                    if (includeOperatingSystemVersion)
                    {
                        return $"{osName}.{osVersion}-{cpuArch}";
                    }
                    
                    return $"{osName}-{cpuArch}";
                }
                if (((OperatingSystem.IsLinux() && identifierType == RuntimeIdentifierType.Specific) && includeOperatingSystemVersion == false) ||
                    includeOperatingSystemVersion == false)
                {
                    return $"{osName}-{cpuArch}";
                }
            }
            else if(!OperatingSystem.IsLinux() && identifierType is (RuntimeIdentifierType.DistroSpecific or RuntimeIdentifierType.VersionLessDistroSpecific))
            {
                Console.WriteLine("WARNING: Function not supported on Windows or macOS." +
                                  " Calling method using RuntimeIdentifierType.Specific instead.");
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific);
            }
            
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
            if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
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
        /// Detects possible Runtime Identifiers that could be applicable to the system calling the method.
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

            if (OperatingSystem.IsLinux())
            {
                possibles.Add(RuntimeIdentifierType.VersionLessDistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
                possibles.Add(RuntimeIdentifierType.DistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));
            }

            return possibles;
        }
    }
}