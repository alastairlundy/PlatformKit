using System.Collections.Generic;
using System.Threading.Tasks;

using PlatformKit.Abstractions.Models;

namespace PlatformKit.Abstractions.OperatingSystems;

public interface IOperatingSystemAppInformation
{
    Task<IEnumerable<AppModel>> GetInstalledAppsAsync();

    Task<bool> IsAppInstalledAsync(string appName);
}