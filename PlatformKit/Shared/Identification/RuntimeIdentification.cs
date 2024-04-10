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
using System.Text;

using PlatformKit.Extensions;

using PlatformKit.Internal.Exceptions;

using PlatformKit.Windows;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.FreeBSD;

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

        /// <summary>
        /// Returns the CPU architecture as a string in the format that a RuntimeID uses.
        /// </summary>
        /// <returns></returns>
        protected static string GetArchitectureString()
        {
            string cpuArch = RuntimeInformation.OSArchitecture switch
            {
                Architecture.X64 => "x64",
                Architecture.X86 => "x86",
                Architecture.Arm => "arm",
                Architecture.Arm64 => "arm64",
#if NET6_0_OR_GREATER
                Architecture.S390x => "s390x",
#endif
                _ => null
            };

            return cpuArch;
        }

        /// <summary>
        /// Returns the OS name as a string in the format that a RuntimeID uses.
        /// </summary>
        /// <param name="identifierType"></param>
        /// <returns></returns>
        /// <exception cref="OperatingSystemDetectionException"></exception>
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

                if (identifierType == RuntimeIdentifierType.Specific)
                {
                    osName = LinuxAnalyzer.GetLinuxDistributionInformation().Identifier_Like.ToLower();
                }
                else if (identifierType == RuntimeIdentifierType.DistroSpecific || identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
                {
                    osName = LinuxAnalyzer.GetLinuxDistributionInformation().Identifier.ToLower();
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
        protected static string GetOsVersionString()
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
                else if (!WindowsAnalyzer.IsWindows10() && !WindowsAnalyzer.IsWindows11())
                {
                    switch (WindowsAnalyzer.GetWindowsVersionToEnum())
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
                osVersion = LinuxAnalyzer.GetLinuxDistributionVersionAsString();
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
                        osVersion += ".0";
                        break;
                    }
            }
            if (OperatingSystem.IsMacOS())
            {
                var version = MacOsAnalyzer.GetMacOsVersion();

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
        /// Programmatically generates a .NET Runtime Identifier.
        /// Note: Microsoft advises against programmatically creating Runtime IDs but this may be necessary in some instances.
        /// For More Information Visit: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        /// </summary>
        /// <param name="identifierType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
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

            throw new ArgumentException();
        }
        
        /// <summary>
        /// Programmatically generates a .NET Runtime Identifier based on the system calling the method.
        /// Note: Microsoft advises against programmatically creating Runtime IDs but this may be necessary in some instances.
        /// For More Information Visit: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
        /// </summary>
        /// <returns></returns>
        public static string GenerateRuntimeIdentifier(RuntimeIdentifierType identifierType, bool includeOperatingSystemName, bool includeOperatingSystemVersion)
        {
            var osName = GetOsNameString(identifierType);
            var cpuArch = GetArchitectureString();
            
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
                var osVersion = GetOsVersionString();

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
                Console.WriteLine("WARNING: Function not supported on Windows or macOS." +
                                  " Calling method using RuntimeIdentifierType.Specific instead.");
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
        
        /// <summary>
        /// Detect the Target Framework Moniker (TFM) of the currently running system.
        /// Note: This does not detect .NET Standard TFMs, UWP TFMs, Windows Phone TFMs, Silverlight TFMs, and Windows Store TFMs.
        ///
        /// IOS and Android version specific TFM generation isn't supported at this time but may be added in a future release.
        /// </summary>
        /// <param name="targetFrameworkType">The type of TFM to generate.</param>
        /// <returns></returns>
        /// <exception cref="OperatingSystemDetectionException">Thrown if there's an issue detecting the Windows Version whilst running on Windows.</exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static string GetTargetFrameworkMoniker(TargetFrameworkMonikerType targetFrameworkType)
        {
            Version frameworkVersion = new Version(RuntimeInformation.FrameworkDescription.ToLower().Replace(".net", String.Empty)
                .Replace("core", String.Empty)
                .Replace(" ", String.Empty).AddMissingZeroes());
            
            if (RuntimeInformation.FrameworkDescription.ToLower().Contains("core"))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("netcoreapp");

                stringBuilder.Append(frameworkVersion.Major);
                stringBuilder.Append(".");
                stringBuilder.Append(frameworkVersion.Minor);

                return stringBuilder.ToString();
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                
                if (RuntimeInformation.FrameworkDescription.ToLower().Contains(".net"))
                {
                    stringBuilder.Append("net");
                    
                    if (frameworkVersion.Major < 5)
                    {
                        stringBuilder.Append(frameworkVersion.Major);
                        stringBuilder.Append(frameworkVersion.Minor);
                                                    
                        if (frameworkVersion.Build != 0)
                        {
                            stringBuilder.Append(frameworkVersion.Build);
                        }

                        return stringBuilder.ToString();
                    }

                    stringBuilder.Append(frameworkVersion.Major);
                    stringBuilder.Append(".");
                    stringBuilder.Append(frameworkVersion.Minor);

                    if (targetFrameworkType == TargetFrameworkMonikerType.Generic)
                    {
                        return stringBuilder.ToString();
                    }

                    if(targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemSpecific || targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
                    {
                        if(OperatingSystem.IsMacOS())
                        {
                            stringBuilder.Append("-");
                            stringBuilder.Append("macos");
                        }
#if NET5_0_OR_GREATER
                        else if (OperatingSystem.IsMacCatalyst())
                        {
                            stringBuilder.Append("-");
                            stringBuilder.Append("maccatalyst");
                        }
#endif
                        else if (OperatingSystem.IsWindows())
                        {
                            stringBuilder.Append("-");
                            stringBuilder.Append("windows");
                        }
                        else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
                        {
                            //Do nothing because Linux and FreeBSD don't have dedicated TFMs yet
                        }
#if NET5_0_OR_GREATER
                            else if (OperatingSystem.IsAndroid())
                            {
                                stringBuilder.Append("-");
                                stringBuilder.Append("android");
                            }
                            else if (OperatingSystem.IsIOS())
                            {
                                stringBuilder.Append("-");
                                stringBuilder.Append("ios");
                            }
                            else if (OperatingSystem.IsTvOS())
                            {
                                stringBuilder.Append("-");
                                stringBuilder.Append("tvos");
                            }
                            else if (OperatingSystem.IsWatchOS())
                            {
                                stringBuilder.Append("-");
                                stringBuilder.Append("watchos");
                            }

                            if (frameworkVersion.Major >= 8)
                            {
                                if (OperatingSystem.IsBrowser())
                                {
                                    stringBuilder.Append("-");
                                    stringBuilder.Append("browser");
                                }
                            }
#endif
                            
                        if (targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific && frameworkVersion.Major >= 5)
                        {
                            if(OperatingSystem.IsMacOS())
                            {
                                var macOsVersion = MacOsAnalyzer.GetMacOsVersion();

                                stringBuilder.Append(macOsVersion.Major);
                                stringBuilder.Append(".");
                                stringBuilder.Append(macOsVersion.Minor);
                            }
                            else if(OperatingSystem.IsLinux())
                            {
                                //Do nothing because Linux doesn't have a version specific TFM.
                            }
                            else if(OperatingSystem.IsWindows())
                            {
                                var windowsVersion = WindowsAnalyzer.GetWindowsVersion();

                                var windowsVersionEnum = WindowsAnalyzer.GetWindowsVersionToEnum();

                                switch (windowsVersionEnum)
                                {
                                    case WindowsVersion.Win7 or WindowsVersion.Win7SP1:
                                        stringBuilder.Append("7.0");
                                        break;
                                    case WindowsVersion.Win8:
                                        stringBuilder.Append("8.0");
                                        break;
                                    case WindowsVersion.Win8_1:
                                        stringBuilder.Append("8.1");
                                        break;
                                    default:
                                        if (WindowsAnalyzer.IsWindows10() || WindowsAnalyzer.IsWindows11())
                                        {
                                            if (windowsVersion.Build >= 14393)
                                            {
                                                stringBuilder.Append(windowsVersion);
                                            }

                                            break;
                                        }

                                        throw new OperatingSystemDetectionException();
                                }
                                    
                                return stringBuilder.ToString();
                            }
                        }
                            
                        return stringBuilder.ToString();
                    }
                }
                else if (RuntimeInformation.FrameworkDescription.ToLower().Contains("mono"))
                {
                    stringBuilder.Clear();
                    stringBuilder.Append("mono");
                    
                    if(OperatingSystem.IsMacOS())
                    {
                        stringBuilder.Append("mac");
                    }
#if NET5_0_OR_GREATER
                    else if (OperatingSystem.IsAndroid())
                    {
                        stringBuilder.Append("android");
                    }
#endif
                    else
                    {
                       return stringBuilder.ToString();
                    }
                }
            }

            throw new PlatformNotSupportedException();
        }
        
    }
}