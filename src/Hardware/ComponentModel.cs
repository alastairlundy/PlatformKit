/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023
    Copyright (c) NeverSpyTech Limited 2022

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
using PlatformKit.Mac;
using PlatformKit.Windows;

namespace PlatformKit.Hardware
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