
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
            throw new Exception(exception.ToString());
        }
    }
}