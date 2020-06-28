using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        private readonly EntrevistaDAL entrevistaDAL;

        public ClienteController(Context context)
        {
            _context = context;
            clienteDAL = new ClienteDAL(context);
            enderecoDAL = new EnderecoDAL(context);
            entrevistaDAL = new EntrevistaDAL(context);
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

        // GET: Clientes/Create
        public IActionResult Create()
        {
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

                    model.Entrevista.CPF = cpf;
                    await entrevistaDAL.GravaEntrevista(model.Entrevista);
                    var idEntrevista = (from e in _context.Entrevistas where e.CPF == cpf select e).Single();


                    model.Cliente.EnderecoId = idEndereco.EnderecoId;
                    model.Cliente.EntrevistaId = idEntrevista.EntrevistaId;
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
        public ActionResult Edit(Cliente cliente)
        {

            return RedirectToAction("Index");
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            return await ObterVisaoClientePorId(id);
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Cliente cliente)
        {
            return RedirectToAction("Index");
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

            return View(cliente);
        }
    }
}