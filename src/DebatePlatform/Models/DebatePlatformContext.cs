using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq;

namespace DebatePlatform.Models
{
    public class DebatePlatformContext : DbContext
    {
        public virtual DbSet<Argument> Arguments { get; set; }
        public virtual DbSet<User> Users { get; set; }


        //public DebatePlatformContext(DbContextOptions<DebatePlatformContext> options)
        //    : base(options)
        //{
        //}
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