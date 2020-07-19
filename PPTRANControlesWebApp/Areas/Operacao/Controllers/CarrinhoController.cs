using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Operacao.Controllers
{
    [Area("Operacao")]
    [Authorize]
    public class CarrinhoController : Controller
    {
        private readonly CaixaDAL caixaDAL;
        private readonly ClienteDAL clienteDAL;                       
        private readonly ProdutoDAL produtoDAL;                       
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public CarrinhoController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            caixaDAL = new CaixaDAL(context);
            clienteDAL = new ClienteDAL(context);
            produtoDAL = new ProdutoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }
        // GET: Carrinho
        public ActionResult Index()
        {
            return View();
        }

        // GET: Carrinho/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: Carrinho/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Carrinho/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                // TODO: Add insert logic here

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: Carrinho/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: Carrinho/Edit/5
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

        // GET: Carrinho/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Carrinho/Delete/5
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
    }
}