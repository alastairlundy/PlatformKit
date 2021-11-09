/* MIT License

Copyright (c) 2018-2021 AluminiumTech

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

// ReSharper disable InconsistentNaming
namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows
{
    public enum WindowsVersion
    {
        WinVista,
        // ReSharper disable once InconsistentNaming
        WinVistaSP1,
        // ReSharper disable once InconsistentNaming
        WinVistaSP2,
        Win7,
        // ReSharper disable once InconsistentNaming
        Win7SP1,
        Win8,
        Win8_1,
        Win10_v1507,
        Win10_v1511,
        Win10_v1607,
        Win10_v1703,
        Win10_v1709,
        Win10_v1709_Mobile,
        Win10_v1803,
        Win10_v1809,
        Win10_v1903,
        Win10_v1909,
        Win10_v2004,
        Win10_20H2,
        Win10_21H1,
        
        Win11_21H2,
        NotDetected,
        Unsupported,
    }
}