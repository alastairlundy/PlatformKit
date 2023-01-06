/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
<<<<<<< HEAD
 You can view the PlatformKit Licenses at https://neverspy.tech/platformkit-commercial-license or in the file PlatformKit_Commercial_License.txt

 To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
=======
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
>>>>>>> main
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;

namespace PlatformKit.Mac;

/// <summary>
/// A class to represent basic macOS System Information.
/// </summary>
public class MacOsSystemInformation
{
    public Version MacOsVersion
    {
        get
        {
           return macOsAnalyzer.DetectMacOsVersion();
        }
    }

    public Version DarwinVersion
    {
        get
        {
            return macOsAnalyzer.DetectDarwinVersion();
        }
    }

    public Version XnuVersion
    {
        get
        {
           return macOsAnalyzer.DetectXnuVersion();
        }
    }

    public string MacOsBuildNumber
    {
        get
        {
           return macOsAnalyzer.DetectMacOsBuildNumber();
        }
    }

    public MacProcessorType ProcessorType
    {
        get
        {
           return macOsAnalyzer.GetMacProcessorType();
        }
    }

    protected MacOSAnalyzer macOsAnalyzer;
    
    public MacOsSystemInformation()
    {
        macOsAnalyzer = new MacOSAnalyzer();
    }
}