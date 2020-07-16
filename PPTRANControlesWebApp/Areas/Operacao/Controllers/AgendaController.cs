using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data.DAL.Operacao;

namespace PPTRANControlesWebApp.Areas.Operacao.Controllers
{
    //REVISADO_20200715
    [Area("Operacao")]
    [Authorize]
    public class AgendaController : Controller
    {
        private readonly AgendaDAL agendaDAL;
        private readonly ApplicationContext context;
        private readonly UserManager<AppIdentityUser> userManager;

        public AgendaController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            agendaDAL = new AgendaDAL(context);
        }

        // GET: Agenda
        public async Task<IActionResult> Index()
        {
            return View(await agendaDAL.ObterAgendaClassificadaIdCliente().ToListAsync());
        }

        // GET: Agenda/Details
        public async Task<IActionResult> Details(int? id)
        {
            return await ObterVisaoAgendaPorId(id);
        }

        // GET: Agenda/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoAgendaPorId(id);
        }

        // POST: Agenda/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Agenda agenda)
        {
            if (id != agenda.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    agenda.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await agendaDAL.GravarAgenda(agenda);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(agenda);
        }

        // GET: Agenda/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Agenda/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Agenda agenda)
        {
            try
            {
                if (agenda.Data != null && agenda.ClienteId != null)
                {
                    agenda.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await agendaDAL.GravarAgenda(agenda);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(agenda);
        }

        // GET: Agenda/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoAgendaPorId(id);
        }

        // POST: Agenda/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var agenda = await agendaDAL.InativarAgendaPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
        private async Task<IActionResult> ObterVisaoAgendaPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var agenda = await agendaDAL.ObterAgendaPorId((long)id);
            if (agenda == null)
            {
                return NotFound();
            }

            return View(agenda);
        }
    }
}