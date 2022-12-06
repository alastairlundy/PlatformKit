using System;

namespace PlatformKit.Internal.Exceptions;

public class WindowsEditionDetectionException : Exception
{
    public WindowsEditionDetectionException() : base("Failed to detect the Edition of Windows running on this computer.")
    {
        
    }
}