using System;
using System.IO;

using AluminiumTech.PlatformKit.enums;

using AluminiumTech.PlatformKit.PlatformSpecifics.Generic;

namespace AluminiumTech.PlatformKit.PlatformSpecifics
{
     class MacPlatform : UnixPlatform
    {
        protected ProcessManager processManager;

        public MacPlatform()
        {
            processManager = new ProcessManager();
        }
        
        /// <summary>
        /// Returns the Darwin version.
        /// </summary>
        /// <returns></returns>
        public string GetDarwinVersion()
        {
            //Runs uname -r which on macOS returns the Darwin version.
           return GetUnixKernelVersionToString();
        }
        
        /// <summary>
        /// Returns the machine name.
        /// </summary>
        /// <returns></returns>
        public string GetMachineName()
        {
            return GetUnameInformation("-n");
        }
        
        /// <summary>
        /// Returns the XNU Mach version.
        /// </summary>
        /// <returns></returns>
        public string GetXNUVersionToString()
        {
            var strings = GetUnixKernelInformationToString().Split();
            
            string result = "";
            
            foreach (string s in strings)
            {
                if (s.StartsWith("root:xnu"))
                {
                    result = s;
                }
                else
                {
                    //Do nothing
                }
            }

            result = result.Replace("root:xnu-", " ");
            result = result.Replace("/RELEASE_X86_64", " ");
            return result;
        }

        public Version GetXNUVersionToVersion()
        {
            var v = GetXNUVersionToString();
            v = v.Replace("~", ".");
            return new Version(v);
        }

        /// <summary>
        /// Returns the macOS version as a string
        /// </summary>
        /// <returns></returns>
        public string GetMacOSVersionToString()
        {
            return GetSwVersInformation("-productVersion");
        }
        
        /// <summary>
        /// Returns the macOS version as a Version
        /// </summary>
        /// <returns></returns>
        public Version GetMacOSVersionToVersion()
        {
           return new Version(GetMacOSVersionToString());
        }
        
        /// <summary>
        /// Return the macOS Build Number as a String
        /// </summary>
        /// <returns></returns>
        public string GetMacOSBuildNumber()
        {
            return GetSwVersInformation("-buildVersion");
        }
        
        /// <summary>
        /// Executes sw_vers in the Terminal and runs it with a parameter.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected string GetSwVersInformation(string parameter)
        {
            processManager.RunProcess("sw_vers "+ parameter);
            TextReader reader = Console.In;
            return reader.ToString();
        }
    }
}