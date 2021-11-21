using System;
using System.Runtime.InteropServices;
using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Models;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Enums;

// ReSharper disable InconsistentNaming

namespace AluminiumTech.DevKit.PlatformKit
{
    public class RuntimeIdentification
    {
        protected OSAnalyzer _osAnalyzer;
        protected PlatformManager _platformManager;
        protected OSVersionAnalyzer versionAnalyzer;


        public RuntimeIdentification()
        {
            _platformManager = new PlatformManager();
            versionAnalyzer = new OSVersionAnalyzer();
            _osAnalyzer = new OSAnalyzer();
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

            if (_platformManager.IsWindows())
            {
                osName = "win";
            }
            if (_platformManager.IsMac())
            {
                osName = "osx";
            }
            if (_platformManager.IsLinux())
            {
                if (useGenericTFM)
                {
                    osName = "linux";
                }
                else
                {
                    osName = _osAnalyzer.GetLinuxDistributionInformation().Name;
                }
            }

            return osName;
        }

        protected string GetOsVersionString()
        {
            string osVersion = null;

            if (_platformManager.IsWindows())
            {
                if (versionAnalyzer.IsWindows10())
                {
                    osVersion = "10";
                }
                else if (versionAnalyzer.IsWindows11())
                {
                    osVersion = "11";
                }
            }

            if (_platformManager.IsLinux())
            {
                var version = versionAnalyzer.DetectLinuxDistributionVersionAsString();
                //osVersion = version.GetFriendlyVersionToString(FriendlyVersionFormatStyle.MajorDotMinor);
                osVersion = version;
            }

            if (_platformManager.IsMac())
            {
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
        /// </summary>
        /// <returns></returns>
        public string GenerateGenericTFM()
        {
            var osName = GetOsNameString(true);
            var cpuArch = GetArchitectureString();

            return $"{osName}-{cpuArch}";
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public string GenerateSpecificTFM()
        {
            var osName = GetOsNameString(false);
            var osVersion = GetOsVersionString();
            var cpuArch = GetArchitectureString();

            if (_platformManager.IsWindows())
            {
                return $"{osName}-{cpuArch}";
            }
            else
            {
                return $"{osName}.{osVersion}-{cpuArch}";
            }
        }

        /// <summary>
        ///     Detects the TFM if running on .NET 5 or later and generates the TFM if running on .NET Standard 2.0 or later.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once InconsistentNaming
        public string DetectTFM()
        {
#if NET5_0_OR_GREATER
            return RuntimeInformation.RuntimeIdentifier;
#endif
            return GenerateGenericTFM();
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public RuntimeIdentifier GenerateTFM()
        {
            var runtimeIdentifier = new RuntimeIdentifier();
            runtimeIdentifier.GeneratedGenericIdentifier = GenerateGenericTFM();
            runtimeIdentifier.GeneratedSpecificIdentifier = GenerateSpecificTFM();

            runtimeIdentifier.DotNetIdentifier = DetectTFM();

            return runtimeIdentifier;
        }
    }
}