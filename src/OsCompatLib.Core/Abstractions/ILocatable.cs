using System.Threading.Tasks;

namespace OsCompatLib.Core.Abstractions;

public interface ILocatable
{
    Task<string?> LocateAsync();
}