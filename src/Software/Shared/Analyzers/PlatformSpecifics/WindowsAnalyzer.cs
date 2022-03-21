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
using AluminiumTech.DevKit.PlatformKit.Exceptions;
using AluminiumTech.DevKit.PlatformKit.Software.Windows;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;

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
    /// Detect the Edition of Windows being run.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="OperatingSystemDetectionException"></exception>
    public WindowsEdition DetectWindowsEdition()
    {
        if (_osAnalyzer.IsWindows())
        {

            var edition = GetWindowsRegistryValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion",
                "EditionID");

            if (edition.ToLower().Contains("home"))
            {
                
            }
            else if (edition.ToLower().Contains("pro") && !edition.ToLower().Contains("education"))
            {
                
            }
            else if (edition.ToLower().Contains("pro") && edition.ToLower().Contains("education"))
            {
                return W
            }
            else if (edition.ToLower().Contains("server")){  
                return WindowsEdition.Server;
            }
            else if (edition.ToLower().Contains("government"))
            {
                return WindowsEdition.GovernmentEdition;
            } 
            throw new OperatingSystemDetectionException();
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    // ReSharper disable once InconsistentNaming
    public string GetWMIValue(string query, string wmiClass)
    {
        if (_osAnalyzer.IsWindows())
        { 
           // var result = processManager.
            //"/c Get-WmiObject -" + query + "'SELECT * FROM meta_class WHERE __class = '" + wmiClass);

          //  return result.Replace(wmiClass, String.Empty).Replace(" ", String.Empty);
        } 
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="query"></param>
    /// <param name="wmiClass"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
    // ReSharper disable once InconsistentNaming
    public string GetWindowsManagementInstrumentationValue(string query, string wmiClass)
    {
        return GetWMIValue(query, wmiClass);
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
            var result = _processManager.RunCmdCommand("/c REG QUERY " + query + " /v " + value);
                    
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