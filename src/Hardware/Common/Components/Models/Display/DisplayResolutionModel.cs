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

namespace PlatformKit.Hardware.Components
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