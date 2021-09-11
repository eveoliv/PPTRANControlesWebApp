using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using PPTRANControlesWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Models.Administracao;
using PPTRANControlesWebApp.Areas.Identity.Models;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    [Area("Administracao")]
    [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor)]
    public class RepasseController : Controller
    {
        private readonly RepasseDAL repasseDAL;
        private readonly ClinicaDAL clinicaDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public RepasseController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            repasseDAL = new RepasseDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
            clinicaDAL = new ClinicaDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var repasses = await repasseDAL.ObterRepassesClassificadosPorClinica().ToListAsync();

            if (roleUser.FirstOrDefault() == RolesNomes.Administrador)
            {
                return View(repasses);
            }

            if (roleUser.FirstOrDefault() == RolesNomes.Gestor)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;

                repasses = repasses.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

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

        public async Task<IActionResult> Create()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var repasses = await repasseDAL.ObterRepassesClassificadosPorClinica().ToListAsync();

            if (roleUser.FirstOrDefault() == RolesNomes.Administrador)
            {
                CreateViewBags();

                return View();
            }

            return View("Denied");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RepasseViewModel model)
        {
            var userId = userManager.GetUserAsync(User).Result.Id;           

            try
            {
                if (ModelState.IsValid)
                {
                    var repasse = new Repasse
                    {
                        Profissional = model.Profissional,
                        Valor = model.Valor,
                        IdUser = userId,
                        ClinicaId = model.ClinicaId
                    };

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

        private void CreateViewBags()
        {                      
            ViewBag.Clinicas = clinicaDAL.ObterClinicasClassificadasPorNome().ToList();

            var lista = new List<string>
                {
                    EnumHelper.Funcao.Medico.ToString(),
                    EnumHelper.Funcao.Psicologo.ToString()
                };

            ViewBag.Funcoes = lista;
        }
    }
}