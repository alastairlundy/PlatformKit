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

namespace AluminiumTech.DevKit.PlatformKit.Hardware.Windows.Enums
{
    /// <summary>
    /// 
    /// </summary>
    public enum WindowsDeviceFamily
    {
        /// <summary>
        /// Windows 10 [Home,Pro,Education,Enterprise,LTSC,Server]
        /// </summary>
        Desktop,
        /// <summary>
        /// Windows 10 Xbox OS.
        /// </summary>
        Xbox,
        HoloLens,
        /// <summary>
        /// Windows 10 Teams OS.
        /// </summary>
        SurfaceHub,
        [Obsolete("Windows 10 Mobile is no longer supported by Microsoft and has not been updated past Windows 10 Version 1709 for Mobile.")]
        Mobile
    }
}