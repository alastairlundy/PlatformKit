/*
    PlatformKit
    
    Copyright (c) Alastair Lundy 2018-2023
    Copyright (c) NeverSpyTech Limited 2022

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
using PlatformKit.Internal.Exceptions;

namespace PlatformKit.FreeBSD;

// ReSharper disable once InconsistentNaming
public class FreeBSDAnalyzer
{
    protected readonly ProcessManager _processManager;

    public FreeBSDAnalyzer()
    {
        _processManager = new ProcessManager();
    }

    // ReSharper disable once InconsistentNaming
    /// <summary>
    /// Detects and Returns the Installed version of FreeBSD
    /// </summary>
    /// <returns></returns>
    public Version DetectFreeBSDVersion()
    {
        if (OSAnalyzer.IsFreeBSD())
        {
            var v = _processManager.RunProcess("", "uname", "-v");

            v = v.Replace("FreeBSD", String.Empty);

            var arr = v.Split(' ');

            var rel = arr[0].Replace("-release", String.Empty);

            int dotCounter = 0;

            foreach (char c in rel)
            {
                if (c == '.')
                {
                    dotCounter++;
                }
            }

            if (dotCounter == 1)
            {
                rel += ".0.0";
            }
            else if (dotCounter == 2)
            {
                rel += ".0.0";
            }

            return Version.Parse(rel);
        }
        else
        {
            throw new OperatingSystemDetectionException();
        }
    }
}