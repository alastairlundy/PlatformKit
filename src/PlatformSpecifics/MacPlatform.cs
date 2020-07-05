using System;
using System.Runtime.InteropServices;

namespace AluminiumTech.PlatformKit.PlatformSpecifics
{
    public class MacPlatform
    {
        protected ProcessManager processManager;

        public MacPlatform()
        {
            processManager = new ProcessManager();
        }
        
        //TODO: Double Check that this works
        public string GetDarwinVersion()
        {
            string macOS = RuntimeInformation.OSDescription ?? throw new ArgumentNullException("RuntimeInformation.OSDescription");
                        
            macOS = macOS.Replace("Darwin", " ");
            macOS = macOS.Replace("Kernel Version", " ");
            // macOSKernel.Replace("", "");
            macOS = macOS.Replace("root:xnu", "xnu");
            macOS = macOS.Replace("/RELEASE_X86_64", " ");
            
            string x = macOS;
            
            Console.WriteLine("New kernel string: " + x);
            return macOS;
        }

        public string GetXNUVersion()
        {
            
        }

        public string GetMacOSVersion()
        {
            
        }
        
        
    }
}