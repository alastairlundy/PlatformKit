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
using System.Diagnostics;
using System.IO;
using System.Runtime.Versioning;
using System.Text;

#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif  

namespace PlatformKit;

/// <summary>
/// 
/// </summary>
public static class CommandRunner
{

 /// <summary>
 /// Run a command on macOS located in the /usr/bin/ folder.
 /// </summary>
 /// <param name="command"></param>
 /// <param name="runAsAdministrator"></param>
 /// <returns></returns>
 /// <exception cref="PlatformNotSupportedException"></exception>
 public static string RunCommandOnMac(string command, bool runAsAdministrator = false)
    {
        if (!OperatingSystem.IsMacOS())
        {
            throw new PlatformNotSupportedException();
        }
        
        string location = "/usr/bin/";
                
        string[] array = command.Split(' ');

        if (array.Length > 1)
        {
            StringBuilder stringBuilder = new StringBuilder();

            foreach (string argument in array)
            {
                stringBuilder.Append($"{argument} ");
            }

            string args = stringBuilder.ToString().Replace(array[0], string.Empty);
                
            return ProcessRunner.RunProcessOnMac(location, array[0], args);
        }
        else
        {
            if (runAsAdministrator)
            {
                command = command.Insert(0, "sudo ");
            }
                
            return ProcessRunner.RunProcessOnMac(location, command);
        }

    }

    /// <summary>
    /// Run a command or program as if inside a terminal on FreeBSD.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
    public static string RunCommandOnFreeBsd(string command, bool runAsAdministrator = false)
    {
        return RunCommandOnLinux(command, runAsAdministrator);
    }

    /// <summary>
    /// Runs commands in the Windows Cmd Command Prompt.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="processStartInfo"></param>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static string RunCmdCommand(string command, ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false)
    {
        return ProcessRunner.RunProcessOnWindows(Environment.SystemDirectory, "cmd", command, processStartInfo, runAsAdministrator);
    }

    /// <summary>
    /// Runs commands in Windows Powershell.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="processStartInfo"></param>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static string RunPowerShellCommand(string command, ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false)
    {
        if (OperatingSystem.IsWindows() == false)
        {
            throw new PlatformNotSupportedException();
        }
        
        string location =
            $"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}System32{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0";
        
        return ProcessRunner.RunProcessOnWindows(location, "powershell", command, processStartInfo, runAsAdministrator);
    }

    /// <summary>
    /// Run a command or program as if inside a terminal on Linux.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
    public static string RunCommandOnLinux(string command, bool runAsAdministrator = false)
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
            for (int index = 1; index < args.Length; index++)
            {
                stringBuilder.Append(args[index].Replace(command, string.Empty));
            }
        }

        string processArguments = stringBuilder.ToString();

        if (!Directory.Exists(location))
        {
            throw new DirectoryNotFoundException("Could not find directory " + location);
        }

        if (runAsAdministrator)
        {
            command = command.Insert(0, "sudo ");
        }
                    
        return ProcessRunner.RunProcessOnLinux(location, command, processArguments);
    }
}