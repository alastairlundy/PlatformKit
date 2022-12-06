using System;

namespace PlatformKit.Internal.Exceptions;

public class WindowsVersionDetectionException : Exception
{
    public WindowsVersionDetectionException() : base("Failed to detect the version of Windows currently running.")
    {
        
    }
}