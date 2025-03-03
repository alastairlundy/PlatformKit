﻿/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

 using System;
 using System.Runtime.InteropServices;
// ReSharper disable CheckNamespace

 namespace PlatformKit.Platforms.Specifics;

public class DarwinPlatform : Platform, IEquatable<DarwinPlatform>
{
    /// <summary>
    /// 
    /// </summary>
    public Version DarwinVersion { get; }
    
    public DarwinPlatform(string name, Version darwinVersion, Version operatingSystemVersion, Version kernelVersion, string buildNumber, Architecture processorArchitecture) : base(name, operatingSystemVersion, kernelVersion, PlatformFamily.Darwin, buildNumber, processorArchitecture)
    {
        DarwinVersion = darwinVersion;
    }

    public bool Equals(DarwinPlatform other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;
        return base.Equals(other) && DarwinVersion.Equals(other.DarwinVersion);
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != typeof(DarwinPlatform)) return false;
        return Equals((DarwinPlatform)obj);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(base.GetHashCode(), DarwinVersion);
    }

    public static bool operator ==(DarwinPlatform left, DarwinPlatform right)
    {
        return Equals(left, right);
    }

    public static bool operator !=(DarwinPlatform left, DarwinPlatform right)
    {
        return !Equals(left, right);
    }
}