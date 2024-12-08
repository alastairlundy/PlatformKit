﻿/*
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
using System.Runtime.Versioning;

using CliWrap;
using CliWrap.Buffered;

using PlatformKit.Internal.Localizations;
using PlatformKit.OperatingSystems.Abstractions;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems
{
    public class FreeBsdOperatingSystem : AbstractOperatingSystem
    {

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// Detects and Returns the Installed version of FreeBSD
        /// </summary>
        /// <returns></returns>
#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]        
#endif
        public override Version GetOperatingSystemVersion()
        {
            if (OperatingSystem.IsFreeBSD() == false)
            {
                throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_FreeBsdOnly);
            }

            var result = Cli.Wrap("/usr/bin/uname")
                .WithArguments("-v")
                .WithWorkingDirectory(Environment.CurrentDirectory)
                .ExecuteBufferedAsync();
            
            result.Task.RunSynchronously();
            
            string versionString = result.Task.Result.StandardOutput.Replace("FreeBSD", string.Empty)
                .Split(' ')[0].Replace("-release", string.Empty);
            
            return Version.Parse(versionString);
        }

        public override Version GetKernelVersion()
        {
            throw new NotImplementedException();
        }

#if NET5_0_OR_GREATER
        [SupportedOSPlatform("freebsd")]
        [UnsupportedOSPlatform("macos")]
        [UnsupportedOSPlatform("linux")]
        [UnsupportedOSPlatform("windows")]
        [UnsupportedOSPlatform("browser")]
        [UnsupportedOSPlatform("ios")]
        [UnsupportedOSPlatform("android")]
        [UnsupportedOSPlatform("tvos")]
        [UnsupportedOSPlatform("watchos")]        
#endif
        public override string GetOperatingSystemBuildNumber()
        {
            return GetOperatingSystemVersion().Build.ToString();
        }
    }
}