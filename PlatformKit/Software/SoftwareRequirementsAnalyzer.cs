/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using PlatformKit.FreeBSD;
using PlatformKit.Linux;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Software;

/// <summary>
/// 
/// </summary>
public class SoftwareRequirementsAnalyzer
{
    private readonly WindowsAnalyzer _windowsAnalyzer;
    private readonly MacOsAnalyzer _macOsAnalyzer;
    private readonly LinuxAnalyzer _linuxAnalyzer;
    private readonly FreeBsdAnalyzer _freeBsdAnalyzer;

    public SoftwareRequirementsAnalyzer()
    {
        _windowsAnalyzer = new WindowsAnalyzer();
        _macOsAnalyzer = new MacOsAnalyzer();
        _linuxAnalyzer = new LinuxAnalyzer();
        _freeBsdAnalyzer = new FreeBsdAnalyzer();
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