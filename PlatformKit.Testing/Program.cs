using AluminiumTech.DevKit.PlatformKit;

using System;
using System.Diagnostics;

namespace PlatformKit.Testing
{
    class Program
    {
        static void Main(string[] args)
        {
            Platform platform = new Platform();
            ProcessManager processManager = new ProcessManager();
            OSVersionAnalyzer versionAnalyzer = new OSVersionAnalyzer();
            
            Console.WriteLine("Desc: " +  System.Runtime.InteropServices.RuntimeInformation.OSDescription);
            Console.WriteLine("AppName: " + platform.GetAppName() + " v" + platform.GetAppVersion());
            Console.WriteLine("WindowsEnum: " + versionAnalyzer.GetWindowsVersionToEnum());
            Console.WriteLine("Framework: " + System.Runtime.InteropServices.RuntimeInformation.FrameworkDescription);

            
            var result = 0;
            var addition = new Action(()=> result = 30 + 10);
            processManager.RunActionOn(addition);
            Console.WriteLine("Result of 30 + 10 is: " + result + " which is being stored in " + nameof(result));
            
            processManager.RunProcessWindows("cmd.exe ", "ping duckduckgo.com");
        }
    }
}