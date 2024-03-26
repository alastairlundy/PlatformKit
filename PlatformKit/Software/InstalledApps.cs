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
    /// <summary>
    /// Detect what Snap packages (if any) are installed on a linux distribution or on macOS.
    /// </summary>
    /// <returns>Returns a list of installed snaps. Returns an empty array if no Snaps are installed. </returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a Platform other than Linux, macOS, and FreeBsd. </exception>
    internal static AppModel[] GetInstalledSnaps()
    {
        List<AppModel> apps = new List<AppModel>();

        if (PlatformAnalyzer.IsLinux() || PlatformAnalyzer.IsMac() || PlatformAnalyzer.IsFreeBSD())
        {
            bool useSnap = Directory.Exists("/snap/bin");

            if (useSnap)
            {
                string[] snapResults = CommandRunner.RunCommandOnLinux("ls /snap/bin").Split(' ');

                foreach (var snap in snapResults)
                {
                    apps.Add(new AppModel(snap, "/snap/bin"));
                }

                return apps.ToArray();
            }

            apps.Clear();
            return apps.ToArray();
        }

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    ///
    /// Platforms Supported On: Linux and FreeBsd.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    // ReSharper disable once IdentifierTypo
    internal static AppModel[] GetInstalledFlatpaks()
    {
        List<AppModel> apps = new List<AppModel>();

        if (PlatformAnalyzer.IsLinux() || PlatformAnalyzer.IsFreeBSD())
        {
            bool useFlatpaks;
            
            string[] flatpakTest = CommandRunner.RunCommandOnLinux("flatpak --version").Split(' ');
                
            if (flatpakTest[0].Contains("Flatpak"))
            {
                Version.Parse(flatpakTest[1]);

                useFlatpaks = true;
            }
            else
            {
                useFlatpaks = false;
            }

            if (useFlatpaks)
            {
#if NET5_0_OR_GREATER
                string[] flatpakResults = CommandRunner.RunCommandOnLinux("flatpak list --columns=name")
                    .Split(Environment.NewLine);
#else
                string[] flatpakResults = CommandRunner.RunCommandOnLinux("flatpak list --columns=name")
                    .Split(Convert.ToChar(Environment.NewLine));
#endif

                var installLocation = CommandRunner.RunCommandOnLinux("flatpak --installations");
                
                foreach (var flatpak in flatpakResults)
                {
                    apps.Add(new AppModel(flatpak, installLocation));
                }
                
                return apps.ToArray();
            }

            apps.Clear();
            return apps.ToArray();
        }

        throw new PlatformNotSupportedException();
    }
    
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
                foreach (var snap in GetInstalledSnaps())
                {
                    apps.Add(snap);
                }
            }
            if(includeFlatpaks){
                foreach (var flatpak in GetInstalledFlatpaks())
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
            
            if (useSnap)
            {
                foreach (var flatpak in GetInstalledFlatpaks())
                {
                    apps.Add(flatpak);
                }
            }

            if (useFlatpak)
            {
                foreach (var snap in GetInstalledSnaps())
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