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
using System.Runtime.Versioning;

using PlatformKit.OperatingSystems.Abstractions;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.OperatingSystems.Windows
{
    public class WindowsOperatingSystem : IWindowsOperatingSystem
    {
        /// <summary>
        /// Detects Windows Version and returns it as a System.Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        // ReSharper disable once MemberCanBePrivate.Global
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public Version GetOperatingSystemVersion()
        {
            if (OperatingSystem.IsWindows())
            {
                return Version.Parse(RuntimeInformation.OSDescription
                    .Replace("Microsoft Windows", string.Empty)
                    .Replace(" ", string.Empty));
            }

            throw new PlatformNotSupportedException();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public Version GetKernelVersion()
        {
            return GetOperatingSystemVersion();
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        public string GetOperatingSystemBuildNumber()
        {
            // ReSharper disable once RedundantAssignment
            string output = string.Empty;
            
            Version osVersion = GetOperatingSystemVersion();

            if (osVersion.Revision != 0)
            {
                output = $"{osVersion.Build}.{osVersion.Revision}";
            }
            else
            {
                output = $"{osVersion.Build}";
            }

            return output;
        }
    }
}