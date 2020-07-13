using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting.Internal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Areas.Financeiro.Controllers
{
    [Area("Financeiro")]
    [Authorize]
    public class CaixaController : Controller
    {
        private readonly ApplicationContext _context;
        private readonly CaixaDAL caixaDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> _userManager;

        public CaixaController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
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
        public async Task<IActionResult> Details(long id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        // GET: Caixa/Create
        public IActionResult Create()
        {
            var clinicas = _context.Clinicas.OrderBy(i => i.Nome).ToList();
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;
            
            return View();
        }

        // POST: Caixa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaixaViewModel model)
        {
            var cpf = model.Caixa.Cliente.CPF;
            var usuarioId = _userManager.GetUserAsync(User).Result.Id;

            //var idCol = (from c in _context.Colaboradores where c.CPF == usuarioCpf select c).FirstOrDefault();
            var idCli = (from c in _context.Clientes where c.CPF == cpf select c).FirstOrDefault();            

            try
            {
                if (model.Caixa.Cliente.CPF != null && idCli != null )
                {
                    //model.Caixa.ColaboradorId = idCol.ColaboradorId; Id do usuario logado
                    //model.Caixa.CpfUser = usuarioCpf;
                    model.Caixa.IdUser = usuarioId;
                    model.Caixa.ClienteId = idCli.Id;
                    model.Caixa.ClinicaId = model.Clinica.Id;
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

        /*********************************************************************/
        private bool ValidaCpfLancamento(string cpf)
        {
            var cliCpf = clienteDAL.ObterClientePorCpf(cpf);
            var colCpf = colaboradorDAL.ObterColaboradorPorCPF(cpf);

            if (cliCpf != null || colCpf != null)
            {
                return true;
            }
        
            return false;
        }

        /*************************************************************************/
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

            ViewBag.Clinica = caixa.Clinica.Alias;
            ViewBag.Cliente = caixa.Cliente.Nome;                   
            ViewBag.Colaborador = _userManager.FindByIdAsync(caixa.IdUser).Result.Nome;

            return View(caixa);
        }
    }
}