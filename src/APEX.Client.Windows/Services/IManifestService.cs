using APEX.Client.Windows.Data;
using GhostCore;
using System.Threading.Tasks;

namespace APEX.Client.Windows.Services
{
    public interface IManifestService
    {
        Task<ISafeTaskResult<FileManifest>> GetManifestAsync(SyncedFolder syncedFolder);
    }
}