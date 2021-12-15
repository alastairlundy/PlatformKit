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
using System.Collections.Generic;
using System.Runtime.InteropServices;

using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;
using AluminiumTech.DevKit.PlatformKit.Exceptions;

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows;

// ReSharper disable InconsistentNaming

namespace AluminiumTech.DevKit.PlatformKit.Runtime
{
    /// <summary>
    /// A class to manage RuntimeId detection and programmatic generation.
    /// </summary>
    public class RuntimeIdentification
    {
        protected OSAnalyzer osAnalyzer;
        protected PlatformManager platformManager;
        protected OSVersionAnalyzer versionAnalyzer;
        
        /// <summary>
        /// 
        /// </summary>
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

        protected string GetOsNameString(RuntimeIdentifierType identifierType)
        {
            string osName = null;

            if (identifierType == RuntimeIdentifierType.AnyGeneric)
            {
                return "any";
            }
            else
            {
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
                    if (identifierType == RuntimeIdentifierType.Generic)
                    {
                        return "linux";
                    }
                    else if (identifierType == RuntimeIdentifierType.Specific)
                    {
                        osName = osAnalyzer.GetLinuxDistributionInformation().Identifier_Like.ToLower();
                    }
                    else if (identifierType == RuntimeIdentifierType.DistroSpecific || identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
                    {
                        osName = osAnalyzer.GetLinuxDistributionInformation().Name.ToLower();
                    }
                    else
                    {
                        throw new OperatingSystemVersionDetectionException();
                    }
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
                            throw new OperatingSystemVersionDetectionException();
                        default:
                            throw new OperatingSystemVersionDetectionException();
                    }
                }
            }
            if (platformManager.IsLinux())
            {
                osVersion = versionAnalyzer.DetectLinuxDistributionVersionAsString();

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
                     identifierType == RuntimeIdentifierType.Specific && platformManager.IsLinux() ||
                     identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {
                return GenerateRuntimeIdentifier(identifierType, true, false);
            }
            else if (identifierType == RuntimeIdentifierType.Specific && !platformManager.IsLinux()
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
                     platformManager.IsLinux() && identifierType == RuntimeIdentifierType.DistroSpecific ||
                     platformManager.IsLinux() && identifierType == RuntimeIdentifierType.VersionLessDistroSpecific)
            {
                var osVersion = GetOsVersionString();

                if (platformManager.IsWindows())
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

                if (platformManager.IsMac())
                {
                    if (includeOperatingSystemVersion)
                    {
                        throw new NotImplementedException();
                    }
                    else
                    {
                        return $"{osName}-{cpuArch}";
                    }
                }

                if ((platformManager.IsLinux() && 
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
                if (((platformManager.IsLinux() && identifierType == RuntimeIdentifierType.Specific) && includeOperatingSystemVersion == false) ||
                    includeOperatingSystemVersion == false)
                {
                    return $"{osName}-{cpuArch}";
                }
            }
            else if(!platformManager.IsLinux() && (identifierType == RuntimeIdentifierType.DistroSpecific || identifierType == RuntimeIdentifierType.VersionLessDistroSpecific))
            {
                Console.WriteLine("WARNING: Function not supported on Windows or macOS. Calling method using RuntimeIdentifierType.Specific instead.");
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
#endif
            if (platformManager.IsLinux())
            {
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific);
            }
            else
            {
                return GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific);
            }
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

            if (platformManager.IsLinux())
            {
                possibles.Add(RuntimeIdentifierType.VersionLessDistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false));
                possibles.Add(RuntimeIdentifierType.DistroSpecific,GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific));
            }

            return possibles;
        }
        
        /// <summary>
        /// Generates a RuntimeIdentifier object model, detects information, and return it.
        /// </summary>
        /// <returns></returns>
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        public RuntimeIdentifier GenerateRuntimeIdentifier()
        {
            var runtimeIdentifier = new RuntimeIdentifier();

            runtimeIdentifier.AnyGenericIdentifier = GenerateRuntimeIdentifier(RuntimeIdentifierType.AnyGeneric);
            runtimeIdentifier.GenericIdentifier = GenerateRuntimeIdentifier(RuntimeIdentifierType.Generic);
            runtimeIdentifier.SpecificIdentifier = GenerateRuntimeIdentifier(RuntimeIdentifierType.Specific);

            if (platformManager.IsLinux())
            {
                runtimeIdentifier.DistroSpecificIdentifier =
                    GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific);
                
                runtimeIdentifier.VersionLessDistroSpecificIdentifier =
                    GenerateRuntimeIdentifier(RuntimeIdentifierType.DistroSpecific, true, false);
            }
            else
            {
                runtimeIdentifier.DistroSpecificIdentifier = "N/A";
                runtimeIdentifier.VersionLessDistroSpecificIdentifier = "N/A";
            }
            
            runtimeIdentifier.DotNetIdentifier = DetectRuntimeIdentifier();

            return runtimeIdentifier;
        }
    }
}