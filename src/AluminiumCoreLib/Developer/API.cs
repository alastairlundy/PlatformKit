/*
MIT License

Copyright (c) 2018 AluminiumTech

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
using AluminiumCoreLib.Utilities;
using System;
using System.Collections.Generic;

namespace AluminiumCoreLib.Developer{
    /// <summary>
    /// A class to help deal with differnt APIs
    /// </summary>
    public class API : Feature{
        public List<Feature> FeatureList { get; set; }
        
        /// <summary>
        /// 
        /// </summary>
        public API() {
            MinimumSupportedAPIVersion = new System.Version();
            RecommendedAPIVersion = new System.Version();
            FeatureList = new List<Feature>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minSupportedAPI"></param>
        public API(System.Version minSupportedAPI){
            MinimumSupportedAPIVersion = minSupportedAPI ?? throw new ArgumentNullException(nameof(minSupportedAPI));
            RecommendedAPIVersion = new System.Version();
            FeatureList = new List<Feature>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minSupportedAPI"></param>
        /// <param name="recommendedAPI"></param>
        public API(System.Version minSupportedAPI, System.Version recommendedAPI){
            MinimumSupportedAPIVersion = minSupportedAPI ?? throw new ArgumentNullException(nameof(minSupportedAPI));
            RecommendedAPIVersion = recommendedAPI ?? throw new ArgumentNullException(nameof(recommendedAPI));
            FeatureList = new List<Feature>();
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="minSupportedAPI"></param>
        /// <param name="recommendedAPI"></param>
        /// <param name="featureList"></param>
        public API(System.Version minSupportedAPI, System.Version recommendedAPI, List<Feature> featureList){
            MinimumSupportedAPIVersion = minSupportedAPI ?? throw new ArgumentNullException(nameof(minSupportedAPI));
            RecommendedAPIVersion = recommendedAPI ?? throw new ArgumentNullException(nameof(recommendedAPI));
            FeatureList = featureList ?? throw new ArgumentNullException(nameof(featureList));
        }       
    }
}