using System;
using System.IO;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Generic
{
     class UnixPlatform
    {
        protected ProcessManager processManager;

        public UnixPlatform()
        {
            processManager = new ProcessManager();
        }

        protected string GetUnixKernelInformationToString()
        {
            return GetUnameInformation("-a");
        }
        
        protected string GetUnixKernelVersionToString()
        {
            return GetUnameInformation("-r");
        }
        
        /// <summary>
        /// Returns the output of running "uname" with a specified parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        protected string GetUnameInformation(string parameter)
        {
            try
            {
                processManager.RunProcess("uname " + parameter);
                //processManager.RunConsoleCommand("uname " + parameter);
                TextReader reader = Console.In;
                return reader.ReadLine();
            }
            catch
            {
                processManager.ExecuteBuiltInLinuxCommand("uname " + parameter);
                TextReader reader = Console.In;
                return reader.ReadLine();
            }
        }
    }
}