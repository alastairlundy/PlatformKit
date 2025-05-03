/*
    PlatformKit
    Copyright (c) 2020-2025 Alastair Lundy

    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;

using Microsoft.Extensions.DependencyInjection;

namespace PlatformKit.Extensions;

public static class DependencyInjectionExtensions
{
    /// <summary>
    /// 
    /// </summary>
    /// <param name="services"></param>
    /// <param name="serviceLifetime"></param>
    /// <returns></returns>
    public static IServiceCollection UsePlatformKit(this IServiceCollection services, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
    {
        services = services.Add(typeof(IUnixPlatformProvider), typeof(UnixPlatformProvider), serviceLifetime);
        services = services.Add(typeof(IDarwinPlatformProvider), typeof(DarwinPlatformProvider), serviceLifetime);
        services = services.Add(typeof(IAndroidPlatformProvider), typeof(AndroidPlatformProvider), serviceLifetime);
        services = services.Add(typeof(IWindowsPlatformProvider), typeof(WindowsPlatformProvider), serviceLifetime);
        
        services = services.Add(typeof(IMacSystemProfilerInfoProvider), typeof(MacSystemProfilerInfoProvider), serviceLifetime);
        
        services = services.Add(typeof(ISteamOsInfoProvider), typeof(SteamOsInfoProvider), serviceLifetime);
        services = services.Add(typeof(ILinuxOsReleaseProvider), typeof(LinuxOsReleaseProvider), serviceLifetime);
        
        services = services.Add(typeof(IWindowsSystemInfoProvider), typeof(WindowsSystemInfoProvider), serviceLifetime);
        services = services.Add(typeof(IWinRegistrySearcher), typeof(WinRegistrySearcher), serviceLifetime);
        
        return services;
    }

    private static IServiceCollection Add(this IServiceCollection services, Type serviceType, Type implementationType, ServiceLifetime serviceLifetime)
    {
        switch (serviceLifetime)
        {
            case ServiceLifetime.Singleton:
                services = services.AddSingleton(serviceType, implementationType);
                break;
            case ServiceLifetime.Scoped:
                services.AddScoped(serviceType, implementationType);
                break;
            case ServiceLifetime.Transient:
                services.AddTransient(serviceType, implementationType);
                break;
            default:
                services.AddSingleton(serviceType, implementationType);
                break;
        }
        
        return services;
    }
    
    
}