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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.InteropServices;

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Enums;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Mac;
using AluminiumTech.DevKit.PlatformKit.Runtime;

namespace AluminiumTech.DevKit.PlatformKit
{
    /// <summary>
    /// A class that helps get system information.
    /// </summary>
    public class PlatformManager
    {
        
        public PlatformManager()
        {
            
        }

        /// <summary>
        /// Returns whether or not the current OS is macOS.
        /// </summary>
        /// <returns></returns>
        public bool IsMac()
        {
           return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.OSX);
        }
        
        public bool IsAppleSiliconMac()
        {
            return GetMacProcessorType() == MacProcessorType.AppleSilicon;
        }

        public MacProcessorType GetMacProcessorType()
        {
            try
            {
                if (IsMac())
                {
                     switch (System.Runtime.InteropServices.RuntimeInformation.OSArchitecture)
                    {
                        case Architecture.Arm64:
                            return MacProcessorType.AppleSilicon;
                        case Architecture.X64:
                            return MacProcessorType.Intel;
                        default:
                            return MacProcessorType.NotDetected;
                    }
                }

                throw new PlatformNotSupportedException();
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception(exception.ToString());
            }
        } 

        /// <summary>
        /// Returns whether or not the current OS is Windows.
        /// </summary>
        /// <returns></returns>
        public bool IsWindows()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Windows);
        }

        /// <summary>
        /// Returns whether or not the current OS is Linux based.
        /// </summary>
        /// <returns></returns>
        public bool IsLinux()
        {
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.Linux);
        }

        // ReSharper disable once InconsistentNaming
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// <exception cref="PlatformNotSupportedException">Throws an error if run on .NET Standard 2 or .NET Core 2.1 or earlier.</exception>
        public bool IsFreeBSD()
        {
#if NETCOREAPP3_0_OR_GREATER
            return System.Runtime.InteropServices.RuntimeInformation.IsOSPlatform(System.Runtime.InteropServices.OSPlatform.FreeBSD);
#endif
            throw new PlatformNotSupportedException();
        }
        
        /// <summary>
        /// Determine what OS is being run
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        // ReSharper disable once InconsistentNaming
        public System.Runtime.InteropServices.OSPlatform GetOSPlatform() {
            System.Runtime.InteropServices.OSPlatform osPlatform = System.Runtime.InteropServices.OSPlatform.Create("Other Platform");
            // Check if it's windows
            osPlatform = IsWindows() ? System.Runtime.InteropServices.OSPlatform.Windows : osPlatform;
            // Check if it's osx
            osPlatform = IsMac() ? System.Runtime.InteropServices.OSPlatform.OSX : osPlatform;
            // Check if it's Linux
            osPlatform = IsLinux() ? System.Runtime.InteropServices.OSPlatform.Linux : osPlatform;
            
#if NETCOREAPP3_0_OR_GREATER
            // Check if it's FreeBSD
            osPlatform = IsFreeBSD() ? System.Runtime.InteropServices.OSPlatform.FreeBSD : osPlatform;
#endif

            return osPlatform;
        }

        /// <summary>
        /// Return's the executing app's assembly.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public Assembly GetAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public string GetAppName()
        {
            return GetAssembly().GetName().Name;
        }
        
        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        public Version GetAppVersion()
        {
            return GetAssembly().GetName().Version;
        }

        /// <summary>
        ///     Displays license information in the Console from a Text File.
        ///     The information stays in the Console window for the the duration specified in the parameter.
        /// </summary>
        // ReSharper disable once UnusedMember.Global
        public void ShowLicenseInConsole(string pathToTextFile, int durationMilliSeconds)
        {
            try
            {
                // ReSharper disable once HeapView.ObjectAllocation.Evident
                var licenseWatch = new Stopwatch();

                var lines = File.ReadAllLines(pathToTextFile);

                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                
                Console.WriteLine("                                                         ");
                Console.WriteLine("                                                         ");

                licenseWatch.Start();

                while (licenseWatch.ElapsedMilliseconds <= durationMilliSeconds)
                {
                    //Do nothing to make sure everybody sees the license.
                }

                licenseWatch.Stop();
                licenseWatch.Reset();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }

        /// <summary>
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            // ReSharper disable once IdentifierTypo
            var bitness = Environment.Is64BitProcess ? "64 Bit" : "32 Bit";

            var app = "";

            if (GetAppName()?.Length > 0)
                // ReSharper disable once HeapView.ObjectAllocation
                app = "App " + GetAppName() + " v" + GetAppVersion() + " " + bitness;

            // ReSharper disable once HeapView.ObjectAllocation
            var running = " running on RunTimeID: " + new RuntimeIdentification().DetectRuntimeIdentifier();

            // ReSharper disable once HeapView.ObjectAllocation
            return app + running;
        }
    }
}