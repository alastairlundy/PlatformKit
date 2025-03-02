using System;
using System.IO;

namespace PlatformKit.Internal.Helpers;

internal static class WinPowershellInfo
{
    internal static readonly string Location = $"{Environment.SystemDirectory}{Path.DirectorySeparatorChar}System32" +
                                               $"{Path.DirectorySeparatorChar}WindowsPowerShell{Path.DirectorySeparatorChar}v1.0";
}