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

using AluminiumTech.DevKit.PlatformKit.Exceptions;
using AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Mac;

namespace AluminiumTech.DevKit.PlatformKit.Analyzers.PlatformSpecifics;

// ReSharper disable once InconsistentNaming
public static class MacOSVersionAnalyzer
{
           /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="OperatingSystemVersionDetectionException"></exception>
        /// <exception cref="PlatformNotSupportedException"></exception>
        /// <exception cref="Exception"></exception>
        public static MacOsVersion GetMacOsVersionToEnum(this OSVersionAnalyzer osVersionAnalyzer, Version input)
        {
            try
            {
                if (new PlatformManager().IsMac())
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
                    if (input.Major == 12) return MacOsVersion.v12_Monterrey;

                    throw new OperatingSystemVersionDetectionException();
                }
                else
                {
                    throw new PlatformNotSupportedException();
                }
            }
            catch(Exception exception)
            {
                Console.WriteLine(exception);
                throw new Exception(exception.ToString());
            }
        }
    
    /*
     /// <summary>
     /// 
     /// </summary>
     /// <returns></returns>
     /// <exception cref="PlatformNotSupportedException">Throws an error if called on an OS besides macOS.</exception>
     /// <exception cref="Exception"></exception>
              public Version DetectMacOsVersion()
              {
                  try
                  {
                      if (_platformManager.IsMac())
                      {
                          var description = System.Runtime.InteropServices.RuntimeInformation.OSDescription;
                          description = description.Replace("", string.Empty);

                          string[] descArray = description.Split(' ');
                          
                          foreach (var d in descArray)
                          {
                              if (!d.Contains(string.Empty))
                              {
                                  description = d;
                              }
                          }
                      
                          return Version.Parse(description);
                      }
                      else
                      {
                          throw new PlatformNotSupportedException();
                      }
                  }
          catch(Exception exception)
          {
              throw new Exception(exception.ToString());
          }
      }
      */
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
}