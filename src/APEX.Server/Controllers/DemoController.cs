using APEX.Core;
using APEX.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Concurrent;
using System.Collections.Immutable;
using System.Diagnostics.CodeAnalysis;

namespace APEX.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class DemoController : ControllerBase
    {
        private readonly IManifestService _manifestService;
        private readonly UserManager<ApexUser> _userManager;
        private readonly ApexDbContext _ctx;

        public DemoController(ApexDbContext context, UserManager<ApexUser> userManager, IManifestService manifestService)
        {
            _manifestService = manifestService;
            _userManager = userManager;
            _ctx = context;
        }

        public async Task<IActionResult> Seed()
        {
            var user = await _userManager.FindByNameAsync(User.Identity.Name);
            var cont = await _ctx.FileContainers.FirstOrDefaultAsync(x => x.Id == 2);

            var root = await _ctx.Files.FirstOrDefaultAsync(x => x.Id == cont.RootFolderId);
            root.Children = new List<FileEntry>();
            root.Children.Add(
                new FileEntry
                {
                    Name = "Test Folder 1",
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsFolder = true,
                    Children = new List<FileEntry>
                    {
                        new FileEntry
                        {
                            Name = "Test File 1",
                            CreatedBy = user,
                            ModifiedBy = user,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            MimeType = "application/json",
                            Size = 124
                        },
                        new FileEntry
                        {
                            Name = "Test File 2",
                            CreatedBy = user,
                            ModifiedBy = user,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            MimeType = "application/json",
                            Size = 634
                        }
                    },
                });
            root.Children.Add(
                new FileEntry
                {
                    Name = "Test Folder 2",
                    CreatedBy = user,
                    ModifiedBy = user,
                    DateCreated = DateTime.Now,
                    DateModified = DateTime.Now,
                    IsFolder = true,
                    Children = new List<FileEntry>
                    {
                        new FileEntry
                        {
                            Name = "Test File 3",
                            CreatedBy = user,
                            ModifiedBy = user,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            MimeType = "application/json",
                            Size = 67534
                        },
                        new FileEntry
                        {
                            Name = "Test File 4",
                            CreatedBy = user,
                            ModifiedBy = user,
                            DateCreated = DateTime.Now,
                            DateModified = DateTime.Now,
                            MimeType = "application/json",
                            Size = 1258
                        }
                    },
                });

            await _ctx.SaveChangesAsync();

            return Ok();
        }
    }
}