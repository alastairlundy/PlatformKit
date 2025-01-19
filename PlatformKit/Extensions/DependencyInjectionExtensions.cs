/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using Microsoft.Extensions.DependencyInjection;

using PlatformKit.Providers;

using PlatformKit.Specializations.Linux;
using PlatformKit.Specializations.Mac;
using PlatformKit.Specializations.Windows;
using PlatformKit.Specializations.Windows.Abstractions;

using PlatformKit.Specifics.Abstractions;

namespace PlatformKit.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <returns></returns>
    public static IServiceCollection UsePlatformKit(this IServiceCollection services)
    {
        services = services.AddSingleton(typeof(IAndroidPlatformProvider), typeof(AndroidPlatformProvider));
        services = services.AddSingleton(typeof(IWindowsPlatformProvider), typeof(WindowsPlatformProvider));
        
        services = services.AddSingleton(typeof(IMacSystemProfilerInfoProvider), typeof(MacSystemProfilerInfoProvider));
        
        services = services.AddSingleton(typeof(ISteamOsInfoProvider), typeof(SteamOsInfoProvider));
        services = services.AddSingleton(typeof(ILinuxOsReleaseProvider), typeof(LinuxOsReleaseProvider));
        
        services = services.AddSingleton(typeof(IWindowsSystemInfoProvider), typeof(WindowsSystemInfoProvider));
        services = services.AddSingleton(typeof(IWMISearcher), typeof(WMISearcher));
        services = services.AddSingleton(typeof(IWinRegistrySearcher), typeof(WinRegistrySearcher));
        
        return services;
    }
    
    
}