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

namespace PlatformKit.Hardware.Common
{
    public class DisplayResolutionModel
    {
        public int VerticalResolutionPx { get; set; }
        public int HorizontalResolutionPx { get; set; }

        public int TotalPixelsPx => VerticalResolutionPx * HorizontalResolutionPx;
        
        public double DisplaySizeInches { get; set; }
        
        public bool IsHiDpi { get; set; }

        public double PixelsPerInch => DiagonalResolutionPx / DisplaySizeInches;
        
        public double DiagonalResolutionPx
        {
            get
            {
                try
                {
                    return Math.Sqrt(Convert.ToDouble((VerticalResolutionPx * VerticalResolutionPx) +
                                                      (HorizontalResolutionPx * HorizontalResolutionPx)));
                }
                catch
                {
                    throw new ArgumentNullException();
                }
               
            }
        }

        public double DotPitchMillimeters => (DisplaySizeInches / DiagonalResolutionPx) * (25.4 / DisplaySizeInches);
    }
}