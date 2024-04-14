/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using PlatformKit.Internal.Deprecation;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Software;

/// <summary>
/// 
/// </summary>
[Obsolete(DeprecationMessages.DeprecationV4)]
public class SoftwareRequirementsAnalyzer
{
    private readonly MacOsAnalyzer _macOsAnalyzer;
    private readonly LinuxAnalyzer _linuxAnalyzer;
    private readonly WindowsAnalyzer _windowsAnalyzer;

    public SoftwareRequirementsAnalyzer()
    {
        _macOsAnalyzer = new MacOsAnalyzer();
        _linuxAnalyzer = new LinuxAnalyzer();
        _windowsAnalyzer = new WindowsAnalyzer();
    }

    [Obsolete(DeprecationMessages.DeprecationV4)]
    public bool HasRequiredLinuxKernelVersion(Version requiredLinuxKernel)
    {
        return _linuxAnalyzer.IsAtLeastKernelVersion(requiredLinuxKernel);
    }

    [Obsolete(DeprecationMessages.DeprecationV4)]
    public bool HasRequiredMacOsVersion(MacOsVersion requiredMacOsVersionVersion)
    {
        return _macOsAnalyzer.IsAtLeastMacOSVersion(requiredMacOsVersionVersion);
    }

    [Obsolete(DeprecationMessages.DeprecationV4)]
    public bool HasRequiredWindowsVersion(WindowsVersion requiredWindowsVersion)
    {
        return _windowsAnalyzer.IsAtLeastWindowsVersion(requiredWindowsVersion);
    }
}