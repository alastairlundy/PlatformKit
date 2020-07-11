using System;
using System.IO;

namespace AluminiumTech.PlatformKit.PlatformSpecifics.Generic
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
            processManager.RunProcess("uname + " + parameter);
            //processManager.RunConsoleCommand("uname " + parameter);
            TextReader reader = Console.In;
            return reader.ReadLine();
        }
    }
}