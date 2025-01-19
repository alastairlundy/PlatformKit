/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Runtime.InteropServices;
using PlatformKit.Specializations.Windows;

// ReSharper disable ConvertToPrimaryConstructor

namespace PlatformKit.Specifics;

public class WindowsPlatform : Platform, IEquatable<WindowsPlatform>
{
    /// <summary>
    /// 
    /// </summary>
    public WindowsEdition Edition { get; }
    
    public WindowsPlatform(string name, Version operatingSystemVersion, Version kernelVersion, string buildNumber,
        WindowsEdition edition, Architecture processorArchitecture) : base(name, operatingSystemVersion, kernelVersion,
        PlatformFamily.WindowsNT, buildNumber, processorArchitecture)
    {
        Edition = edition;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    /// <returns></returns>
    public bool Equals(WindowsPlatform other)
    {
        if (other == null)
        {
            return false;
        }
        
        return Name == other.Name && Edition == other.Edition
            && OperatingSystemVersion == other.OperatingSystemVersion
            && KernelVersion == other.KernelVersion
            && BuildNumber == other.BuildNumber
            && Family == other.Family;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="obj"></param>
    /// <returns></returns>
    public override bool Equals(object obj)
    {
        if (obj is null)
        {
            return false;
        }

        if (obj is WindowsPlatform platform)
        {
            return Equals(platform);
        }
        // ReSharper disable once RedundantIfElseBlock
        else
        {
            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode()
    {
       return HashCode.Combine(Name, Edition, OperatingSystemVersion, KernelVersion, BuildNumber, Family,
           ProcessorArchitecture);
    }
}