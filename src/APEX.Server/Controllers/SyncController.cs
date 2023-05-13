using APEX.Core;
using APEX.Data;
using APEX.Server.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System.Diagnostics.CodeAnalysis;
using System.IO.Enumeration;

namespace APEX.Server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SyncController : ControllerBase
    {
        private readonly IManifestService _manifestService;
        private readonly UserManager<ApexUser> _userManager;
        private readonly ApexDbContext _ctx;

        public SyncController(ApexDbContext context, UserManager<ApexUser> userManager, IManifestService manifestService)
        {
            _manifestService = manifestService;
            _userManager = userManager;
            _ctx = context;
        }

        [Authorize]
        [HttpGet("fileContainers")]
        public async Task<IActionResult> GetFileContainers()
        {
            var usr = await _userManager.FindByNameAsync(User.Identity.Name);

            var query = from x in _ctx.UserContainerMappings
                        join y in _ctx.FileContainers on x.ContainerId equals y.Id
                        where x.UserId == usr.Id
                        select y;

            var fileContainers = await query.AsNoTracking().ToListAsync();

            return Ok(fileContainers);
        }

        [HttpGet("syncTickets")]
        public async Task<IActionResult> GetSyncTicketsResponseAsync([FromBody] GetUploadTicketsRequest request)
        {
            var query = from x in _ctx.FileContainers
                        join y in _ctx.Files on x.RootFolderId equals y.Id
                        where x.Id == request.FileContainerId
                        select y;

            var fentry = await query.AsNoTracking().FirstOrDefaultAsync();

            var remoteManifest = (await _ctx.Set<GetFileTreeDTO>()
                                           .FromSqlRaw("GetFileTree @p0, @p1", request.FileContainerId, request.IncludeFolders ? 1 : 0)
                                           .AsNoTracking()
                                           .ToListAsync())
                                           .Select(x => new FileManifestEntry
                                           {
                                               RelativePath = x.Path,
                                               Checksum = x.Hash,
                                               Id = x.Id
                                           });

            var comp = new FileManifestEntryComparer();
            var toUpload = request.LocalManifest.Entries.Except(remoteManifest, comp);
            var toDownload = remoteManifest.Except(request.LocalManifest.Entries);

            return Ok(new GetSyncTicketsResponse
            {
                ToDownload = toDownload,
                ToUpload = toUpload
            });
        }
    }


    public class GetSyncTicketsResponse
    {
        public IEnumerable<FileManifestEntry> ToDownload { get; set; }
        public IEnumerable<FileManifestEntry> ToUpload { get; set; }
    }

    public class FileManifestEntryComparer : IEqualityComparer<FileManifestEntry>
    {
        public bool Equals(FileManifestEntry x, FileManifestEntry y)
        {
            return x.Checksum == y.Checksum;
        }

        public int GetHashCode([DisallowNull] FileManifestEntry obj)
        {
            return obj.GetHashCode();
        }
    }

    public class GetUploadTicketsRequest
    {
        public long FileContainerId { get; set; }
        public bool IncludeFolders { get; set; }
        public FileManifestReq LocalManifest { get; set; }
    }
}
