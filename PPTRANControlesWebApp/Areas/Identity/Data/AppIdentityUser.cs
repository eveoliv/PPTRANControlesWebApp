using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System.Data;

namespace PPTRANControlesWebApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the AppIdentityUser class
    public class AppIdentityUser : IdentityUser
    {
        public string Nome { get; set; }
        public long ClinicaId { get; set; }
        public long ColaboradorId { get; set; }
    }
}
