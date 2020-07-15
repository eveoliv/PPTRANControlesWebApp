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
    [Area("Operacao")]
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ClienteDAL clienteDAL;
        private readonly ProdutoDAL produtoDAL;
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
            produtoDAL = new ProdutoDAL(context);
            enderecoDAL = new EnderecoDAL(context);
            historicoDAL = new HistoricoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }

        // GET: Clientes/
        public async Task<IActionResult> Index()
        {
            var clientes = await clienteDAL.ObterClientesPorNome().ToListAsync();
            return View(clientes);
        }

        // GET: Clientes/Details/
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // GET: Cliente/EntrevistaPsicologo/
        public async Task<IActionResult> EntrevistaPsi(long? id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // GET: Cliente/EntrevistaMedicos/
        public async Task<IActionResult> EntrevistaMed(long? id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // GET: Clientes/Create/
        public IActionResult Create()
        {            
            var clinicas = context.Clinicas.OrderBy(i => i.Nome).ToList();
                clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;

            var medicos = context.Colaboradores
                .Where(c => c.Funcao == EnumHelper.Funcao.Medico)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome).ToList();
                medicos.Insert(0, new Colaborador() { Id = 0, Nome = "Médico(a)" });
            ViewBag.Medicos = medicos;

            var psicologos = context.Colaboradores
                .Where(c => c.Funcao == EnumHelper.Funcao.Psicologo)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome).ToList();
                psicologos.Insert(0, new Colaborador() { Id = 0, Nome = "Psicologo(a)" });
            ViewBag.Psicologos = psicologos;

            var historicos = context.Historicos.OrderBy(h => h.Nome).ToList();
                historicos.Insert(0, new Historico() { Id = 0, Nome = "Histórico" });
            ViewBag.Historicos = historicos;

            return View();
        }

        // POST: Clientes/Create/OK!!!
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

        // GET: Clientes/Edit/
        public async Task<IActionResult> Edit(long id)
        {            
            return await ObterVisaoClientePorId(id);
        }

        // POST: Clientes/Edit/
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

        // GET: Clientes/Delete/
        public async Task<IActionResult> Delete(long id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // POST: Clientes/Delete/
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var cliente = await clienteDAL.ObterClientePorId(id);

            cliente.Status = EnumHelper.Status.Inativo;
            await clienteDAL.GravarCliente(cliente);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
        private async Task<IActionResult> ObterVisaoClientePorId(long? id)
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

            CarregarViewBagsPorNome(cliente);

            CarregarViewBagsComLista(cliente);

            return View(cliente);
        }

        private void CarregarViewBagsPorNome(Cliente cliente)
        {
            ViewBag.ClinicaNome = 
                cliente.Clinica.Alias.ToString();

            ViewBag.HistoricoNome = 
                cliente.Historico.Nome.ToString();

            ViewBag.MedicoNome = 
                colaboradorDAL.ObterColaboradorPorId((long)cliente.MedicoId).Result.Nome.ToString();

            ViewBag.PsicologoNome = 
                colaboradorDAL.ObterColaboradorPorId((long)cliente.PsicologoId).Result.Nome.ToString();
            
        }

        private void CarregarViewBagsComLista(Cliente cliente)
        {
            ViewBag.Clinicas =
                new SelectList(context.Clinicas
                .OrderBy(b => b.Nome), "Id", "Alias", cliente.Id);

            ViewBag.Medico =
                new SelectList(context.Colaboradores
                .Where(m => m.Funcao == EnumHelper.Funcao.Medico)
                .Where(m => m.Status == EnumHelper.Status.Ativo)
                .OrderBy(m => m.Nome), "Id", "Nome", cliente.Id);

            ViewBag.Psicologo =
                new SelectList(context.Colaboradores
                .Where(m => m.Funcao == EnumHelper.Funcao.Psicologo)
                .Where(m => m.Status == EnumHelper.Status.Ativo)
                .OrderBy(m => m.Nome), "Id", "Nome", cliente.Id);

            ViewBag.Historico = 
                new SelectList(context.Historicos
                .OrderBy(h => h.Nome), "Id", "Nome", cliente.Id);
        }
    }
}