/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Collections.Generic;
using System.IO;

using PlatformKit.Linux;

namespace PlatformKit.Software;

public class InstalledApps
{
    /*public static AppModel[] Get()
    {
        ProcessManager processManager = new ProcessManager();
        if (OSAnalyzer.IsWindows())
        {
            
            foreach (File file in Envir)
            {
                
            }
        }
        else if (OSAnalyzer.IsMac())
        {
            
        }
        else if (OSAnalyzer.IsLinux())
        {
            List<AppModel> apps = new List<AppModel>();

            LinuxAnalyzer linuxAnalyzer = new LinuxAnalyzer();
            
            bool useSnap = Directory.Exists("/snap/bin");
            bool useFlatpak;

            try
            {
                string[] flatpakTest = processManager.RunLinuxCommand("flatpak --version").Split(" ");
                
                if (flatpakTest[0].Contains("Flatpak"))
                {
                    Version.Parse(flatpakTest[1]);

                    useFlatpak = true;
                }
                else
                {
                    useFlatpak = false;
                }
            }
            catch
            {
                useFlatpak = false;
            }
            
            if (useSnap)
            {
                foreach (var flatpak in linuxAnalyzer.GetInstalledFlatpaks())
                {
                    apps.Add(flatpak);
                }
            }

            if (useFlatpak)
            {
                foreach (var snap in linuxAnalyzer.GetInstalledSnaps())
                {
                    apps.Add(snap);
                }
            }
            
            foreach (var app in linuxAnalyzer.GetInstalledApps())
            {
                apps.Add(app);
            }

            return apps.ToArray();
        }
        else if (OSAnalyzer.IsFreeBSD())
        {
            
        }
    }*/

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appModel"></param>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static void Open(AppModel appModel)
    {
        ProcessManager processManager = new ProcessManager();
        
        if (PlatformAnalyzer.IsWindows())
        {
            processManager.RunProcessWindows(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (PlatformAnalyzer.IsMac())
        {
            processManager.RunProcessMac(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (PlatformAnalyzer.IsLinux())
        {
            processManager.RunProcessLinux(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (PlatformAnalyzer.IsFreeBSD())
        {
            processManager.RunProcessFreeBsd(appModel.InstallLocation, appModel.ExecutableName);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}