
using System;

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