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

using AluminiumTech.DevKit.PlatformKit.Analyzers;
using AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;

using AluminiumTech.DevKit.PlatformKit.Deprecation;

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Mac;

namespace AluminiumTech.DevKit.PlatformKit
{
    /// <summary>
    /// A class that helps get system information.
    /// </summary>
    public class PlatformManager
    {
        protected OSAnalyzer _osAnalyzer;
        
        public PlatformManager()
        {
            _osAnalyzer = new OSAnalyzer();
        }

        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use OSAnalyzer's IsWindows() method instead.")]
        public bool IsWindows()
        {
            return _osAnalyzer.IsWindows();
        }
        
        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use OSAnalyzer's IsLinux() method instead.")]
        public bool IsLinux()
        {
            return _osAnalyzer.IsLinux();
        }
        
        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use OSAnalyzer's IsMac() method instead.")]
        public bool IsMac()
        {
            return _osAnalyzer.IsMac();
        }

        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use OSAnalyzer's GetOSPlatform() method instead.")]
        public OSPlatform GetOSPlatform()
        {
            return _osAnalyzer.GetOSPlatform();
        }
        
        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use OSAnalyzer's IsFreeBSD() method instead.")]
        public bool IsFreeBSD()
        {
            return _osAnalyzer.IsFreeBSD();
        }

        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use OSAnalyzer's IsAppleSiliconMac() method instead.")]
        public bool IsAppleSiliconMac()
        {
            return _osAnalyzer.IsAppleSiliconMac();
        }

        [Obsolete(DeprecationMessages.DeprecationV3 + " . Please use OSAnalyzer's GetMacProcessorType() method instead.")]
        public MacProcessorType GetMacProcessorType()
        {
            return _osAnalyzer.GetMacProcessorType();
        }
        
        /// <summary>
        /// Return's the executing app's assembly.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once MemberCanBePrivate.Global
        // ReSharper disable once MemberCanBeMadeStatic.Global
        [Obsolete(DeprecationMessages.DeprecationV3 + ". Please use the PlatformIdentification method instead.")]
        public Assembly GetAssembly()
        {
            return Assembly.GetEntryAssembly();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        [Obsolete(DeprecationMessages.DeprecationV3 + ". Please use the PlatformIdentification method instead.")]
        public string GetAppName()
        {
            return GetAssembly().GetName().Name;
        }
        
        /// <summary>
        /// Return an app's version as a Version data type.
        /// </summary>
        /// <returns></returns>
        // ReSharper disable once UnusedMember.Global
        [Obsolete(DeprecationMessages.DeprecationV3 + ". Please use the PlatformIdentification method instead.")]
        public Version GetAppVersion()
        {
            return GetAssembly().GetName().Version;
        }

        /// <summary>
        ///     Displays license information in the Console from a Text File.
        ///     The information stays in the Console window for the the duration specified in the parameter.
        /// </summary>
        /// <param name="pathToTextFile"></param>
        /// <param name="durationMilliSeconds">The duration to keep the information in the Console. Any value less than 100 will result in an exception being thrown.</param>
        /// <exception cref="ArgumentOutOfRangeException">Is thrown when the duration milliseconds is less than 100 milliseconds.</exception>
        // ReSharper disable once UnusedMember.Global
        public void ShowLicenseInConsole(string pathToTextFile, int durationMilliSeconds)
        {
            try
            {
                if (durationMilliSeconds is 0 or < 100)
                {
                    throw new ArgumentOutOfRangeException();
                }
                
             //   bool isMarkDownFile = pathToTextFile.ToLower().EndsWith(".md");
                
                var oldPath = pathToTextFile;
                
               /* if (isMarkDownFile)
                {
                    pathToTextFile = pathToTextFile.Replace(".md", ".txt");
                    
                    File.Move(oldPath, pathToTextFile);
                }
                */

                // ReSharper disable once HeapView.ObjectAllocation.Evident
                var licenseWatch = new Stopwatch();

                var lines = File.ReadAllLines(pathToTextFile);

                foreach (var line in lines)
                {
                    Console.WriteLine(line);
                }
                
                Console.WriteLine("\r\n");
                Console.WriteLine("\r\n");

                licenseWatch.Start();

                while (licenseWatch.ElapsedMilliseconds <= durationMilliSeconds)
                {
                    //Do nothing to make sure everybody sees the license.
                }

                licenseWatch.Stop();
                licenseWatch.Reset();

              /*  if (isMarkDownFile)
                {
                    File.Move(pathToTextFile, oldPath);
                }
                */
                Console.Clear();
            }
            catch (Exception exception)
            {
                Console.WriteLine(exception.ToString());
                throw new Exception(exception.ToString());
            }
        }
    }
}