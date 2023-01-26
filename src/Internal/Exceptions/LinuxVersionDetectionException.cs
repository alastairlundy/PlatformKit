using System;

namespace PlatformKit.Internal.Exceptions;

public class LinuxVersionDetectionException : Exception
{
    public LinuxVersionDetectionException() : base("Failed to detect the version of Linux running on this computer.")
    {
        
    }
}