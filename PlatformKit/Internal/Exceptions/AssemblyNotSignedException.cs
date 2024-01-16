/*
      PlatformKit
      
      Copyright (c) Alastair Lundy 2018-2024
      
      This Source Code Form is subject to the terms of the Mozilla Public
      License, v. 2.0. If a copy of the MPL was not distributed with this
      file, You can obtain one at http://mozilla.org/MPL/2.0/.
   */

using System;
using System.Reflection;

namespace PlatformKit.Internal.Exceptions;

internal class AssemblyNotSignedException : Exception
{
    internal AssemblyNotSignedException() : base(
        "Project '" + Assembly.GetEntryAssembly()?.GetName().Name + "' using the PlatformKit SDK does not have a signed assembly" +
        " and so cannot use PlatformKit SDK under a commercial license. For support please contact NEVERSPY TECH at https://neverspy.tech .")
    {
        
    }
}