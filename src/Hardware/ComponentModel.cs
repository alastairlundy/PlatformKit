/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
 */

using System;
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Hardware.Common
{
    public abstract class ComponentModel : ManufactureInformation
    {
        protected WindowsAnalyzer _windowsAnalyzer;
        protected MacOSAnalyzer _macOsAnalyzer;

        public ComponentModel()
        {
            _windowsAnalyzer = new WindowsAnalyzer();
            _macOsAnalyzer = new MacOSAnalyzer();
        }
        
        public bool CanDetectInformation { get; set; }

        protected virtual void DetectWindows()
        {
            throw new NotImplementedException();
        }

        protected virtual void DetectMac()
        {
            throw new NotImplementedException();
        }

        protected virtual void DetectLinux()
        {
            throw new NotImplementedException();
        }

        public void Detect()
        {
            if (OSAnalyzer.IsWindows())
            {
                DetectWindows();
            }
            else if (OSAnalyzer.IsMac())
            {
                 DetectMac();
            }
            else if (OSAnalyzer.IsLinux())
            {
                 DetectLinux();
            }

            throw new PlatformNotSupportedException();
        }
    }
}