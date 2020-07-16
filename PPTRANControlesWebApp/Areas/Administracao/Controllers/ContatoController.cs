using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Models.Administracao;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    //REVISADO_20200715
    [Area("Administracao")]
    [Authorize]
    public class ContatoController : Controller
    {
        private readonly ContatoDAL contatoDAL;
        private readonly EnderecoDAL enderecoDAL;
        private readonly ApplicationContext context;
        private readonly UserManager<AppIdentityUser> userManager;

        public ContatoController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            contatoDAL = new ContatoDAL(context);
            enderecoDAL = new EnderecoDAL(context);
        }

        // GET: Contato
        public async Task<IActionResult> Index()
        {
            return View(await contatoDAL.ObterContatosClassificadosPorNome().ToListAsync());
        }

        // GET: Contato/Details
        public async Task<IActionResult> Details(int? id)
        {
            return await ObterVisaoContatoPorId(id);
        }

        // GET: Contato/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoContatoPorId(id);
        }

        // POST: Contato/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Contato contato)
        {
            if (id != contato.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    contato.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await contatoDAL.GravarContato(contato);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(contato);
        }

        // GET: Contato/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Contato/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ContatoViewModel model)
        {
            try
            {
                if (model.Contato.Nome != null)
                {
                    await enderecoDAL.GravarEndereco(model.Endereco);

                    model.Contato.EnderecoId = model.Endereco.Id;
                    model.Contato.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await contatoDAL.GravarContato(model.Contato);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Contato);
        }

        // GET: Contato/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoContatoPorId(id);
        }

        // POST: Contato/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var contato = await contatoDAL.EliminarContatoPorId((long)id);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
        private async Task<IActionResult> ObterVisaoContatoPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contato = await contatoDAL.ObterContatoPorId((long)id);
            if (contato == null)
            {
                return NotFound();
            }

            return View(contato);
        }
    }
}
