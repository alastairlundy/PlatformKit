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

using PlatformKit.Linux.Enums;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Linux;

public class SteamOsAnalyzer : LinuxAnalyzer
{
    
    /// <summary>
    /// Returns true for SteamOS3 based distributions. 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    // ReSharper disable once InconsistentNaming
    public bool IsSteamOS(bool includeHoloIsoAsSteamOs)
    {
        if (OperatingSystem.IsLinux())
        {
            var distroInfo = GetLinuxDistributionInformation();
            var distroBase = GetDistroBase(distroInfo);

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

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// Returns whether a device running SteamOS is running in Desktop Mode or in Gaming Mode.
    /// IMPORTANT: DO NOT RUN on non SteamOS 3 based operating systems.
    /// </summary>
    /// <param name="includeHoloIsoAsSteamOs"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Thrown if Holo ISO is detected and Holo ISO isn't counted as SteamOS.</exception>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on any OS that isn't SteamOS 3</exception>
    public SteamOSMode GetSteamOsMode(bool includeHoloIsoAsSteamOs)
    {
        if (IsSteamOS(includeHoloIsoAsSteamOs))
        {
            var distroBase = GetDistroBase();

            var isSteamOsExcludingHolo = IsSteamOS(false);

            if (distroBase == LinuxDistroBase.Manjaro)
            {
                if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
                {
                    return SteamOSMode.DesktopMode;
                }
                else
                {
                    return SteamOSMode.OsIsNotSteamOS;
                }
            }
            else if (distroBase == LinuxDistroBase.Arch)
            {
                if (includeHoloIsoAsSteamOs || isSteamOsExcludingHolo)
                {
                    return SteamOSMode.GamingMode;
                }
                else
                {
                    return SteamOSMode.OsIsNotSteamOS;
                }
            }

            return SteamOSMode.OsIsNotSteamOS;
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}