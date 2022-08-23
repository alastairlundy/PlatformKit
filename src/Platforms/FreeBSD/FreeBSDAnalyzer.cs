/* MIT License

Copyright (c) 2018-2022 AluminiumTech

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
*/

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