/*
    PlatformKit
    Copyright (c) 2024 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

namespace PlatformKit.Specifics;

public class WindowsPlatform : Platform, IEquatable<WindowsPlatform>
{
    public WindowsPlatform(string name, Version operatingSystemVersion, Version kernelVersion, string buildNumber) : base(name, operatingSystemVersion, kernelVersion, PlatformFamily.WindowsNT, buildNumber)
    {
        
    }

    public bool Equals(WindowsPlatform other)
    {
        throw new NotImplementedException();
    }

    public override bool Equals(object obj)
    {
        if (obj is null) return false;
        if (ReferenceEquals(this, obj)) return true;
        if (obj.GetType() != GetType()) return false;
        return Equals((WindowsPlatform)obj);
    }

    public override int GetHashCode()
    {
        throw new NotImplementedException();
    }
}