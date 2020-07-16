using Models;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    //REVISADO_20200715
    [Area("Administracao")]
    [Authorize]
    public class ClinicaController : Controller
    {
        private readonly ClinicaDAL clinicaDAL;
        private readonly EnderecoDAL enderecoDAL;
        private readonly ApplicationContext context;
        private readonly UserManager<AppIdentityUser> userManager;

        public ClinicaController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            clinicaDAL = new ClinicaDAL(context);
            enderecoDAL = new EnderecoDAL(context);
        }

        // GET: Clinica
        public async Task<IActionResult> Index()
        {            
            return View(await clinicaDAL.ObterClinicasClassificadasPorNome().ToListAsync());
        }

        // GET: Clinica/Details
        public async Task<IActionResult> Details(int? id)
        {
            return await ObterVisaoClinicaPorId(id);
        }

        // GET: Clinica/Edit
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoClinicaPorId(id);
        }

        // POST: Clinica/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Clinica clinica)
        {
            if (id != clinica.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    clinica.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await clinicaDAL.GravarClinica(clinica);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(clinica);
        }

        // GET: Clinica/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Clinica/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task <IActionResult> Create(ClinicaViewModel model)
        {
            try
            {
                if (model.Clinica.Nome != null && model.Clinica.CNPJ != null)
                {                                       
                    await enderecoDAL.GravarEndereco(model.Endereco);

                    model.Clinica.EnderecoId = model.Endereco.Id;
                    model.Clinica.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await clinicaDAL.GravarClinica(model.Clinica);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Clinica);
        }
      
        // GET: Clinica/Delete
        public async Task <IActionResult> Delete(long? id)
        {
            return await ObterVisaoClinicaPorId(id);
        }

        // POST: Clinica/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var clinica = await clinicaDAL.InativarClinicaPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
        private async Task<IActionResult> ObterVisaoClinicaPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var clinica = await clinicaDAL.ObterClinicaPorId((long)id);
            if (clinica == null)
            {
                return NotFound();
            }

            return View(clinica);
        }
    }
}