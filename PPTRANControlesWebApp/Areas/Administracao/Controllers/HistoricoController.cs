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
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    [Area("Administracao")]
    [Authorize]
    public class HistoricoController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;
        private readonly ApplicationContext _context;
        private readonly HistoricoDAL historicoDAL;
        
        public HistoricoController(ApplicationContext context,
            UserManager<AppIdentityUser> userManager)
        {
            this.userManager = userManager;

            _context = context;
            historicoDAL = new HistoricoDAL(context);           
        }

        // GET: Historico
        public async Task<IActionResult> Index()
        {
            var historico = 
                await historicoDAL.ObterHistoricoPorNome().ToListAsync();
            return View(historico);
        }

        // GET: Historico/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Historico/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Historico historico)
        {
            var usuario = await userManager.GetUserAsync(User);
            try
            {
                if (ModelState.IsValid)
                {
                    historico.IdUser = usuario.Id;
                    await historicoDAL.GravarHitorico(historico);
                }

                return RedirectToAction(nameof(Index));
            }
            catch(DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            return View(historico);
        }

        // GET: Historico/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            return await ObterVisaoHistoricoPorId(id);
        }

        // POST: Historico/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Historico/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Historico/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        /*************************************************************************/
        private async Task<IActionResult> ObterVisaoHistoricoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var historico = await historicoDAL.ObterHistoricoPorId((long)id);
            if (historico == null)
            {
                return NotFound();
            }            

            return View(historico);
        }
    }
}