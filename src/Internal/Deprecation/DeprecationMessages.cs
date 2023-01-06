/*
 PlatformKit is dual-licensed under the GPLv3 and the PlatformKit Licenses.
 
 You may choose to use PlatformKit under either license so long as you abide by the respective license's terms and restrictions.
 
 You can view the GPLv3 in the file GPLv3_License.md .
<<<<<<< HEAD
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
=======
 You can view the PlatformKit Licenses at https://neverspy.tech
  
  To use PlatformKit under a commercial license you must purchase a license from https://neverspy.tech
>>>>>>> main
 
 Copyright (c) AluminiumTech 2018-2022
 Copyright (c) NeverSpy Tech Limited 2022
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