/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */


using System.Runtime.Versioning;
using System.Threading.Tasks;

namespace PlatformKit.Linux.Abstractions;

public interface ILinuxOsReleaseProvider
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="propertyName"></param>
    /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
     Task<string> GetPropertyValueAsync(string propertyName);
    
     /// <summary>
     /// 
     /// </summary>
     /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
     Task<LinuxOsReleaseInfo> GetReleaseInfoAsync();

     /// <summary>
     /// 
     /// </summary>
     /// <returns></returns>
#if NET5_0_OR_GREATER
    [SupportedOSPlatform("linux")]
#endif
     Task<LinuxDistroBase> GetDistroBaseAsync();
}