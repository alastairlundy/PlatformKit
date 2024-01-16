/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023
    
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Runtime.InteropServices;
using PlatformKit.Extensions;
using PlatformKit.Internal.Exceptions;


namespace PlatformKit.Mac;

// ReSharper disable once InconsistentNaming
    /// <summary>
    /// A class to Detect macOS versions, macOS features, and find out more about a user's macOS installation.
    /// </summary>
    public class MacOsAnalyzer
    {
        private readonly ProcessManager _processManager;

        public MacOsAnalyzer()
        {
            _processManager = new ProcessManager();
        }
        
        /// <summary>
        /// Returns whether or not a Mac is Apple Silicon based.
        /// </summary>
        /// <returns></returns>
        public bool IsAppleSiliconMac()
        {
            return GetMacProcessorType().Equals(MacProcessorType.AppleSilicon);
        }
        
        /// <summary>
        /// Gets the type of Processor in a given Mac returned as the MacProcessorType
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public MacProcessorType GetMacProcessorType()
        {
            try
            {
                if (OSAnalyzer.IsMac())
                {
                    return System.Runtime.InteropServices.RuntimeInformation.OSArchitecture switch
                    {
                        Architecture.Arm64 => MacProcessorType.AppleSilicon,
                        Architecture.X64 => MacProcessorType.Intel,
                        _ => MacProcessorType.NotDetected
                    };
                }
                throw new PlatformNotSupportedException();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw;
            }
        }

        /// <summary>
        /// Gets a value from the Mac System Profiler information associated with a key
        /// </summary>
        /// <param name="macSystemProfilerDataType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public string GetMacSystemProfilerInformation(MacSystemProfilerDataType macSystemProfilerDataType, string key)
        {
            string info = _processManager.RunMacCommand("system_profiler SP" + macSystemProfilerDataType);

#if NETSTANDARD2_0_OR_GREATER
            string[] array = info.Split(Convert.ToChar(Environment.NewLine));
#elif NET5_0_OR_GREATER
            string[] array = info.Split(Environment.NewLine);
#endif
            foreach (var str in array)
            {
                if (str.ToLower().Contains(key.ToLower()))
                {
                    return str.Replace(key, String.Empty).Replace(":", String.Empty);
                }
            }

            throw new ArgumentException();
        }    
    
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public bool IsSecureVirtualMemoryEnabled()
        {
            var result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.SoftwareDataType, "Secure Virtual Memory");

            if (result.ToLower().Contains("disabled"))
            {
                return false;
            }
            else if (result.ToLower().Contains("enabled"))
            {
                return true;
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
        /// <exception cref="ArgumentException"></exception>
        public bool IsSystemIntegrityProtectionEnabled()
        {
            var result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.SoftwareDataType, "System Integrity Protection");

            if (result.ToLower().Contains("disabled"))
            {
                return false;
            }
            else if (result.ToLower().Contains("enabled"))
            {
                return true;
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
        /// <exception cref="ArgumentException"></exception>
        public bool IsActivationLockEnabled()
        {
            var result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Activation Lock Status");

            if (result.ToLower().Contains("disabled"))
            {
                return false;
            }
            else if (result.ToLower().Contains("enabled"))
            {
                return true;
            }
            else
            {
                throw new ArgumentException();
            }
        }
        
        /// <summary>
    /// Returns macOS version as a macOS version enum.
    /// </summary>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on any platform besides macOS.</exception>
    /// <returns></returns>
    public MacOsVersion GetMacOsVersionToEnum()
    {
        return GetMacOsVersionToEnum(DetectMacOsVersion());
    }

    /// <summary>
    /// Converts a macOS version to a macOS version enum.
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on any platform besides macOS.</exception>
    /// <exception cref="Exception"></exception>
    public MacOsVersion GetMacOsVersionToEnum(Version input)
    {
            if (OSAnalyzer.IsMac())
            {
                if (input.Major == 10)
                {
                    switch (input.Minor)
                    {
                        case 0:
                            //return MacOsVersion.v10_0_Cheetah;
                            return MacOsVersion.NotSupported;
                        case 1:
                            // return MacOsVersion.v10_1_Puma;
                            return MacOsVersion.NotSupported;
                        case 2:
                            // return MacOsVersion.v10_2_Jaguar;
                            return MacOsVersion.NotSupported;
                        case 3:
                            // return MacOsVersion.v10_3_Panther;
                            return MacOsVersion.NotSupported;
                        case 4:
                            // return MacOsVersion.v10_4_Tiger;
                            return MacOsVersion.NotSupported;
                        case 5:
                            // return MacOsVersion.v10_5_Leopard;
                            return MacOsVersion.NotSupported;
                        case 6:
                            //return MacOsVersion.v10_6_SnowLeopard;
                            return MacOsVersion.NotSupported;
                        case 7:
                            //return MacOsVersion.v10_7_Lion;
                            return MacOsVersion.NotSupported;
                        case 8:
                            //return MacOsVersion.v10_8_MountainLion;
                            return MacOsVersion.NotSupported;
                        case 9:
                            return MacOsVersion.v10_9_Mavericks;
                        case 10:
                            return MacOsVersion.v10_10_Yosemite;
                        case 11:
                            return MacOsVersion.v10_11_ElCapitan;
                        case 12:
                            return MacOsVersion.v10_12_Sierra;
                        case 13:
                            return MacOsVersion.v10_13_HighSierra;
                        case 14:
                            return MacOsVersion.v10_14_Mojave;
                        case 15:
                            return MacOsVersion.v10_15_Catalina;
                        //This is for compatibility reasons.
                        case 16:
                            return MacOsVersion.v11_BigSur;
                    }
                }

                if (input.Major == 11) return MacOsVersion.v11_BigSur;
                if (input.Major == 12) return MacOsVersion.v12_Monterey;
                if (input.Major == 13) return MacOsVersion.v13_Ventura;
                if (input.Major == 14) return MacOsVersion.v14_Sonoma;
                
                throw new MacOsVersionDetectionException();
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
    }

    /// <summary>
    /// Converts a macOS version enum to a macOS version as a Version object.
    /// </summary>
    /// <param name="macOsVersion"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Throws an exception if an unsupported macOS version is provided.</exception>
    /// <exception cref="MacOsVersionDetectionException">Throws an exception if detection fails.</exception>
    public Version GetMacOsVersionFromEnum(MacOsVersion macOsVersion)
    {
        switch (macOsVersion)
        {
            case MacOsVersion.v10_9_Mavericks:
                return new(10, 9);
            case MacOsVersion.v10_10_Yosemite:
                return new(10, 10);
            case MacOsVersion.v10_11_ElCapitan:
                return new(10, 11);
            case MacOsVersion.v10_12_Sierra:
                return new(10, 12);
            case MacOsVersion.v10_13_HighSierra:
                return new(10, 13);
            case MacOsVersion.v10_14_Mojave:
                return new(10, 14);
            case MacOsVersion.v10_15_Catalina:
                return new(10, 15);
            case MacOsVersion.v11_BigSur:
                return new(11, 0);
            case MacOsVersion.v12_Monterey:
                return new(12, 0);
            case MacOsVersion.v13_Ventura:
                return new(13, 0);
            case MacOsVersion.v14_Sonoma:
                return new Version(14, 0);
            case MacOsVersion.NotSupported:
                throw new PlatformNotSupportedException();
            case MacOsVersion.NotDetected:
                throw new MacOsVersionDetectionException();
            default:
                throw new ArgumentException();
        }        
    }


    /// <summary>
    /// Checks to see whether the specified version of macOS is the same or newer than the installed version of macOS.
    /// </summary>
    /// <param name="macOsVersion"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on any platform besides macOS.</exception>
    // ReSharper disable once InconsistentNaming
    public bool IsAtLeastVersion(MacOsVersion macOsVersion)
    {
        if (OSAnalyzer.IsMac())
        {
            var detected = DetectMacOsVersion();

            return detected.IsAtLeast(GetMacOsVersionFromEnum(macOsVersion));
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
    
    public bool IsAtLeastVersion(Version macOsVersion)
    {
        if (OSAnalyzer.IsMac())
        {
            return IsAtLeastVersion(GetMacOsVersionToEnum(macOsVersion));
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
    

    /// <summary>
    /// Detects macOS System Information.
    /// </summary>
    /// <returns></returns>
    public MacOsSystemInformation DetectMacSystemInformation()
    {
        return new MacOsSystemInformation()
        {
            ProcessorType = GetMacProcessorType(),
            MacOsBuildNumber = DetectMacOsBuildNumber(),
            MacOsVersion = DetectMacOsVersion(),
            DarwinVersion = DetectDarwinVersion(),
            XnuVersion = DetectXnuVersion()
        };
    }

    /// <summary>
    /// Detects the Darwin Version on macOS
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on any platform besides macOS.</exception>
    public Version DetectDarwinVersion()
    {
        if (OSAnalyzer.IsMac())
        {
            var desc = RuntimeInformation.OSDescription;
            var arr = desc.Split(' ');

            int dotCounter = arr[1].CountDotsInString();

            if (dotCounter == 2)
            {
                arr[1] += ".0";
                return Version.Parse(arr[1]);
            }
            if (dotCounter == 3)
            {
                return Version.Parse(arr[1]);
            }
          
            throw new PlatformNotSupportedException();
        }
        else
        {

            throw new PlatformNotSupportedException();
        }
    }
    
    /// <summary>
    /// Detects macOS's XNU Version.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Not supported on platforms other than macOS</exception>
    public Version DetectXnuVersion()
    {
        if (OSAnalyzer.IsMac())
        {
            var desc = RuntimeInformation.OSDescription;
            var arr = desc.Split(' ');
        
            for (int index = 0; index < arr.Length; index++)
            {
                if (arr[index].ToLower().StartsWith("root:xnu-"))
                {
                    arr[index] = arr[index].Replace("root:xnu-", String.Empty)
                        .Replace("~", ".");

                    if (IsAppleSiliconMac())
                    {
                        arr[index] = arr[index].Replace("/RELEASE_ARM64_T", String.Empty);
                        
                        arr[index] = arr[index].Remove((arr.Length + 1) - 3);
                        // M1 specific code
                        // arr[index] = arr[index].Replace("/RELEASE_ARM64_T8101", String.Empty);
                    }
                    else
                    {
                        arr[index] = arr[index].Replace("/RELEASE_X86_64", String.Empty);
                    }

                    return Version.Parse(arr[index]);
                }
            }

            throw new PlatformNotSupportedException();
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    /// <summary>
    /// Detects the macOS version and returns it as a System.Version object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on any platform besides macOS.</exception>
    public Version DetectMacOsVersion()
    {
        if (OSAnalyzer.IsMac())
        {
            try
            {
                var version = GetMacSwVersInfo()[1].Replace("ProductVersion:", String.Empty).Replace(" ", String.Empty);

                int dotCounter = 0;

                foreach (var str in version)
                {
                    if (str == '.')
                    {
                        dotCounter++;
                    }
                }

                if (dotCounter == 1)
                {
                    version += ".0";
                }
                if (dotCounter == 2)
                {
                    version += ".0";
                }
        
                return Version.Parse(version);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
    
    /// <summary>
    /// Detects the Build Number of the installed version of macOS.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="Exception"></exception>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if not run o n macOS.</exception>
    public string DetectMacOsBuildNumber()
    {
        if (OSAnalyzer.IsMac())
        {
            try
            {
                return GetMacSwVersInfo()[2].ToLower().Replace("BuildVersion:", String.Empty).Replace(" ", String.Empty);
            }
            catch(Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    // ReSharper disable once IdentifierTypo
    /// <summary>
    /// Gets info from sw_vers command on Mac.
    /// </summary>
    /// <returns></returns>
    private string[] GetMacSwVersInfo()
    {
        // ReSharper disable once StringLiteralTypo
        return _processManager.RunMacCommand("sw_vers").Split(Convert.ToChar(Environment.NewLine));
    }
}