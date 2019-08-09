/*
MIT License

Copyright (c) 2018-2019 AluminiumTech

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

namespace AluminiumCoreLib{
    /// <summary>
    /// 
    /// </summary>
    public class Version{
        /**
     *
     * The major version in Major.Minor.Build.Revision
     */
        private int major;
        /// <summary>
        /// The minor version in Major.Minor.Build.Revision
        /// </summary>
        private int minor;
        /// <summary>
        /// The build version in Major.Minor.Build.Revision
        /// </summary>
        private int build;
        /// <summary>
        /// The revision version in Major.Minor.Build.Revision
        /// </summary>
        private int revision;
        /// <summary>
        /// An optional suffix for a version such as "alpha2"
        /// </summary>
        private string suffix;

        public Version(){
            //Assign default values
            major = 1;
            minor = 0;
            build = 0;
            revision = 0;
            suffix = "";
        }
        public Version(int major, int minor, int build, int revision){
            this.major = major;
            this.minor = minor;
            this.build = build;
            this.revision = revision;
            suffix = "";
        }
        public Version(string major, string minor, string build, string revision){
            this.major = int.Parse(major);
            this.minor = int.Parse(minor);
            this.build = int.Parse(build);
            this.revision = int.Parse(revision);
            suffix = "";
        }
        public Version(string version){
            char[] dot = {'.'};
            string[] versions = version.Split(dot, 5);

            this.major = int.Parse(versions[0]);
            this.minor = int.Parse(versions[1]);
            this.build = int.Parse(versions[2]);
            this.revision = int.Parse(versions[3]);
            this.suffix = versions[4];
        }

        /// <summary>
        /// Gets the minor version
        /// </summary>
        /// <returns></returns>
        public int GetMinorVersion(){
            return minor;
        }
        /// <summary>
        /// Sets the minor version
        /// </summary>
        /// <returns></returns>
        public void SetMinorVersion(int minor){
            this.minor = minor;
        }
        /// <summary>
        /// Gets the major version
        /// </summary>
        /// <returns></returns>
        public int GetMajorVersion(){
            return major;
        }
        /// <summary>
        /// Sets the major version
        /// </summary>
        /// <returns></returns>
        public void SetMajorVersion(int major){
            this.major = major;
        }
        /// <summary>
        /// Gets the revision version
        /// </summary>
        /// <returns></returns>
        public int GetRevisionVersion(){
            return revision;
        }
        /// <summary>
        /// Sets the revision version
        /// </summary>
        /// <returns></returns>
        public void SetRevisionVersion(int revision){
            this.revision = revision;
        }
        /// <summary>
        /// Gets the build version
        /// </summary>
        /// <returns></returns>
        public int GetBuildVersion(){
            return build;
        }
        /// <summary>
        /// Sets the build version
        /// </summary>
        /// <returns></returns>
        public void SetBuildVersion(int build){
            this.build = build;
        }
        /// <summary>
        /// Sets the String suffix of a version.
        /// </summary>
        /// <returns></returns>
        public void SetSuffix(string suffix){
            this.suffix = suffix;
        }
        /// <summary>
        /// Return the String suffix of a version.
        /// </summary>
        /// <returns></returns>
        public string GetSuffix(){
            return suffix;
        }
        /// <summary>
        /// Returns the Version as a String in the format of Major.Minor.Build.Revision
        /// </summary>
        /// <returns></returns>
        public override string ToString(){
            if (suffix.Length == 0){
                return major + "." + minor + "." + build + "." + revision;
            }
            else{
                return major + "." + minor + "." + build + "." + revision + "." + suffix;
            }
        }
        /// <summary>
        /// Returns a System.Version object.
        /// </summary>
        /// <returns></returns>
        public System.Version ToSystemVersion(){
            return new System.Version(major, minor, build, revision);
        }

        /// <summary>
        /// Compare to another version and returns if they are equal or not.
        /// </summary>
        /// <param name="version"></param>
        /// <returns></returns>
        public bool Equals(Version version){
            return (Compare(this.GetRevisionVersion(), version.GetRevisionVersion()) && Compare(this.GetBuildVersion(), version.GetBuildVersion()) && Compare(this.GetMinorVersion(), version.GetMinorVersion()) && Compare(this.GetMajorVersion(), version.GetMajorVersion()));
        }
        /// <summary>
        /// Compare to a System.Version and returns if they are equal or not.
        /// </summary>
        /// <param name="version">A System.Version object</param>
        /// <returns></returns>
        public bool Equals(System.Version version){
            return (Compare(this.GetRevisionVersion(), version.Revision) && Compare(this.GetBuildVersion(), version.Build) && Compare(this.GetMinorVersion(), version.Minor) && Compare(this.GetMajorVersion(), version.Major));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="NewVersion"></param>
        /// <param name="InstalledVersion"></param>
        /// <returns></returns>
        private bool CompareIsNewer(int NewVersion, int InstalledVersion){
            return (NewVersion < InstalledVersion);
        }
    }
}