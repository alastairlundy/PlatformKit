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
using AlastairLundy.CliInvoke;
using AlastairLundy.CliInvoke.Abstractions;
using AlastairLundy.CliInvoke.Builders;
using AlastairLundy.CliInvoke.Builders.Abstractions;
using AlastairLundy.Extensions.IO.Files;
using AlastairLundy.Extensions.Processes;
using AlastairLundy.Extensions.Processes.Piping;

using AlastairLundy.Extensions.Processes.Utilities;
using PlatformKit.Internal.Localizations;
using AlastairLundy.Extensions.Processes.Abstractions;
using AlastairLundy.Extensions.Processes.Abstractions.Piping;



#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
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
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("linux")]
#endif
        public static async Task<string> GetMacSystemProfilerInformationAsync(this MacOperatingSystem macOperatingSystem,
            MacSystemProfilerDataType macSystemProfilerDataType, string key)
        {
            return await GetMacSystemProfilerInformationAsync(macSystemProfilerDataType, key);
        }
    
        /// <summary>
        /// Gets a value from the Mac System Profiler information associated with a key
        /// </summary>
        /// <param name="macSystemProfilerDataType"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        /// <exception cref="PlatformNotSupportedException">Throw if run on an Operating System that isn't macOS.</exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("linux")]
#endif
        public static async Task<string> GetMacSystemProfilerInformationAsync(this MacSystemProfilerDataType macSystemProfilerDataType, string key)
        {
            if (OperatingSystem.IsMacOS() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }

            ICliCommandConfigurationBuilder commandBuilder = new CliCommandConfigurationBuilder("/usr/bin/system_profiler")
                .WithArguments("SP" + macSystemProfilerDataType)
                .WithWorkingDirectory("/usr/bin/")
                .WithValidation(ProcessResultValidation.None);

            CliCommandConfiguration command = commandBuilder.Build();

            IProcessPipeHandler processPipeHandler = new ProcessPipeHandler();
            
            ICliCommandInvoker commandRunner = new CliCommandInvoker(
               new PipedProcessRunner(new ProcessRunnerUtility(new FilePathResolver()), processPipeHandler),
               processPipeHandler, new CommandProcessFactory());
            
            BufferedProcessResult result = await commandRunner.ExecuteBufferedAsync(command);
            
            string info = result.StandardOutput;

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