/* MIT License

Copyright (c) 2018-2022 AluminiumTech

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
using PlatformKit.Internal.Licensing;

namespace PlatformKit;

public class ConsoleHelper
{
    public ConsoleHelper()
    {
        LicenseManager.CheckLicenseStatus();
    }

        /// <summary>
    ///  Displays license information in the Console from a Text File.
    ///  The information stays in the Console window for the the duration specified in the parameter.
    /// </summary>
    /// <param name="pathToTextFile"></param>
    /// <param name="durationMilliSeconds">The duration to keep the information in the Console. Any value less than 500 will result in an exception being thrown.</param>
    /// <exception cref="ArgumentOutOfRangeException">Is thrown when the duration milliseconds is less than 500 milliseconds.</exception>
    // ReSharper disable once UnusedMember.Global
    public static void ShowLicenseInConsole(string pathToTextFile, int durationMilliSeconds)
    {
        try
        {
            if (durationMilliSeconds is 0 or < 500)
            {
                throw new ArgumentOutOfRangeException();
            }

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
                
            Console.Clear();
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception.ToString());
            throw new Exception(exception.ToString());
        }
    }
}