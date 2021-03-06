﻿using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using DebatePlatform.Models;

namespace DebatePlatform.Migrations
{
    [DbContext(typeof(DebatePlatformContext))]
    [Migration("20161005154308_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
            modelBuilder
                .HasAnnotation("ProductVersion", "1.0.0-rtm-21431")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DebatePlatform.Models.ApplicationUser", b =>
                {
                    b.Property<string>("Id");

                    b.Property<int>("AccessFailedCount");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Email")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<bool>("EmailConfirmed");

                    b.Property<bool>("LockoutEnabled");

                    b.Property<DateTimeOffset?>("LockoutEnd");

                    b.Property<string>("NormalizedEmail")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedUserName")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("PasswordHash");

                    b.Property<string>("PhoneNumber");

                    b.Property<bool>("PhoneNumberConfirmed");

                    b.Property<string>("SecurityStamp");

                    b.Property<bool>("TwoFactorEnabled");

                    b.Property<string>("UserName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedEmail")
                        .HasName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasName("UserNameIndex");

                    b.ToTable("AspNetUsers");
                });

            modelBuilder.Entity("DebatePlatform.Models.Argument", b =>
                {
                    b.Property<int>("ArgumentId")
                        .ValueGeneratedOnAdd();

                    b.Property<bool>("IsAffirmative");

                    b.Property<bool>("IsCitation");

                    b.Property<int>("ParentId");

                    b.Property<int>("Strength");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.HasKey("ArgumentId");

                    b.HasIndex("ParentId");

                    b.HasIndex("UserId");

                    b.ToTable("Arguments");
                });

            modelBuilder.Entity("DebatePlatform.Models.Citation", b =>
                {
                    b.Property<int>("CitationId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArgumentId");

                    b.Property<string>("Creator");

                    b.Property<string>("Date");

                    b.Property<string>("Description");

                    b.Property<string>("Format");

                    b.Property<string>("Institution");

                    b.Property<string>("Text");

                    b.Property<string>("Title");

                    b.Property<string>("URL");

                    b.HasKey("CitationId");

                    b.ToTable("Citations");
                });

            modelBuilder.Entity("DebatePlatform.Models.EditVote", b =>
                {
                    b.Property<int>("EditVoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ProposedEditId");

                    b.Property<string>("UserId");

                    b.HasKey("EditVoteId");

                    b.HasIndex("ProposedEditId");

                    b.HasIndex("UserId");

                    b.ToTable("EditVotes");
                });

            modelBuilder.Entity("DebatePlatform.Models.ProposedEdit", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArgumentId");

                    b.Property<bool>("IsAffirmative");

                    b.Property<bool>("IsDelete");

                    b.Property<int>("ParentId");

                    b.Property<string>("Reason");

                    b.Property<string>("Text");

                    b.Property<string>("UserId");

                    b.Property<int>("Votes");

                    b.HasKey("Id");

                    b.HasIndex("ArgumentId");

                    b.HasIndex("UserId");

                    b.ToTable("ProposedEdits");
                });

            modelBuilder.Entity("DebatePlatform.Models.Vote", b =>
                {
                    b.Property<int>("VoteId")
                        .ValueGeneratedOnAdd();

                    b.Property<int>("ArgumentId");

                    b.Property<string>("UserId");

                    b.HasKey("VoteId");

                    b.HasIndex("ArgumentId");

                    b.HasIndex("UserId");

                    b.ToTable("Votes");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole", b =>
                {
                    b.Property<string>("Id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken();

                    b.Property<string>("Name")
                        .HasAnnotation("MaxLength", 256);

                    b.Property<string>("NormalizedName")
                        .HasAnnotation("MaxLength", 256);

                    b.HasKey("Id");

                    b.HasIndex("NormalizedName")
                        .HasName("RoleNameIndex");

                    b.ToTable("AspNetRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("RoleId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.ToTable("AspNetRoleClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd();

                    b.Property<string>("ClaimType");

                    b.Property<string>("ClaimValue");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserClaims");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider");

                    b.Property<string>("ProviderKey");

                    b.Property<string>("ProviderDisplayName");

                    b.Property<string>("UserId")
                        .IsRequired();

                    b.HasKey("LoginProvider", "ProviderKey");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserLogins");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("RoleId");

                    b.HasKey("UserId", "RoleId");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId");

                    b.ToTable("AspNetUserRoles");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId");

                    b.Property<string>("LoginProvider");

                    b.Property<string>("Name");

                    b.Property<string>("Value");

                    b.HasKey("UserId", "LoginProvider", "Name");

                    b.ToTable("AspNetUserTokens");
                });

            modelBuilder.Entity("DebatePlatform.Models.Argument", b =>
                {
                    b.HasOne("DebatePlatform.Models.Argument", "Parent")
                        .WithMany("Children")
                        .HasForeignKey("ParentId");

                    b.HasOne("DebatePlatform.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DebatePlatform.Models.EditVote", b =>
                {
                    b.HasOne("DebatePlatform.Models.ProposedEdit", "ProposedEdit")
                        .WithMany()
                        .HasForeignKey("ProposedEditId");

                    b.HasOne("DebatePlatform.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DebatePlatform.Models.ProposedEdit", b =>
                {
                    b.HasOne("DebatePlatform.Models.Argument", "Argument")
                        .WithMany("ProposedEdits")
                        .HasForeignKey("ArgumentId");

                    b.HasOne("DebatePlatform.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("DebatePlatform.Models.Vote", b =>
                {
                    b.HasOne("DebatePlatform.Models.Argument", "Argument")
                        .WithMany("Votes")
                        .HasForeignKey("ArgumentId");

                    b.HasOne("DebatePlatform.Models.ApplicationUser", "User")
                        .WithMany()
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Claims")
                        .HasForeignKey("RoleId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("DebatePlatform.Models.ApplicationUser")
                        .WithMany("Claims")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("DebatePlatform.Models.ApplicationUser")
                        .WithMany("Logins")
                        .HasForeignKey("UserId");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.EntityFrameworkCore.IdentityRole")
                        .WithMany("Users")
                        .HasForeignKey("RoleId");

                    b.HasOne("DebatePlatform.Models.ApplicationUser")
                        .WithMany("Roles")
                        .HasForeignKey("UserId");
                });
        }
    }
}
