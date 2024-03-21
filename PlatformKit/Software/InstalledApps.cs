/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;

namespace PlatformKit.Software;

public class InstalledApps
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="appModel"></param>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static void Open(AppModel appModel)
    {
        ProcessManager processManager = new ProcessManager();
        
        if (OSAnalyzer.IsWindows())
        {
            processManager.RunProcessWindows(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (OSAnalyzer.IsMac())
        {
            processManager.RunProcessMac(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (OSAnalyzer.IsLinux())
        {
            processManager.RunProcessLinux(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (OSAnalyzer.IsFreeBSD())
        {
            processManager.RunProcessFreeBsd(appModel.InstallLocation, appModel.ExecutableName);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}