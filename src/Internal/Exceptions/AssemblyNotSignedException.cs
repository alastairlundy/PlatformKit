/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023

    This program is free software: you can redistribute it and/or modify
    it under the terms of the GNU Lesser General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
     any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU General Public License for more details.

    You should have received a copy of the GNU Lesser General Public License
    along with this program.  If not, see <https://www.gnu.org/licenses/>.
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