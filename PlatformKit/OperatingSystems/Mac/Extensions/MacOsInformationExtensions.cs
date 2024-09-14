/*
        MIT License
       
       Copyright (c) 2020-2024 Alastair Lundy
       
       Permission is hereby granted, free of charge, to any person obtaining a copy
       of this software and associated documentation files (the "Software"), to deal
       in the Software without restriction, including without limitation the rights
       to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
       copies of the Software, and to permit persons to whom the Software is
       furnished to do so, subject to the following conditions:
       
       The above copyright notice and this permission notice shall be included in all
       copies or substantial portions of the Software.
       
       THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
       IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
       FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
       AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
       LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
       OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
       SOFTWARE.
   */

using System;

using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0
using OperatingSystem = PlatformKit.Extensions.OperatingSystem.OperatingSystemExtension;
#endif

namespace PlatformKit.OperatingSystems.Mac.Extensions;

public static class MacOsInformationExtensions
{
    /// <summary>
    /// Detects macOS System Information.
    /// </summary>
    /// <returns></returns>
    public static MacOsSystemInformationModel GetMacSystemInformationModel(this MacOperatingSystem macOperatingSystem)
    {
        if (OperatingSystem.IsMacOS())
        {
            return new MacOsSystemInformationModel()
            {
                ProcessorType = macOperatingSystem.GetMacProcessorType(),
                MacOsBuildNumber = macOperatingSystem.GetOperatingSystemBuildNumber(),
                MacOsVersion = macOperatingSystem.GetOperatingSystemVersion(),
                DarwinVersion = macOperatingSystem.GetDarwinVersion(),
                XnuVersion = macOperatingSystem.GetKernelVersion()
            };
        }
        else
        {
            throw new PlatformNotSupportedException(Resources.Exceptions_PlatformNotSupported_MacOnly);
        }
    }
}