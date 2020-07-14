using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Data.DAL.Financeiro;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Areas.Operacao.Controllers
{
    [Area("Operacao")]
    [Authorize]
    public class ClienteController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly ClienteDAL clienteDAL;
        private readonly EnderecoDAL enderecoDAL;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly ProdutoDAL produtoDAL;

        public ClienteController(ApplicationContext context)
        {
            _context = context;
            clienteDAL = new ClienteDAL(context);
            enderecoDAL = new EnderecoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
            produtoDAL = new ProdutoDAL(context);
        }

        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = await clienteDAL.ObterClientesPorNome().ToListAsync();
            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // GET: Cliente/Entrevista Psicologo->Details
        public async Task<IActionResult> EntrevistaPsi(long? id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // GET: Cliente/Entrevista Medicos->Details
        public async Task<IActionResult> EntrevistaMed(long? id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {            
            var clinicas = _context.Clinicas.OrderBy(i => i.Nome).ToList();
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;


            var medicos = _context.Colaboradores
                .Where(c => c.Funcao == EnumHelper.Funcao.Medico)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome).ToList();
            medicos.Insert(0, new Colaborador() { Id = 0, Nome = "Médico(a)" });
            ViewBag.Medicos = medicos;

            var psicologos = _context.Colaboradores
                .Where(c => c.Funcao == EnumHelper.Funcao.Psicologo)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome).ToList();
            psicologos.Insert(0, new Colaborador() { Id = 0, Nome = "Psicologo(a)" });
            ViewBag.Psicologos = psicologos;


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
                    var cpf = model.Cliente.CPF;

                    model.Endereco.CPF = cpf;
                    await enderecoDAL.GravarEndereco(model.Endereco);
                    var idEndereco = (from e in _context.Enderecos where e.CPF == cpf select e).Single();

                    model.Cliente.DtCadastro = DateTime.Today;
                    model.Cliente.EnderecoId = idEndereco.Id;
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

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(long id)
        {            
            return await ObterVisaoClientePorId(id);
        }

        // POST: Clientes/Edit/5
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

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // POST: Clientes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var cliente = await clienteDAL.ObterClientePorId(id);

            cliente.Status = EnumHelper.Status.Inativo;
            await clienteDAL.GravarCliente(cliente);
            return RedirectToAction(nameof(Index));
        }

        /*************************************************************************/
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

            ViewBag.MedicoNome = 
                colaboradorDAL.ObterColaboradorPorId((long)cliente.MedicoId).Result.Nome.ToString();

            ViewBag.PsicologoNome = 
                colaboradorDAL.ObterColaboradorPorId((long)cliente.PsicologoId).Result.Nome.ToString();
        }

        private void CarregarViewBagsComLista(Cliente cliente)
        {
            ViewBag.Clinicas =
                new SelectList(_context.Clinicas
                .OrderBy(b => b.Nome), "Id", "Alias", cliente.Id);

            ViewBag.Medico =
                new SelectList(_context.Colaboradores
                .Where(m => m.Funcao == EnumHelper.Funcao.Medico)
                .Where(m => m.Status == EnumHelper.Status.Ativo)
                .OrderBy(m => m.Nome), "Id", "Nome", cliente.Id);

            ViewBag.Psicologo =
                new SelectList(_context.Colaboradores
                .Where(m => m.Funcao == EnumHelper.Funcao.Psicologo)
                .Where(m => m.Status == EnumHelper.Status.Ativo)
                .OrderBy(m => m.Nome), "Id", "Nome", cliente.Id);
        }
    }
}