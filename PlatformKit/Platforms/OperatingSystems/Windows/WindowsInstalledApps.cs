using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using CliRunner;
using PlatformKit.Core.Models;
using PlatformKit.Internal.Localizations;

#if NETSTANDARD2_0 || NETSTANDARD2_1
using OperatingSystem = AlastairLundy.Extensions.Runtime.OperatingSystemExtensions;
#endif

namespace PlatformKit.OperatingSystems.Windows;

public partial class WindowsOperatingSystem : IOperatingSystem
{
    
    public async Task<IEnumerable<AppModel>> GetInstalledAppsAsync()
    {
     

    }
}