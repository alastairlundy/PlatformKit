/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023

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

// ReSharper disable All

using System;

namespace PlatformKit.Mac
{
    /// <summary>
    /// An enum representing macOS versions.
    /// </summary>
    public enum MacOsVersion
    {
        v10_9_Mavericks,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_10_Yosemite,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_11_ElCapitan,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_12_Sierra,
        //[Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_13_HighSierra,
        v10_14_Mojave,
        v10_15_Catalina,
        /// <summary>
        /// First version of macOS to move away from Major version 10 in [Major].[Minor].[Update]
        /// First version to support Apple Silicon on M1 powered Macs and newer.
        /// </summary>
        v11_BigSur,
        v12_Monterey,
        v13_Ventura,
        NotDetected,
        NotSupported,
    }
}