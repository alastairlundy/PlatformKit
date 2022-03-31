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

// ReSharper disable All

using System;

namespace AluminiumTech.DevKit.PlatformKit.PlatformSpecifics.Mac
{
    public enum MacOsVersion
    {
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        v10_0_Cheetah,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        v10_1_Puma,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        v10_2_Jaguar,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        v10_3_Panther,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        v10_4_Tiger,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV3)]
        v10_5_Leopard,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        v10_6_SnowLeopard,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        v10_7_Lion,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV4)]
        v10_8_MountainLion,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_9_Mavericks,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_10_Yosemite,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_11_ElCapitan,
        [Obsolete(Deprecation.DeprecationMessages.DeprecationV5)]
        v10_12_Sierra,
        v10_13_HighSierra,
        v10_14_Mojave,
        v10_15_Catalina,
        /// <summary>
        /// First version of macOS to move away from Major version 10 in [Major].[Minor].[Update]
        /// First version to support Apple Silicon on M1 powered Macs and newer.
        /// </summary>
        v11_BigSur,
        v12_Monterey,
        NotDetected,
        NotSupported,
    }
}