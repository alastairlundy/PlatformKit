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
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;
using PlatformKit.Internal.Deprecation;
using PlatformKit.Internal.Helpers;
using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = Polyfills.OperatingSystemPolyfill;
#else
using System.Runtime.Versioning;
#endif

// ReSharper disable HeapView.DelegateAllocation
// ReSharper disable InvalidXmlDocComment

namespace PlatformKit
{
    /// <summary>
    ///  A class to manage processes on a device and/or start new processes.
    /// </summary>
    [Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
    public static class ProcessRunner
    {

        /// <summary>
        /// Run a process on Windows with Arguments
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        /// <param name="windowStyle">Whether to open the window in full screen mode, normal size, minimized, or a different window mode.</param>
        /// <exception cref="Exception"></exception>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("windows")]
#endif
        [Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
        public static string RunProcessOnWindows(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null,
            bool runAsAdministrator = false, bool insertExeInExecutableNameIfMissing = true,
            ProcessWindowStyle windowStyle = ProcessWindowStyle.Normal)
        {
            if (OperatingSystem.IsWindows() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_WindowsOnly);
            }
            
            BufferedCommandResult result = Cli.Wrap(executableName)
                .WithArguments(arguments)
                .WithWorkingDirectory(executableLocation)
                .ExecuteBufferedSync();
            
            return result.StandardOutput;
        }

        /// <summary>
        ///  Run a Process on macOS
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="processArguments">Arguments to be passed to the executable.</param>
        [Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
        public static string RunProcessOnMac(string executableLocation, string executableName,
            string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            if (OperatingSystem.IsMacOS() == false && OperatingSystem.IsMacCatalyst() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
            }
            
            BufferedCommandResult result = Cli.Wrap(executableName)
                .WithArguments(arguments)
                .WithWorkingDirectory(executableLocation)
                .ExecuteBufferedSync();

            return result.StandardOutput;
        }

        /// <summary>
        /// Run a Process on Linux
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        [Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
        public static string RunProcessOnLinux(string executableLocation, string executableName,
            string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            if (OperatingSystem.IsLinux() == false && OperatingSystem.IsFreeBSD() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_LinuxOnly);
            }

            BufferedCommandResult result = Cli.Wrap(executableName)
                .WithArguments(arguments)
                .WithWorkingDirectory(executableLocation)
                .ExecuteBufferedSync();
            
            return result.StandardOutput;
        }

        /// <summary>
        /// Run a Process on FreeBSD
        /// </summary>
        /// <param name="executableLocation">The working directory of the executable.</param>
        /// <param name="executableName">The name of the file to be run.</param>
        /// <param name="arguments">Arguments to be passed to the executable.</param>
        /// <returns></returns>
        [Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
        public static string RunProcessOnFreeBsd(string executableLocation, string executableName, string arguments = "", ProcessStartInfo processStartInfo = null)
        {
            return RunProcessOnLinux(executableLocation, executableName, arguments, processStartInfo);
        }
    }
}