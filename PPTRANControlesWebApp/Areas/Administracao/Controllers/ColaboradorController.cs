using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    [Area("Administracao")]
    [Authorize]
    public class ColaboradorController : Controller
    {
        private readonly UserManager<AppIdentityUser> userManager;       

        private readonly ApplicationContext _context;
        private readonly EnderecoDAL enderecoDAL;
        private readonly ColaboradorDAL colaboradorDAL;        

        public ColaboradorController(ApplicationContext context, 
            UserManager<AppIdentityUser> userManager)
        {
            this.userManager = userManager;           

            _context = context;
            enderecoDAL = new EnderecoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);

        }

        // GET: Colaboradores
        public async Task<IActionResult> Index()
        {
            var colaboradores = 
                await colaboradorDAL.ObterColaboradoresPorNome().ToListAsync();            
            return View(colaboradores);
        }

        // GET: Colaboradores/Details/5
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

        // GET: Colaboradores/Create
        public IActionResult Create()
        {
            var clinicas = _context.Clinicas.OrderBy(i => i.Nome).ToList();
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;                             

            return View();
        }

        // POST: Colaboradores/Create
        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColaboradorViewModel model)
        {
            var usuarioId = userManager.GetUserAsync(User).Result.Id;

            try
            {
                if (model.Colaborador.Nome != null && model.Colaborador.CPF != null)
                {
                    var cpf = model.Colaborador.CPF;

                    model.Endereco.CPF = cpf;
                    await enderecoDAL.GravarEndereco(model.Endereco);
                    var idEndereco = (from e in _context.Enderecos where e.CPF == cpf select e).FirstOrDefault();

                    model.Colaborador.IdUser = usuarioId;
                    model.Colaborador.DtCadastro = DateTime.Today;
                    model.Colaborador.Endereco.Id = idEndereco.Id;
                    await colaboradorDAL.GravarColaborador(model.Colaborador);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Colaborador);
        }


        // GET: Colaboradores/Edit/5
        public async Task<IActionResult> Edit(long id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

        // POST: Colaboradores/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Colaborador colaborador)
        {
            if (id != colaborador.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    await colaboradorDAL.GravarColaborador(colaborador);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(colaborador);
        }

        // GET: Colaboradores/Delete/5
        public async Task<IActionResult> Delete(long id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

        // POST: Colaboradores/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long id)
        {
            var colaborador = await colaboradorDAL.ObterColaboradorPorId(id);

            colaborador.Status = EnumHelper.Status.Inativo;
            await colaboradorDAL.GravarColaborador(colaborador);
            return RedirectToAction(nameof(Index));
        }


        /*************************************************************************/
        private async Task<IActionResult> ObterVisaoColaboradorPorId(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await colaboradorDAL.ObterColaboradorPorId((long)id);
            if (colaborador == null)
            {
                return NotFound();
            }

            CarregarViewBagsPorNome(colaborador);
            CarregarViewBagsComLista(colaborador);

            return View(colaborador);
        }

        private void CarregarViewBagsPorNome(Colaborador colaborador)
        {
            ViewBag.ClinicaNome = colaborador.Clinica.Alias;
        }

        private void CarregarViewBagsComLista(Colaborador colaborador)
        {
            ViewBag.Clinicas =
              new SelectList(_context.Clinicas.OrderBy(b => b.Nome), "Id", "Alias", colaborador.Id);
        }

    }
}