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
using System.Threading.Tasks;

using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

namespace PlatformKit.OperatingSystems.Linux.Extensions
{
    public static class LinuxSteamOsExtensions
    {
        /// <summary>
        /// Detects if a Linux distro is Steam OS.
        /// </summary>
        /// <returns>true if running on a SteamOS 3.x based distribution; returns false otherwise.</returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
        // ReSharper disable once InconsistentNaming
        public static async Task<bool> IsSteamOSAsync(this LinuxOperatingSystem linuxOperatingSystem, bool includeHoloIsoAsSteamOs)
        {
            if (OperatingSystem.IsLinux() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }

            LinuxOsReleaseModel distroInfo = await linuxOperatingSystem.GetLinuxOsReleaseAsync();
            LinuxDistroBase distroBase = linuxOperatingSystem.GetDistroBase(distroInfo);

            if (distroBase == LinuxDistroBase.Manjaro || distroBase == LinuxDistroBase.Arch)
            {
                return (includeHoloIsoAsSteamOs && distroInfo.PrettyName.ToLower().Contains("holo") ||
                        distroInfo.PrettyName.ToLower().Contains("steamos"));
            }
            if (distroBase == LinuxDistroBase.Debian && distroInfo.PrettyName.ToLower().Contains("steamos"))
            {
                return false;
            }

            return false;
        }

        /// <summary>
        /// Detects whether a device running SteamOS 3.x is running in Desktop Mode or in Gaming Mode.
        /// </summary>
        /// <param name="linuxOperatingSystem"></param>
        /// <param name="includeHoloIsoAsSteamOs">Whether to consider Holo ISO as Steam OS.</param>
        /// <returns>the SteamOS mode being run if run on SteamOS.</returns>
        /// <exception cref="ArgumentException">Thrown if Holo ISO is detected and if Holo ISO isn't counted as SteamOS.</exception>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't SteamOS 3</exception>
        public static async Task<SteamOSMode> GetSteamOsModeAsync(this LinuxOperatingSystem linuxOperatingSystem, bool includeHoloIsoAsSteamOs)
        {
            bool isSteamOs = await IsSteamOSAsync(linuxOperatingSystem, includeHoloIsoAsSteamOs);
        
            if (isSteamOs == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);    
            }
        
            LinuxDistroBase distroBase = await linuxOperatingSystem.GetDistroBaseAsync();

            bool isSteamOsExcludingHolo = await IsSteamOSAsync(linuxOperatingSystem, false);

            if (distroBase == LinuxDistroBase.Manjaro)
            {
                if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
                {
                    return SteamOSMode.DesktopMode;
                }

                return SteamOSMode.OsIsNotSteamOS;
            }

            if (distroBase == LinuxDistroBase.Arch)
            {
                if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
                {
                    return SteamOSMode.GamingMode;
                }

                return SteamOSMode.OsIsNotSteamOS;
            }

            return SteamOSMode.OsIsNotSteamOS;
        }
    }
}