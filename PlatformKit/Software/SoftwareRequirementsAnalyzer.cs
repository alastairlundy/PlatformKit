/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;

using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Software;

/// <summary>
/// 
/// </summary>
public class SoftwareRequirementsAnalyzer
{
    protected MacOSAnalyzer _macOsAnalyzer;
    protected LinuxAnalyzer _linuxAnalyzer;
    protected WindowsAnalyzer _windowsAnalyzer;

    public SoftwareRequirementsAnalyzer()
    {
        _macOsAnalyzer = new MacOSAnalyzer();
        _linuxAnalyzer = new LinuxAnalyzer();
        _windowsAnalyzer = new WindowsAnalyzer();
    }

    public bool HasRequiredLinuxKernelVersion(Version requiredLinuxKernel)
    {
        return _linuxAnalyzer.IsAtLeastKernelVersion(requiredLinuxKernel);
    }

    public bool HasRequiredMacOsVersion(MacOsVersion requiredMacOsVersionVersion)
    {
        return _macOsAnalyzer.IsAtLeastMacOSVersion(requiredMacOsVersionVersion);
    }

    public bool HasRequiredWindowsVersion(WindowsVersion requiredWindowsVersion)
    {
        return _windowsAnalyzer.IsAtLeastWindowsVersion(requiredWindowsVersion);
    }
}