using System;
using System.Runtime.Versioning;

#if NETSTANDARD2_0
    using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.Windows;

/// <summary>
/// A class to make searching WMI easier.
/// </summary>
// ReSharper disable once InconsistentNaming
public class WMISearcher
{
        // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Gets information from a WMI class in WMI.
    /// </summary>
    /// <param name="wmiClass"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't Windows.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static string GetWMIClass(string wmiClass)
    {
        if (OperatingSystem.IsWindows())
        {
            return CommandRunner.RunPowerShellCommand("Get-WmiObject -Class " + wmiClass + " | Select-Object *");
        }

        throw new PlatformNotSupportedException();
    }
    
    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Gets a property/value in a WMI Class from WMI.
    /// </summary>
    /// <param name="property"></param>
    /// <param name="wmiClass"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on a platform that isn't Windows.</exception>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("windows")]
#endif
    public static string GetWMIValue(string property, string wmiClass)
    {
        if (OperatingSystem.IsWindows())
        {
            var arr = CommandRunner.RunPowerShellCommand("Get-CimInstance -Class " + wmiClass + " -Property " + property).Split(Convert.ToChar(Environment.NewLine));
            
           foreach (var str in arr)
           {
               if (str.ToLower().StartsWith(property.ToLower()))
               {
                   return str
                       .Replace(" : ", String.Empty)
                       .Replace(property, String.Empty)
                       .Replace(" ", String.Empty);
                       
               }
           }
           
           throw new ArgumentException();
        }

        throw new PlatformNotSupportedException();
    }
}