using System;

using PlatformKit;

namespace PlatformKit.Software;
{
    public class InstalledApps
    {


        public static AppModel[] Get()
        {
            if (OSAnalyzer.IsWindows())
            {
                return GetWindowsApps();
            }
            else if (OSAnalyzer.IsMac())
            {
                return GetMacApps();
            }
            else if (OSAnalyzer.IsLinux())
            {
                return GetLinuxApps();
            }
        }

        protected static AppModel[] GetMacApps()
        {
            
        }
        
        protected static AppModel[] GetWindowsApps()
        {
            
        }
        
        protected static AppModel[] GetLinuxApps()
        {
            foreach (var str )
            {
                
            }
        }

        public static void Open(AppModel appModel)
        {
            
        }
    }
}