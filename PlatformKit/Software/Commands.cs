using System;
using System.Diagnostics;

namespace PlatformKit.Software;

public class Commands
{
    public static string Run(OperatingSystem operatingSystem, string command)
    {
        switch (OperatingSystem.)
        {
            case Pl
        }
    }

    public static string GetOptions()
    {
        
    }

/// <summary>
    /// Run a command on macOS located in the /usr/bin/ folder.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static string RunMac(string command)
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

                return RunProcessMac(location, array[0], args);
            }
            else
            {
                return RunProcessMac(location, command);
            }
        }

        throw new PlatformNotSupportedException();
    }
    
    /// <summary>
    /// Run a command or program as if inside a terminal on FreeBSD.
    /// </summary>
    /// <param name="command"></param>
    /// <param name="processStartInfo"></param>
    /// <returns></returns>
    public string RunFreeBsd(string command, ProcessStartInfo processStartInfo = null)
    {
        return RunLinuxCommand(command, processStartInfo);
    }
    
    /// <summary>
    /// Run a command or program as if inside a terminal on Linux.
    /// </summary>
    /// <param name="command"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public string RunLinux(string command, ProcessStartInfo processStartInfo = null)
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
                    
                return RunProcessLinux(location, command, processArguments, processStartInfo);
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