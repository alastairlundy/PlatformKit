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
using AlastairLundy.Extensions.Runtime;
using PlatformKit.Core;

using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems.Mac.Extensions
{
    public static class MacOsExtensibilityExtensions
    {
        /// <summary>
        /// Gets a value from the Mac System Profiler information associated with a key
        /// </summary>
        /// <param name="macOperatingSystem"></param>
        /// <param name="macSystemProfilerDataType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetMacSystemProfilerInformation(this MacOperatingSystem macOperatingSystem,
            MacSystemProfilerDataType macSystemProfilerDataType, string key)
        {
            return GetMacSystemProfilerInformation(macSystemProfilerDataType, key);
        }
    
        /// <summary>
        /// Gets a value from the Mac System Profiler information associated with a key
        /// </summary>
        /// <param name="macSystemProfilerDataType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
        public static string GetMacSystemProfilerInformation(MacSystemProfilerDataType macSystemProfilerDataType, string key)
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }
        
            string info = CommandRunner.RunCommandOnMac("system_profiler SP" + macSystemProfilerDataType);

#if NETSTANDARD2_0_OR_GREATER
        string[] array = info.Split(Convert.ToChar(Environment.NewLine));
#elif NET5_0_OR_GREATER
            string[] array = info.Split(Environment.NewLine);
#endif
            foreach (string str in array)
            {
                if (str.ToLower().Contains(key.ToLower()))
                {
                    return str.Replace(key, string.Empty).Replace(":", string.Empty);
                }
            }

            throw new ArgumentException();
        }
    }
}