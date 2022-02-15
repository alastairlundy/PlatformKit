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
using System.IO;

using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Deprecation;
using AluminiumTech.DevKit.PlatformKit.Software.Windows;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows;

[Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
public static class WindowsProcessManagerExtensions
{
    [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
    public static string GetWindowsRegistryValue(string query, string value, string failMessage)
    {
        return new WindowsSoftwareDetection().GetWindowsRegistryValue(query, value, failMessage);
    }

    [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
    // ReSharper disable once InconsistentNaming
    public static string GetWMIValue(string query, string wmiClass, string failMessage)
    {
        return new WindowsSoftwareDetection().GetWindowsManagementInstrumentationValue(query, wmiClass, failMessage);
    }
    
}