/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2024
    
    This Source Code Form is subject to the terms of the Mozilla Public
    License, v. 2.0. If a copy of the MPL was not distributed with this
    file, You can obtain one at http://mozilla.org/MPL/2.0/.
 */

namespace PlatformKit.Extensions;

public static class StringExtensions
{
    public static int CountDotsInString(this string str)
    {
        int dotCounter = 0;

        foreach (char c in str)
        {
            if (c == '.')
            {
                dotCounter++;
            }
        }

        return dotCounter;
    }
}