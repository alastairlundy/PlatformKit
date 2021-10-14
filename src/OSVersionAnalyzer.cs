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

using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics;

namespace AluminiumTech.DevKit.PlatformKit
{
    // ReSharper disable once InconsistentNaming
    public class OSVersionAnalyzer
    {
        /// <summary>
        /// Detects Windows Version and returns it as an enum.
        /// </summary>
        /// <returns></returns>
        public WindowsVersion GetWindowsVersionToEnum()
        {
            switch (DetectWindowsVersion())
            {
                case "6.0.6000":
                    return WindowsVersion.WinVista;
                case "6.0.6001":
                    return WindowsVersion.WinVistaSP1;
                case "6.0.6002":
                    return WindowsVersion.WinVistaSP2;
                case "6.1.7600":
                    return WindowsVersion.Win7;
                case "6.1.7601":
                    return WindowsVersion.Win7SP1;
                case "6.2.9200":
                    return WindowsVersion.Win8;
                case "6.3.9600":
                    return WindowsVersion.Win8_1;
                case "10.0.10240":
                    return WindowsVersion.Win10_v1507;
                case "10.0.10586":
                    return WindowsVersion.Win10_v1511;
                case "10.0.14393":
                    return WindowsVersion.Win10_v1607;
                case "10.0.15063":
                    return WindowsVersion.Win10_v1703;
                case "10.0.15254":
                    return WindowsVersion.Win10_v1709ForMobile;
                case "10.0.16299":
                    return WindowsVersion.Win10_v1709;
                case "10.0.17134":
                    return WindowsVersion.Win10_v1803;
                case "10.0.17763":
                    return WindowsVersion.Win10_v1809;
                case "10.0.18362":
                    return WindowsVersion.Win10_v1903;
                case "10.0.18363":
                    return WindowsVersion.Win10_v1909;
                case "10.0.19041":
                    return WindowsVersion.Win10_v2004;
                case "10.0.19042":
                    return WindowsVersion.Win10_20H2;
                case "10.0.19043":
                    return WindowsVersion.Win10_21H1;
                case "10.0.22000":
                    return WindowsVersion.Win11_21H2;
                default:
                    return WindowsVersion.NotDetected;
            }
        }

        /// <summary>
        /// Detects Windows Version and returns it as a System.Version
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public Version GetWindowsVersion()
        {
            try
            {
                return Version.Parse(DetectWindowsVersion());
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception(exception.ToString());
            }
        }

        // ReSharper disable once MemberCanBePrivate.Global
        protected string DetectWindowsVersion()
        {
            var description = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
            description = description.Replace("Microsoft Windows", " ");

            string[] descArray = description.Split(' ');

            foreach (var d in descArray)
            {
                if (!d.Contains(string.Empty))
                {
                    return d;
                }
            }

            return description;
        }

       /*
        protected string DetectMacVersion()
        
        {
           
        }

        protected string DetectLinuxVersion()
        {
            
        }
        */
    }
}