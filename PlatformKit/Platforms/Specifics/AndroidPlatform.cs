/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Runtime.InteropServices;

namespace PlatformKit.Specifics
{
    public class AndroidPlatform : Platform, ICloneable, IEquatable<AndroidPlatform>
    {
        public string DeviceName { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public int SdkLevel { get; }
        
        /// <summary>
        /// The Android version code name.
        /// </summary>
        public string VersionCodeName { get; }
        
        public AndroidPlatform(string name, Version operatingSystemVersion, Version kernelVersion, int sdkLevel,
            string versionCodeName, string buildNumber, string deviceName, Architecture processorArchitecture) : base(name,
            operatingSystemVersion, kernelVersion, PlatformFamily.Android, buildNumber, processorArchitecture)
        {
            SdkLevel = sdkLevel;
            VersionCodeName = versionCodeName;
            DeviceName = deviceName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(AndroidPlatform? other)
        {
            if (other == null)
            {
                return false;
            }
            
            return SdkLevel.Equals(other.SdkLevel) &&
                   VersionCodeName.Equals(other.VersionCodeName) &&
                   Name.Equals(other.Name) &&
                   OperatingSystemVersion.Equals(other.OperatingSystemVersion) &&
                   KernelVersion.Equals(other.KernelVersion) &&
                   BuildNumber.Equals(other.BuildNumber) &&
                   Family.Equals(other.Family);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (obj.GetType() != GetType())
            {
                return false;
            }

            return Equals((AndroidPlatform)obj);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(SdkLevel, VersionCodeName,
                Name, OperatingSystemVersion, KernelVersion, Family, BuildNumber, DeviceName);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public new object Clone()
        {
            return new AndroidPlatform(Name, OperatingSystemVersion,
                KernelVersion, SdkLevel, VersionCodeName, BuildNumber, DeviceName, ProcessorArchitecture);
        }
    }
}