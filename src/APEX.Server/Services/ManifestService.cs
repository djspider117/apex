using APEX.Core;
using APEX.Data;
using GhostCore;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;

namespace APEX.Server.Services
{
    public class ManifestService : IManifestService
    {
        private readonly ApexDbContext _ctx;

        public ManifestService(ApexDbContext context)
        {
            _ctx = context;
        }

        public async Task<ISafeTaskResult<FileManifest>> GetManifestAsync(string syncedFolderPath)
        {
            var rv = new FileManifest
            {
                RootPath = syncedFolderPath,
                GeneratedAt = DateTime.Now,
                Entries = new ConcurrentBag<FileManifestEntry>()
            };

            var fileTree = await _ctx.Database.SqlQueryRaw<GetFileTreeDTO>("GetFileTree 3, 0").ToListAsync();

            Parallel.ForEach(fileTree, f =>
            {
                rv.Entries.Add(new FileManifestEntry
                {
                    RelativePath = f.Path,
                    //LastModifiedUtc = DateTime.UtcNow,
                });
            });

            return new SafeTaskResult<FileManifest>(rv);
        }
    }

    public class GetFileTreeDTO
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Path { get; set; }
        public string Hash { get; set; }
    }
}