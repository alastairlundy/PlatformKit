using System.Collections.Generic;
using System.Threading.Tasks;
using PlatformKit.Core.Models;

namespace PlatformKit.OperatingSystems.Mac;

public partial class MacOperatingSystem : IOperatingSystem
{
    public async Task<IEnumerable<AppModel>> GetInstalledAppsAsync()
    {
        throw new System.NotImplementedException();
    }
}