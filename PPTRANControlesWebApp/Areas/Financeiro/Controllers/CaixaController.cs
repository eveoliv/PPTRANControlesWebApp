using Models;
using System;
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
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using PPTRANControlesWebApp.Data.DAL.Administracao;

namespace PPTRANControlesWebApp.Areas.Financeiro.Controllers
{
    //REVISADO_20200718
    [Area("Financeiro")]
    [Authorize]
    public class CaixaController : Controller
    {
        private readonly CaixaDAL caixaDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly ClinicaDAL clinicaDAL;
        private readonly ProdutoDAL produtoDAL;
        private readonly HistoricoDAL historicoDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public CaixaController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            caixaDAL = new CaixaDAL(context);
            clienteDAL = new ClienteDAL(context);
            produtoDAL = new ProdutoDAL(context);
            clinicaDAL = new ClinicaDAL(context);
            historicoDAL = new HistoricoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);

        }

        public async Task<IActionResult> Index(DateTime dateTime)
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            if (dateTime.Year == 1)
                dateTime = DateTime.Today;

            var lancamentos =
                await caixaDAL.ObterLancamentosClassificadosPorClienteNome(dateTime).ToListAsync();

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lancamentos = lancamentos.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

            ViewBag.DataAual = dateTime.ToString("dd/MM/yyyy");

            return View(lancamentos);
        }

        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoLancamentoPorIdNaoPago(id);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Caixa caixa)
        {

            if (caixa.FormaPgto == EnumHelper.FormaPgto.Selecionar)
            {
                return RedirectToAction("Edit");
            }

            if (id != caixa.Id)
            {
                return NotFound();
            }

            if (id != null)
            {
                try
                {
                    caixa.StatusPgto = EnumHelper.YesNo.Sim;
                    caixa.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await caixaDAL.GravarLancamento(caixa);

                    var idCli = caixa.ClienteId;

                    var lancamentoNaoPago =
                        caixaDAL.ObterLancamentoNaoPagoPeloClienteIdNoCaixa((long)idCli);

                    if (lancamentoNaoPago == 0)
                    {
                        var cliente = context.Clientes.Find((long)idCli);
                        cliente.StatusPgto = EnumHelper.YesNo.Sim;
                        await clienteDAL.GravarCliente(cliente);
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(caixa);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditFracao(int? id, Caixa caixa, string valor1, string valor2, int FormaPgto1, int FormaPgto2)
        {
            var v1 = Convert.ToDecimal(valor1);
            var v2 = Convert.ToDecimal(valor2);

            if (valor1 == null || valor2 == null)
                return RedirectToAction("Edit", new { id });

            if (caixa.Valor != (v1 + v2))
                return RedirectToAction("Edit", new { id });            

            if (id != caixa.Id)
                return NotFound();

            if (id != null)
            {
                try
                {
                    caixa.Valor = v1;
                    caixa.FormaPgto = (EnumHelper.FormaPgto)FormaPgto1;
                    await BaixaFracionada(caixa);

                    var fracao = new Caixa
                    {
                        Valor = v2,
                        ProdutoId = 7,
                        Data = caixa.Data,
                        Tipo = caixa.Tipo,
                        IdUser = caixa.IdUser,
                        Status = caixa.Status,
                        ClienteId = caixa.ClienteId,
                        ClinicaId = caixa.ClinicaId,
                        StatusPgto = caixa.StatusPgto,
                        FormaPgto = (EnumHelper.FormaPgto)FormaPgto2
                    };

                    await caixaDAL.GravarLancamento(fracao);                    

                }
                catch (DbUpdateConcurrencyException)
                {
                    throw;
                }

                return RedirectToAction("Index");
            }
            return View(caixa);
        }

        public async Task<IActionResult> Create()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            CarregarViewBagsCreate(roleUser);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaixaViewModel model)
        {
            try
            {
                if (model.Caixa.Valor > 0)
                {
                    var cpf = model.Cliente.CPF;

                    var idCli = PesquisarClientePorCpf(cpf);
                    if (idCli != 0)
                    {
                        model.Caixa.ClienteId = idCli;
                    }

                    var idCol = PesquisarColaboradorPorCpf(cpf);
                    if (idCol != 0)
                    {
                        model.Caixa.ColaboradorId = idCol;
                    }
                    else
                    {
                        var userId = userManager.GetUserAsync(User).Result.Id;
                        var usuario = await userManager.FindByIdAsync(userId);
                        var idLogado = colaboradorDAL.ObterColaboradorPorEmail(usuario.ToString()).Result.Id;
                        model.Caixa.ColaboradorId = idLogado;
                        idCol = (long)idLogado;
                    }


                    if (idCli != 0 || idCol != 0)
                    {
                        if (model.Caixa.ProdutoId == 0)
                            model.Caixa.ProdutoId = 6;

                        if (model.Caixa.HistoricoId == 0)
                            model.Caixa.HistoricoId = 8;

                        if (model.Caixa.Tipo == EnumHelper.Tipo.Credito)
                            model.Caixa.StatusPgto = EnumHelper.YesNo.Sim;


                        model.Caixa.IdUser = userManager.GetUserAsync(User).Result.Id;
                        await caixaDAL.GravarLancamento(model.Caixa);
                    }

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

        [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor)]
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {

            if (ClienteExiste((long)id))
            {
                var idCli = caixaDAL.ObterLancamentoPorId((long)id).Result.Cliente.Id;
                var cliente = context.Clientes.Find((long)idCli);
                cliente.StatusPgto = EnumHelper.YesNo.Não;
                await clienteDAL.GravarCliente(cliente);
            }

            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var caixa = await caixaDAL.InativarLancamentoPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        public IActionResult Recibo(long id)
        {
            var pagamentos = caixaDAL.ObterLancamentoPagoPeloCliente(id).ToList();

            if (pagamentos.Any())
            {
                ViewBag.ClinicaNome = pagamentos.Select(c => c.Clinica.Nome).FirstOrDefault();
                ViewBag.ClienteNome = pagamentos.Select(c => c.Cliente.Nome).FirstOrDefault();
                ViewBag.ClienteCpf = pagamentos.Select(c => c.Cliente.CPF).FirstOrDefault();

                var cliId = pagamentos.Select(c => c.ClinicaId).FirstOrDefault();
                var cliEndereco = clinicaDAL.ObterClinicaPorId((long)cliId);
                var cliRua = cliEndereco.Result.Endereco.Rua;
                var cliNum = cliEndereco.Result.Endereco.Numero;
                var cliBairro = cliEndereco.Result.Endereco.Bairro;
                var cliFone = cliEndereco.Result.Telefone1;
                ViewBag.ClinicaRuaNum = cliRua + ", Nº " + cliNum;
                ViewBag.ClinicaBairroFone = cliBairro + " - Fone: " + cliFone;

                var valorTotal = pagamentos.Sum(v => v.Valor);
                ViewBag.ValotTotal = valorTotal;

                var valorExt = Converter.toExtenso(valorTotal);
                ViewBag.ValorExt = valorExt;

                var userId = userManager.GetUserAsync(User).Result.ColaboradorId;

                var colabNome = colaboradorDAL.ObterColaboradorPorIdFull(userId);
                ViewBag.ColabNome = colabNome.Result.Nome;

                ViewBag.Data = DataBuilder.PtBr_Data();

                return View(pagamentos);
            }

            return View(pagamentos);
        }

        /****** Metodos Privados do Controller ******/
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

            ViewBag.Usuario = userManager.FindByIdAsync(caixa.IdUser).Result.Nome;

            return View(caixa);
        }

        private void CarregarViewBagsDetails(Caixa caixa)
        {
            try
            {
                ViewBag.Colaborador = caixa.Colaborador.Nome.ToString();
            }
            catch (System.Exception)
            {
                ViewBag.Colaborador = "";
            }

            try
            {
                ViewBag.Cliente = caixa.Cliente.Nome.ToString();
            }
            catch (System.Exception)
            {
                ViewBag.Cliente = "";
            }

            ViewBag.Clinica = caixa.Clinica.Alias.ToString();
            ViewBag.Historico = caixa.Historico.Nome.ToString();

            ViewBag.Usuario = userManager.FindByIdAsync(caixa.IdUser).Result.Nome;
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
            //clinicas.Insert(0, new Clinica() { Id = 0, Alias = "" });
            ViewBag.Clinicas = clinicas;

            var historicos = historicoDAL.ObterHistoricosClassificadosPorNome().ToList();
            historicos.Insert(0, new Historico() { Id = 0, Nome = "" });
            ViewBag.Historicos = historicos;

            var produtos = produtoDAL.ObterProdutosClassificadosPorNome().ToList();
            produtos.Insert(0, new Produto() { Id = 0, Nome = "" });
            ViewBag.Produtos = produtos;
        }

        private long PesquisarClientePorCpf(string cpf)
        {
            long idCliente;
            try
            {
                var cliente = clienteDAL.ObterClientePorCpf(cpf).Result.Id;
                idCliente = (long)cliente;
            }
            catch (System.Exception)
            {

                idCliente = 0;
            }
            return idCliente;
        }

        private long PesquisarColaboradorPorCpf(string cpf)
        {
            long idColab;
            try
            {
                var colaborador = colaboradorDAL.ObterColaboradorPorCpf(cpf).Result.Id;
                idColab = (long)colaborador;
            }
            catch (System.Exception)
            {

                idColab = 0;
            }
            return idColab;
        }

        private async Task<IActionResult> ObterVisaoLancamentoPorIdNaoPago(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var caixa = await caixaDAL.ObterLancamentoPorIdNaoPago((long)id);
            if (caixa == null)
            {
                return RedirectToAction(nameof(Index));
            }

            return View(caixa);
        }

        private bool ClienteExiste(long? id)
        {
            return caixaDAL.ObterLancamentoPorId((long)id).Result.ClienteId != null;
        }

        private async Task BaixaFracionada(Caixa caixa)
        {
            caixa.StatusPgto = EnumHelper.YesNo.Sim;
            caixa.IdUser = userManager.GetUserAsync(User).Result.Id;
            await caixaDAL.GravarLancamento(caixa);

            var idCli = caixa.ClienteId;

            var lancamentoNaoPago =
                caixaDAL.ObterLancamentoNaoPagoPeloClienteIdNoCaixa((long)idCli);

            if (lancamentoNaoPago == 0)
            {
                var cliente = context.Clientes.Find((long)idCli);
                cliente.StatusPgto = EnumHelper.YesNo.Sim;
                await clienteDAL.GravarCliente(cliente);
            }
        }
    }
}