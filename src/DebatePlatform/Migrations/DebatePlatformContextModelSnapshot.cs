using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DebatePlatform.Models;

namespace DebatePlatform.Migrations
{
    [DbContext(typeof(DebatePlatformContext))]
    partial class DebatePlatformContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DebatePlatform.Models.Argument", b =>
                {
                    b.Property<int>("ArgumentId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAffirmative");

                    b.Property<int>("ParentId");

                    b.Property<int>("Strength");

                    b.Property<string>("Text");

                    b.Property<int>("UserId");

                    b.HasKey("ArgumentId");

                    b.HasIndex("ParentId");

                    b.HasIndex("UserId");

                    b.ToTable("Arguments");
                });

            modelBuilder.Entity("DebatePlatform.Models.User", b =>
                {
                    b.Property<int>("UserId")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("Username");

                    b.HasKey("UserId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("DebatePlatform.Models.Argument", b =>
                {
                    b.HasOne("DebatePlatform.Models.Argument", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("DebatePlatform.Models.User", "User")
                        .WithMany("Arguments")
                        .HasForeignKey("UserId");
                });
        }
    }
}
