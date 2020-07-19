using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Models;

namespace PPTRANControlesWebApp.Areas.Identity.Data
{
    // Add profile data for application users by adding properties to the AppIdentityUser class
    public class AppIdentityUser : IdentityUser
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
                
    }
}
