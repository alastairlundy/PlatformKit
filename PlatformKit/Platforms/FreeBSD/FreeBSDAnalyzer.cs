﻿/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using PlatformKit.Internal.Deprecation;
using PlatformKit.Internal.Exceptions;

namespace PlatformKit.FreeBSD;

// ReSharper disable once InconsistentNaming
[Obsolete(DeprecationMessages.DeprecationV4)]
public class FreeBSDAnalyzer : FreeBsdAnalyzer
{
    
}

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
        if (OperatingSystem.IsFreeBSD())
        {
            var version = _processManager.RunProcessLinux("", "uname", "-v");

            version = version.Replace("FreeBSD", String.Empty);

            var arr = version.Split(' ');

            var rel = arr[0].Replace("-release", String.Empty);

            int dotCounter = 0;

            foreach (char c in rel)
            {
                if (c == '.')
                {
                    dotCounter++;
                }
            }

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
}