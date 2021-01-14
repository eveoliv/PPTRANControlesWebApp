using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PPTRANControlesWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    [Area("Administracao")]
    [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor)]
    public class RepasseController : Controller
    {
        private readonly RepasseDAL repasseDAL;
        private readonly ApplicationContext context;
        private readonly UserManager<AppIdentityUser> userManager;

        public RepasseController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            repasseDAL = new RepasseDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            var repasses = await repasseDAL.ObterRepassesClassificadosPorNome().ToListAsync();

            return View(repasses);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoRepassePorId(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Repasse repasse)
        {
            if (id != repasse.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    if (ModelState.IsValid)
                    {
                        repasse.IdUser = userManager.GetUserAsync(User).Result.Id;
                        await repasseDAL.GravarRepasse(repasse);
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    ModelState.AddModelError("", "Valor informado não é um numero válido.");
                }

                return RedirectToAction("Index");
            }
            return View(repasse);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Repasse repasse)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    repasse.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await repasseDAL.GravarRepasse(repasse);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            return RedirectToAction(nameof(Index));
        }

        /****** Metodos Privados do Controller ******/
        private async Task<IActionResult> ObterVisaoRepassePorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repasse = await repasseDAL.ObterRepassePorId((long)id);
            if (repasse == null)
            {
                return NotFound();
            }

            return View(repasse);
        }
    }
}