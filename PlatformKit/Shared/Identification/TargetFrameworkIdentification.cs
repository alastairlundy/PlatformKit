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
using System.Text;

using AlastairLundy.Extensions.System.Versioning;

using PlatformKit.Internal.Exceptions;

using PlatformKit.Mac;
using PlatformKit.Windows;
    
#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Identification;

/// <summary>
/// A class to manage RuntimeId detection and programmatic generations
/// </summary>
public class TargetFrameworkIdentification
{
    // ReSharper disable once InconsistentNaming
        protected static string GetNetCoreTFM(Version frameworkVersion)
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.Append("netcoreapp");

            stringBuilder.Append(frameworkVersion.Major);
            stringBuilder.Append(".");
            stringBuilder.Append(frameworkVersion.Minor);

            return stringBuilder.ToString();
        }

        // ReSharper disable once InconsistentNaming
        protected static string GetNetFrameworkTFM(Version frameworkVersion)
        {
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

        public static Version GetDotNetVersion()
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
        /// <exception cref="OperatingSystemDetectionException">Thrown if there's an issue detecting the Windows Version whilst running on Windows.</exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        public static string GetTargetFrameworkMoniker(TargetFrameworkMonikerType targetFrameworkType)
        {
            Version frameworkVersion = GetDotNetVersion();
            
            if (RuntimeInformation.FrameworkDescription.ToLower().Contains("core"))
            {
                return GetNetCoreTFM(frameworkVersion);
            }
            else
            {
                StringBuilder stringBuilder = new StringBuilder();
                
                if (RuntimeInformation.FrameworkDescription.ToLower().Contains(".net"))
                {
                    stringBuilder.Append("net");
                    
                    if (frameworkVersion.Major < 5)
                    {
                        return GetNetFrameworkTFM(frameworkVersion);
                    }

                    stringBuilder.Append(frameworkVersion.Major);
                    stringBuilder.Append(".");
                    stringBuilder.Append(frameworkVersion.Minor);

                    if (targetFrameworkType == TargetFrameworkMonikerType.Generic)
                    {
                        return stringBuilder.ToString();
                    }

                    if(targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemSpecific || targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific)
                    {
                        if(OperatingSystem.IsMacOS())
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
                        else if (OperatingSystem.IsWindows())
                        {
                            stringBuilder.Append("-");
                            stringBuilder.Append("windows");
                        }
                        else if (OperatingSystem.IsLinux() || OperatingSystem.IsFreeBSD())
                        {
                            //Do nothing because Linux and FreeBSD don't have dedicated TFMs yet
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

                            if (frameworkVersion.Major >= 8)
                            {
                                if (OperatingSystem.IsBrowser())
                                {
                                    stringBuilder.Append("-");
                                    stringBuilder.Append("browser");
                                }
                            }
#endif
                            
                        if (targetFrameworkType == TargetFrameworkMonikerType.OperatingSystemVersionSpecific && frameworkVersion.Major >= 5)
                        {
                            if(OperatingSystem.IsMacOS())
                            {
                                Version macOsVersion = MacOsAnalyzer.GetMacOsVersion();

                                stringBuilder.Append(macOsVersion.Major);
                                stringBuilder.Append(".");
                                stringBuilder.Append(macOsVersion.Minor);
                            }
                            else if(OperatingSystem.IsLinux())
                            {
                                //Do nothing because Linux doesn't have a version specific TFM.
                            }
                            else if(OperatingSystem.IsWindows())
                            {
                                Version winVersion = WindowsAnalyzer.GetWindowsVersion();

                                if (winVersion.Build == 9200)
                                {
                                    stringBuilder.Append("8.0");
                                }
                                else if (winVersion.Build == 9600)
                                {
                                    stringBuilder.Append("8.1");
                                }
                                else if (winVersion.Build >= 14393)
                                {
                                    WindowsVersion winVersionEnum = WindowsAnalyzer.GetWindowsVersionToEnum(winVersion);

                                    if (WindowsAnalyzer.IsWindows10(winVersionEnum))
                                    {
                                        stringBuilder.Append("10");
                                    }
                                    else if (WindowsAnalyzer.IsWindows11(winVersionEnum))
                                    {
                                        stringBuilder.Append("11");
                                    }
                                    else
                                    {
                                        throw new PlatformNotSupportedException();
                                    }
                                }
                                else
                                {
                                    throw new PlatformNotSupportedException();
                                }

                                return stringBuilder.ToString();
                            }
                        }
                            
                        return stringBuilder.ToString();
                    }
                }
                else if (RuntimeInformation.FrameworkDescription.ToLower().Contains("mono"))
                {
                    return GetMonoTFM();
                }
            }

            throw new PlatformNotSupportedException();
        }
}