using System.Threading.Tasks;
using CliWrap;
using CliWrap.Buffered;

namespace PlatformKit.Internal.Helpers;

internal static class CliWrapExecuteSyncExtensions
{
    internal static BufferedCommandResult ExecuteSync(this Command command)
    {
        Task<BufferedCommandResult> task = command.ExecuteBufferedAsync();
        task.RunSynchronously();
        
        task.Wait();

        return task.Result;
    }
}