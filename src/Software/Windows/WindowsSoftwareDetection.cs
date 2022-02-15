using System;
using AluminiumTech.DevKit.PlatformKit.Analyzers;

namespace AluminiumTech.DevKit.PlatformKit.Software.Windows;

public class WindowsSoftwareDetection
{
    protected OSAnalyzer _osAnalyzer;
    protected ProcessManager _processManager;
    
    public WindowsSoftwareDetection()
    {
        _osAnalyzer = new OSAnalyzer();
        _processManager = new ProcessManager();
    }
    
        /// <summary>
        ///  Gets the value of a registry key in the Windows registry.
        /// </summary>
        /// <param name="processManager"></param>
        /// <param name="query"></param>
        /// <param name="value"></param>
        /// <param name="failMessage"></param>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an exception if run on macOS or Linux.</exception>
        public string GetWindowsRegistryValue(string query, string value, string failMessage){
            if (_osAnalyzer.IsWindows())
            {
                try{
                    var result = _processManager.RunCmdCommand("/c REG QUERY " + query + " /v " + value);
                    
                    if (result != null)
                    {
                        result = result.Replace(value, String.Empty)
                            .Replace("REG_SZ", String.Empty)
                            .Replace(" ", String.Empty);
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
        public string GetWMIValue(string query, string wmiClass, string failMessage){
            if (_osAnalyzer.IsWindows())
            {
                try
                {
                    var result = _processManager.RunPowerShellCommand($"/c Get-WmiObject -" + query + "'SELECT * FROM meta_class WHERE __class = '" + wmiClass);

                    result = result.Replace(wmiClass, String.Empty).Replace(" ", String.Empty);
                    
                    return result;
                }
                catch{
                    return failMessage;
                }
            }

            throw new PlatformNotSupportedException();
        }
}