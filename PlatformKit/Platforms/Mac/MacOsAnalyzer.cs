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
using System.Runtime.InteropServices;

using AlastairLundy.System.Extensions.StringExtensions;
using AlastairLundy.System.Extensions.VersionExtensions;
    
using PlatformKit.Internal.Exceptions;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Mac;

// ReSharper disable once InconsistentNaming
    /// <summary>
    /// A class to Detect macOS versions, macOS features, and find out more about a user's macOS installation.
    /// </summary>
    public class MacOsAnalyzer
    {
        
        /// <summary>
        /// Returns whether a Mac is Apple Silicon based.
        /// </summary>
        /// <returns></returns>
        public static bool IsAppleSiliconMac()
        {
            return GetMacProcessorType().Equals(MacProcessorType.AppleSilicon);
        }
        
        /// <summary>
        /// Gets the type of Processor in a given Mac returned as the MacProcessorType
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public static MacProcessorType GetMacProcessorType()
        {
            if (OperatingSystem.IsMacOS())
            {
                return RuntimeInformation.OSArchitecture switch
                {
                    Architecture.Arm64 => MacProcessorType.AppleSilicon,
                    Architecture.X64 => MacProcessorType.Intel,
                    _ => MacProcessorType.NotDetected
                };
            }
            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// Gets a value from the Mac System Profiler information associated with a key
        /// </summary>
        /// <param name="macSystemProfilerDataType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static string GetMacSystemProfilerInformation(MacSystemProfilerDataType macSystemProfilerDataType, string key)
        {
            string info = CommandRunner.RunCommandOnMac("system_profiler SP" + macSystemProfilerDataType);

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
        /// Returns whether the Mac running this method has Secure Virtual Memory enabled or not.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsSecureVirtualMemoryEnabled()
        {
            var result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.SoftwareDataType, "Secure Virtual Memory");

            if (result.ToLower().Contains("disabled"))
            {
                return false;
            }

            if (result.ToLower().Contains("enabled"))
            {
                return true;
            }

            throw new ArgumentException();
        }
    
        /// <summary>
        ///  Returns whether the Mac running this method has System Integrity Protection enabled or not.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsSystemIntegrityProtectionEnabled()
        {
            var result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.SoftwareDataType, "System Integrity Protection");

            if (result.ToLower().Contains("disabled"))
            {
                return false;
            }

            if (result.ToLower().Contains("enabled"))
            {
                return true;
            }

            throw new ArgumentException();
        }

        /// <summary>
        ///  Returns whether the Mac running this method has Activation Lock enabled or not.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static bool IsActivationLockEnabled()
        {
            var result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Activation Lock Status");

            if (result.ToLower().Contains("disabled"))
            {
                return false;
            }

            if (result.ToLower().Contains("enabled"))
            {
                return true;
            }

            throw new ArgumentException();
        }
        
        /// <summary>
        /// Returns macOS version as a macOS version enum.
        /// </summary>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on a platform that isn't macOS.</exception>
        /// <returns></returns>
        public static MacOsVersion GetMacOsVersionToEnum()
        {
            return GetMacOsVersionToEnum(GetMacOsVersion());
        }

        /// <summary>
        /// Converts a macOS version to a macOS version enum.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on a platform that isn't macOS.</exception>
        /// <exception cref="Exception"></exception>
        public static MacOsVersion GetMacOsVersionToEnum(Version input)
        {
            if (OperatingSystem.IsMacOS())
            {
                    if (input.Major == 10)
                    {
                        switch (input.Minor) {
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

            throw new PlatformNotSupportedException();
        }

    /// <summary>
    /// Converts a macOS version enum to a macOS version as a Version object.
    /// </summary>
    /// <param name="macOsVersion"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Throws an exception if an unsupported macOS version is provided.</exception>
    /// <exception cref="MacOsVersionDetectionException">Throws an exception if macOS version detection fails.</exception>
    public static Version GetMacOsVersionFromEnum(MacOsVersion macOsVersion)
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
    /// <param name="macOsVersion">A MacOsVersion enum representing a major version of macOS.</param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on a platform that isn't macOS.</exception>
    // ReSharper disable once InconsistentNaming
    public static bool IsAtLeastVersion(MacOsVersion macOsVersion)
    {
        if (OperatingSystem.IsMacOS())
        {
            return GetMacOsVersion().IsAtLeast(GetMacOsVersionFromEnum(macOsVersion));
        }

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// Checks to see whether the specified version of macOS is the same or newer than the installed version of macOS.
    /// </summary>
    /// <param name="macOsVersion"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on a platform that isn't macOS. </exception>
    public static bool IsAtLeastVersion(Version macOsVersion)
    {
        if (OperatingSystem.IsMacOS())
        {
            return IsAtLeastVersion(GetMacOsVersionToEnum(macOsVersion));
        }

        throw new PlatformNotSupportedException();
    }
    

    /// <summary>
    /// Detects macOS System Information.
    /// </summary>
    /// <returns></returns>
    public static MacOsSystemInformation GetMacSystemInformation()
    {
        return new MacOsSystemInformation()
        {
            ProcessorType = GetMacProcessorType(),
            MacOsBuildNumber = GetMacOsBuildNumber(),
            MacOsVersion = GetMacOsVersion(),
            DarwinVersion = GetDarwinVersion(),
            XnuVersion = GetXnuVersion()
        };
    }

    /// <summary>
    /// Detects the Darwin Version on macOS
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't macOS.</exception>
    public static Version GetDarwinVersion()
    {
        if (OperatingSystem.IsMacOS())
        {
            return Version.Parse(RuntimeInformation.OSDescription.Split(' ')[1].AddMissingZeroes());
        }

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// Detects macOS's XNU Version.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't macOS.</exception>
    public static Version GetXnuVersion()
    {
        if (OperatingSystem.IsMacOS())
        {
            var array = RuntimeInformation.OSDescription.Split(' ');
        
            for (int index = 0; index < array.Length; index++)
            {
                if (array[index].ToLower().StartsWith("root:xnu-"))
                {
                    array[index] = array[index].Replace("root:xnu-", String.Empty)
                        .Replace("~", ".");

                    if (IsAppleSiliconMac())
                    {
                        array[index] = array[index].Replace("/RELEASE_ARM64_T", String.Empty).Remove((array.Length + 1) - 3);
                    }
                    else
                    {
                        array[index] = array[index].Replace("/RELEASE_X86_64", String.Empty);
                    }

                    return Version.Parse(array[index]);
                }
            }

            throw new PlatformNotSupportedException();
        }

        throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// Detects the macOS version and returns it as a System.Version object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't macOS.</exception>
    public static Version GetMacOsVersion()
    {
        if (OperatingSystem.IsMacOS())
        {
            return Version.Parse(GetMacSwVersInfo()[1].Replace("ProductVersion:", string.Empty)
                .Replace(" ", string.Empty).AddMissingZeroes());
        }

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// Detects the Build Number of the installed version of macOS.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't macOS.</exception>
    public static string GetMacOsBuildNumber()
    {
        if (OperatingSystem.IsMacOS())
        {
            return GetMacSwVersInfo()[2].ToLower().Replace("BuildVersion:",
                String.Empty).Replace(" ", String.Empty);
        }

        throw new PlatformNotSupportedException();
    }

    // ReSharper disable once IdentifierTypo
    /// <summary>
    /// Gets info from sw_vers command on Mac.
    /// </summary>
    /// <returns></returns>
    private static string[] GetMacSwVersInfo()
    {
        // ReSharper disable once StringLiteralTypo
        return CommandRunner.RunCommandOnMac("sw_vers").Split(Convert.ToChar(Environment.NewLine));
    }
}