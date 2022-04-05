/* MIT License

Copyright (c) 2018-2022 AluminiumTech

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
using System.Collections.Generic;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics.VersionAnalyzers;
using AluminiumTech.DevKit.PlatformKit.Exceptions;
using AluminiumTech.DevKit.PlatformKit.Software.Windows;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;

/// <summary>
/// 
/// </summary>
public class WindowsAnalyzer
{
    protected readonly ProcessManager _processManager;
    protected readonly OSAnalyzer _osAnalyzer;
    
    public WindowsAnalyzer()
    {
        _processManager = new ProcessManager();
        _osAnalyzer = new OSAnalyzer();
    }

    /// <summary>
    /// Detects the Edition of Windows being run.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperatingSystemDetectionException"></exception>
    public WindowsEdition DetectWindowsEdition()
    {
        try
        {
            if (_osAnalyzer.IsWindows())
            {
                var desc = _processManager.RunPowerShellCommand("systeminfo");

                //Console.WriteLine(desc);

                var arr = desc.Replace(":", String.Empty).Split(' ');
                
                var edition = arr[41].Replace("OS", String.Empty);

                //var edition = GetWMIValue("Name", "Win32_OperatingSystem");

                //var edition = GetWindowsRegistryValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
                //   "EditionID");

                if (edition.ToLower().Contains("home"))
                {
                    return WindowsEdition.Home;
                }
                else if (edition.ToLower().Contains("pro") && edition.ToLower().Contains("workstation"))
                {
                    return WindowsEdition.ProfessionalForWorkstations;
                }
                else if (edition.ToLower().Contains("pro") && !edition.ToLower().Contains("education"))
                {
                    return WindowsEdition.Professional;
                }
                else if (edition.ToLower().Contains("pro") && edition.ToLower().Contains("education"))
                {
                    return WindowsEdition.ProfessionalForEducation;
                }
                else if (!edition.ToLower().Contains("pro") && edition.ToLower().Contains("education"))
                {
                    return WindowsEdition.Education;
                }
                else if (edition.ToLower().Contains("server"))
                {
                    return WindowsEdition.Server;
                }
                else if (edition.ToLower().Contains("enterprise") && edition.ToLower().Contains("ltsc") &&
                         !edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.EnterpriseLTSC;
                }
                else if (edition.ToLower().Contains("enterprise") && !edition.ToLower().Contains("ltsc") &&
                         !edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.EnterpriseSemiAnnualChannel;
                }
                else if (edition.ToLower().Contains("enterprise") && edition.ToLower().Contains("ltsc") &&
                         edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.IoTEnterpriseLTSC;
                }
                else if (edition.ToLower().Contains("enterprise") && !edition.ToLower().Contains("ltsc") &&
                         edition.ToLower().Contains("iot"))
                {
                    return WindowsEdition.IoTEnterprise;
                }

                if (WindowsVersionAnalyzer.IsWindows11())
                {
                    if (edition.ToLower().Contains("se"))
                    {
                        return WindowsEdition.SE;
                    }
                }

                throw new OperatingSystemDetectionException();
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
        catch(Exception exception)
        {
            Console.WriteLine(exception.ToString());
            throw new Exception(exception.ToString());
        }
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Gets information from a WMI class in WMI.
    /// </summary>
    /// <param name="wmiClass"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public string GetWMIClass(string wmiClass)
    {
        if (_osAnalyzer.IsWindows())
        {
            var result = _processManager.RunPowerShellCommand("Get-WmiObject -Class " + wmiClass + " | Select-Object *");
            // var result = _processManager.RunPowerShellCommand("Get-CimInstance -Class " + wmiClass);
            
//#if DEBUG
  //          Console.WriteLine(result);              
//#endif

            return result;
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
    /// <exception cref="PlatformNotSupportedException"></exception>
    public string GetWMIValue(string property, string wmiClass)
    {
        if (_osAnalyzer.IsWindows())
        {
            var result = _processManager.RunPowerShellCommand("Get-CimInstance -Class " 
                                                              + wmiClass + " -Property " + property);

            #if DEBUG
           //     Console.WriteLine(result);
            #endif
            
            var arr = result.Split(Convert.ToChar("\r\n"));
            
           foreach (var str in arr)
           {
               Console.WriteLine(str);
               
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
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    /// <summary>
    ///  Gets the value of a registry key in the Windows registry.
    /// </summary>
    /// <param name="query"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
    public string GetWindowsRegistryValue(string query, string value){
        if (_osAnalyzer.IsWindows())
        {
            var result = _processManager.RunCmdCommand("REG QUERY " + query + " /v " + value);
                    
                if (result != null)
                {
                    return result.Replace(value, String.Empty)
                        .Replace("REG_SZ", String.Empty)
                        .Replace(" ", String.Empty);
                }
                else
                {
                    throw new ArgumentNullException();
                }
        }

        throw new PlatformNotSupportedException();
    }
}