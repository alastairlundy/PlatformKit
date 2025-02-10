/*
        MIT License
       
       Copyright (c) 2020-2025 Alastair Lundy
       
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
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;

using AlastairLundy.Extensions.System.Strings.Versioning;

using PlatformKit.Internal.Deprecation;
using PlatformKit.Internal.Exceptions;

using PlatformKit.Mac;
using PlatformKit.Windows;
    
#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#endif

namespace PlatformKit.Identification;

/// <summary>
/// A class to manage RuntimeId detection and programmatic generations
/// </summary>
[Obsolete(DeprecationMessages.DeprecationV5)]
public class TargetFrameworkIdentification
{
    // ReSharper disable once InconsistentNaming
    protected static string GetNetTFM()
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
    protected static string GetOsSpecificNetTFM(TargetFrameworkMonikerType targetFrameworkMonikerType)
    {
        Version frameworkVersion = GetDotNetVersion();
        StringBuilder stringBuilder = new StringBuilder();
        stringBuilder.Append(GetNetTFM());
        
        if (OperatingSystem.IsMacOS())
        {
            stringBuilder.Append('-');
            stringBuilder.Append("macos");

            if (targetFrameworkMonikerType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
            {
                stringBuilder.Append(".");
                stringBuilder.Append(RuntimeIdentification.GetOsVersionString());
            }
        }
#if NET5_0_OR_GREATER
        else if (OperatingSystem.IsMacCatalyst())
        {
            stringBuilder.Append('-');
            stringBuilder.Append("maccatalyst");
        }
#endif
        else if (OperatingSystem.IsWindows())
        {
            stringBuilder.Append('-');
            stringBuilder.Append("windows");

            if (targetFrameworkMonikerType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
            {
                Version winVersion = WindowsAnalyzer.GetWindowsVersion();

                if (winVersion.Build == 9200 || winVersion.Build == 9600)
                {
                    stringBuilder.Append(RuntimeIdentification.GetOsVersionString());
                }
                else if (winVersion.Build >= 14393)
                {
                    stringBuilder.Append(winVersion);
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
            stringBuilder.Append('-');
            stringBuilder.Append("android");
        }
        else if (OperatingSystem.IsIOS())
        {
            stringBuilder.Append('-');
            stringBuilder.Append("ios");
        }
        else if (OperatingSystem.IsTvOS())
        {
            stringBuilder.Append('-');
            stringBuilder.Append("tvos");
        }
        else if (OperatingSystem.IsWatchOS())
        {
            stringBuilder.Append('-');
            stringBuilder.Append("watchos");
        }
#endif
#if NET8_0_OR_GREATER
        if (frameworkVersion.Major >= 8)
        {
            if (OperatingSystem.IsBrowser())
            {
                 stringBuilder.Append('-');
                 stringBuilder.Append("browser");
            }
        }
#endif
        return stringBuilder.ToString();
    }

    // ReSharper disable once InconsistentNaming
        protected static string GetNetCoreTFM()
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
        protected static string GetNetFrameworkTFM()
        {
            Version frameworkVersion = GetDotNetVersion();
            StringBuilder stringBuilder = new StringBuilder();
            
            stringBuilder.Append("net");
            stringBuilder.Append(frameworkVersion.Major);
            stringBuilder.Append(frameworkVersion.Minor);
                                                    
            if (frameworkVersion.Build != 0)
            {
                stringBuilder.Append(frameworkVersion.Build);
            }

            return stringBuilder.ToString();
        }

        // ReSharper disable once InconsistentNaming
        protected static string GetMonoTFM()
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
        public static Version GetDotNetVersion()
        {
            string frameworkDescription = RuntimeInformation.FrameworkDescription.ToLower();

            string versionString = frameworkDescription
                .Replace(".net", string.Empty)
                .Replace("core", string.Empty)
                .Replace("mono", string.Empty)
                .Replace("framework", string.Empty)
                .Replace(" ", string.Empty)
                .AddMissingZeroes(numberOfZeroesNeeded: 3);

            return Version.Parse(versionString);
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