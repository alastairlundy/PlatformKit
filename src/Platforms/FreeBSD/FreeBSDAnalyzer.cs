/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;

using PlatformKit.Internal.Licensing;


namespace PlatformKit.FreeBSD;

// ReSharper disable once InconsistentNaming
public class FreeBSDAnalyzer
{
    protected readonly ProcessManager _processManager;
    protected readonly OSAnalyzer _osAnalyzer;

    public FreeBSDAnalyzer()
    {
        _processManager = new ProcessManager();
        _osAnalyzer = new OSAnalyzer();
        
        PlatformKitConstants.CheckLicenseState();
    }

    // ReSharper disable once InconsistentNaming
    public Version DetectFreeBSDVersion()
    {
        var v = _processManager.RunProcess("", "uname", "-v");

        v = v.Replace("FreeBSD", String.Empty);

        var arr = v.Split(' ');

        var rel = arr[0].Replace("-release", String.Empty);

        int dotCounter = 0;
        
        foreach (char c in rel)
        {
            if (c == '.')
            {
                dotCounter++;
            }
        }

        if (dotCounter == 1)
        {
            rel += ".0.0";
        }
        else if (dotCounter == 2)
        {
            rel += ".0.0";
        }

        return Version.Parse(rel);
    }
}