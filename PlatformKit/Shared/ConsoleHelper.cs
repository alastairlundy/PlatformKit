/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Diagnostics;
using System.IO;
using PlatformKit.Internal.Deprecation;

namespace PlatformKit;

[Obsolete(DeprecationMessages.DeprecationV4)]
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
    [Obsolete(DeprecationMessages.DeprecationV4)]
    public static void ShowLicenseInConsole(string pathToTextFile, int durationMilliSeconds)
    {
            if (durationMilliSeconds is 0 or < 500)
            {
                throw new ArgumentOutOfRangeException();
            }

            Stopwatch licenseWatch = new Stopwatch();

            string[] lines = File.ReadAllLines(pathToTextFile);

            foreach (var line in lines)
            {
                Console.WriteLine(line);
            }
                
            Console.WriteLine(Environment.NewLine);
            Console.WriteLine(Environment.NewLine);

            licenseWatch.Start();

            while (licenseWatch.ElapsedMilliseconds <= durationMilliSeconds)
            {
                //Do nothing to make sure everybody sees the license.
            }

            licenseWatch.Stop();
            Console.Clear();
    }
}