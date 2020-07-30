using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using PPTRANControlesWebApp.Models.Usuario;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace PPTRANControlesWebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = RolesNomes.Administrador)]
    public class UsuarioController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;

        public UsuarioController(UserManager<AppIdentityUser> userManager)
        {
            this.userManager = userManager;
        }

        public IActionResult Index()
        {
            var usuarios = userManager.Users.Select(usuario => new UsuarioViewModel(usuario)).ToList();

            return View(usuarios);
        }

        public IActionResult Edit(string id)
        {
            return View();
        }
    }
}