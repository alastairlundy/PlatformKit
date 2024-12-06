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
using System.Collections.Generic;
using System.Runtime.InteropServices;
using AlastairLundy.Extensions.System.Strings.Versioning;
using PlatformKit.Internal.Exceptions;

using PlatformKit.Windows;
using PlatformKit.Linux;
using PlatformKit.Linux.Models;
using PlatformKit.Mac;
using PlatformKit.FreeBSD;
using PlatformKit.Internal.Localizations;


#if NETSTANDARD2_0
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

// ReSharper disable InconsistentNaming

namespace PlatformKit.Identification
{
    /// <summary>
    /// A class to manage RuntimeId detection and programmatic generation.
    /// </summary>
    public class RuntimeIdentification
    {

        /// <summary>
        /// Returns the CPU architecture as a string in the format that a RuntimeID uses.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        protected static string GetArchitectureString()
        {
            return RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => "x64",
                Architecture.X86 => "x86",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
#if NET6_0_OR_GREATER
                Architecture.S390x => "s390x",
                Architecture.Wasm => throw new PlatformNotSupportedException(),
#endif
                _ => null
            };
        }

        /// <summary>
        /// Returns the OS name as a string in the format that a RuntimeID uses.
        /// </summary>
        /// <param name="identifierType"></param>
        /// <returns></returns>
        /// <exception cref="LinuxVersionDetectionException"></exception>
        protected static string GetOsNameString(RuntimeIdentifierType identifierType)
        {
            string osName = null;

            if (identifierType == RuntimeIdentifierType.AnyGeneric)
            {
                return "any";
            }
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

                LinuxOsReleaseModel osRelease = LinuxOsReleaseRetriever.GetLinuxOsRelease();

                if (identifierType == RuntimeIdentifierType.Specific)
                {
                    osName = osRelease.Identifier_Like.ToLower();
                }
                else if (identifierType == RuntimeIdentifierType.DistroSpecific || identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
                {
                    osName = osRelease.Identifier.ToLower();
                }
                else
                {
                    throw new LinuxVersionDetectionException();
                }
            }

            return osName;
        }
        
