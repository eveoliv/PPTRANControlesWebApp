using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Models.Usuario;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;

namespace PPTRANControlesWebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = RolesNomes.Administrador)]
    public class UsuarioController : Controller
    {        
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly AppIdentityContext appIdentityContext;
        private readonly UserManager<AppIdentityUser> userManager;

        public UsuarioController(AppIdentityContext appIdentityContext,
            UserManager<AppIdentityUser> userManager,RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;            
            this.appIdentityContext = appIdentityContext;
        }

        public IActionResult Index()
        {
            var usuarios 
                = userManager.Users.Where(usuario => usuario.Nome != RolesNomes.Administrador)
                .Select(usuario => new UsuarioViewModel(usuario)).ToList();            

            return View(usuarios);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var usuario = await userManager.FindByIdAsync(id);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var model = new UsuarioEditViewModel(usuario, roleManager, roleUser);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await userManager.FindByIdAsync(model.Id);
                var roleUser = await userManager.GetRolesAsync(usuario);

                await userManager.RemoveFromRolesAsync(usuario, roleUser);

                var newRole = model.Role;
                if (newRole == RolesNomes.Administrador)
                {
                    await userManager.AddToRoleAsync(usuario, RolesNomes.Administrador);
                }

                if (newRole == RolesNomes.Gestor)
                {
                    await userManager.AddToRoleAsync(usuario, RolesNomes.Gestor);
                }

                if (newRole == RolesNomes.Operador)
                {
                    await userManager.AddToRoleAsync(usuario, RolesNomes.Operador);
                }

                if (newRole == RolesNomes.Inativo)
                {
                    await userManager.AddToRoleAsync(usuario, RolesNomes.Inativo);
                }

                return RedirectToAction("Index");

            }

            return View();
        }
    }
}