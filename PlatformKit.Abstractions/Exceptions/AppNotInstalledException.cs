using System;

using PlatformKit.Abstractions.Localizations;

namespace PlatformKit.Abstractions.Exceptions;

public class AppNotInstalledException : Exception
{

    public AppNotInstalledException(string appName) : base(Resources.Exceptions_AppModels_NotInstalled.Replace("{x}", appName))
    {
        
    }
}