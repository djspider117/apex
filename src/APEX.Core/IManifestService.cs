using APEX.Data;
using GhostCore;
using System.Threading.Tasks;

namespace APEX.Core
{
    public interface IManifestService
    {
        Task<ISafeTaskResult<FileManifest>> GetManifestAsync(string syncedFolderPath);
    }
}