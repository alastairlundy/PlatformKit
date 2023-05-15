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

namespace PlatformKit.Internal.Deprecation
{
    internal static class DeprecationMessages
    {
        private const string V4 = "v4.0.0";
        private const string V5 = "v5.0.0";
        
        private const string FeatureDeprecation = "This feature is deprecated and will be removed";
        
        internal const string FutureDeprecation = FeatureDeprecation + " in a future version.";
        
        internal const string DeprecationV4 = FeatureDeprecation + " in " + V4;
        internal const string DeprecationV5 = FeatureDeprecation + " in " + V5;
    }
}