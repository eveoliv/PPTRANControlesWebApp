﻿using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
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

        // GET: Caixa
        public async Task<IActionResult> Index()
        {
            var lancamentos = await caixaDAL.ObterLancamentosClassificadosPorProduto().ToListAsync();
            return View(lancamentos);
        }

        // GET: Caixa/Details
        public async Task<IActionResult> Details(long? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        // GET: Caixa/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        // POST: Caixa/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int? id, IFormCollection collection)
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

        // GET: Caixa/Create
        public IActionResult Create()
        {
            CarregarViewBagsCreate();
            return View();
        }

        // POST: Caixa/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CaixaViewModel model)
        {
            try
            {
                if (model.Caixa.Cliente.CPF != null)
                {
                    var cpf = model.Caixa.Cliente.CPF;

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

                    if (idCli != 0 || idCol != 0)
                    {
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

        // GET: Caixa/Delete
        public async Task<IActionResult> Delete(long? id)
        {
            return await ObterVisaoLancamentoPorId(id);
        }

        // POST: Caixa/Delete
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(long? id)
        {
            var IdUser = userManager.GetUserAsync(User).Result.Id;
            var caixa = await caixaDAL.InativarLancamentoPorId((long)id, IdUser);
            return RedirectToAction(nameof(Index));
        }

        // Metodos Privados do Controller
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

        private void CarregarViewBagsCreate()
        {
            var clinicas = clinicaDAL.ObterClinicasClassificadasPorNome().ToList();
            clinicas.Insert(0, new Clinica() { Id = 0, Alias = "" });
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
    }
}