using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Operacao.Controllers
{
    //REVISADO_20200715
    [Area("Operacao")]
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ClienteDAL clienteDAL;
        private readonly ClinicaDAL clinicaDAL;        
        private readonly EnderecoDAL enderecoDAL;
        private readonly HistoricoDAL historicoDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public ClienteController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            clienteDAL = new ClienteDAL(context);
            clinicaDAL = new ClinicaDAL(context);            
            enderecoDAL = new EnderecoDAL(context);
            historicoDAL = new HistoricoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {          
            return View(await clienteDAL.ObterClientesClassificadosPorNome().ToListAsync());
        }

        public ActionResult partialview()
        {
            return partialview();
        }

        // GET: Clientes/Details
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoClientePorId(id, "Detail");
        }
       
        // GET: Clientes/Edit
        public async Task<IActionResult> Edit(long? id)
        {            
            return await ObterVisaoClientePorId(id, "Edit");
        }

        // POST: Clientes/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    cliente.IdUser = userManager.GetUserAsync(User).Result.Id;                    
                    await clienteDAL.GravarCliente(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        // GET: Clientes/Create/
        public IActionResult Create()
        {
            CarregarViewBagsCreate();

            return View();
        }
  
        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel model)
        {
            try
            {
                if (model.Cliente.Nome != null && model.Cliente.CPF != null)
                {                                      
                    await enderecoDAL.GravarEndereco(model.Endereco);

                    model.Cliente.DtCadastro = DateTime.Today;
                    model.Cliente.EnderecoId = model.Endereco.Id;
                    model.Cliente.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await clienteDAL.GravarCliente(model.Cliente);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Cliente);
        }

        // GET: Clientes/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoClientePorId(id, "");
        }

        // POST: Clientes/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var cliente = await clienteDAL.InativarClientePorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Entrevistas do Médico e Psicólogo
        public async Task<IActionResult> EntrevistaPsi(long? id)
        {
            return await ObterVisaoClientePorId(id, "");
        }
       
        public async Task<IActionResult> EntrevistaMed(long? id)
        {
            return await ObterVisaoClientePorId(id, "");
        }

        // Metodos Privados do Controller
        private async Task<IActionResult> ObterVisaoClientePorId(long? id, string chamada)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await clienteDAL.ObterClientePorId((long)id);
            if (cliente == null)
            {
                return NotFound();
            }

            if (chamada == "Detail")            
                CarregarViewBagsDetails(cliente);

            if (chamada == "Edit")
                CarregarViewBagsEdit(cliente);

            return View(cliente);
        }

        private void CarregarViewBagsDetails(Cliente cliente)
        {
            /*
            ViewBag.ClinicaNome = cliente.Clinica.Alias.ToString();
            ViewBag.HistoricoNome = cliente.Historico.Nome.ToString();
            */

            ViewBag.MedicoNome = colaboradorDAL.ObterColaboradorPorId((long)cliente.MedicoId).Result.Nome.ToString();

            ViewBag.PsicologoNome = colaboradorDAL.ObterColaboradorPorId((long)cliente.PsicologoId).Result.Nome.ToString();            
        }

        private void CarregarViewBagsEdit(Cliente cliente)
        {
            ViewBag.Clinicas = new SelectList(clinicaDAL.ObterClinicasClassificadasPorNome(), "Id", "Alias", cliente.Id);

            ViewBag.Medico = new SelectList(colaboradorDAL.ObterMedicosClassificadosPorNome(), "Id", "Nome", cliente.Id);

            ViewBag.Psicologo = new SelectList(colaboradorDAL.ObterPsicologosClassificadosPorNome(), "Id", "Nome", cliente.Id);

            ViewBag.Historico = new SelectList(historicoDAL.ObterHistoricosClassificadosPorNome(), "Id", "Nome", cliente.Id);
        }

        private void CarregarViewBagsCreate()
        {            
            var clinicas = clinicaDAL.ObterClinicasClassificadasPorNome().ToList();
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;

            var medicos = colaboradorDAL.ObterMedicosClassificadosPorNome().ToList();
            medicos.Insert(0, new Colaborador() { Id = 0, Nome = "Médico(a)" });
            ViewBag.Medicos = medicos;

            var psicologos = colaboradorDAL.ObterPsicologosClassificadosPorNome().ToList();
            psicologos.Insert(0, new Colaborador() { Id = 0, Nome = "Psicologo(a)" });
            ViewBag.Psicologos = psicologos;

            var historicos = historicoDAL.ObterHistoricosClassificadosPorNome().ToList();
            historicos.Insert(0, new Historico() { Id = 0, Nome = "Histórico" });
            ViewBag.Historicos = historicos;
        }
    }
}