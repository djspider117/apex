using APEX.Data;
using APEX.Server.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace APEX.Server
{
    public class ApexDbContext : IdentityDbContext<ApexUser, ApexRole, long>
    {
        public DbSet<FileContainer> FileContainers { get; set; }
        public DbSet<FileEntry> Files { get; set; }
        public DbSet<UserContainerMapping> UserContainerMappings { get; set; }

        public ApexDbContext(DbContextOptions<ApexDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<GetFileTreeDTO>().HasNoKey();

            var cascadeFKs = builder.Model.GetEntityTypes()
                .SelectMany(t => t.GetForeignKeys())
                .Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade);

            foreach (var fk in cascadeFKs)
            {
                fk.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(builder);
        }
    }
}
