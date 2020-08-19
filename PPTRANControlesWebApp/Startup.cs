using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using PPTRANControlesWebApp.Areas.Identity;
using Microsoft.Extensions.DependencyInjection;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;

namespace PPTRANControlesWebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            /* DEV */
            services.AddDbContext<ApplicationContext>(options => options.UseMySql(Configuration.GetConnectionString("AppContextLocalConn")));

            /* PROD */
            //services.AddDbContext<ApplicationContext>(options => options.UseMySql(Configuration.GetConnectionString("AppContextUolConn")));

            services.AddDbContext<AppIdentityContext>(options => options.UseSqlite(Configuration.GetConnectionString("AppIdentityConn")));

            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            //options altera as preferencias de validacao
            services.AddDefaultIdentity<AppIdentityUser>(options =>
            {
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
            })
                .AddRoles<IdentityRole>()                
                .AddErrorDescriber<IdentityErrorDescriberPtBr>()
                .AddEntityFrameworkStores<AppIdentityContext>();
          
            services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromSeconds(10);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCookiePolicy();
            app.UseSession();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "areas",
                    template: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            });

            CreateRoles(serviceProvider).Wait();
            CreateAdmin(serviceProvider).Wait();
        }

        private async Task CreateRoles(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppIdentityUser>>();

            string[] rolesNames = { RolesNomes.Administrador, RolesNomes.Gestor, RolesNomes.Operador, RolesNomes.Inativo };

            IdentityResult result;

            foreach (var namesRole in rolesNames)
            {
                var roleExist = await roleManager.RoleExistsAsync(namesRole);
                if (!roleExist)
                {
                    result = await roleManager.CreateAsync(new IdentityRole(namesRole));
                }
            }
        }

        private async Task CreateAdmin(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<AppIdentityUser>>();
        
            var _user = await userManager.FindByNameAsync("Administrador");
            
            if (_user == null)
            {
                var poweruser = new AppIdentityUser
                {
                    UserName = "Admin@email.com",
                    Email = "Admin@email.com",
                    Nome = "Administrador",
                    ClinicaId = 0,
                    ColaboradorId = 1                    
                };

                string pwd = "123456";

                var createPowerUser = await userManager.CreateAsync(poweruser, pwd);

                if (createPowerUser.Succeeded)
                {                    
                    await userManager.AddToRoleAsync(poweruser, RolesNomes.Administrador);
                }
            }
        }
    }
}