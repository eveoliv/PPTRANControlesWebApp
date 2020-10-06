using System;
using Microsoft.AspNetCore;
using PPTRANControlesWebApp.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using PPTRANControlesWebApp.Models;
using PPTRANControlesWebApp.Data.DAL;

namespace PPTRANControlesWebApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                try
                {
                    var applicationcontext = services.GetRequiredService<ApplicationContext>();
                    var identityContext = services.GetRequiredService<AppIdentityContext>();
                    ContextInitializer.Initialize(applicationcontext);                                                           
                    ContextIdentityInitializer.Initialize(identityContext);
                }
                catch (Exception ex)
                {
                    var logger = services.GetRequiredService<ILogger<Program>>();
                    logger.LogError(ex, "Um erro ocorreu ao popular a base de dados.");
                }
            }

            host.Run();
        }
    

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>()
                .UseIISIntegration();
    }
}
