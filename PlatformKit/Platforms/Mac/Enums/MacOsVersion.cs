/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
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
        /// First version to support Apple Silicon Macs.
        /// </summary>
        v11_BigSur,
        v12_Monterey,
        v13_Ventura,
        v14_Sonoma,
        NotDetected,
        NotSupported,
    }
}