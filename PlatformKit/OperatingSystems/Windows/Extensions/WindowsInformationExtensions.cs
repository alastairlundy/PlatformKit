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
using System.Runtime.Versioning;
using System.Threading.Tasks;

using PlatformKit.Internal.Exceptions.Windows;
using PlatformKit.Internal.Localizations;
using PlatformKit.OperatingSystems.Windows.Extensions.Extensibility;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems.Windows.Extensions
{
    public static class WindowsInformationExtensions
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="WindowsEditionDetectionException">Throws an exception if operating system detection fails.</exception>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't Windows.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public static async Task<WindowsEdition> GetWindowsEditionAsync(this WindowsOperatingSystem windowsOperatingSystem)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }

            WindowsSystemInformationModel systemInformationModel = await windowsOperatingSystem.GetWindowsSystemInformationAsync();
            string edition = systemInformationModel.OsName.ToLower();
                
            if (edition.Contains("home"))
            {
                return WindowsEdition.Home;
            }
            else if (edition.Contains("pro") && edition.Contains("workstation"))
            {
                return WindowsEdition.ProfessionalForWorkstations;
            }
            else if (edition.Contains("pro") && !edition.Contains("education"))
            {
                return WindowsEdition.Professional;
            }
            else if (edition.Contains("pro") && edition.Contains("education"))
            {
                return WindowsEdition.ProfessionalForEducation;
            }
            else if (!edition.Contains("pro") && edition.Contains("education"))
            {
                return WindowsEdition.Education;
            }
            else if (edition.Contains("server"))
            {
                return WindowsEdition.Server;
            }
            else if (edition.Contains("enterprise") && edition.Contains("ltsc") &&
                     !edition.Contains("iot"))
            {
                return WindowsEdition.EnterpriseLTSC;
            }
            else if (edition.Contains("enterprise") && !edition.Contains("ltsc") &&
                     !edition.Contains("iot"))
            {
                return WindowsEdition.EnterpriseSemiAnnualChannel;
            }
            else if (edition.Contains("enterprise") && edition.Contains("ltsc") &&
                     edition.Contains("iot"))
            {
                return WindowsEdition.IoTEnterpriseLTSC;
            }
            else if (edition.Contains("enterprise") && !edition.Contains("ltsc") &&
                     edition.Contains("iot"))
            {
                return WindowsEdition.IoTEnterprise;
            }
            else if (edition.Contains("iot") && edition.Contains("core"))
            {
                return WindowsEdition.IoTCore;
            }
            else if (edition.Contains("team"))
            {
                return WindowsEdition.Team;
            }

            if (await windowsOperatingSystem.IsWindows11Async())
            {
                if (edition.Contains("se"))
                {
                    return WindowsEdition.SE;
                }
            }

            throw new WindowsEditionDetectionException();
        }
    }
}