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
using System.Linq;
using System.Text;

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

namespace PlatformKit;

/// <summary>
/// 
/// </summary>
[Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
internal static class CommandRunner
{
    /// <summary>
    /// Run a command or program as if inside a terminal on FreeBSD.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
    [Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
    internal static string RunCommandOnFreeBsd(string command, bool runAsAdministrator = false)
    {
        return RunCommandOnLinux(command, runAsAdministrator);
    }

    /// <summary>
    /// Run a command or program as if inside a terminal on Linux.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
    [Obsolete(DeprecationMessages.DeprecationV5UseCliRunnerInstead)]
    internal static string RunCommandOnLinux(string command, bool runAsAdministrator = false)
    {
        if (OperatingSystem.IsLinux() == false && OperatingSystem.IsFreeBSD() == false)
        {
            throw new PlatformNotSupportedException();
        }
        
        string location = "/usr/bin/";
        
        string[] args = command.Split(' ');
        command = args[0];

        StringBuilder stringBuilder = new StringBuilder();
            
        if (args.Length > 0)
        {
            stringBuilder.AppendJoin(' ', args.Skip(1));
        }
        
        if (runAsAdministrator)
        {
            command = command.Insert(0, "sudo ");
        }
        
        var result = Cli.Wrap($"{location}{Path.DirectorySeparatorChar}{command}")
            .WithArguments(stringBuilder.ToString())
            .ExecuteBufferedSync();
        
        return result.StandardOutput;
    }
}