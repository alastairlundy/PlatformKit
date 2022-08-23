
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