/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

#nullable enable
using System;

namespace PlatformKit.Software;

public class AppModel
{
    public AppModel(string executableName, string installLocation)
    {
        this.ExecutableName = executableName;
        this.InstallLocation = installLocation;
    }

public string? Author { get; set; }
    
    public string? FriendlyName { get; set; }
    public string ExecutableName { get; set; }
    public string InstallLocation { get; set; }
    public Version? InstalledVersion { get; set; }
}