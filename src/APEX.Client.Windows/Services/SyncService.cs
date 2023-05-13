using APEX.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace APEX.Client.Windows.Services
{
    public sealed class SyncService : IDisposable
    {
        private readonly ISettingsService _settingService;
        private readonly IManifestService _manifestService;
        private readonly IChecksumProvider _checksumProvider;
        private readonly List<FileSystemWatcher> _watchers = new List<FileSystemWatcher>();

        public SyncService(ISettingsService settingsService, IManifestService manifestService, IChecksumProvider checksumProvider)
        {
            _settingService = settingsService;
            _manifestService = manifestService;
            _checksumProvider = checksumProvider;
        }

        public async Task Start()
        {
            await PerformStartupManifestCheckAsync();

            //WatchSyncedFolders();
        }

        private async Task PerformStartupManifestCheckAsync()
        {
            foreach (var syncFolder in _settingService.Settings.SyncedFolders)
            {
                var manifestResult = await _manifestService.GetManifestAsync(syncFolder.Path);
                if (manifestResult.IsFaulted)
                    throw manifestResult.DetailedException; // TODO: log

                var manifest = manifestResult.ResultValue;
                Console.WriteLine(manifest);
            }
        }

        private void WatchSyncedFolders()
        {
            foreach (var syncFolder in _settingService.Settings.SyncedFolders)
            {
                var watcher = new FileSystemWatcher(syncFolder.Path)
                {
                    IncludeSubdirectories = true,
                    NotifyFilter = NotifyFilters.Attributes
                                 | NotifyFilters.CreationTime
                                 | NotifyFilters.DirectoryName
                                 | NotifyFilters.FileName
                                 | NotifyFilters.LastAccess
                                 | NotifyFilters.LastWrite
                                 | NotifyFilters.Security
                                 | NotifyFilters.Size
                };

                watcher.Changed += Watcher_Changed;
                watcher.Created += Watcher_Created;
                watcher.Deleted += Watcher_Deleted;
                watcher.Renamed += Watcher_Renamed;
                watcher.Error += Watcher_Error;

                // TODO: make sure once every few minutes we recheck manifest to see if the watcher didn't miss any file due to small InternalBufferSize

                watcher.EnableRaisingEvents = true;
            }
        }

        private void Watcher_Error(object sender, ErrorEventArgs e)
        {
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
        }

        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {
        }

        private void Watcher_Changed(object sender, FileSystemEventArgs e)
        {
        }

        public void Dispose()
        {
            foreach (var watcher in _watchers)
            {
                watcher.EnableRaisingEvents = false;
                watcher.Dispose();
            }

            _watchers.Clear();
        }
    }
}
