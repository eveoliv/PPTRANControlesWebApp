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

        public ClienteController(Context context)
        {
            _context = context;
            clienteDAL = new ClienteDAL(context);
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
                if (model.Cliente.Nome != null)
                {
                    _context.Add(model.Cliente);
                    await _context.SaveChangesAsync();

                    _context.Add(model.Endereco);
                    await _context.SaveChangesAsync();

                    var enderecoCliente = (from p in _context.Clientes where p.CPF == model.Cliente.CPF select p).Single();
                    enderecoCliente.EnderecoId = _context.Enderecos.Select(e => e.EnderecoId).Max();

                    _context.Update(model.Cliente);
                    await _context.SaveChangesAsync();


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
        public ActionResult Edit(long id)
        {
            return View();
        }

        // POST: Clientes/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Cliente cliente)
        {
           
            return RedirectToAction("Index");
        }

        // GET: Clientes/Delete/5
        public ActionResult Delete(long id)
        {
            return View();
        }

        // POST: Clientes/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(Cliente cliente)
        {
           

            return RedirectToAction("Index");
        }


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