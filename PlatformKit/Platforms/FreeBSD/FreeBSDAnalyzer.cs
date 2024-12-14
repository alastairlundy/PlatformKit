/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using AlastairLundy.Extensions.Strings.Versioning;

using PlatformKit.Internal.Deprecation;

#if NETSTANDARD2_0
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

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
            string version = _processManager.RunProcessLinux("", "uname", "-v").Replace("FreeBSD", string.Empty);

            string rel = version.Split(' ')[0].Replace("-release", string.Empty);

            return Version.Parse(rel.AddMissingZeroes(2));
        }

        throw new PlatformNotSupportedException();
    }
}