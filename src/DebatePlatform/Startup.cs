﻿using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore;
using DebatePlatform.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Linq;

namespace DebatePlatform
{
    public class Startup
    {
        public IConfigurationRoot Configuration { get; set; }
        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json");
            Configuration = builder.Build();
            DebatePlatformContext db = new DebatePlatformContext();
            if (db.Roles.FirstOrDefault(r => r.Name == "admin") == null)
            {
                db.Roles.Add(new IdentityRole() { Name = "admin" });
            }
            if (db.Roles.FirstOrDefault(r => r.Name == "user") == null)
            {
                db.Roles.Add(new IdentityRole() { Name = "user" });
            }
            db.SaveChanges();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();
            services.AddEntityFramework()
                .AddDbContext<DebatePlatformContext>(options =>
                    options.UseSqlServer(Configuration["Data:DefaultConnection:ConnectionString"]));
            services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<DebatePlatformContext>()
                .AddDefaultTokenProviders();
        }

        public void Configure(IApplicationBuilder app)
        {
            app.UseIdentity();
            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
            app.UseStaticFiles();
            app.Run(async (context) =>
            {
                await context.Response.WriteAsync("Hello World!");
            });
        }

    }
}
