/* MIT License

Copyright (c) 2018-2021 AluminiumTech

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
using System.IO;

using AluminiumTech.DevKit.PlatformKit.Analyzers;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Windows;

public static class WindowsProcessManagerExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="processManager"></param>
    /// <param name="query"></param>
    /// <param name="value"></param>
    /// <param name="failMessage"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
    public static string GetRegistryValue(this ProcessManager processManager, string query, string value, string failMessage){
            if (new OSAnalyzer().IsWindows())
            {
                try{
                    processManager.RunCmdCommand("/c REG QUERY " + query + " /v " + value);

                    TextReader reader = Console.In;
                    string result = reader.ReadLine();
                    
                    if (result != null)
                    {
                        result = result.Replace(value, String.Empty).Replace("REG_SZ", String.Empty).Replace(" ", String.Empty);
                        return result;
                    }
                }
                catch{
                    return failMessage;
                }
            }

            throw new PlatformNotSupportedException();
        }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="processManager"></param>
    /// <param name="query"></param>
    /// <param name="wmiClass"></param>
    /// <param name="failMessage"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
    // ReSharper disable once InconsistentNaming
        public static string GetWMIValue(this ProcessManager processManager, string query, string wmiClass, string failMessage){
            if (new OSAnalyzer().IsWindows())
            {
                try
                {
                    processManager.RunPowerShellCommand(
                        "/c Get-WmiObject -query 'SELECT * FROM meta_class WHERE __class = '" + wmiClass);

                    TextReader reader = Console.In;
                
                    string result = reader.ReadLine()?.Replace(wmiClass, String.Empty).Replace(" ", String.Empty);
                
                    reader.Close();
                    return result;
                }
                catch{
                    return failMessage;
                }
            }

            throw new PlatformNotSupportedException();
        }
}