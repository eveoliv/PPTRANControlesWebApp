using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Controllers
{
    public class ClienteController : Controller
    {
        private readonly Context _context;
        private readonly ClienteDAL clienteDAL;
        private readonly EnderecoDAL enderecoDAL;

        public ClienteController(Context context)
        {
            _context = context;
            clienteDAL = new ClienteDAL(context);
            enderecoDAL = new EnderecoDAL(context);
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

        // GET: Cliente/Entrevista->Details
        public async Task<IActionResult> Entrevista(long? id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {            
            var clinicas = _context.Clinicas.OrderBy(i => i.Nome).ToList();
            clinicas.Insert(0, new Clinica() { ClinicaId = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;

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
                    model.Cliente.EnderecoId = idEndereco.EnderecoId;
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
            if (id != cliente.ClienteId)
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

            ViewBag.Clinicas = 
                new SelectList(_context.Clinicas.OrderBy(b => b.Nome), "ClinicaId", "Alias", cliente.ClienteId);

            return View(cliente);
        }
    }
}