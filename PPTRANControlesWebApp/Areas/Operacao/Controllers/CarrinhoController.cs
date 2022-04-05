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
        private static bool alerta = false;

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
            var cliente = await clienteDAL.ObterClientePorId((long)clienteId);
            var temMedico = cliente.MedicoId != null ? 1 : 0;
            var temPsico = cliente.PsicologoId != null ? 2 : 0;

            var listaProds = new List<long?>
            {
                model.Carrinho.Produto1Id,
                model.Carrinho.Produto2Id,
                model.Carrinho.Produto3Id,
                model.Carrinho.Produto4Id
            };

            if( !ValidaExameVsProfissional(listaProds, temMedico, temPsico))
            {
                alerta = true;
                return RedirectToRoute(new { controller = "Carrinho", action = "Create", id = clienteId });
            }

            try
            {
                if (model.Carrinho.Produto1Id != null)
                {
                    model.Carrinho.Id = null;
                    model.Carrinho.ClienteId = clienteId;
                    model.Carrinho.Data = DateTime.Today;
                    model.Carrinho.IdUser = userManager.GetUserAsync(User).Result.Id;
                    await carrinhoDAL.GravarCarrinho(model.Carrinho);

                    var prodLista = new List<long?>();

                    if (model.Carrinho.Produto1Id == 3)
                    {
                        prodLista.Add(1);
                        prodLista.Add(2);
                    }
                    else
                    {
                        prodLista.Add(model.Carrinho.Produto1Id);
                    }

                    if (model.Carrinho.Produto2Id != null && model.Carrinho.Produto2Id != 3)
                        prodLista.Add(model.Carrinho.Produto2Id);

                    //if (model.Carrinho.Produto3Id != null)
                    //    prodLista.Add(model.Carrinho.Produto3Id);

                    foreach (var p in prodLista)
                    {
                        if (p != 0)
                        {
                            var produto = await produtoDAL.ObterProdutoPorId((long)p);
                            await IncluirLancamentoExameNoCaixa(cliente, produto, model.FormaPagamento);
                        }
                    }
                }

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
        private async Task IncluirLancamentoExameNoCaixa(Cliente cliente, Produto produto, string formaPgto)
        {
            var caixa = new Caixa
            {
                Data = DateTime.Today,
                Valor = produto.Valor,
                ProdutoId = produto.Id,
                ClienteId = cliente.Id,
                Status = EnumHelper.Status.Ativo
            };

            if (formaPgto != "Selecionar Forma Pagamento")
            {
                caixa.StatusPgto = EnumHelper.YesNo.Sim;
            }
            else
            {
                caixa.StatusPgto = EnumHelper.YesNo.Não;
            }

            caixa.ClinicaId = cliente.ClinicaId;

            if (cliente.HistoricoId == null)
            {
                caixa.HistoricoId = 8;
            }
            else
            {
                caixa.HistoricoId = cliente.HistoricoId;
            }

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
                case "Cortesia":
                    caixa.FormaPgto = EnumHelper.FormaPgto.Cortesia;
                    break;
            }

            caixa.IdUser = userManager.GetUserAsync(User).Result.Id;
            await caixaDAL.GravarLancamentoPorCarrinho(caixa);
        }

        private void CarregarViewBagsCreate(long? id)
        {
            if (alerta)
            {
                alerta = false;
                ViewBag.Alerta = "*O Cliente não possui o profissional correspondente ao exame solicitado.";
            }

            ViewBag.Cliente = clienteDAL.ObterClientePorId((long)id).Result.Nome;

            ViewBag.Caixa = caixaDAL.ObterLancamentoNaoPagoPeloClienteIdNoCaixa((long)id);

            var produtos = produtoDAL.ObterProdutosClassificadosPorId().ToList();
            produtos = produtos.Where(i => i.Id != 6 && i.Id != 7).ToList();
            //produtos.Insert(0, new Produto() { Id = 0, Nome = "" });
            ViewBag.Produtos = produtos;

            var pagamentos = new List<string>
            {
                { "Selecionar Forma Pagamento" },
                { "Dinheiro" },
                { "Cartao" },
                { "Cheque" },
                { "Transferencia" },
                { "Cortesia" }
            };

            SelectList formaPagamento = new SelectList(pagamentos);
            ViewData["FormaPagamento"] = formaPagamento;
        }

        private bool ValidaExameVsProfissional(List<long?> lista, long? temMedico, long? temPsico)
        {
            if ( lista.Contains(1) && temMedico == 1 || lista.Contains(2) && temPsico == 2 )
                return true;

            if ( lista.Contains(3) && temPsico == 2 && temMedico == 1 )
                return true;

            if (lista.Contains(4) || lista.Contains(5) && temMedico == 2)
                return true;

            return false;
        }

    }
}