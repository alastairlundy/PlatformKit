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

using System;
using System.Runtime.InteropServices;
using PlatformKit.Internal.Deprecation;

namespace PlatformKit;

    // ReSharper disable once InconsistentNaming
    [Obsolete(DeprecationMessages.DeprecationV5)]
    public static class PlatformAnalyzer
    {

        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static bool IsWindows()
        {
#if NET5_0_OR_GREATER
                  return OperatingSystem.IsWindows();
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#endif
        }
        
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static bool IsMac()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsMacOS();
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
        }
        
        [Obsolete(DeprecationMessages.DeprecationV5)]
        public static bool IsLinux()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsLinux();
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
#endif
        }
        
        [Obsolete(DeprecationMessages.DeprecationV5)]
        // ReSharper disable once InconsistentNaming
        public static bool IsFreeBSD()
        {
#if NET5_0_OR_GREATER
            return OperatingSystem.IsFreeBSD();
#else
        #if NETCOREAPP3_0_OR_GREATER
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(OSPlatform.FreeBSD);
        #else
            throw new PlatformNotSupportedException();
        #endif
#endif
        }
    }