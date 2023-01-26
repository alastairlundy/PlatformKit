using System;

namespace PlatformKit.Internal.Exceptions;

public class MacOsVersionDetectionException : Exception
{
    public MacOsVersionDetectionException() : base("Failed to detect the version of macOS running on this computer.")
    {
        
    }
}