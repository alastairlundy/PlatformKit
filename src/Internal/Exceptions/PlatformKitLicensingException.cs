using System;

namespace PlatformKit.Internal.Exceptions;

internal class PlatformKitLicensingException : Exception
{

    internal PlatformKitLicensingException() : base("PlatformKit's License Context has not been set. To use PlatformKit please set the appropriate license context.")
    {
        
    }
}