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

using CliWrap;
using CliWrap.Buffered;

using PlatformKit.Internal.Deprecation;
using PlatformKit.Internal.Exceptions;
using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
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
        /// <returns>true if the currently running Mac uses Apple Silicon; false if running on an Intel Mac.</returns>
        public static bool IsAppleSiliconMac()
        {
            return OperatingSystem.IsMacOS() && RuntimeInformation.OSArchitecture == Architecture.Arm64;
        }
        
        /// <summary>
        /// Gets the type of Processor in a given Mac returned as the MacProcessorType
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static MacProcessorType GetMacProcessorType()
        {
            if (OperatingSystem.IsMacOS())
            {
                return RuntimeInformation.OSArchitecture switch
                {
                    Architecture.Arm64 => MacProcessorType.AppleSilicon,
                    Architecture.X64 => MacProcessorType.Intel,
                    _ => MacProcessorType.NotSupported
                };
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }
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
            var task = Cli.Wrap("/usr/bin/system_profiler")
                .WithArguments("system_profiler SP" + macSystemProfilerDataType)
                .ExecuteBufferedAsync();
            
            task.Task.RunSynchronously();

            task.Task.Wait();

            string info = task.Task.Result.StandardOutput;
            
#if NETSTANDARD2_0_OR_GREATER
            string[] array = info.Split(Convert.ToChar(Environment.NewLine));
#elif NET5_0_OR_GREATER
            string[] array = info.Split(Environment.NewLine);
#endif
            foreach (string str in array)
            {
                if (str.ToLower().Contains(key.ToLower()))
                {
                    return str.Replace(key, string.Empty).Replace(":", string.Empty);
                }
            }

            throw new ArgumentException();
        }    
    
        /// <summary>
        /// Returns whether the Mac running this method has Secure Virtual Memory enabled or not.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static bool IsSecureVirtualMemoryEnabled()
        {
            string result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.SoftwareDataType, "Secure Virtual Memory");

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
        /// <returns>true if System Integrity Protection is enabled on this Mac; returns false otherwise.</returns>
        /// <exception cref="ArgumentException"></exception>
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static bool IsSystemIntegrityProtectionEnabled()
        {
            string result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.SoftwareDataType, "System Integrity Protection");

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
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static bool IsActivationLockEnabled()
        {
            string result = GetMacSystemProfilerInformation(MacSystemProfilerDataType.HardwareDataType, "Activation Lock Status");

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
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
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
        public static MacOsVersion GetMacOsVersionToEnum(Version input)
        {
                return input.Major switch
                {
                    10 => input.Minor switch
                    {
                        < 16 => MacOsVersion.NotSupported,
                        //This is for compatibility reasons.
                        16 => MacOsVersion.v11_BigSur,
                        _ => MacOsVersion.NotDetected
                    },
                    11 => MacOsVersion.v11_BigSur,
                    12 => MacOsVersion.v12_Monterey,
                    13 => MacOsVersion.v13_Ventura,
                    14 => MacOsVersion.v14_Sonoma,
                    15 => MacOsVersion.v15_Sequoia,
                    _ => MacOsVersion.NotDetected
                };
        }

    /// <summary>
    /// Converts a macOS version enum to a macOS version as a Version object.
    /// </summary>
    /// <param name="macOsVersion"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
    /// <exception cref="MacOsVersionDetectionException">Throws an exception if macOS version detection fails.</exception>
    public static Version GetMacOsVersionFromEnum(MacOsVersion macOsVersion)
    {
        return macOsVersion switch
        {
            MacOsVersion.v11_BigSur => new(11, 0),
            MacOsVersion.v12_Monterey => new(12, 0),
            MacOsVersion.v13_Ventura => new(13, 0),
            MacOsVersion.v14_Sonoma => new Version(14, 0),
            MacOsVersion.v15_Sequoia => new Version(15,0),
            MacOsVersion.NotSupported => throw new PlatformNotSupportedException(),
            MacOsVersion.NotDetected => throw new MacOsVersionDetectionException(),
            _ => throw new ArgumentException(Resources.Exceptions_Arguments_InvalidMacOsVersionEnum),
        };
    }


    /// <summary>
    /// Checks to see whether the specified version of macOS is the same or newer than the installed version of macOS.
    /// </summary>
    /// <param name="macOsVersion">A MacOsVersion enum representing a major version of macOS.</param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
    // ReSharper disable once InconsistentNaming
    public static bool IsAtLeastVersion(MacOsVersion macOsVersion)
    {
        if (OperatingSystem.IsMacOS())
        {
            return GetMacOsVersion() >= (GetMacOsVersionFromEnum(macOsVersion));
        }
        else
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
    }
    
    /// <summary>
    /// Checks to see whether the specified version of macOS is the same or newer than the installed version of macOS.
    /// </summary>
    /// <param name="macOsVersion"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
    [Obsolete(DeprecationMessages.DeprecationV5)]
    public static bool IsAtLeastVersion(Version macOsVersion)
    {
        if (OperatingSystem.IsMacOS())
        {
            return GetMacOsVersion() >= (macOsVersion);
        }
        else
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
    }
    
    /// <summary>
    /// Detects macOS System Information.
    /// </summary>
    /// <returns></returns>
    public static MacOsSystemInformationModel GetMacSystemInformationModel()
    {
        if (OperatingSystem.IsMacOS())
        {
            return new MacOsSystemInformationModel()
            {
                ProcessorType = GetMacProcessorType(),
                MacOsBuildNumber = GetMacOsBuildNumber(),
                MacOsVersion = GetMacOsVersion(),
                DarwinVersion = GetDarwinVersion(),
                XnuVersion = GetXnuVersion()
            };
        }
        else
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
    }

    /// <summary>
    /// Detects the Darwin Version on macOS
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
    public static Version GetDarwinVersion()
    {
        if (OperatingSystem.IsMacOS())
        {
            return Version.Parse(RuntimeInformation.OSDescription.Split(' ')[1]);
        }

        throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
    }
    
    /// <summary>
    /// Detects macOS's XNU Version.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
    public static Version GetXnuVersion()
    {
        if (!OperatingSystem.IsMacOS())
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);    
        }
        
        string[] array = RuntimeInformation.OSDescription.Split(' ');
        
        for (int index = 0; index < array.Length; index++)
        {
            if (array[index].ToLower().StartsWith("root:xnu-"))
            {
                array[index] = array[index].Replace("root:xnu-", string.Empty)
                    .Replace("~", ".");

                if (IsAppleSiliconMac())
                {
                    array[index] = array[index].Replace("/RELEASE_ARM64_T", string.Empty).Remove(array.Length - 4);
                }
                else
                {
                    array[index] = array[index].Replace("/RELEASE_X86_64", string.Empty);
                }

                return Version.Parse(array[index]);
            }
        }

        throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// Detects the macOS version and returns it as a System.Version object.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
    public static Version GetMacOsVersion()
    {
        if (OperatingSystem.IsMacOS())
        {
            return Version.Parse(GetMacSwVersInfo()[1].Replace("ProductVersion:", string.Empty)
                .Replace(" ", string.Empty));
        }
        else
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
    }
    
    /// <summary>
    /// Detects the Build Number of the installed version of macOS.
    /// </summary>
    /// <returns>the build number of the installed version of macOS.</returns>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
    public static string GetMacOsBuildNumber()
    {
        if (OperatingSystem.IsMacOS())
        {
            return GetMacSwVersInfo()[2].ToLower().Replace("BuildVersion:",
                string.Empty).Replace(" ", string.Empty);
        }
        else
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
    }

    // ReSharper disable once IdentifierTypo
    /// <summary>
    /// Gets info from sw_vers command on Mac.
    /// </summary>
    /// <returns></returns>
    private static string[] GetMacSwVersInfo()
    {
        var task = Cli.Wrap("/usr/bin/sw_vers")
            .ExecuteBufferedAsync();

        task.Task.RunSynchronously();

        task.Task.Wait();
        
        // ReSharper disable once StringLiteralTypo
        return task.Task.Result.StandardOutput.Split(Convert.ToChar(Environment.NewLine));
    }
}