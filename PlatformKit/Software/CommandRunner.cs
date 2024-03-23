using System;
using System.Diagnostics;
using System.IO;
using PlatformKit.Extensions;
using PlatformKit.Linux;

namespace PlatformKit.Software;

/// <summary>
/// 
/// </summary>
public static class CommandRunner
{

/// <summary>
    /// Run a command on macOS located in the /usr/bin/ folder.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static string RunCommandOnMac(string command)
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
                return ProcessRunner.RunProcessOnMac(location, command);
            }
        }

        throw new PlatformNotSupportedException();
    }

    /// <summary>
    /// Run a command or program as if inside a terminal on FreeBSD.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static string RunCommandOnFreeBsd(string command)
    {
        return RunCommandOnLinux(command);
    }

    /// <summary>
    /// Runs commands in the Windows Cmd Command Prompt.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static string RunCmdCommand(string command, ProcessStartInfo processStartInfo = null)
    {
        return ProcessRunner.RunProcessOnWindows(Environment.SystemDirectory, "cmd", command, processStartInfo);
    }

    /// <summary>
    /// Runs commands in Windows Powershell.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static string RunPowerShellCommand(string command, ProcessStartInfo processStartInfo = null)
    {
        if (PlatformAnalyzer.IsWindows())
        {
            var location = Environment.SystemDirectory + Path.DirectorySeparatorChar 
                                                       //+ "System32" +
                                                       + Path.DirectorySeparatorChar + "WindowsPowerShell" +
                                                       Path.DirectorySeparatorChar + "v1.0";
            return ProcessRunner.RunProcessOnWindows(location, "powershell", command, processStartInfo);
        }
                
        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// Run a command or program as if inside a terminal on Linux.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    public static string RunCommandOnLinux(string command)
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