using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    //REVISADO_20200715
    [Area("Administracao")]
    [Authorize(Roles = RolesNomes.Administrador)]
    public class ColaboradorController : Controller
    {
        private readonly ClinicaDAL clinicaDAL;
        private readonly EnderecoDAL enderecoDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;        
        private readonly UserManager<AppIdentityUser> userManager;       

        public ColaboradorController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            clinicaDAL = new ClinicaDAL(context);
            enderecoDAL = new EnderecoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }

        // GET: Colaboradores
        public async Task<IActionResult> Index()
        {                                     
            return View(await colaboradorDAL.ObterColaboradoresClassificadosPorNome().ToListAsync());
        }

        // GET: Colaboradores
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

        // GET: Colaboradores
        public async Task<IActionResult> Edit(long id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

        // POST: Colaboradores/Edit
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
                    colaborador.IdUser = userManager.GetUserAsync(User).Result.Id;
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

        // GET: Colaboradores/Create
        public IActionResult Create()
        {
            CarregarViewBagsCreate();

            return View();
        }        

        // POST: Colaboradores/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColaboradorViewModel model)
        {          
            try
            {                
                if (model.Colaborador.Nome != null && model.Colaborador.CPF != null)
                {
                    await enderecoDAL.GravarEndereco(model.Endereco);

                    model.Colaborador.DtCadastro = DateTime.Today;
                    model.Colaborador.EnderecoId = model.Endereco.Id;
                    model.Colaborador.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await colaboradorDAL.GravarColaborador(model.Colaborador);

                    if (model.Colaborador.Funcao != EnumHelper.Funcao.Medico && model.Colaborador.Funcao != EnumHelper.Funcao.Psicologo)
                    {
                        await CadastrarNovoUsuario(model);
                    }

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Colaborador);
        }
     
        // GET: Colaboradores/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

        // POST: Colaboradores/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var colaborador = await colaboradorDAL.InativarColaboradorPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
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
            
            CarregarViewBagsEdit(colaborador);

            return View(colaborador);
        }

        private void CarregarViewBagsDetails(Colaborador colaborador)
        {
            ViewBag.ClinicaNome = colaborador.Clinica.Alias;
        }

        private void CarregarViewBagsEdit(Colaborador colaborador)
        {            
            ViewBag.Clinicas = new SelectList(clinicaDAL.ObterClinicasClassificadasPorNome(), "Id", "Alias", colaborador.Id);
        }

        private void CarregarViewBagsCreate()
        {
            var clinicas = clinicaDAL.ObterClinicasClassificadasPorNome().ToList();
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;
        }

        private async Task CadastrarNovoUsuario(ColaboradorViewModel model)
        {
            var primeiroNome = model.Colaborador.Nome.Substring(0, model.Colaborador.Nome.IndexOf(" "));

            var novoUsuario = new AppIdentityUser
            {
                UserName = model.Colaborador.Email,
                Email = model.Colaborador.Email,
                Nome = primeiroNome,
                ClinicaId = (long)model.Colaborador.ClinicaId,
                ColaboradorId = (long)model.Colaborador.Id,
            };

            string pwd = model.Colaborador.CPF.Replace(".", "").Replace("-", "");

            var createPowerUser = await userManager.CreateAsync(novoUsuario, pwd);

            if (createPowerUser.Succeeded)
            {
                if (model.Colaborador.Funcao == EnumHelper.Funcao.Administrador)
                {
                    await userManager.AddToRoleAsync(novoUsuario, RolesNomes.Administrador);
                }

                if (model.Colaborador.Funcao == EnumHelper.Funcao.Gestor)
                {
                    await userManager.AddToRoleAsync(novoUsuario, RolesNomes.Gestor);
                }

                if (model.Colaborador.Funcao == EnumHelper.Funcao.Operador)
                {
                    await userManager.AddToRoleAsync(novoUsuario, RolesNomes.Operador);
                }
            }
        }
    }
}