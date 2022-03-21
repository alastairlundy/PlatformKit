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
using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics.VersionAnalyzers;

namespace AluminiumTech.DevKit.PlatformKit.Software.Windows;

[Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
public class WindowsSoftwareDetection : IWindowsSoftwareDetection
{
    protected OSAnalyzer _osAnalyzer;
    protected ProcessManager _processManager;

    protected WindowsAnalyzer _windowsAnalyzer;
    
    public WindowsSoftwareDetection()
    {
        _osAnalyzer = new OSAnalyzer();
        _processManager = new ProcessManager();
    }

    
    public string GetWindowsRegistryValue(string query, string value, string failMessage)
    {
        try
        {
            return _windowsAnalyzer.GetWindowsRegistryValue(query, value);
        }
        catch
        {
            return failMessage;
        }
    }

    public string GetWMIValue(string query, string wmiClass, string failMessage)
    {
        try
        {
            return _windowsAnalyzer.GetWMIValue(query, wmiClass);
        }
        catch
        {
            return failMessage;
        }
    }
    
    public string GetWindowsManagementInstrumentationValue(string query, string wmiClass, string failMessage)
    {
        return GetWMIValue(query, wmiClass, failMessage);
    }
}