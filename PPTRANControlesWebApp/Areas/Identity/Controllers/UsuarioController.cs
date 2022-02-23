using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Models.Usuario;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using PPTRANControlesWebApp.Areas.Identity.Pages.Account;

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
            UserManager<AppIdentityUser> userManager, RoleManager<IdentityRole> roleManager)
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

        [Authorize(Roles = RolesNomes.Administrador /*+ "," + RolesNomes.Gestor*/)]
        public IActionResult Create()
        {

            return View();
        }

        [Authorize(Roles = RolesNomes.Administrador /*+ "," + RolesNomes.Gestor*/)]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(UsuarioViewModel model)
        {
            if (userManager.Users.Any(usu => usu.ColaboradorId == Convert.ToInt64(model.Id) || usu.Email == model.Email))
            {
                ViewBag.msg = " - Perfil ja cadastrado para esse Usuário.";
                return RedirectToAction("Create", "Usuario");
            }

            try
            {
                var primeiroNome = model.Nome;

                char[] espaco = { ' ' };
                var check = primeiroNome.IndexOfAny(espaco);

                if (check > 0)
                {
                    primeiroNome = primeiroNome.Substring(0, model.Nome.IndexOf(" "));
                }

                var novoUsuario = new AppIdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                    Nome = primeiroNome,
                    ClinicaId = Convert.ToInt64(model.ClinicaId),
                    ColaboradorId = Convert.ToInt64(model.Id),
                };

                string pwd = model.Pwd;

                var createPowerUser = await userManager.CreateAsync(novoUsuario, pwd);

                if (createPowerUser.Succeeded)
                    await userManager.AddToRoleAsync(novoUsuario, RolesNomes.Operador);

                return RedirectToAction("Index", "Usuario");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return RedirectToAction("Create", "Usuario");
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

        [HttpPost]
        public async Task<IActionResult> Reset(UsuarioEditViewModel model)
        {
            var user = await userManager.FindByIdAsync(model.Id);
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            var pwd = DateTime.Today.Year + user.Email;
            var result = await userManager.ResetPasswordAsync(user, token, pwd);

            return RedirectToAction("Index");
        }
    }
}