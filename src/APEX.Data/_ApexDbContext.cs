using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APEX.Data
{
    public class ApexDbContext : IdentityDbContext<ApexUser, IdentityRole<long>, long>
    {
        public DbSet<FileContainer> FileContainers { get; set; }
        public DbSet<FileEntry> Files { get; set; }
        public DbSet<UserContainerMapping> UserContainerMappings { get; set; }
    }
}
