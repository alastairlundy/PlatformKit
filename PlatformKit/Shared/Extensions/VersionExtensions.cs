/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2024
    
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

using System;
using System.Text;

namespace PlatformKit.Extensions;

public static class VersionExtensions
{
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="version"></param>
    /// <param name="versionToBeCompared"></param>
    /// <returns></returns>
    public static bool IsAtLeast(this Version version, Version versionToBeCompared)
    {
            var detected = version;
            var expected = versionToBeCompared;

            if (detected.Major >= expected.Major)
            {
                if (detected.Minor >= expected.Minor)
                {
                    if (detected.Build >= expected.Build)
                    {
                        if (detected.Revision >= expected.Revision)
                        {
                            return true;
                        }

                        return false;
                    }

                    return false;
                }

                return false;
            }

            return false;
    }
}