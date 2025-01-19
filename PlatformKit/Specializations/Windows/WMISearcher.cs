/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Runtime.Versioning;
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Buffered;
using CliRunner.Extensions;
using CliRunner.Specializations;

using PlatformKit.Internal.Localizations;
using PlatformKit.Specializations.Windows.Abstractions;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.OSCompatibilityLib.Polyfills.OperatingSystem;
#endif

namespace PlatformKit.Specializations.Windows
{
    /// <summary>
    /// A class to make searching WMI easier.
    /// </summary>
// ReSharper disable once InconsistentNaming
    public class WMISearcher : IWMISearcher
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
        public async Task<string> GetWMIClassAsync(string wmiClass)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }

            BufferedCommandResult task = await ClassicPowershellCommand.CreateInstance()
                .WithArguments($"Get-WmiObject -Class {wmiClass} | Select-Object *")
                .ExecuteBufferedAsync(new CommandRunner(new CommandPipeHandler()));
            
            return task.StandardOutput;
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
        public async Task<string> GetWMIValueAsync(string property, string wmiClass)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }
            
            var result = await ClassicPowershellCommand.CreateInstance()
                .WithArguments($"Get-CimInstance -Class {wmiClass} -Property {property}")
                .ExecuteBufferedAsync(new CommandRunner(new CommandPipeHandler()));
            
            string[] arr = result.StandardOutput.Split(Convert.ToChar(Environment.NewLine));
                
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