/* MIT License

Copyright (c) 2018-2020 AluminiumTech

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
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Generic;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics
{
    /// <summary>
    /// A class to help detect platform specific information regarding Linux.
    /// </summary>
     class LinuxPlatform : UnixPlatform
    {
        protected ProcessManager processManager;

        public LinuxPlatform()
        {
            processManager = new ProcessManager();
        }
        
        /// <summary>
        /// Returns the Linux Desktop Environment used by a Distro.
        /// </summary>
        /// <returns></returns>
        public string GetLinuxDesktopEnvironment()
        {
            processManager.RunProcess("echo $XDG_CURRENT_DESKTOP");
            TextReader reader = Console.In;
            return reader.ReadLine();
        }
        
        /// <summary>
        /// Gets all the information about the Linux Kernel returned as a String.
        /// </summary>
        /// <returns></returns>
        public string GetLinuxKernelInformation()
        {
            return GetUnixKernelVersionToString();
        }

        /// <summary>
        /// Returns the Linux Kernel Version as reported by "uname -r"
        /// </summary>
        /// <returns></returns>
        public string GetLinuxKernelVersionToString()
        {
            return GetUnixKernelVersionToString();
        }

        /// <summary>
        /// Returns the name of the Linux Distribution as reported by "lsb_release --id"
        /// </summary>
        /// <returns></returns>
        public string GetDistributionName()
        {
            return GetLsb_releaseInformation("--id").Replace("Distributor ID: ", " ");
        }
        
        /// <summary>
        /// Returns the "Release" of the Distribution (what we call the Version)
        /// </summary>
        /// <returns></returns>
        public string GetDistributionVersion()
        {
            return GetLsb_releaseInformation("--release").Replace("Release: ", " ");
        }
        
        /// <summary>
        /// Returns the description of the Linux Distribution
        /// </summary>
        /// <returns></returns>
        public string GetDistributionDescription()
        {
            return GetLsb_releaseInformation("--description").Replace("Description: ", " ");
        }

        /// <summary>
        /// Returns the output of running "lsb_release" with a specified parameter
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        public string GetLsb_releaseInformation(string parameter)
        {
            try
            {
                processManager.RunProcess("lsb_release " + parameter);
                // processManager.RunConsoleCommand("lsb_release " + parameter); 
                TextReader reader = Console.In; 
                return reader.ReadLine();
            }
            catch
            {
                processManager.ExecuteBuiltInLinuxCommand("lsb_release " + parameter);
                TextReader reader = Console.In; 
                return reader.ReadLine();
            }
            
        }
    }
}