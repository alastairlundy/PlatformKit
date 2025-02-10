/*
        MIT License
       
       Copyright (c) 2020-2025 Alastair Lundy
       
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
using System.Threading.Tasks;

using CliRunner;
using CliRunner.Abstractions;
using CliRunner.Builders;
using CliRunner.Builders.Abstractions;
using CliRunner.Specializations.Configurations;

using PlatformKit.Internal.Localizations;

// ReSharper disable RedundantBoolCompare

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

namespace PlatformKit.Windows
{
    /// <summary>
    /// A class to make searching WMI easier.
    /// </summary>
// ReSharper disable once InconsistentNaming
    public class WMISearcher
    {
        private readonly ICommandRunner _commandRunner;

        public WMISearcher(ICommandRunner commandRunner)
        {
            _commandRunner = commandRunner;
        }
        
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

            ICommandBuilder commandBuilder = new CommandBuilder(
                    new ClassicPowershellCommandConfiguration())
                .WithArguments($"Get-WmiObject -Class {wmiClass} | Select-Object *");
            
            Command command = commandBuilder.Build();
            
            BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);
            
            return result.StandardOutput;
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

            ICommandBuilder commandBuilder = new CommandBuilder(new ClassicPowershellCommandConfiguration())
                .WithArguments($"Get-CimInstance -Class {wmiClass} -Property {property}");
            
            Command command = commandBuilder.Build();
            
            BufferedCommandResult result = await _commandRunner.ExecuteBufferedAsync(command);
            
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