/*
        MIT License
       
       Copyright (c) 2020-2024 Alastair Lundy
       
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

// ReSharper disable All

using System;
using PlatformKit.Internal.Deprecation;

namespace PlatformKit.OperatingSystems.Mac
{
    /// <summary>
    /// An enum representing macOS versions.
    /// </summary>
    public enum MacOsVersion
    {
        v10_15_Catalina,
        /// <summary>
        /// First version of macOS to move away from Major version 10 in [Major].[Minor].[Update]
        /// First version to support Apple Silicon Macs.
        /// </summary>
        v11_BigSur,
        v12_Monterey,
        v13_Ventura,
        v14_Sonoma,
        v15_Sequoia,
        NotDetected,
        NotSupported,
    }
}