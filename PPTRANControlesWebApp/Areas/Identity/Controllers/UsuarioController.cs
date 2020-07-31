using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Models.Usuario;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace PPTRANControlesWebApp.Areas.Identity.Controllers
{
    [Area("Identity")]
    [Authorize(Roles = RolesNomes.Administrador)]
    public class UsuarioController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UsuarioController(UserManager<AppIdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            var usuarios = userManager.Users.Select(usuario => new UsuarioViewModel(usuario)).ToList();

            return View(usuarios);
        }

        public async Task<IActionResult> Edit(string id)
        {
            var usuario = await userManager.FindByIdAsync(id);   
            
       
                

            var model = new UsuarioEditViewModel(usuario, roleManager);

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(UsuarioEditViewModel model)
        {
            if (ModelState.IsValid)
            {
                var usuario = await userManager.FindByIdAsync(model.Id);

                var remocaoResult = await userManager.RemoveFromRolesAsync(
                    usuario, 
                    roleManager.Roles.Select(funcao => funcao.Name));

                if (remocaoResult.Succeeded)
                {
                    var funcoesSelecionadas = model
                        .Funcoes
                        .Where(f => f.Selecionado)
                        .Select(f => f.Nome)
                        .ToArray();

                    var adicaoResult = await userManager.AddToRolesAsync(usuario, funcoesSelecionadas);

                    if (adicaoResult.Succeeded)
                    {
                        return RedirectToAction("Index");
                    }
                }
            }

            return View();
        }
    }
}