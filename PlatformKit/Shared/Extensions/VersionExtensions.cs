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
    /// <param name="str"></param>
    /// <param name="numberOfZeroesNeeded">The number of zeroes to add. Valid values are 0 through 3. Defaults to 3.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException"></exception>
    public static string AddMissingZeroes(this Version version, string str, int numberOfZeroesNeeded = 3)
    {
        StringBuilder stringBuilder = new StringBuilder();
        int dots = str.CountDotsInString();

        if (dots == 0)
        {
            stringBuilder.Append(".");
            stringBuilder.Append("0");
        }
        if (dots == 1 && numberOfZeroesNeeded >= 1)
        {
            stringBuilder.Append(".");
            stringBuilder.Append("0");
        }
        if (dots == 2 && numberOfZeroesNeeded >= 2)
        {
            stringBuilder.Append(".");
            stringBuilder.Append("0");
        }
        if (dots == 3 && numberOfZeroesNeeded == 3)
        {
            return str;
        }

        var newDots = stringBuilder.ToString().CountDotsInString();
        
        if (newDots == 1 && numberOfZeroesNeeded == 1 || 
            newDots == 2 && numberOfZeroesNeeded == 2 || 
            newDots == 3 && numberOfZeroesNeeded == 3)
        {
            return stringBuilder.ToString();
        }
        
        throw new ArgumentException();
    }
    
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