using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Financeiro.Controllers
{
    //REVISADO_20200715
    [Area("Financeiro")]
    [Authorize]
    public class CaixaController : Controller
    {
        private readonly CaixaDAL caixaDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly HistoricoDAL historicoDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public CaixaController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            caixaDAL = new CaixaDAL(context);
            clienteDAL = new ClienteDAL(context);
            historicoDAL = new HistoricoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);

        }

        // GET: Caixa
        public async Task<IActionResult> Index()
        {
            var lancamentos = await caixaDAL.ObterLancamentosClassificadosPorCliente().ToListAsync();
            return View(lancamentos);
        }       

        // GET: Caixa/Details
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        // GET: Caixa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        // POST: Caixa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, IFormCollection collection)
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

        // GET: Caixa/Create
        public IActionResult Create()
        {
            CarregarViewBagsCreate();
            return View();
        }
       
        // POST: Caixa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaixaViewModel model)
        { 
            try
            {
                if (model.Caixa.Cliente.CPF != null)
                {                                      
                    model.Caixa.IdUser = userManager.GetUserAsync(User).Result.Id; ;

                    await caixaDAL.GravarLancamento(model.Caixa);

                    return RedirectToAction(nameof(Index));
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Caixa);
        }

        // GET: Caixa/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        // POST: Caixa/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var caixa = await caixaDAL.InativarLancamentoPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
        private async Task<IActionResult> ObterVisaoLancamentoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caixa = await caixaDAL.ObterLancamentoPorId((long)id);
            if (caixa == null)
            {
                return NotFound();
            }

            CarregarViewBagsDetails(caixa);

            return View(caixa);
        }

        private void CarregarViewBagsDetails(Caixa caixa)
        {
            ViewBag.Clinica = caixa.Clinica.Alias;
            ViewBag.Cliente = caixa.Cliente.Nome;
            ViewBag.Colaborador = userManager.FindByIdAsync(caixa.IdUser).Result.Nome;
        }

        private void CarregarViewBagsCreate()
        {
            var clinicas = context.Clinicas.OrderBy(i => i.Nome).ToList();
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;

            var historico = context.Historicos.OrderBy(h => h.Nome).ToList();
            historico.Insert(0, new Historico() { Id = 0, Nome = "Historico" });
            ViewBag.Historicos = historico;
        }
    }
}