/* MIT License

Copyright (c) 2018-2021 AluminiumTech

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

namespace AluminiumTech.DevKit.PlatformKit.Deprecation
{
    public static class DeprecationMessages
    {
        private const string V2_5 = "v2.5.0";
        private const string V2_6 = "v2.6.0";
        private const string V2_7 = "v2.7.0";
        private const string V2_8 = "v2.8.0";
        private const string V2_9 = "v2.9.0";
        private const string V2_10 = "v2.10.0";
        
        private const string V3 = "v3.0.0";
        private const string V4 = "v4.0.0";
        private const string V5 = "v5.0.0";

        private const string CodeDeprecation = "This code is deprecated and will be removed";
        private const string FeatureDeprecation = "This feature is deprecated and will be removed";
        
        public const string FutureDeprecation = CodeDeprecation + " in a future version.";

        public const string DeprecationV2_5 = CodeDeprecation + " in " + V2_5;
        
        public const string DeprecationV2_6 = CodeDeprecation + " in " + V2_6;
        public const string DeprecationV2_7 = CodeDeprecation + " in " + V2_7;
        public const string DeprecationV2_8 = CodeDeprecation + " in " + V2_8;
        public const string DeprecationV2_9 = CodeDeprecation + " in " + V2_9;
        public const string DeprecationV2_10 = CodeDeprecation + " in " + V2_10;
        
        public const string DeprecationV3 = CodeDeprecation + " in " + V3;
        public const string DeprecationV4 = CodeDeprecation + " in " + V4;
        public const string DeprecationV5 = CodeDeprecation + " in " + V5;
    }
}