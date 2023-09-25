/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
     any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
 */

using System;
using System.Diagnostics;
using System.IO;

namespace PlatformKit;

public class ConsoleHelper
{
    public ConsoleHelper()
    {
        
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
            throw;
        }
    }
}