using System;
using PlatformKit.Core.Internal.Localizations;

namespace PlatformKit.Core.Exceptions;

public class AppNotInstalledException : Exception
{

    public AppNotInstalledException(string appName) : base(Resources.Exceptions_AppModels_NotInstalled.Replace("{x}", appName))
    {
        
    }
}