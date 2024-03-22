/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using PlatformKit.Extensions;

namespace PlatformKit.FreeBSD;

/// <summary>
/// A class to detect FreeBSD versions and features.
/// </summary>
public class FreeBsdAnalyzer
{
    private readonly ProcessManager _processManager;

    public FreeBsdAnalyzer()
    {
        _processManager = new ProcessManager();
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Detects and Returns the Installed version of FreeBSD
    /// </summary>
    /// <returns></returns>
    public Version DetectFreeBSDVersion()
    { 
        if (PlatformAnalyzer.IsWindows())
        {
            var version = _processManager.RunProcessLinux("", "uname", "-v").Replace("FreeBSD", String.Empty);

            var arr = version.Split(' ');

            var rel = arr[0].Replace("-release", String.Empty);

            int dotCounter = rel.CountDotsInString();

            if (dotCounter == 1)
            {
                rel += ".0.0";
            }
            else if (dotCounter == 2)
            {
                rel += ".0.0";
            }

            return Version.Parse(rel);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
    
    /// <summary>
    /// Checks to see whether the specified version of FreeBSD is the same or newer than the installed version of FreeBSD.
    /// </summary>
    /// <param name="expectedVersion"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public bool IsAtLeastVersion(Version expectedVersion)
    {
        if (PlatformAnalyzer.IsFreeBSD())
        {
            return DetectFreeBSDVersion().IsAtLeast((expectedVersion));
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}