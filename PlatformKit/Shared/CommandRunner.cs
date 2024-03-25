/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.IO;

namespace PlatformKit;

/// <summary>
/// 
/// </summary>
public static class CommandRunner
{
 /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="platform"></param>
    /// <returns></returns>
    public static string GetOptions(Platform platform)
    {
        if (platform == Platform.Linux)
        {
            return 
        }
    }
*/

 /// <summary>
 /// Run a command on macOS located in the /usr/bin/ folder.
 /// </summary>
 /// <param name="command"></param>
 /// <param name="runAsAdministrator"></param>
 /// <returns></returns>
 /// <exception cref="PlatformNotSupportedException"></exception>
 public static string RunCommandOnMac(string command, bool runAsAdministrator = false)
    {
        if (PlatformAnalyzer.IsMac())
        {
            var location = "/usr/bin/";
                
            string[] array = command.Split(' ');

            if (array.Length > 1)
            {
                string args = "";

                foreach (string arg in array)
                {
                    args += arg + " ";
                }

                args = args.Replace(array[0], String.Empty);
                
                
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

        throw new PlatformNotSupportedException();
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
    public static string RunPowerShellCommand(string command, ProcessStartInfo processStartInfo = null, bool runAsAdministrator = false)
    {
        if (PlatformAnalyzer.IsWindows())
        {
            var location = Environment.SystemDirectory + Path.DirectorySeparatorChar 
                                                       + "System32" +
                                                       + Path.DirectorySeparatorChar + "WindowsPowerShell" +
                                                       Path.DirectorySeparatorChar + "v1.0";
            return ProcessRunner.RunProcessOnWindows(location, "powershell", command, processStartInfo, runAsAdministrator);
        }
                
        throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// Run a command or program as if inside a terminal on Linux.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="runAsAdministrator"></param>
    /// <returns></returns>
    public static string RunCommandOnLinux(string command, bool runAsAdministrator = false)
    {
        try
        {
            if (PlatformAnalyzer.IsLinux() || PlatformAnalyzer.IsFreeBSD())
            {
                var location = "/usr/bin/";

                var args = command.Split(' ');
                command = args[0];
                var processArguments = "";

                if (args.Length > 0)
                {
                    for (int index = 1; index < args.Length; index++)
                    {
                        processArguments += args[index].Replace(command, String.Empty);
                    }
                }

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

            throw new PlatformNotSupportedException();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            throw;
        }
    }
}