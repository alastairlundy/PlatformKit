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
using System;

namespace AluminiumCoreLib.Utilities{
    /// <summary>
    /// Compare different versions.
    /// </summary>
    public class VersionComparison{

        /// <summary>
        /// Compare a NewVersion and the InstalledVersion. 
        /// Returns a type of Version.
        /// </summary>
        /// <param name="NewVersion"></param>
        /// <param name="InstalledVersion"></param>
        public bool CompareVersion(System.Version NewVersion, System.Version InstalledVersion) {
            if (Compare(NewVersion.Revision, InstalledVersion.Revision)) {
                return true;
            }
            else if (Compare(NewVersion.Build, InstalledVersion.Build))
            {
                return true;
            }
            else if (Compare(NewVersion.Minor, InstalledVersion.Minor))
            {
               return true;
            }
            else if (Compare(NewVersion.Major, InstalledVersion.Major))
            {
              return true;
            }
            else{
                return false;
            }
        }
        /// <summary>
        /// If the NewVersion is higher than the old version, return true.
        /// </summary>
        /// <param name="NewVersion"></param>
        /// <param name="InstalledVersion"></param>
        /// <returns></returns>
        public bool Compare(int NewVersion, int InstalledVersion){
            if(NewVersion < InstalledVersion){
                return true;
            }
            return false;
        }
    }
}