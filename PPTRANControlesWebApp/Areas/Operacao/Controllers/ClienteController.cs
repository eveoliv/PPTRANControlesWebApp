using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using System.Collections.Generic;
using PPTRANControlesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using PPTRANControlesWebApp.Data.DAL.Administracao;
using PPTRANControlesWebApp.Models.Operacao;

namespace PPTRANControlesWebApp.Areas.Operacao.Controllers
{
    //REVISADO_20200715   
    [Area("Operacao")]
    [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor + "," + RolesNomes.Operador)]
    public class ClienteController : Controller
    {
        private readonly ClienteDAL clienteDAL;
        private readonly ClinicaDAL clinicaDAL;
        private readonly EnderecoDAL enderecoDAL;
        private readonly HistoricoDAL historicoDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;
        static bool verificaCliente = false;
        static bool validaCpf = false;

        public ClienteController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            clienteDAL = new ClienteDAL(context);
            clinicaDAL = new ClinicaDAL(context);
            enderecoDAL = new EnderecoDAL(context);
            historicoDAL = new HistoricoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }

        public async Task<IActionResult> Index()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            //Data cadastro, clinica, nome, cpf, telefone, pgto realizado, opções

            var lista = await clienteDAL.ObterClientesClassificadosPorNomeNoMes().ToListAsync();

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lista = lista.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

            return View(lista);
        }

        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoClientePorId(id, "Detail");
        }

        public async Task<IActionResult> Edit(long? id)
        {
            return await ObterVisaoClientePorId(id, "Edit");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Cliente cliente)
        {
            if (id != cliente.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    Convert.ToDateTime(cliente.DtHabHum);
                    Convert.ToDateTime(cliente.DtNascimento);
                    cliente.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await clienteDAL.GravarCliente(cliente);
                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(cliente);
        }

        public async Task<IActionResult> Create()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            CarregarViewBagsCreate(roleUser);

            if (verificaCliente) ViewBag.CpfExistente = "  -  O CPF informado já esta cadastrado!";

            if (validaCpf) ViewBag.CpfInvalido = "  -  O CPF informado não é um CPF válido!";

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ClienteViewModel model)
        {
            var cpfValido = ValidadorDeCpf.IsCpf(model.Cliente.CPF);

            if ( !cpfValido )
            {
                validaCpf = true;
                return RedirectToAction("Create", "Cliente");
            }

            try
            {
                var clienteExiste = ValidaClienteCreate(model.Cliente.CPF);                

                if (model.Cliente.Nome != null && clienteExiste == false)
                {
                    await enderecoDAL.GravarEndereco(model.Endereco);

                    model.Cliente.DtCadastro = DateTime.Today;
                    model.Cliente.EnderecoId = model.Endereco.Id;
                    model.Cliente.IdUser = userManager.GetUserAsync(User).Result.Id;

                    await clienteDAL.GravarCliente(model.Cliente);

                    return RedirectToAction(nameof(Index));
                }
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }

            verificaCliente = true;
            return RedirectToAction("Create", "Cliente");
        }

        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoClientePorId(id, "");
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var cliente = await clienteDAL.InativarClientePorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> EntrevistaPsi(long? id)
        {
            return await ObterVisaoClientePorId(id, "");
        }

        public async Task<IActionResult> EntrevistaMed(long? id)
        {
            return await ObterVisaoClientePorId(id, "");
        }

        private static bool viewEdit = false;
        public async Task<IActionResult> EntrevistaMedPCD(long? id)
        {
            CarregarViewBagsFormularioPCD();
            return await ObterVisaoClientePorId(id, "");
        }

        private static IList<string> EditarFormPCD = new List<string>();
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult EditEntrevistaPCD(FormPdcViewModel form)
        {
            EditarFormPCD.Clear();
            EditarFormPCD.Add(form.Posto);
            EditarFormPCD.Add(form.Cnpj);

            EditarFormPCD.Add(form.Medico1);
            EditarFormPCD.Add(form.CRM1);
            EditarFormPCD.Add(form.Port1);
            EditarFormPCD.Add(form.Espec1);
            EditarFormPCD.Add(form.Aut1);
            EditarFormPCD.Add(form.CPF1);

            EditarFormPCD.Add(form.Medico2);
            EditarFormPCD.Add(form.CRM2);
            EditarFormPCD.Add(form.Port2);
            EditarFormPCD.Add(form.Espec2);
            EditarFormPCD.Add(form.Aut2);
            EditarFormPCD.Add(form.CPF2);

            viewEdit = true;
            return RedirectToRoute(new { controller = "Cliente", action = "EntrevistaMedPCD", id = form.Id });
        }

        public async Task<IActionResult> LaudoMedicoPCD(long? id)
        {
            CarregarViewBagsFormularioPCD();
            return await ObterVisaoClientePorId(id, "");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LaudoMedicoPCD(FormPdcViewModel form)
        {
            EditarFormPCD.Clear();
            EditarFormPCD.Add(form.Posto);
            EditarFormPCD.Add(form.Cnpj);

            EditarFormPCD.Add(form.Medico1);
            EditarFormPCD.Add(form.CRM1);
            EditarFormPCD.Add(form.Port1);
            EditarFormPCD.Add(form.Espec1);
            EditarFormPCD.Add(form.Aut1);
            EditarFormPCD.Add(form.CPF1);

            EditarFormPCD.Add(form.Medico2);
            EditarFormPCD.Add(form.CRM2);
            EditarFormPCD.Add(form.Port2);
            EditarFormPCD.Add(form.Espec2);
            EditarFormPCD.Add(form.Aut2);
            EditarFormPCD.Add(form.CPF2);

            viewEdit = true;
            return RedirectToRoute(new { controller = "Cliente", action = "LaudoMedicoPCD", id = form.Id });
        }

        public IActionResult Pesquisa()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Pesquisa(PesquisaViewModel model)
        {
            var diasDif = (model.DtFim - model.DtInicio);           

            if ( (diasDif.Days + 1) > 31)
            {
                ViewBag.ErroData = "A pesquisa por DATA deve ter um intervalo de no máximo 31 dias.";
                return View();
            }
            
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var lista =
                clienteDAL.ObterHistoricoDeClientes(model.Nome, model.Cpf, model.DtInicio, model.DtFim).ToList();

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lista = lista.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

            if (model.Nome != null && lista.Count > 100)
            {
                ViewBag.ErroNome = "A pesquisa por NOME ultrapassou 100 registros, utilize Nome e Sobrenome para refinar a busca.";
                return View();
            }

            return View(lista);
        }

        /****** Metodos Privados do Controller ******/
        private async Task<IActionResult> ObterVisaoClientePorId(long? id, string chamada)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await clienteDAL.ObterClientePorId((long)id);
            if (cliente == null)
            {
                return NotFound();
            }

            if (chamada == "Detail")
                CarregarViewBagsDetails(cliente);

            if (chamada == "Edit")
                CarregarViewBagsEdit(cliente);

            ViewBag.Data = DataBuilder.PtBr_Data();
            return View(cliente);
        }

        private void CarregarViewBagsDetails(Cliente cliente)
        {
            if (cliente.MedicoId >= 1)
            {
                ViewBag.MedicoNome = colaboradorDAL.ObterColaboradorPorId((long)cliente.MedicoId).Result.Nome.ToString();
            }
            else
            {
                ViewBag.MedicoNome = "NÃO SELECIONADO";
            }


            if (cliente.PsicologoId >= 1)
            {
                ViewBag.PsicologoNome = colaboradorDAL.ObterColaboradorPorId((long)cliente.PsicologoId).Result.Nome.ToString();
            }
            else
            {
                ViewBag.PsicologoNome = "NÃO SELECIONADO";
            }
        }

        private void CarregarViewBagsEdit(Cliente cliente)
        {
            ViewBag.Clinicas
                = new SelectList(clinicaDAL.ObterClinicasClassificadasPorNome(), "Id", "Alias", cliente.Id);

            ViewBag.Medico
                = new SelectList(colaboradorDAL.ObterMedicosClassificadosPorNome(), "Id", "Nome", cliente.Id);

            ViewBag.Psicologo
                = new SelectList(colaboradorDAL.ObterPsicologosClassificadosPorNome(), "Id", "Nome", cliente.Id);

            ViewBag.Historico
                = new SelectList(historicoDAL.ObterHistoricosClassificadosPorNome(), "Id", "Nome", cliente.Id);
        }

        private void CarregarViewBagsCreate(IList<string> userRole)
        {
            var userId = userManager.GetUserAsync(User).Result.ColaboradorId;

            var clinicas = clinicaDAL.ObterClinicasClassificadasPorNome().ToList();
            if (userRole.FirstOrDefault() != RolesNomes.Administrador)
            {
                var idClinica = colaboradorDAL.ObterColaboradorPorId(userId).Result.ClinicaId;
                clinicas = clinicas.Where(c => c.Id == idClinica).ToList();
            }
            //clinicas.Insert(0, new Clinica() { Id = 0, Alias = "Clinica" });
            ViewBag.Clinicas = clinicas;

            var medicos = colaboradorDAL.ObterMedicosClassificadosPorNome().ToList();
            if (userRole.FirstOrDefault() != RolesNomes.Administrador)
            {
                var idClinica = colaboradorDAL.ObterColaboradorPorId(userId).Result.ClinicaId;
                medicos = medicos.Where(m => m.ClinicaId == idClinica).ToList();
            }
            //medicos.Insert(0, new Colaborador() { Id = 0, Nome = "Médico(a)" });
            ViewBag.Medicos = medicos;

            var psicologos = colaboradorDAL.ObterPsicologosClassificadosPorNome().ToList();
            if (userRole.FirstOrDefault() != RolesNomes.Administrador)
            {
                var idClinica = colaboradorDAL.ObterColaboradorPorId(userId).Result.ClinicaId;
                psicologos = psicologos.Where(p => p.ClinicaId == idClinica).ToList();
            }
            //psicologos.Insert(0, new Colaborador() { Id = 0, Nome = "Psicologo(a)" });
            ViewBag.Psicologos = psicologos;

            var historicos = historicoDAL.ObterHistoricosClassificadosPorNome().Where(h => h.Id < 10).ToList();
            //historicos.Insert(0, new Historico() { Id = 0, Nome = "Histórico" });
            ViewBag.Historicos = historicos;
        }

        private void CarregarViewBagsFormularioPCD()
        {
            if (viewEdit == true)
            {
                ViewBag.UnidEmissora = EditarFormPCD[0];
                ViewBag.UnidCnpjf = EditarFormPCD[1];

                ViewBag.Med1Nome = EditarFormPCD[2];
                ViewBag.Med1CRM = EditarFormPCD[3];
                ViewBag.Med1Port = EditarFormPCD[4];
                ViewBag.Med1Espec = EditarFormPCD[5];
                ViewBag.Med1Aut = EditarFormPCD[6];
                ViewBag.Med1CPF = EditarFormPCD[7];

                ViewBag.Med2Nome = EditarFormPCD[8];
                ViewBag.Med2CRM = EditarFormPCD[9];
                ViewBag.Med2Port = EditarFormPCD[10];
                ViewBag.Med2Espec = EditarFormPCD[11];
                ViewBag.Med2Aut = EditarFormPCD[12];
                ViewBag.Med2CPF = EditarFormPCD[13];
                viewEdit = false;
            }
            else
            {
                ViewBag.UnidEmissora = "POSTO DETRAN ARMENIA";
                ViewBag.UnidCnpjf = "15.519.361/0001-16";

                ViewBag.Med1Nome = "VALMIR CLARET FEDRIGO";
                ViewBag.Med1CRM = "29957";
                ViewBag.Med1Port = "1488/11";
                ViewBag.Med1Espec = "Medicina do Tráfego";
                ViewBag.Med1Aut = "214/12";
                ViewBag.Med1CPF = "212.182.856-72";

                ViewBag.Med2Nome = "VERA LUCIA LIENDO VILLALVA";
                ViewBag.Med2CRM = "108112";
                ViewBag.Med2Port = "1800/16";
                ViewBag.Med2Espec = "Medicina do Tráfego";
                ViewBag.Med2Aut = "1210/17";
                ViewBag.Med2CPF = "187.069.998-08";

            }
        }

        private bool ValidaClienteCreate(string cpf)
        {
            try
            {
                var verifica = clienteDAL.ObterClientePorCpf(cpf).Result.Id;
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}