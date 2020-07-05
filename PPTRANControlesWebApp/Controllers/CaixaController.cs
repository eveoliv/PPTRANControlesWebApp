using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Controllers
{
    public class CaixaController : Controller
    {
        private readonly Context _context;
        private readonly CaixaDAL caixaDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly ColaboradorDAL colaboradorDAL;

        public CaixaController(Context context)
        {
            _context = context;
            caixaDAL = new CaixaDAL(context);
            clienteDAL = new ClienteDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }

        // GET: Caixa
        public async Task<IActionResult> Index()
        {
            var lancamentos = await caixaDAL.ObterLancamentos().ToListAsync();
            return View(lancamentos);
        }

        // GET: Caixa/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Caixa/Create
        public IActionResult Create()
        {
            var clinicas = _context.Clinicas.OrderBy(i => i.Nome).ToList();
            clinicas.Insert(0, new Clinica() { ClinicaId = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;
            return View();
        }

        // POST: Caixa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Data, CPF, Historico, Ref, Tipo, FormaPgto, Valor, Clinica")] CaixaViewModel model)
        {
            //var cpf = model.Caixa.Cliente.CPF;
            try
            {
                if (ModelState.IsValid /*&& ValidaCpfLancamento(cpf)*/)
                {
                    //var idCli = (from e in _context.Clientes where e.CPF == cpf select e).Single();

                    //model.Caixa.ClienteId = idCli.ClienteId;
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

        // GET: Caixa/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Caixa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
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

        // GET: Caixa/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Caixa/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        private bool ValidaCpfLancamento(string cpf)
        {
            var cliCpf = clienteDAL.ObterClientePorCPF(cpf);
            var colCpf = colaboradorDAL.ObterColaboradorPorCPF(cpf);

            if (cliCpf != null || colCpf != null)
            {
                return true;
            }
        
            return false;
        }
    }
}