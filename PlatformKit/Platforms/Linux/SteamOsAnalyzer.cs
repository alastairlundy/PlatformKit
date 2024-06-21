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
    /// Detects if a Linux distro is Steam OS.
    /// </summary>
    /// <returns>true if running on a SteamOS 3.x based distribution; returns false otherwise.</returns>
    /// <exception cref="PlatformNotSupportedException">Thrown if not run on a Linux based Operating System.</exception>
    // ReSharper disable once InconsistentNaming
    public static bool IsSteamOS(bool includeHoloIsoAsSteamOs)
    {
        if (!OperatingSystem.IsLinux())
        {
            throw new PlatformNotSupportedException();
        }
        
        LinuxOsRelease distroInfo = LinuxOsReleaseRetriever.GetLinuxOsRelease();
        LinuxDistroBase distroBase = GetDistroBase(distroInfo);

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
    /// <param name="includeHoloIsoAsSteamOs">Whether to consider Holo ISO as Steam OS.</param>
    /// <returns>the SteamOS mode being run if run on SteamOS.</returns>
    /// <exception cref="ArgumentException">Thrown if Holo ISO is detected and if Holo ISO isn't counted as SteamOS.</exception>
    /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't SteamOS 3</exception>
    public static SteamOSMode GetSteamOsMode(bool includeHoloIsoAsSteamOs)
    {
        if (!IsSteamOS(includeHoloIsoAsSteamOs))
        {
            throw new PlatformNotSupportedException();    
        }
        
        LinuxDistroBase distroBase = GetDistroBase();

        bool isSteamOsExcludingHolo = IsSteamOS(false);

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