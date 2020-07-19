using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PPTRANControlesWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    //REVISADO_20200715
    [Area("Administracao")]
    [Authorize]
    public class HistoricoController : Controller
    {
        private readonly HistoricoDAL historicoDAL;
        private readonly ApplicationContext context;
        private readonly UserManager<AppIdentityUser> userManager;
        
        public HistoricoController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            historicoDAL = new HistoricoDAL(context);           
        }

        // GET: Historico
        public async Task<IActionResult> Index()
        {
            return View(await historicoDAL.ObterHistoricosClassificadosPorNome().ToListAsync());
        }

        // GET: Historico/Details
        public async Task<IActionResult> Details(int? id)
        {
            return await ObterVisaoHistoricoPorId(id);
        }

        // GET: Historico/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoHistoricoPorId(id);
        }

        // POST: Historico/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Historico historico)
        {
            if (id != historico.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    historico.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await historicoDAL.GravarHistorico(historico);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
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
            try
            {
                if (historico.Nome != null)
                {                                    
                    historico.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await historicoDAL.GravarHistorico(historico);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(historico);
        }

        // GET: Historico/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoHistoricoPorId(id);
        }

        // POST: Historico/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var historico = await historicoDAL.InativarHistoricoPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
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