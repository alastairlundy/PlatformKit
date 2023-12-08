/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2023
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */


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