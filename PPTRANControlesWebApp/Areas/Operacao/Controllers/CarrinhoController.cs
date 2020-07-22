using Models;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Data.DAL.Administracao;
using PPTRANControlesWebApp.Data.DAL.Operacao;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

namespace PPTRANControlesWebApp.Areas.Operacao.Controllers
{
    [Area("Operacao")]
    [Authorize]
    public class CarrinhoController : Controller
    {
        private readonly CaixaDAL caixaDAL;
        private readonly ClienteDAL clienteDAL;
        private readonly ProdutoDAL produtoDAL;
        private readonly CarrinhoDAL carrinhoDAL;
        private readonly HistoricoDAL historicoDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public CarrinhoController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            caixaDAL = new CaixaDAL(context);
            produtoDAL = new ProdutoDAL(context);
            clienteDAL = new ClienteDAL(context);
            carrinhoDAL = new CarrinhoDAL(context);
            historicoDAL = new HistoricoDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }

        // GET: Carrinho/Create/
        public IActionResult Create(int? id)
        {
            CarregarViewBagsCreate(id);         

            return View();
        }

        // POST: Clientes/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Carrinho carrinho)
        {
            var clienteId = carrinho.Id;
            try
            {
                if (carrinho.Produto1Id != null)
                {
                    carrinho.Id = null;
                    carrinho.ClienteId = clienteId;
                    carrinho.Data = DateTime.Today;
                    carrinho.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await carrinhoDAL.GravarCarrinho(carrinho);

                    var prodLista = new List<long?>();
                    prodLista.Add(carrinho.Produto1Id);
                    prodLista.Add(carrinho.Produto2Id);
                    prodLista.Add(carrinho.Produto3Id);

                    foreach (var p in prodLista)
                    {
                        if (p != 0)
                        {
                            await IncluirLancamentoExameNoCaixa(p, clienteId);
                        }
                    }
                }

                var cliente = context.Clientes.Find((long)clienteId);
                cliente.StatusPgto = EnumHelper.YesNo.Não;
                await clienteDAL.GravarCliente(cliente);

                return RedirectToAction("Index", "Cliente");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(carrinho);
        }

        // GET: Carrinho/Edit/
        public IActionResult Edit(int? id)
        {
            return View();
        }

        // POST: Carrinho/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(long? id, Carrinho carrinho)
        {
            try
            {
                if (carrinho.Produto1Id != null)
                {
                    await carrinhoDAL.GravarCarrinho(carrinho);
                }

                return RedirectToAction("Index", "Cliente");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(carrinho);
        }

        // Metodos Privados do Controller
        private async Task IncluirLancamentoExameNoCaixa(long? produtoId, long? clienteId)
        {
            var cliente = clienteDAL.ObterClientePorId((long)clienteId);

            var produto = produtoDAL.ObterProdutoPorId((long)produtoId);

            var caixa = new Caixa();
            caixa.Data = DateTime.Today;
            caixa.ProdutoId = produtoId;
            caixa.ClienteId = clienteId;
            caixa.Valor = produto.Result.Valor;
            caixa.StatusPgto = EnumHelper.YesNo.Não;
            caixa.ClinicaId = cliente.Result.ClinicaId;
            caixa.HistoricoId = cliente.Result.HistoricoId;
            caixa.FormaPgto = EnumHelper.FormaPgto.Selecionar;
            caixa.IdUser = userManager.GetUserAsync(User).Result.Id;
            await caixaDAL.GravarLancamentoPorCarrinho(caixa);
        }

        private void CarregarViewBagsCreate(long? id)
        {
            ViewBag.Cliente = clienteDAL.ObterClientePorId((long)id).Result.Nome;

            ViewBag.Caixa = caixaDAL.ObterLancamentoNaoPagoPeloClienteIdNoCaixa((long)id);

            var produtos = produtoDAL.ObterProdutosClassificadosPorId().ToList();
            produtos.Insert(0, new Produto() { Id = 0, Nome = "" });
            ViewBag.Produtos = produtos;
        }
    }
}