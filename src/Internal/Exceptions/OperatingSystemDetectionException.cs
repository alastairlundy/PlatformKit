
using System;

namespace PlatformKit.Internal.Exceptions
{
    internal class OperatingSystemDetectionException : Exception
    {
        internal OperatingSystemDetectionException() : base("Failed to detect details about the Operating System running.")
        {
            
        }
    }
}