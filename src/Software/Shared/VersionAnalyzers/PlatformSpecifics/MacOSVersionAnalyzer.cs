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
using System.Runtime.InteropServices;
using AluminiumTech.DevKit.PlatformKit.Exceptions;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Mac;
using AluminiumTech.DevKit.PlatformKit.Software.Mac.Models;

//Move namespace in v3.
namespace AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics.VersionAnalyzers;

// ReSharper disable once InconsistentNaming
/// <summary>
/// 
/// </summary>
public static class MacOSVersionAnalyzer
{
    /// <summary>
    /// Returns macOS version as a macOS version enum.
    /// </summary>
    /// <param name="osVersionAnalyzer"></param>
    /// <returns></returns>
    public static MacOsVersion GetMacOsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer)
    {
        return GetMacOsVersionToEnum(osVersionAnalyzer, DetectMacOsVersion(osVersionAnalyzer));
    }

    /// <summary>
    /// Converts a macOS version to a macOS version enum.
    /// </summary>
    /// <param name="osVersionAnalyzer"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="OperatingSystemDetectionException"></exception>
    /// <exception cref="PlatformNotSupportedException">Thrown if run on Windows or macOS</exception>
    /// <exception cref="Exception"></exception>
    public static MacOsVersion GetMacOsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer, Version input)
    {
        try
        {
            if (new OSAnalyzer().IsMac())
            {
                if (input.Major == 10)
                {
                    switch (input.Minor)
                    {
                        case 0:
                            return MacOsVersion.v10_0_Cheetah;
                        case 1:
                            return MacOsVersion.v10_1_Puma;
                        case 2:
                            return MacOsVersion.v10_2_Jaguar;
                        case 3:
                            return MacOsVersion.v10_3_Panther;
                        case 4:
                            return MacOsVersion.v10_4_Tiger;
                        case 5:
                            return MacOsVersion.v10_5_Leopard;
                        case 6:
                            return MacOsVersion.v10_6_SnowLeopard;
                        case 7:
                            return MacOsVersion.v10_7_Lion;
                        case 8:
                            return MacOsVersion.v10_8_MountainLion;
                        case 9:
                            return MacOsVersion.v10_9_Mavericks;
                        case 10:
                            return MacOsVersion.v10_10_Yosemite;
                        case 11:
                            return MacOsVersion.v10_11_ElCapitan;
                        case 12:
                            return MacOsVersion.v10_12_Sierra;
                        case 13:
                            return MacOsVersion.v10_13_HighSierra;
                        case 14:
                            return MacOsVersion.v10_14_Mojave;
                        case 15:
                            return MacOsVersion.v10_15_Catalina;
                        //This is for compatibility reasons.
                        case 16:
                            return MacOsVersion.v11_BigSur;
                    }
                }

                if (input.Major == 11) return MacOsVersion.v11_BigSur;
                if (input.Major == 12) return MacOsVersion.v12_Monterey;

                throw new OperatingSystemDetectionException();
            }
            else
            {
                throw new PlatformNotSupportedException();
            }
        }
        catch (Exception exception)
        {
            Console.WriteLine(exception);
            throw new Exception(exception.ToString());
        }
    }

    /// <summary>
    /// Detects macOS System Information.
    /// </summary>
    /// <returns></returns>
    public static MacOsSystemInformation DetectMacSystemInformation()
    {
        MacOsSystemInformation macOsSystemInformation = new MacOsSystemInformation();
        macOsSystemInformation.ProcessorType = new OSAnalyzer().GetMacProcessorType();

        macOsSystemInformation.MacOsBuildNumber = DetectMacOsBuildNumber();
        macOsSystemInformation.MacOsVersion = DetectMacOsVersion(new OSVersionAnalyzer());
        macOsSystemInformation.DarwinVersion = DetectDarwinVersion();
        macOsSystemInformation.XnuVersion = DetectXnuVersion();

        return macOsSystemInformation;
    }

    /// <summary>
    /// Detects the Darwin Version on macOS
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Throws an error if it is run on Windows or Linux (or any OS which isn't macOS)</exception>
    public static Version DetectDarwinVersion()
    {
        if (new OSAnalyzer().IsMac())
        {
            var desc = RuntimeInformation.OSDescription;
            var arr = desc.Split(' ');

            int dotCounter = 0;
            
            foreach(var s in arr[1])
            {
                if (s == '.')
                {
                    dotCounter++;
                }
            }

            if (dotCounter == 2)
            {
                arr[1] = arr[1] + ".0";
                return Version.Parse(arr[1]);
            }
            if (dotCounter == 3)
            {
                return Version.Parse(arr[1]);
            }

            throw new PlatformNotSupportedException();
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
    
    /// <summary>
    /// Detects macOS's XNU Version.
    /// </summary>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException">Not supported on platforms other than macOS</exception>
    public static Version DetectXnuVersion()
    {
        if (new OSAnalyzer().IsMac())
        {
            var desc = RuntimeInformation.OSDescription;
            var arr = desc.Split(' ');
        
            for (int index = 0; index < arr.Length; index++)
            {
                if (arr[index].ToLower().StartsWith("root:xnu-"))
                {
                    arr[index] = arr[index].Replace("root:xnu-", String.Empty)
                        .Replace("/RELEASE_X86_64", String.Empty)
                        .Replace("~", ".");

                    //Console.WriteLine(arr[index]);
                
                    return Version.Parse(arr[index]);
                }
            }

            throw new PlatformNotSupportedException();
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
    
    /// <summary>
    /// Detects the MacOsVersion and returns it as a macOS version enum.
    /// </summary>
    /// <param name="osVersionAnalyzer"></param>
    /// <returns></returns>
    /// <exception cref="PlatformNotSupportedException"></exception>
    public static MacOsVersion DetectMacOsVersionEnum(this OSVersionAnalyzer osVersionAnalyzer)
    {
        if (new OSAnalyzer().IsMac())
        {
            Version darwinVersion = DetectDarwinVersion();

            switch (darwinVersion.Major)
            {
                case 1:
                    if (darwinVersion.Minor == 3)
                    {
                        return MacOsVersion.v10_0_Cheetah;
                    }
                    else if (darwinVersion.Minor > 3)
                    {
                        return MacOsVersion.v10_1_Puma;
                    }
                    else
                    {
                        throw new PlatformNotSupportedException();
                    }
                case 5:
                    return MacOsVersion.v10_1_Puma;
                case 6:
                    return MacOsVersion.v10_2_Jaguar;
                case 7:
                    return MacOsVersion.v10_3_Panther;
                case 8:
                    return MacOsVersion.v10_4_Tiger;
                case 9:
                    return MacOsVersion.v10_5_Leopard;
                case 10:
                    return MacOsVersion.v10_6_SnowLeopard;
                case 11:
                    return MacOsVersion.v10_7_Lion;
                case 12:
                    return MacOsVersion.v10_8_MountainLion;
                case 13:
                    return MacOsVersion.v10_9_Mavericks;
                case 14:
                    return MacOsVersion.v10_10_Yosemite;
                case 15:
                    return MacOsVersion.v10_11_ElCapitan;
                case 16:
                    return MacOsVersion.v10_12_Sierra;
                case 17:
                    return MacOsVersion.v10_13_HighSierra;
                case 18:
                    return MacOsVersion.v10_14_Mojave;
                case 19:
                    return MacOsVersion.v10_15_Catalina;
                case 20:
                    return MacOsVersion.v11_BigSur;
                case 21:
                    return MacOsVersion.v12_Monterey;
                default:
                    throw new PlatformNotSupportedException();
            }
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }

    /// <summary>
    /// Detects the macOS version and returns it as a System.Version object.
    /// </summary>
    /// <param name="osVersionAnalyzer"></param>
    /// <returns></returns>
    public static Version DetectMacOsVersion(this OSVersionAnalyzer osVersionAnalyzer)
    {
        var version = GetMacSwVersInfo()[1].Replace("ProductVersion:", String.Empty).Replace(" ", String.Empty);

        int dotCounter = 0;

        foreach (var str in version)
        {
            if (str == '.')
            {
                dotCounter++;
            }
        }

        if (dotCounter == 1)
        {
            version += ".0";
        }
        if (dotCounter == 2)
        {
            version += ".0";
        }
        
        return Version.Parse(version);
    }
    
    /// <summary>
    /// Detects the macOS Build Number.
    /// </summary>
    /// <returns></returns>
    public static string DetectMacOsBuildNumber()
    {
        return GetMacSwVersInfo()[2].ToLower().Replace("BuildVersion:", String.Empty).Replace(" ", String.Empty);
    }

    // ReSharper disable once IdentifierTypo
    /// <summary>
    /// Gets info from sw_vers command on Mac.
    /// </summary>
    /// <returns></returns>
    private static string[] GetMacSwVersInfo()
    {
        // ReSharper disable once StringLiteralTypo
        return new ProcessManager().RunMacCommand("sw_vers").Split(Convert.ToChar(Environment.NewLine));
    }
}