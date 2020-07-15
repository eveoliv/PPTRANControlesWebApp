using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{   
    [Area("Administracao")]
    [Authorize]
    public class ClinicaController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly ApplicationContext _context;
        private readonly ClinicaDAL clinicaDAL;
        private readonly EnderecoDAL enderecoDAL;

        public ClinicaController(ApplicationContext context, 
            UserManager<AppIdentityUser> userManager)
        {
            this.userManager = userManager;

            _context = context;
            clinicaDAL = new ClinicaDAL(context);
            enderecoDAL = new EnderecoDAL(context);
        }

        // GET: Clinica
        public async Task<IActionResult> Index()
        {
            var clinicas = await clinicaDAL.ObterClinicasClassificadasPorNome().ToListAsync();
            return View(clinicas);
        }

        // GET: Clinica/Details/5
        public async Task<IActionResult> Details(int id)
        {
            return await ObterVisaoClinicaPorId(id);
        }

        // GET: Clinica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clinica/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(ClinicaViewModel model)
        {
            try
            {
                if (model.Clinica.Nome != null)
                {
                    var cnpj = model.Clinica.CNPJ;
                    
                    await enderecoDAL.GravarEndereco(model.Endereco);                   
                    
                    model.Clinica.Endereco.Id = model.Endereco.Id;
                    await clinicaDAL.GravarClinica(model.Clinica);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Clinica);
        }

        // GET: Clinica/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            return await ObterVisaoClinicaPorId(id);
        }

        // POST: Clinica/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Clinica clinica)
        {
            if (id != clinica.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    await clinicaDAL.GravarClinica(clinica);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(clinica);
        }

        // GET: Clinica/Delete/5
        public async Task <IActionResult> Delete(long id)
        {
            return await ObterVisaoClinicaPorId(id);
        }

        // POST: Clinica/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var clinica = await clinicaDAL.ObterClinicaPorId(id);

            clinica.Status = EnumHelper.Status.Inativo;
            await clinicaDAL.GravarClinica(clinica);
            return RedirectToAction(nameof(Index));
        }


        /*************************************************************************/
        private async Task<IActionResult> ObterVisaoClinicaPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinica = await clinicaDAL.ObterClinicaPorId((long)id);
            if (clinica == null)
            {
                return NotFound();
            }

            return View(clinica);
        }
    }
}