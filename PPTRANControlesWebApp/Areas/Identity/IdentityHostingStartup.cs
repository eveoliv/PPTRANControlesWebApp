using System;
using System.Threading.Tasks;
using PPTRANControlesWebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

[assembly: HostingStartup(typeof(PPTRANControlesWebApp.Areas.Identity.IdentityHostingStartup))]
namespace PPTRANControlesWebApp.Areas.Identity
{
    public class IdentityHostingStartup : IHostingStartup
    {
        public void Configure(IWebHostBuilder builder)
        {
            builder.ConfigureServices((context, services) =>
            {
                //services.AddDbContext<AppIdentityContext>(options =>
                //    options.UseSqlite(
                //        context.Configuration.GetConnectionString("AppIdentityConn")));

                ////options altera as preferencias de validacao
                //services.AddDefaultIdentity<AppIdentityUser>(options =>
                //   {
                //       options.Password.RequireNonAlphanumeric = false;
                //       options.Password.RequireUppercase = false;
                //       options.Password.RequireLowercase = false;
                //   })
                //    .AddRoles<IdentityRole>()
                //    .AddErrorDescriber<IdentityErrorDescriberPtBr>()
                //    .AddEntityFrameworkStores<AppIdentityContext>();
            });

        }      
    }
}