        /// <summary>
        /// Returns the OS version as a string in the format that a RuntimeID uses.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="WindowsVersionDetectionException"></exception>
        internal static string GetOsVersionString()
        {
            string osVersion = null;

            if (OperatingSystem.IsWindows())
            {
                if (WindowsAnalyzer.IsWindows10())
                {
                    osVersion = "10";
                }
                else if (WindowsAnalyzer.IsWindows11())
                {
                    osVersion = "11";
                }
                
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LegacyOS);
            }
            if (OperatingSystem.IsLinux())
            {
                osVersion = LinuxOsReleaseRetriever.GetLinuxDistributionVersionAsString();
            }
            if (OperatingSystem.IsFreeBSD())
            {
                osVersion = FreeBsdAnalyzer.GetFreeBSDVersion().ToString();
                
                switch (osVersion.CountDotsInString())
                {
                    case 3:
                        osVersion = osVersion.Remove(osVersion.Length - 4, 4);
                        break;
                    case 2:
                        osVersion = osVersion.Remove(osVersion.Length - 2, 2);
                        break;
                    case 1:
                        break;
                    case 0:
                        osVersion = $"{osVersion}.0";
                        break;
                    }
            }
            if (OperatingSystem.IsMacOS())
            {
                Version version = MacOsAnalyzer.GetMacOsVersion();

                if (version.Major == 10)
                {
                    throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LegacyOS);
                }
                else if (version.Major > 10)
                {
                    osVersion = $"{version.Major}";
                }
            }

            return osVersion;
        }

        /// <summary>
        /// Programmatically generates a .NET Runtime Identifier.
        /// Note: Microsoft advises against programmatically creating Runtime IDs but this may be necessary in some instances.
        /// For More Information Visit: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        /// </summary>
        /// <param name="identifierType"></param>
        /// <returns></returns>
        public static string GenerateRuntimeIdentifier(RuntimeIdentifierType identifierType)
        {
            if (identifierType == RuntimeIdentifierType.AnyGeneric)
            {
                return GenerateRuntimeIdentifier(identifierType, false, false);
            }

            if (identifierType == RuntimeIdentifierType.Generic ||
                identifierType == RuntimeIdentifierType.Specific && (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD()) ||
                identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {
                return GenerateRuntimeIdentifier(identifierType, true, false);
            }

            if (identifierType == RuntimeIdentifierType.Specific && (!OperatingSystem.IsLinux() && !OperatingSystem.IsFreeBSD())
                || identifierType == RuntimeIdentifierType.DistroSpecific)
            {
                return GenerateRuntimeIdentifier(identifierType, true, true);
            }

            return GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic, true, false);
        }
        
        /// <summary>
        /// Programmatically generates a .NET Runtime Identifier based on the system calling the method.
        /// Note: Microsoft advises against programmatically creating Runtime IDs but this may be necessary in some instances.
        /// For More Information Visit: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        /// </summary>
        /// <returns></returns>
        public static string GenerateRuntimeIdentifier(RuntimeIdentifierType identifierType, bool includeOperatingSystemName, bool includeOperatingSystemVersion)
        {
            string osName = GetOsNameString(identifierType);
            string cpuArch = GetArchitectureString();
            
            if (identifierType == RuntimeIdentifierType.AnyGeneric ||
                identifierType == RuntimeIdentifierType.Generic && includeOperatingSystemName == false)
            {
                return $"any-{GetArchitectureString()}";
            }

            if (identifierType == RuntimeIdentifierType.Generic && includeOperatingSystemName)
            {
                return $"{osName}-{cpuArch}";
            }

            if (identifierType == RuntimeIdentifierType.Specific ||
                OperatingSystem.IsLinux() && identifierType == RuntimeIdentifierType.DistroSpecific ||
                OperatingSystem.IsLinux() && identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {
                string osVersion = GetOsVersionString();

                if (OperatingSystem.IsWindows())
                {
                    if (includeOperatingSystemVersion)
                    {
                        return $"{osName}{osVersion}-{cpuArch}";
                    }

                    return $"{osName}-{cpuArch}";
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
            else if((!OperatingSystem.IsLinux() && !OperatingSystem.IsFreeBSD()) && identifierType is (RuntimeIdentifierType.DistroSpecific or RuntimeIdentifierType.VersionLessDistroSpecific))
            {
                Console.WriteLine(Resources.RuntimeInformation_NonLinuxSpecific_Warning);
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific);
            }

            throw new RuntimeIdentifierGenerationException();
        }

        /// <summary>
        /// Detects the RuntimeID if running on .NET 5 or later and generates the generic RuntimeID if running on .NET Standard 2.0 or later.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public static string GetRuntimeIdentifier()
        {
#if NET5_0_OR_GREATER
            return RuntimeInformation.RuntimeIdentifier;
#else
            if (OperatingSystem.IsLinux())
            {
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific);
            }

            return GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific);
#endif
        }

        /// <summary>
        /// Detects possible Runtime Identifiers that could be applicable to the system calling the method.
        /// </summary>
        /// <returns></returns>
        public static Dictionary<RuntimeIdentifierType, string> GetPossibleRuntimeIdentifierCandidates()
        {
            Dictionary<RuntimeIdentifierType, string> possibilities = new Dictionary<RuntimeIdentifierType, string>
            {
                { RuntimeIdentifierType.AnyGeneric, GenerateRuntimeIdentifier(RuntimeIdentifierType.AnyGeneric) },
                { RuntimeIdentifierType.Generic, GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic) },
                { RuntimeIdentifierType.Specific, GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific) }
            };

            if (OperatingSystem.IsLinux())
            {
                possibilities.Add(RuntimeIdentifierType.VersionLessDistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
                possibilities.Add(RuntimeIdentifierType.DistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));
            }

            return possibilities;
        }
    }
}