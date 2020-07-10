using System;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Models;

[assembly: HostingStartup(typeof(PPTRANControlesWebApp.Areas.Identity.IdentityHostingStartup))]
namespace PPTRANControlesWebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) => {
                services.AddDbContext<AppIdentityContext>(options =>
                    options.UseSqlite(
                        context.Configuration.GetConnectionString("AppIdentityConn")));

                services.AddDefaultIdentity<AppIdentityUser>()
                    .AddEntityFrameworkStores<AppIdentityContext>();
            });
        }
    }
}