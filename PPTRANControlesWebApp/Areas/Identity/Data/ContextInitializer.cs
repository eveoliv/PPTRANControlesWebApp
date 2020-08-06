using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ContextIdentityInitializer
    {
        public static void Initialize(AppIdentityContext context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();          
        }
    }
}

