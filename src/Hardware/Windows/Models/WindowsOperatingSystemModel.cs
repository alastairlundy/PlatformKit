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

using PlatformKit.Windows;

namespace PlatformKit.Hardware.Windows;

public class WindowsOperatingSystemModel : AbstractOperatingSystemModel
{
    public new bool IsProprietary => true;

    public WindowsEdition Edition { get; set; }
    public string ReleaseName { get; set; }
    
    public string ReleaseChannel { get; set; }
    public int BuildNumber { get; set; }
    public string ProductId { get; set; }
}