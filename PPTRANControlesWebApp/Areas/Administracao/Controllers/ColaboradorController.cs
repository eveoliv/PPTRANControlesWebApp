using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Models.Administracao;
using PPTRANControlesWebApp.Areas.Identity.Models;

namespace PPTRANControlesWebApp.Areas.Administracao.Controllers
{
    //REVISADO_20200715
    [Area("Administracao")]
    [Authorize]
    public class ColaboradorController : Controller
    {
        private readonly ClinicaDAL clinicaDAL;
        private readonly EnderecoDAL enderecoDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;
        static bool verificaColab = false;

        public ColaboradorController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            clinicaDAL = new ClinicaDAL(context);
            enderecoDAL = new EnderecoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var lista = await colaboradorDAL.ObterColaboradoresClassificadosPorNome().ToListAsync();

            if (roleUser.FirstOrDefault() == RolesNomes.Administrador)
            {
                lista = lista.Where(c => c.Id != 1).ToList();
            }

            if (roleUser.FirstOrDefault() == RolesNomes.Gestor)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lista = lista.Where(c => c.ClinicaId == userClinicaId && c.Funcao != EnumHelper.Funcao.Administrador).ToList();
            }

            if (roleUser.FirstOrDefault() == RolesNomes.Operador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lista = lista.Where(c => c.ClinicaId == userClinicaId &&
                (c.Funcao == EnumHelper.Funcao.Medico || c.Funcao == EnumHelper.Funcao.Psicologo)).ToList();
            }

            return View(lista);
        }

        [Authorize(Roles = RolesNomes.Administrador)]
        public async Task<IActionResult> Alterar()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            return await ObterVisaoColaboradorPorId(usuario.ColaboradorId);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Alterar(long? id, Colaborador colaborador)
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

                return RedirectToAction("Resposta", "Colaborador");
            }
            return View();
        }

        public IActionResult Resposta()
        {
            return View();
        }

        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }


        [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor)]
        public async Task<IActionResult> Edit(long id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

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

        [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor)]
        public async Task<IActionResult> Create()
        {

            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);
            CarregarViewBagsCreate(roleUser);

            if (verificaColab) ViewBag.Msg = " - Este colaborador já esta cadastrado!";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ColaboradorViewModel model)
        {
            try
            {
                var colabExiste = ValidaColaboradorCreate(model.Colaborador.CPF);

                if (model.Colaborador.Nome != null && colabExiste == false)
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
            verificaColab = true;
            return RedirectToAction("Create", "Colaborador");
        }

        [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor)]
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoColaboradorPorId(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var colaborador = await colaboradorDAL.InativarColaboradorPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        /****** Metodos Privados do Controller ******/
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

        private void CarregarViewBagsCreate(IList<string> roleUser)
        {
            var userId = userManager.GetUserAsync(User).Result.ColaboradorId;

            var clinicas = clinicaDAL.ObterClinicasClassificadasPorNome().ToList();
            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var idClinica = colaboradorDAL.ObterColaboradorPorId(userId).Result.ClinicaId;
                clinicas = clinicas.Where(c => c.Id == idClinica).ToList();
            }
            //clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;
        }

        private async Task CadastrarNovoUsuario(ColaboradorViewModel model)
        {
            var primeiroNome = model.Colaborador.Nome;

            char[] espaco = { ' ' };
            var check = primeiroNome.IndexOfAny(espaco);

            if (check > 0)
            {
                primeiroNome = primeiroNome.Substring(0, model.Colaborador.Nome.IndexOf(" "));
            }

            var novoUsuario = new AppIdentityUser
            {
                UserName = model.Colaborador.Email,
                Email = model.Colaborador.Email,
                Nome = primeiroNome,
                ClinicaId = (long)model.Colaborador.ClinicaId,
                ColaboradorId = (long)model.Colaborador.Id,
            };

            //string pwd = model.Colaborador.CPF.Replace(".", "").Replace("-", "");          
            string pwd = DateTime.Today.Year + model.Colaborador.Email;

            var createPowerUser = await userManager.CreateAsync(novoUsuario, pwd);

            if (createPowerUser.Succeeded)
            {
                //Cadastrar novo colab como Gestor, caso seje selecionaro Adm
                //esta alteração deverea ser realizada na gestão de acesso pelo Adm
                if (model.Colaborador.Funcao == EnumHelper.Funcao.Administrador)
                {
                    await userManager.AddToRoleAsync(novoUsuario, RolesNomes.Gestor);
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

        private bool ValidaColaboradorCreate(string cpf)
        {
            try
            {
                var verifica = colaboradorDAL.ObterColaboradorPorCpf(cpf).Result.Id;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}