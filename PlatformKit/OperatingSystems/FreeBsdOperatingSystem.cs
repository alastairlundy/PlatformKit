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
        public override Version GetOperatingSystemVersion()
        {
            if (OperatingSystem.IsFreeBSD())
            {
                string versionString = CommandRunner.RunCommandOnFreeBsd("uname -v").Replace("FreeBSD", string.Empty)
                    .Split(' ')[0].Replace("-release", string.Empty);
            
                return Version.Parse(versionString);
            }
            throw new PlatformNotSupportedException();
        }

        public override Version GetKernelVersion()
        {
            throw new NotImplementedException();
        }

        public override string GetOperatingSystemBuildNumber()
        {
            throw new NotImplementedException();
        }
    }
}