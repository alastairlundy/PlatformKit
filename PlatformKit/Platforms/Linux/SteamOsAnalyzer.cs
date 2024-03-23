/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using PlatformKit.Linux.Enums;

namespace PlatformKit.Linux;

public class SteamOsAnalyzer : LinuxAnalyzer
{
    
    public SteamOsAnalyzer()
    {
        
    }
    
    /// <summary>
    /// Returns true for SteamOS3 based distributions. 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    // ReSharper disable once InconsistentNaming
    public bool IsSteamOS(bool includeHoloIsoAsSteamOs)
    {
        if (PlatformAnalyzer.IsLinux())
        {
            var distroInfo = GetLinuxDistributionInformation();
            var distroBase = GetDistroBase();

            if ((distroBase == LinuxDistroBase.Manjaro || distroBase == LinuxDistroBase.Arch) 
                && distroInfo.PrettyName.ToLower().Contains("steamos") && !distroInfo.PrettyName.ToLower().Contains("holo"))
            {
                return true;
            }
            else if ((distroBase == LinuxDistroBase.Manjaro || distroBase == LinuxDistroBase.Arch) 
                     && distroInfo.PrettyName.ToLower().Contains("holo"))
            {
                return includeHoloIsoAsSteamOs;
            }
            
            if (distroBase == LinuxDistroBase.Ubuntu && distroInfo.PrettyName.ToLower().Contains("steamos"))
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
        if (IsSteamOS(false))
        {
            var distroInfo = GetLinuxDistributionInformation();
            var distroBase = GetDistroBase();
            
            if (distroBase == LinuxDistroBase.Manjaro
                && distroInfo.PrettyName.ToLower().Contains("steamos") && !distroInfo.PrettyName.ToLower().Contains("holo"))
            {
                return SteamOSMode.DesktopMode;
            }
            else if ((distroBase == LinuxDistroBase.Manjaro) 
                     && distroInfo.PrettyName.ToLower().Contains("holo"))
            {
                if (includeHoloIsoAsSteamOs)
                {
                    return SteamOSMode.DesktopMode;
                }
                else
                {
                    return SteamOSMode.OsIsNotSteamOS;
                }
            }
            
            else if (distroBase == LinuxDistroBase.Arch &&
                     distroInfo.PrettyName.ToLower().Contains("steamos") &&
                     !distroInfo.PrettyName.ToLower().Contains("holo"))
            {
                return SteamOSMode.GamingMode;
            }
            else if ((distroBase == LinuxDistroBase.Arch) 
                     && distroInfo.PrettyName.ToLower().Contains("holo"))
            {
                if (includeHoloIsoAsSteamOs)
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