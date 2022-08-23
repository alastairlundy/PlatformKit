
using System;

namespace PlatformKit.Internal.Exceptions
{
    internal class RuntimeIdentifierGenerationException : Exception
    {

        internal RuntimeIdentifierGenerationException() : base("Failed to generate the RuntimeIdentifier given the specified parameters.")
        {
            
        }
        
    }
}