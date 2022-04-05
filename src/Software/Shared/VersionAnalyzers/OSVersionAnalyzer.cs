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

using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics.VersionAnalyzers;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers
{
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// 
    /// </summary>
    public class OSVersionAnalyzer
    {
        protected OSAnalyzer _osAnalyzer;
        
        public OSVersionAnalyzer()
        {
            _osAnalyzer = new OSAnalyzer();
        }

        /// <summary>
        /// Detect the OS version on Windows, macOS, or Linux.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException">
        /// Not yet implemented on FreeBSD either! Please do not run on FreeBSD.
        /// </exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        // ReSharper disable once InconsistentNaming
        public Version DetectOSVersion()
        {
            try
            {
                if (_osAnalyzer.IsWindows())
                {
                    return this.DetectWindowsVersion();
                }
                if (_osAnalyzer.IsLinux())
                {   
                    return this.DetectLinuxDistributionVersion();
                }
                if (_osAnalyzer.IsMac())
                {
                    return this.DetectMacOsVersion();
                }

#if NETCOREAPP3_0_OR_GREATER
                if (_osAnalyzer.IsFreeBSD())
                {
                    throw new NotImplementedException();
                }
#endif
                throw new PlatformNotSupportedException();
            }
            catch (Exception exception)
            {
                throw new Exception(exception.ToString());
            }
        }
    }
}