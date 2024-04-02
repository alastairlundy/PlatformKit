﻿/*
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

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Detects and Returns the Installed version of FreeBSD
    /// </summary>
    /// <returns></returns>
    public Version GetFreeBSDVersion()
    {
        if (PlatformAnalyzer.IsFreeBSD())
        {
            return Version.Parse(CommandRunner.RunCommandOnFreeBsd("uname -v").Replace("FreeBSD", String.Empty)
                .Split(' ')[0].Replace("-release",  string.Empty).AddMissingZeroes());
        }

        throw new PlatformNotSupportedException();
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
            return GetFreeBSDVersion().IsAtLeast((expectedVersion));
        }

        throw new PlatformNotSupportedException();
    }
}