using APEX.Client.Windows.Data;
using GhostCore;
using System.Threading.Tasks;

namespace APEX.Client.Windows.Services
{
    public interface ISettingsService
    {
        AppSettings Settings { get; }

        Task<ISafeTaskResult> LoadSettingsAsync();
        Task ApplySettingsAsync();
    }
}