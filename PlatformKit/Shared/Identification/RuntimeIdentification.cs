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
using System.Text;

using PlatformKit.Extensions;

using PlatformKit.Internal.Exceptions;

using PlatformKit.Windows;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.FreeBSD;


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
        /// Returns the CPU architecture as a string in the format that a RuntimeID uses.
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
                #if NETCOREAPP3_0_OR_GREATER
                if (OSAnalyzer.IsFreeBSD())
                {
                    osName = "freebsd";
                }
                #endif
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
                        throw new LinuxVersionDetectionException();
                    }
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
        /// Programmatically generates a .NET Runtime Identifier.
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
                throw new ArgumentException();
            }
        }
        
        /// <summary>
        /// Programmatically generates a .NET Runtime Identifier based on the system calling the method.
        /// Note: Microsoft advises against programmatically creating Runtime IDs but this may be necessary in some instances.
        /// For More Information Visit: https://learn.microsoft.com/en-us/dotnet/core/rid-catalog
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
            
            throw new RuntimeIdentifierGenerationException();
        }

        /// <summary>
        /// Detects the RuntimeID if running on .NET 5 or later and generates the generic RuntimeID if running on .NET Standard 2.0 or later.
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

            if (OSAnalyzer.IsLinux())
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
        public string GetTargetFrameworkMoniker(TargetFrameworkMonikerType targetFrameworkType)
        {
            if (RuntimeInformation.FrameworkDescription.ToLower().Contains("core"))
            {
                StringBuilder stringBuilder = new StringBuilder();
                stringBuilder.Append("netcoreapp");
                
                var versionString = RuntimeInformation.FrameworkDescription.ToLower().Replace(".net", String.Empty)
                    .Replace("core", String.Empty)
                    .Replace(" ", String.Empty);
                
                versionString = new Version().AddMissingZeroes(versionString);

                Version version = new Version(versionString);

                stringBuilder.Append(version.Major);
                stringBuilder.Append(".");
                stringBuilder.Append(version.Minor);

                return stringBuilder.ToString();
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                
                if (RuntimeInformation.FrameworkDescription.ToLower().Contains(".net"))
                {
                    var versionString = RuntimeInformation.FrameworkDescription.ToLower().Replace(".net", String.Empty)
                        .Replace(" ", String.Empty);
                    
                    versionString = new Version().AddMissingZeroes(versionString);

                    Version version = new Version(versionString);
                    
                    stringBuilder.Append("net");
                    
                    if (version.Major < 5)
                    {
                        stringBuilder.Append(version.Major);
                        stringBuilder.Append(version.Minor);
                                                    
                        if (version.Build != 0)
                        {
                            stringBuilder.Append(version.Build);
                        }

                        return stringBuilder.ToString();
                    }
                    else
                    {
                        stringBuilder.Append(version.Major);
                        stringBuilder.Append(".");
                        stringBuilder.Append(version.Minor);

                        if (targetFrameworkType == TargetFrameworkMonikerType.Generic)
                        {
                            return stringBuilder.ToString();
                        }
                        else if(targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemSpecific || targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
                        {
#if NET5_0_OR_GREATER
                            if (OperatingSystem.IsMacOS())
#else
                            if(OSAnalyzer.IsMac())
#endif
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
                            else if (OSAnalyzer.IsWindows())
                            {
                                stringBuilder.Append("-");
                                stringBuilder.Append("windows");
                            }
                            else if (OSAnalyzer.IsLinux())
                            {
                                //Do nothing because Linux doesn't have a dedicated TFM yet
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

                            if (version.Major >= 8)
                            {
                                if (OperatingSystem.IsBrowser())
                                {
                                    stringBuilder.Append("-");
                                    stringBuilder.Append("browser");
                                }
                            }
#endif


                            if (targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific && version.Major >= 5)
                            {
#if NET5_0_OR_GREATER
                                if (OperatingSystem.IsMacOS())
#else
                                if(OSAnalyzer.IsMac())
#endif
                                {
                                    MacOsAnalyzer macOsAnalyzer = new MacOsAnalyzer();
                                    var macOsVersion = macOsAnalyzer.DetectMacOsVersion();

                                    stringBuilder.Append(macOsVersion.Major);
                                    stringBuilder.Append(".");
                                    stringBuilder.Append(macOsVersion.Minor);
                                }
#if NET5_0_OR_GREATER
                                else if (OperatingSystem.IsLinux())
#else
                                else if(OSAnalyzer.IsLinux())
#endif
                                {
                                    //Do nothing because Linux doesn't have a version specific TFM.
                                }
#if NET5_0_OR_GREATER
                                else if (OperatingSystem.IsWindows())
#else
                                else if(OSAnalyzer.IsWindows())
#endif
                                {
                                    WindowsAnalyzer windowsAnalyzer = new WindowsAnalyzer();
                                    var windowsVersion = windowsAnalyzer.DetectWindowsVersion();

                                    var windowsVersionEnum = windowsAnalyzer.GetWindowsVersionToEnum();

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
                                            if (windowsAnalyzer.IsWindows10() || windowsAnalyzer.IsWindows11())
                                            {
                                                if (windowsVersion.Build >= 14393)
                                                {
                                                    stringBuilder.Append(windowsVersion.ToString());
                                                    break;
                                                }
                                                else
                                                {
                                                    break;
                                                }
                                            }
                                            else
                                            {
                                                throw new OperatingSystemDetectionException();
                                            }
                                    }
                                    
                                    return stringBuilder.ToString();
                                }
                            }
                            
                            return stringBuilder.ToString();
                        }
                    }
                }
                else if (RuntimeInformation.FrameworkDescription.ToLower().Contains("mono"))
                {
                    stringBuilder.Clear();
                    stringBuilder.Append("mono");
                    
#if NET5_0_OR_GREATER
                    if (OperatingSystem.IsMacOS())
#else
                    if(OSAnalyzer.IsMac())
#endif
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