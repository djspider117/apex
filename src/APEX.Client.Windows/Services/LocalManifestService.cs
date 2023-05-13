using APEX.Client.Windows.Data;
using GhostCore;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEX.Client.Windows.Services
{

    public class LocalManifestService : IManifestService
    {
        private readonly IChecksumProvider _checksumProvider;

        public LocalManifestService(IChecksumProvider checksumProvider)
        {
            _checksumProvider = checksumProvider;
        }

        public async Task<ISafeTaskResult<FileManifest>> GetManifestAsync(SyncedFolder syncedFolder)
        {
            var manifest = new FileManifest
            {
                RootPath = syncedFolder.Path,
                GeneratedAt = DateTime.Now,
                Entries = new ConcurrentBag<FileManifestEntry>()
            };

            if (!Directory.Exists(syncedFolder.Path))
                return new SafeTaskResult<FileManifest>($"Folder {syncedFolder.Path} doesn't exist");

            var filePaths = Directory.GetFileSystemEntries(syncedFolder.Path, "*.*", SearchOption.AllDirectories);
            Parallel.ForEach(filePaths, filePath =>
            {
                var finfo = new FileInfo(filePath);
                manifest.Entries.Add(new FileManifestEntry
                {
                    LastModifiedUtc = finfo.LastWriteTimeUtc,
                    RelativePath = filePath.Replace($"{syncedFolder.Path}\\", string.Empty),
                    Checksum = _checksumProvider.GetChecksum(filePath)
                });
            });

            await Task.CompletedTask;
            return new SafeTaskResult<FileManifest>(manifest);
        }
    }

    public class FileManifestEntry
    {
        public string RelativePath { get; set; }
        public DateTime LastModifiedUtc { get; set; }
        public string Checksum { get; set; }
    }

    public class FileManifest
    {
        public string RootPath { get; set; }
        public DateTime GeneratedAt { get; set; }
        public ConcurrentBag<FileManifestEntry> Entries { get; set; }
    }
}

