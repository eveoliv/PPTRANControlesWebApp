using Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Models.Operacao;
using PPTRANControlesWebApp.Data.DAL.Operacao;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;

namespace PPTRANControlesWebApp.Areas.Operacao.Controllers
{
    //REVISADO_20200715
    [Area("Operacao")]
    [Authorize]
    public class AgendaController : Controller
    {
        private readonly AgendaDAL agendaDAL;
        private readonly ClinicaDAL clinicaDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public AgendaController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            agendaDAL = new AgendaDAL(context);
            clinicaDAL = new ClinicaDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }
        
        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserAsync(User).Result.ColaboradorId;
            var agenda = await agendaDAL.ObterAgendaClassificadaNomeCliente().ToListAsync();

            if (userId != 0)
            {
                var userClinicaAlias = colaboradorDAL.ObterColaboradorPorId(userId).Result.Clinica.Alias;
                agenda = agenda.Where(c => c.Clinica == userClinicaAlias).ToList();
            }

            return View(agenda);
        }
        
        public async Task<IActionResult> Details(int? id)
        {
            return await ObterVisaoAgendaPorId(id);
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoAgendaPorId(id);
        }
        
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
       
        public ActionResult Create()
        {           
            CarregarViewBagsCreate();
            return View();
        }    
       
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AgendaViewModel model)
        {
            try
            {
                if (model.Agenda.Data != null && model.Agenda.Nome != null)
                {                              
                    model.Agenda.Clinica = BuscaClinica(model.Agenda.Clinica);
                    model.Agenda.Medico = BuscaColaborador(model.Agenda.Medico);
                    model.Agenda.Psicologo = BuscaColaborador(model.Agenda.Psicologo);
                    model.Agenda.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await agendaDAL.GravarAgenda(model.Agenda);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Agenda);
        }
        
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoAgendaPorId(id);
        }
        
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var agenda = await agendaDAL.InativarAgendaPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        /****** Metodos Privados do Controller ******/
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

        private void CarregarViewBagsCreate()
        {
            var userId = userManager.GetUserAsync(User).Result.ColaboradorId;
            var idClinica = colaboradorDAL.ObterColaboradorPorId(userId).Result.ClinicaId;

            var clinicas = clinicaDAL.ObterClinicasClassificadasPorNome().ToList();
            if (userId != 0)
            {
                clinicas = clinicas.Where(c => c.Id == idClinica).ToList();
            }
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;

            var medicos = colaboradorDAL.ObterMedicosClassificadosPorNome().ToList();
            if (userId != 0)
            {
                medicos = medicos.Where(m => m.ClinicaId == idClinica).ToList();
            }
            medicos.Insert(0, new Colaborador() { Id = 0, Nome = "Médico(a)" });
            ViewBag.Medicos = medicos;

            var psicologos = colaboradorDAL.ObterPsicologosClassificadosPorNome().ToList();
            if (userId != 0)
            {
                psicologos = psicologos.Where(p => p.ClinicaId == idClinica).ToList();
            }
            psicologos.Insert(0, new Colaborador() { Id = 0, Nome = "Psicologo(a)" });
            ViewBag.Psicologos = psicologos;
        }

        private string BuscaColaborador(string id)
        {
            var col = colaboradorDAL.ObterColaboradorPorId(Convert.ToInt64(id)).Result.Nome;
            return col;
        }

        private string BuscaClinica(string id)
        {
            var cli = clinicaDAL.ObterClinicaPorId(Convert.ToInt64(id)).Result.Alias;
            return cli;
        }

    }
}