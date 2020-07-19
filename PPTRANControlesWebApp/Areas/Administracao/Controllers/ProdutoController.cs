using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    //REVISADO_20200715
    [Area("Administracao")]
    [Authorize]
    public class ProdutoController : Controller
    {       
        private readonly ProdutoDAL produtoDAL;       
        private readonly ApplicationContext context;
        private readonly UserManager<AppIdentityUser> userManager;

        public ProdutoController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;            
            produtoDAL = new ProdutoDAL(context);
        }

        // GET: Produto
        public async Task<IActionResult> Index()
        {
            return View(await produtoDAL.ObterProdutosClassificadosPorNome().ToListAsync());
        }

        // GET: Produto/Details
        public async Task<IActionResult> Details(int? id)
        {
            return await ObterVisaoProdutoPorId(id);
        }

        // GET: Produto/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoProdutoPorId(id);
        }

        // POST: Produto/Edit
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
                    produto.IdUser = userManager.GetUserAsync(User).Result.Id;
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

        // GET: Produto/Create
        public ActionResult Create()
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
                if (produto.Nome != null)
                {
                    produto.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await produtoDAL.GravarProduto(produto);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(produto);
        }

        // GET: Produto/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoProdutoPorId(id);
        }

        // POST: Produto/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var produto = await produtoDAL.InativarProdutoPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
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