﻿/*
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
using System.IO;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using PlatformKit.Internal.Localizations;

using PlatformKit.OperatingSystems.Abstractions;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.OperatingSystems.Linux
{

    public class LinuxOperatingSystem : IOperatingSystem
    {

        /// <summary>
        /// Detects the linux kernel version.
        /// </summary>
        /// <returns>the linux kernel version as a C# Version object.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
        public Version GetKernelVersion()
        {
            if (OperatingSystem.IsLinux())
            {
                return Version.Parse(Environment.OSVersion.ToString()
                    .Replace("Unix ", string.Empty));
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        public string GetOperatingSystemBuildNumber()
        {
            Task<string> output = GetOsReleasePropertyValueAsync("VERSION_ID");
            output.Start();

            output.Wait();
            return output.Result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="propertyName"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        internal async Task<string> GetOsReleasePropertyValueAsync(string propertyName)
        {
            if (OperatingSystem.IsLinux())
            {
                string output = string.Empty;

#if NET5_0_OR_GREATER
                string[] osReleaseInfo = await File.ReadAllLinesAsync("/etc/os-release");
#else
                string[] osReleaseInfo = await Task.Run(() => File.ReadAllLines("/etc/os-release"));
#endif
                
                foreach (string s in osReleaseInfo)
                {
                    if (s.ToUpper().StartsWith(propertyName))
                    {
                        output = s.Replace(propertyName, string.Empty);
                    }
                }

                return output;
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }
        }

        /// <summary>
        /// Returns the linux distribution version.
        /// </summary>
        /// <returns>the linux distribution version as a Version object.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based operating system.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        public Version GetOperatingSystemVersion()
        {
            Task<Version> output = GetOperatingSystemVersionAsync();
            output.Start();
            output.Wait();
            return output.Result;
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("linux")]
#endif
        public async Task<Version> GetOperatingSystemVersionAsync()
        {
            if (OperatingSystem.IsLinux())
            {
                string versionString = await GetOsReleasePropertyValueAsync("VERSION=");
                versionString = versionString.Replace("LTS", string.Empty);    
                    
                Version output = Version.Parse(versionString);

                return output;
            }
            else
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }
        }
    }
}