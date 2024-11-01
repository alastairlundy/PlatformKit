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
using System.Runtime.InteropServices;
using System.Text;
using AlastairLundy.Extensions.System.Strings.Versioning;

using PlatformKit.OperatingSystems.Windows;

#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

    namespace PlatformKit.Core.Identification
    {
        
/// <summary>
/// A class to manage RuntimeId detection and programmatic generations
/// </summary>
public class TargetFrameworkIdentification
{
    protected RuntimeIdentification runtimeIdentification;
    protected WindowsOperatingSystem windowsOperatingSystem;
    
    public TargetFrameworkIdentification()
    {
        runtimeIdentification = new RuntimeIdentification();
        windowsOperatingSystem = new WindowsOperatingSystem();
    }
    
    // ReSharper disable once InconsistentNaming
    protected string GetNetTFM()
    {
        Version frameworkVersion = GetDotNetVersion();
        StringBuilder stringBuilder = new StringBuilder();
        
        stringBuilder.Append("net");

        stringBuilder.Append(frameworkVersion.Major);
        stringBuilder.Append(".");
        stringBuilder.Append(frameworkVersion.Minor);
        return stringBuilder.ToString();
    }

    // ReSharper disable once InconsistentNaming
    protected string GetOsSpecificNetTFM(TargetFrameworkMonikerType targetFrameworkMonikerType)
    {
        Version frameworkVersion = GetDotNetVersion();
        StringBuilder stringBuilder = new StringBuilder();

        if (OperatingSystem.IsMacOS())
        {
            stringBuilder.Append("-");
            stringBuilder.Append("macos");

            if (targetFrameworkMonikerType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
            {
                stringBuilder.Append(".");
                stringBuilder.Append(runtimeIdentification.GetOsVersionString());
            }
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

            if (targetFrameworkMonikerType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
            {
                bool isAtLeastWin8 = OperatingSystem.IsWindowsVersionAtLeast(6,2, 9200);
                bool isAtLeastWin8Point1 = OperatingSystem.IsWindowsVersionAtLeast(6, 3, 9600);

                bool isAtLeastWin10V1607 = OperatingSystem.IsWindowsVersionAtLeast(10, 0, 14393);
                
                
                if (isAtLeastWin8 || isAtLeastWin8Point1)
                {
                    stringBuilder.Append(runtimeIdentification.GetOsVersionString());
                }
                else if (isAtLeastWin10V1607)
                {
                    stringBuilder.Append(windowsOperatingSystem.GetOperatingSystemVersion());
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
            }
        }
        else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
        {
            //Do nothing because Linux doesn't have a version specific TFM.
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
#endif
#if NET8_0_OR_GREATER
        if (frameworkVersion.Major >= 8)
        {
            if (OperatingSystem.IsBrowser())
            {
                 stringBuilder.Append("-");
                 stringBuilder.Append("browser");
            }
        }
#endif
        return stringBuilder.ToString();
    }

    // ReSharper disable once InconsistentNaming
        protected string GetNetCoreTFM()
        {
            Version frameworkVersion = GetDotNetVersion();
            
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("netcoreapp");

            stringBuilder.Append(frameworkVersion.Major);
            stringBuilder.Append(".");
            stringBuilder.Append(frameworkVersion.Minor);

            return stringBuilder.ToString();
        }

        // ReSharper disable once InconsistentNaming
        protected string GetNetFrameworkTFM()
        {
            Version frameworkVersion = GetDotNetVersion();
            
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append(frameworkVersion.Major);
            stringBuilder.Append(frameworkVersion.Minor);
                                                    
            if (frameworkVersion.Build != 0)
            {
                stringBuilder.Append(frameworkVersion.Build);
            }

            return stringBuilder.ToString();
        }

        // ReSharper disable once InconsistentNaming
        protected string GetMonoTFM()
        {
            StringBuilder stringBuilder = new StringBuilder();
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

            return stringBuilder.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Version GetDotNetVersion()
        {
            return new Version(RuntimeInformation.FrameworkDescription.ToLower().Replace(".net", string.Empty)
                .Replace("core", string.Empty)
                .Replace(" ", string.Empty).AddMissingZeroes());
        }
    
        /// <summary>
        /// Detect the Target Framework Moniker (TFM) of the currently running system.
        /// Note: This does not detect .NET Standard TFMs, UWP TFMs, Windows Phone TFMs, Silverlight TFMs, and Windows Store TFMs.
        ///
        /// IOS and Android version specific TFM generation isn't supported at this time but may be added in a future release.
        /// </summary>
        /// <param name="targetFrameworkType">The type of TFM to generate.</param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public string GetTargetFrameworkMoniker(TargetFrameworkMonikerType targetFrameworkType)
        {
            Version frameworkVersion = GetDotNetVersion();
            
            if (RuntimeInformation.FrameworkDescription.ToLower().Contains("core"))
            {
                return GetNetCoreTFM();
            }
            else if (RuntimeInformation.FrameworkDescription.ToLower().Contains("mono"))
            {
                return GetMonoTFM();
            }
            else
            {
                if (RuntimeInformation.FrameworkDescription.ToLower().Contains(".net"))
                {
                    if (frameworkVersion.Major < 5)
                    {
                        return GetNetFrameworkTFM();
                    }
                    if (targetFrameworkType == TargetFrameworkMonikerType.Generic)
                    {
                        return GetNetTFM();
                    }
                    if(targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemSpecific || targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
                    {
                        return GetOsSpecificNetTFM(targetFrameworkType);
                    }
                }
            }

            throw new PlatformNotSupportedException();
        }
    }
}