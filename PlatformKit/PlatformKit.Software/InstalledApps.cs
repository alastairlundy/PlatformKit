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

namespace PlatformKit.Software;

public class InstalledApps
{
    // ReSharper disable once IdentifierTypo
    protected static AppModel[] GetOnLinux(bool includeSnaps = false, bool includeFlatpaks = false)
    {
        List<AppModel> apps = new List<AppModel>();

        if (PlatformAnalyzer.IsLinux())
        {
#if NET5_0_OR_GREATER
            string[] binResult = CommandRunner.RunCommandOnLinux("ls -F /usr/bin | grep -v /").Split(Environment.NewLine);
#else
            string[] binResult = CommandRunner.RunCommandOnLinux("ls -F /usr/bin | grep -v /").Split(Convert.ToChar(Environment.NewLine));
#endif

            foreach (var app in binResult)
            {
                apps.Add(new AppModel(app, "/usr/bin"));
            }
            
            if (includeSnaps)
            {
                foreach (var snap in InstalledSnaps.Get())
                {
                    apps.Add(snap);
                }
            }
            if(includeFlatpaks){
                foreach (var flatpak in InstalledFlatpaks.Get())
                {
                    apps.Add(flatpak);
                }
            }

            return apps.ToArray();
        }

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    public static AppModel[] Get()
    {
        if (PlatformAnalyzer.IsWindows())
        {
            throw new NotImplementedException();
        }
        else if (PlatformAnalyzer.IsMac())
        {
            throw new NotImplementedException();
        }
        else if (PlatformAnalyzer.IsLinux() || PlatformAnalyzer.IsFreeBSD())
        {
            List<AppModel> apps = new List<AppModel>();
            
            bool useSnap = Directory.Exists("/snap/bin");
            bool useFlatpak;

            try
            {
                string[] flatpakTest = CommandRunner.RunCommandOnLinux("flatpak --version").Split(' ');
                
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
            
            if (useFlatpak)
            {
                foreach (var flatpak in InstalledFlatpaks.Get())
                {
                    apps.Add(flatpak);
                }
            }

            if (useSnap)
            {
                foreach (var snap in InstalledSnaps.Get())
                {
                    apps.Add(snap);
                }
            }
            
            foreach (var app in GetOnLinux())
            {
                apps.Add(app);
            }

            return apps.ToArray();
        }

        throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="appModel"></param>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static void Open(AppModel appModel)
    {
        if (PlatformAnalyzer.IsWindows())
        {
            ProcessRunner.RunProcessOnWindows(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (PlatformAnalyzer.IsMac())
        {
           ProcessRunner.RunProcessOnMac(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (PlatformAnalyzer.IsLinux())
        {
            ProcessRunner.RunProcessOnLinux(appModel.InstallLocation, appModel.ExecutableName);
        }
        else if (PlatformAnalyzer.IsFreeBSD())
        {
           ProcessRunner.RunProcessOnFreeBsd(appModel.InstallLocation, appModel.ExecutableName);
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}