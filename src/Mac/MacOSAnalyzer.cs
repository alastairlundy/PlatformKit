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
using System.Runtime.InteropServices;
using PlatformKit.Mac.Enums;

//Move namespace in V3
namespace PlatformKit.Mac
{
    
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// macOS specific extensions to the OSAnalyzer class.
    /// </summary>
    public static class MacOSAnalyzer
    {
        
        /*
         *     public string GetMacSystemProfilerValue(MacSystemProfilerDataType dataType, string value);

    public MacOsSystemInformation GetMacSwVersionInformation();
         * 
         */
        
        /// <summary>
        /// Returns whether or not a Mac is Apple Silicon based.
        /// </summary>
        /// <returns></returns>
        public static bool IsAppleSiliconMac(this OSAnalyzer osAnalyzer)
        {
            return GetMacProcessorType(osAnalyzer) == MacProcessorType.AppleSilicon;
        }
        
        /// <summary>
        /// Gets the type of Processor in a given Mac returned as the MacProcessorType
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public static MacProcessorType GetMacProcessorType(this OSAnalyzer osAnalyzer)
        {
            try
            {
                if (osAnalyzer.IsMac())
                {
                    return System.Runtime.InteropServices.RuntimeInformation.OSArchitecture switch
                    {
                        Architecture.Arm64 => MacProcessorType.AppleSilicon,
                        Architecture.X64 => MacProcessorType.Intel,
                        _ => MacProcessorType.NotDetected
                    };
                }

                throw new PlatformNotSupportedException();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception(exception.ToString());
            }
        } 
    }
}
