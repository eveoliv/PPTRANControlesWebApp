using Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;
using PPTRANControlesWebApp.Models.Operacao;
using PPTRANControlesWebApp.Data.DAL.Operacao;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
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

        public IActionResult Create(int? id)
        {
            CarregarViewBagsCreate(id);

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CarrinhoViewModel model)
        {
            var clienteId = model.Id;
            try
            {
                if (model.Carrinho.Produto1Id != null)
                {
                    model.Carrinho.Id = null;
                    model.Carrinho.ClienteId = clienteId;
                    model.Carrinho.Data = DateTime.Today;
                    model.Carrinho.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await carrinhoDAL.GravarCarrinho(model.Carrinho);

                    var prodLista = new List<long?>
                    {
                        model.Carrinho.Produto1Id
                    };

                    if (model.Carrinho.Produto2Id != null)
                        prodLista.Add(model.Carrinho.Produto2Id);

                    if (model.Carrinho.Produto3Id != null)
                        prodLista.Add(model.Carrinho.Produto3Id);

                    foreach (var p in prodLista)
                    {
                        if (p != 0)
                        {
                            await IncluirLancamentoExameNoCaixa(p, clienteId, model.FormaPagamento);
                        }
                    }
                }

                var cliente = context.Clientes.Find((long)clienteId);
                if (model.FormaPagamento != "Selecionar Forma Pagamento")
                {
                    cliente.StatusPgto = EnumHelper.YesNo.Sim;
                }
                else
                {
                    cliente.StatusPgto = EnumHelper.YesNo.Não;
                }
                await clienteDAL.GravarCliente(cliente);

                return RedirectToAction("Index", "Cliente");
            }
            catch (DbUpdateException)
            {
                ModelState.AddModelError("", "Não foi possível inserir os dados.");
            }
            return View(model.Carrinho);
        }

        public IActionResult Edit(int? id)
        {
            return View();
        }

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

        /****** Metodos Privados do Controller ******/
        private async Task IncluirLancamentoExameNoCaixa(long? produtoId, long? clienteId, string formaPgto)
        {
            var cliente = clienteDAL.ObterClientePorId((long)clienteId);

            var produto = produtoDAL.ObterProdutoPorId((long)produtoId);

            var caixa = new Caixa();
            caixa.Data = DateTime.Today;
            caixa.ProdutoId = produtoId;
            caixa.ClienteId = clienteId;
            caixa.Valor = produto.Result.Valor;

            if (formaPgto != "Selecionar Forma Pagamento")
            {
                caixa.StatusPgto = EnumHelper.YesNo.Sim;
            }
            else
            {
                caixa.StatusPgto = EnumHelper.YesNo.Não;
            }
            caixa.ClinicaId = cliente.Result.ClinicaId;
            caixa.HistoricoId = cliente.Result.HistoricoId;

            switch (formaPgto)
            {
                case "Selecionar Forma Pagamento":
                    caixa.FormaPgto = EnumHelper.FormaPgto.Selecionar;
                    break;
                case "Dinheiro":
                    caixa.FormaPgto = EnumHelper.FormaPgto.Dinheiro;
                    break;
                case "Cartao":
                    caixa.FormaPgto = EnumHelper.FormaPgto.Cartao;
                    break;
                case "Cheque":
                    caixa.FormaPgto = EnumHelper.FormaPgto.Cheque;
                    break;
                case "Transferencia":
                    caixa.FormaPgto = EnumHelper.FormaPgto.Transferencia;
                    break;
            }

            caixa.IdUser = userManager.GetUserAsync(User).Result.Id;
            await caixaDAL.GravarLancamentoPorCarrinho(caixa);
        }

        private void CarregarViewBagsCreate(long? id)
        {
            ViewBag.Cliente = clienteDAL.ObterClientePorId((long)id).Result.Nome;

            ViewBag.Caixa = caixaDAL.ObterLancamentoNaoPagoPeloClienteIdNoCaixa((long)id);

            var produtos = produtoDAL.ObterProdutosClassificadosPorId().ToList();
            produtos = produtos.Where(i => i.Id != 6).ToList();
            //produtos.Insert(0, new Produto() { Id = 0, Nome = "" });
            ViewBag.Produtos = produtos;

            var pagamentos = new List<string>
            {
                { "Selecionar Forma Pagamento" },
                { "Dinheiro" },
                { "Cartao" },
                { "Cheque" },
                { "Transferencia" }
            };

            SelectList formaPagamento = new SelectList(pagamentos);
            ViewData["FormaPagamento"] = formaPagamento;
        }
    }
}