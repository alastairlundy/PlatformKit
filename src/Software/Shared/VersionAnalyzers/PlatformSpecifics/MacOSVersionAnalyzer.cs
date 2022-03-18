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
public static class MacOSVersionAnalyzer
{
    public static MacOsVersion GetMacOsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer)
    {
        return GetMacOsVersionToEnum(osVersionAnalyzer, DetectMacOsVersion_HardCoded(osVersionAnalyzer));
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="osVersionAnalyzer"></param>
    /// <param name="input"></param>
    /// <returns></returns>
    /// <exception cref="OperatingSystemVersionDetectionException"></exception>
    /// <exception cref="PlatformNotSupportedException"></exception>
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

                throw new OperatingSystemVersionDetectionException();
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
    
/*
    // ReSharper disable once InconsistentNaming
    public static bool IsAtLeastMacOSVersion(this OSVersionAnalyzer osVersionAnalyzer, MacOsVersion macOsVersion)
    {
        var detected_mac = DetectMacOSVersion();
        
                /// <summary>
            var detected = versionAnalyzer.DetectOSVersion();

            var expected = versionAnalyzer.GetMacVersionFromEnum(macOsVersion);
            
            return (detected.Build >= expected.Build);
        }
    }
    */

    public static MacOsSystemInformation DetectMacSystemInformation()
    {
        MacOsSystemInformation macOsSystemInformation = new MacOsSystemInformation();
        macOsSystemInformation.ProcessorType = new OSAnalyzer().GetMacProcessorType();
        
        macOsSystemInformation.MacOsVersion = DetectMacOsVersion_HardCoded(new OSVersionAnalyzer());
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
    /// Detects the MacOsVersion
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

    public static Version DetectMacOsVersion_HardCoded(this OSVersionAnalyzer osVersionAnalyzer)
    {
        var xnuversion = DetectXnuVersion();
        
        var darwinVersion = DetectDarwinVersion();
        var minorVersion = darwinVersion.Minor;
        
        switch (darwinVersion.Major){
            case 1:
                switch (minorVersion)
                {
                    case 1:
                        throw new PlatformNotSupportedException();
                    case 2:
                        throw new PlatformNotSupportedException();
                    case 3:
                        return new Version(10, 0, 0, 0);
                    case 4:
                        return new Version(10, 1, 0, 0);
                    default:
                        throw new PlatformNotSupportedException();
                }
            case 5:
                return new Version(10, 1, minorVersion, 0);
            case 6:
                return new Version(10, 2, minorVersion, 0);
            case 7:
                return new Version(10, 3, minorVersion, 0);
            case 8:
                return new Version(10, 4, minorVersion, 0);
            case 9:
                return new Version(10, 5, minorVersion, 0);
            case 10:
                return new Version(10, 6, minorVersion, 0);
            case 11:
                switch (minorVersion)
                {
                        case 4:
                           if (darwinVersion.Build == 0)
                           {
                               return new Version(10, 7, 4, 0);
                           }
                           else
                           {
                               return new Version(10, 7, 5, 0);
                           }
                        default:
                            if (minorVersion < 4 && minorVersion >= 0)
                            {
                                return new Version(10, 7, minorVersion, 0);
                            }
                            else
                            {
                                throw new PlatformNotSupportedException();
                            }
                }
            case 12:
                switch (minorVersion)
                    {
                        case 6:
                            return new Version(10, 8, 5, 0);
                        default:
                            if (minorVersion < 6 && minorVersion >= 0)
                            {
                                return new Version(10, 8, minorVersion, 0);
                            }
                            else
                            {
                                throw new PlatformNotSupportedException();
                            } 
                    }
            case 13:
                switch (minorVersion)
                {
                        case 0:
                            return new Version(10, 9, 0, 0);
                        default:
                            return new Version(10, 9, minorVersion + 1, 0);
                }
            case 14:
                switch (minorVersion)
                {
                        case 0:
                            return new Version(10, 10, 0, 0);
                        case 1:
                            return new Version(10, 10, 2, 0);
                        default:
                            return new Version(10, 10, minorVersion, 0);
                }
                case 15:
                    switch (minorVersion)
                    {
                        case 0:
                            return new Version(10, 11, 0, 0);
                        default:
                            return new Version(10, 11, minorVersion, 0);
                    }
                case 16:
                    switch (minorVersion)
                    {
                        case 0:
                            return new Version(10, 12, 0, 0);
                        case 1:
                            return new Version(10, 12, 1, 0);
                        default:
                            return new Version(10, 12, minorVersion - 1, 0);
                    }
            case 17:
                if (minorVersion == 0)
                    return new Version(10, 13, 0, 0);
                else
                    return new Version(10, 13, minorVersion - 1, 0);
            case 18:
                switch (minorVersion)
                {
                    case 0:
                        return new Version(10, 14, 0, 0);
                    case 2:
                        if (xnuversion.Minor == 221)
                        {
                            return new Version(10, 14, 1, 0);
                        }
                        else if (xnuversion.Minor == 231)
                        {
                            return new Version(10, 14, 2, 0);
                        }
                        else
                        {
                            return new Version(10, 14, 3, 0);
                        }
                    default:
                        return new Version(10, 14, minorVersion - 1, 0);
                }
            case 19:
                switch (minorVersion)
                {
                    case 0:
                        if (xnuversion.Equals(new Version(6153, 41, 3, 29)))
                        {
                            return new Version(10, 15, 1, 0);
                        }
                        else
                        {
                            return new Version(10, 15, 0, 0);
                        }
                    case 6:
                        if (xnuversion.Minor == 141 && xnuversion.Build == 1)
                        {
                            return new Version(10, 15, 6, 0);
                        }
                        else if(xnuversion.Minor == 141 && xnuversion.Build > 1)
                        {
                            return new Version(10, 15, 7, 0);
                        }
                        else
                        {
                            throw new PlatformNotSupportedException();
                        }
                    default:
                        return new Version(10, 15, minorVersion, 0);
                }
            case 20:
                switch (minorVersion)
                {
                    case 1:
                        if (xnuversion.Minor == 41)
                        {
                            return new Version(11, 0, 0, 0);
                        }
                        else if (xnuversion.Minor == 50)
                        {
                            return new Version(11, 0, 1, 0);
                        }
                        else
                        {
                            throw new PlatformNotSupportedException();
                        }
                    case 2:
                        return new Version(11, 1, 0, 0);
                    case 3:
                        //TODO Investigate proper version detection here when we can detect Build Numbers
                        //For now report as 11.2
                        return new Version(11, 2, 0, 0);
                    case 4:
                        return new Version(11, 3, xnuversion.Build - 1, 0);
                    case 5:
                        return new Version(11, 4, 0, 0);
                    case 6:
                        switch (xnuversion.Build)
                        {
                            case 2:
                                //TODO add proper detection here when we can detect build numbers
                                return new Version(11, 5, 0, 0);
                            case 6:
                                return new Version(11, 6, 0, 0);
                            case 8:
                                return new Version(11, 6, 1, 0);
                            case 14:
                                return new Version(11, 6, 2, 0);
                            case 19:
                                //TODO add proper detection here when we can detect build numbers
                                return new Version(11, 6, 3, 0);
                            case 26:
                                return new Version(11, 6, 5, 0);
                            default:
                                throw new PlatformNotSupportedException();
                        }
                    default:
                            throw new PlatformNotSupportedException();
                }
            case 21:
                switch (xnuversion.Minor)
                {
                    case 30:
                        return new Version(12, 0, 0, 0);
                    case 41:
                        return new Version(12, 0, 1, 0);
                    case 61:
                        return new Version(12, 1, 0, 0);
                    case 80:
                        //TODO add proper detection here when we can detect build numbers (12.2.0 and 12.2.1 share same XNU version)
                        return new Version(12, 2, 0, 0);
                    case 101:
                        return new Version(12, 3, 0, 0);
                    default:
                        throw new PlatformNotSupportedException();
                }
            default:
                    throw new PlatformNotSupportedException();
        }
    }
}