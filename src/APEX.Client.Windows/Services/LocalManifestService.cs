using APEX.Client.Windows.Data;
using APEX.Core;
using APEX.Data;
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

        public async Task<ISafeTaskResult<FileManifest>> GetManifestAsync(string syncedFolderPath)
        {
            var manifest = new FileManifest
            {
                RootPath = syncedFolderPath,
                GeneratedAt = DateTime.Now,
                Entries = new ConcurrentBag<FileManifestEntry>()
            };

            if (!Directory.Exists(syncedFolderPath))
                return new SafeTaskResult<FileManifest>($"Folder {syncedFolderPath} doesn't exist");

            var filePaths = Directory.GetFileSystemEntries(syncedFolderPath, "*.*", SearchOption.AllDirectories);
            Parallel.ForEach(filePaths, filePath =>
            {
                var finfo = new FileInfo(filePath);
                if ((finfo.Attributes & FileAttributes.Directory) == FileAttributes.Directory)
                    return;

                manifest.Entries.Add(new FileManifestEntry
                {
                    LastModifiedUtc = finfo.LastWriteTimeUtc,
                    RelativePath = filePath.Replace($"{syncedFolderPath}\\", string.Empty),
                    //Checksum = _checksumProvider.GetChecksum(filePath) TODO: check why ComputeHash returns null
                });
            });

            await Task.CompletedTask;

            manifest.GeneratedIn = DateTime.Now - manifest.GeneratedAt;
            manifest.GeneratedAt = DateTime.Now;

            return new SafeTaskResult<FileManifest>(manifest);
        }
    }
}

