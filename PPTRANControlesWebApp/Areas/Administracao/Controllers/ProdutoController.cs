using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Data.DAL.Administracao;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    [Area("Administracao")]
    [Authorize]
    public class ProdutoController : Controller
    {
        private readonly UserManager<AppIdentityUser> _userManager;
        private readonly ApplicationContext _context;
        private readonly CaixaDAL caixaDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly ProdutoDAL produtoDAL;

        public ProdutoController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
            caixaDAL = new CaixaDAL(context);
            clienteDAL = new ClienteDAL(context);
            produtoDAL = new ProdutoDAL(context);
        }
        // GET: Produto
        public async Task<IActionResult> Index()
        {
            var produtos = await produtoDAL.ObterProdutos().ToListAsync();
            return View(produtos);
        }

        // GET: Produto/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Produto/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Produto/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Produto produto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    await produtoDAL.GravarProduto(produto);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(produto);
        }

        public IActionResult Carrinho()
        {           
            
            return  View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Carrinho(long id,CaixaViewModel model)
        {
            var cliente = await clienteDAL.ObterClientesPorId(id);
            try
            {
                if (ModelState.IsValid)
                {
                    await produtoDAL.GravarProduto(model.Produto);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Produto);
        }

        // GET: Produto/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            return await ObterVisaoProdutoPorId(id);
        }

        // POST: Produto/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Produto produto)
        {
            if (id != produto.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    await produtoDAL.GravarProduto(produto);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(produto);
        }

        // GET: Produto/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Produto/Delete/5
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

        private async Task<IActionResult> ObterVisaoProdutoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var produto = await produtoDAL.ObterProdutoPorId((long)id);
            if (produto == null)
            {
                return NotFound();
            }

            return View(produto);
        }
    }
}