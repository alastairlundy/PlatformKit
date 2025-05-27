/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Runtime.InteropServices;
// ReSharper disable CheckNamespace

// ReSharper disable MemberCanBePrivate.Global

namespace PlatformKit.Platforms
{
    /// <summary>
    /// 
    /// </summary>
    public class Platform : ICloneable, IEquatable<Platform>
    {
        /// <summary>
        /// 
        /// </summary>
        public string Name { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Version OperatingSystemVersion { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public Version KernelVersion { get; }

        /// <summary>
        /// 
        /// </summary>
        public PlatformFamily Family { get; }
        
        /// <summary>
        /// 
        /// </summary>
        public string BuildNumber { get; }
        
        public Architecture ProcessorArchitecture { get; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="platformName"></param>
        /// <param name="operatingSystemVersion"></param>
        /// <param name="kernelVersion"></param>
        /// <param name="family"></param>
        /// <param name="buildNumber"></param>
        /// <param name="processorArchitecture"></param>
        public Platform(string platformName, Version operatingSystemVersion, Version kernelVersion,
            PlatformFamily family, string buildNumber,
            Architecture processorArchitecture)
        {
            Name = platformName;
            OperatingSystemVersion = operatingSystemVersion;
            KernelVersion = kernelVersion;
            Family = family;
            BuildNumber = buildNumber;
            ProcessorArchitecture = processorArchitecture;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public object Clone()
        {
            return new Platform(Name, OperatingSystemVersion, KernelVersion, Family, BuildNumber, ProcessorArchitecture);
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(Platform? other)
        {
            if (other is null)
            {
                return false;
            }

            return OperatingSystemVersion.Equals(other.OperatingSystemVersion) 
                   && KernelVersion.Equals(other.KernelVersion)
                   && Family.Equals(other.Family)
                   && Name.Equals(other.Name) 
                   && BuildNumber.Equals(other.BuildNumber)
                   && ProcessorArchitecture.Equals(other.ProcessorArchitecture);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object? obj)
        {
            return obj is Platform other && Equals(other);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return HashCode.Combine(Name, OperatingSystemVersion, KernelVersion, (int)Family, BuildNumber);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public static bool operator ==(Platform left, Platform right)
        {
            if (left is null || right is null)
            {
                throw new NullReferenceException($"Cannot compare a null platform. Platform {(left is null ? nameof(left) : nameof(right))} is null");
            }
            
            return left.Equals(right);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        public static bool operator !=(Platform left, Platform right)
        {
            if (left is null || right is null)
            {
                throw new NullReferenceException($"Cannot compare a null platform. Platform {(left is null ? nameof(left) : nameof(right))} is null");
            }
            
            return !(left == right);
        }
    }
}