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
using System.IO;
using System.Runtime.Versioning;

using CliWrap;
using CliWrap.Buffered;

using PlatformKit.Internal.Localizations;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.Windows
{
    /// <summary>
    /// A class to make searching WMI easier.
    /// </summary>
// ReSharper disable once InconsistentNaming
    public class WMISearcher
    {
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Gets information from a WMI class in WMI.
        /// </summary>
        /// <param name="wmiClass"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an Operating System that isn't Windows.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public static string GetWMIClass(string wmiClass)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }

            var task = Cli.Wrap(GetWindowsPowershellLocation())
                .WithArguments($"Get-WmiObject -Class {wmiClass} | Select-Object *")
                .ExecuteBufferedAsync();
                
            task.Task.RunSynchronously();

            return task.Task.Result.StandardOutput;
        }

        private static string GetWindowsPowershellLocation()
        {
            return Environment.SystemDirectory + Path.DirectorySeparatorChar
                                                                   + "System32" + Path.DirectorySeparatorChar + "WindowsPowerShell"
                                                                   + Path.DirectorySeparatorChar + "v1.0" + Path.DirectorySeparatorChar + "Powershell.exe";
        }
        
        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Gets a property/value in a WMI Class from WMI.
        /// </summary>
        /// <param name="property"></param>
        /// <param name="wmiClass"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PlatformNotSupportedException">Thrown if run on an Operating System that isn't Windows.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]
#endif
        public static string GetWMIValue(string property, string wmiClass)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }
            
            var task = Cli.Wrap(GetWindowsPowershellLocation())
                .WithArguments($"Get-CimInstance -Class {wmiClass} -Property {property}")
                .ExecuteBufferedAsync();
                
            task.Task.RunSynchronously();

            string[] arr = task.Task.Result.StandardOutput.Split(Convert.ToChar(Environment.NewLine));
                
            foreach (string str in arr)
            {
                if (str.ToLower().StartsWith(property.ToLower()))
                {
                    return str
                        .Replace(" : ", string.Empty)
                        .Replace(property, string.Empty)
                        .Replace(" ", string.Empty);
                }
            }
           
            throw new ArgumentException();
        }
    }
}