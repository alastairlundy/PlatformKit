/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;

namespace PlatformKit.Internal.Deprecation
{
    internal static class DeprecationMessages
    {
        private const string V5 = "v5.0.0";
        
        private const string FeatureDeprecation = "This feature is deprecated and will be removed";
        
        internal const string FutureDeprecation = FeatureDeprecation + " in a future version.";
        
        internal const string DeprecationV5 = FeatureDeprecation + " in " + V5;
    }
}
