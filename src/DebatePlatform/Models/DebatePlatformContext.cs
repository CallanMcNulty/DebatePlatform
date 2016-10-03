using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace DebatePlatform.Models
{
    public class DebatePlatformContext : IdentityDbContext<ApplicationUser>
    {
        public virtual DbSet<Argument> Arguments { get; set; }
        public virtual DbSet<Vote> Votes { get; set; }
        public virtual DbSet<ProposedEdit> ProposedEdits { get; set; }
        public virtual DbSet<EditVote> EditVotes { get; set; }
        public virtual DbSet<Citation> Citations { get; set; }

        protected override void OnModelCreating( ModelBuilder modelbuilder)
        {
            foreach (var relationship in modelbuilder.Model.GetEntityTypes().SelectMany(e => e.GetForeignKeys()))
            {
                relationship.DeleteBehavior = DeleteBehavior.Restrict;
            }

            base.OnModelCreating(modelbuilder);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DebatePlatform;integrated security=True");
        }

    }
}