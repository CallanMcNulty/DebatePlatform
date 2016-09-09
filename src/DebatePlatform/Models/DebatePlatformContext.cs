using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DebatePlatform.Models
{
    public class DebatePlatformContext : DbContext
    {
        public virtual DbSet<Argument> Arguments { get; set; }

        public DebatePlatformContext(DbContextOptions<DebatePlatformContext> options)
            : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlServer(@"Server=(localdb)\mssqllocaldb;Database=DebatePlatform;integrated security=True");
        }

    }
}