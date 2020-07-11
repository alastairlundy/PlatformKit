using System;

using AluminiumTech.PlatformKit.PlatformSpecifics;

using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace PlatformKitTest
{
    public class UnitTest1
    {
        [Fact]
        public void TestMacPlatform()
        {
         /*
            MacPlatform macPlatform = new MacPlatform();
            macPlatform.GetDarwinVersion();
            macPlatform.GetMachineName();
            macPlatform.GetMacOSBuildNumber();
            macPlatform.GetMacOSVersionToString();
            macPlatform.GetXNUVersionToString();
            macPlatform.GetXNUVersionToVersion();
*/
            //Console.WriteLine(Environment.CommandLine);
           Console.WriteLine(Environment.SystemDirectory);

        }
    }
}