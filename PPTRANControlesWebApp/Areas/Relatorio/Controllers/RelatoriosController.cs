using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Data.DAL.Relatorio;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;
using System;

namespace PPTRANControlesWebApp.Areas.Relatorio.Controllers
{
    [Area("Relatorio")]
    [Authorize(Roles = RolesNomes.Administrador + "," + RolesNomes.Gestor)]
    public class RelatoriosController : Controller
    {
        private readonly RelatorioDAL relatorioDAL;
        private readonly ApplicationContext context;
        private readonly ColaboradorDAL colaboradorDAL;
        private readonly UserManager<AppIdentityUser> userManager;

        public RelatoriosController(ApplicationContext context, UserManager<AppIdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
            relatorioDAL = new RelatorioDAL(context);
            colaboradorDAL = new ColaboradorDAL(context);
        }
       
        public async Task<IActionResult> Consolidado()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var lancamentos = await relatorioDAL.ObterLancamentosClassificadosPorClinica().ToListAsync();

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lancamentos = lancamentos.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

            var credito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito).Sum(c => c.Valor);
            ViewBag.Credito = credito.ToString("0#.####");

            var debito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Debito).Sum(c => c.Valor);
            ViewBag.Debito = debito.ToString("0#.####");

            var total = credito - debito;
            ViewBag.Total = total.ToString("0#.####");

            return View(lancamentos);
        }

        public async Task<IActionResult> Diario()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            //var data = DateTime.Today;
            var data = new DateTime(2021, 1, 4);

            var lancamentos = await relatorioDAL.ObterLancamentosClassificadosPorClinicaDiario(data).ToListAsync();

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lancamentos = lancamentos.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

            var dinheiro = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Dinheiro && c.Tipo == EnumHelper.Tipo.Credito).Sum(c => c.Valor);
            ViewBag.Dinheiro = dinheiro.ToString("0#.####");

            var cartao = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Cartao).Sum(c => c.Valor);
            ViewBag.Cartao = cartao.ToString("0#.####");

            var cheque = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Cheque).Sum(c => c.Valor);
            ViewBag.Cheque = cheque.ToString("0#.####");

            var transf = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Transferencia).Sum(c => c.Valor);
            ViewBag.Transf = transf.ToString("0#.####");           

            var credito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito && c.StatusPgto == EnumHelper.YesNo.Sim).Sum(c => c.Valor);
            ViewBag.Credito = credito.ToString("0#.####");

            var debito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Debito).Sum(c => c.Valor);
            ViewBag.Debito = debito.ToString("0#.####");

            var finalizados = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito && c.StatusPgto == EnumHelper.YesNo.Sim).Sum(c => c.Valor);
            ViewBag.Finalizados = finalizados.ToString("0#.####");

            var abertos = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito && c.StatusPgto == EnumHelper.YesNo.Não).Sum(c => c.Valor);
            ViewBag.Abertos = abertos.ToString("0#.####");

            var total = credito - debito;
            ViewBag.Total = total.ToString("0#.####");

            //-------------------------------------------//

            var totalExameMedicoRealizado = lancamentos.Where(c => c.ProdutoId == 1 || c.ProdutoId == 3 || c.ProdutoId == 5);
            ViewBag.TotalExameMedicoRealizado = totalExameMedicoRealizado.Count();

            var totalExameMedicoRecebido = totalExameMedicoRealizado.Where(c => c.StatusPgto == EnumHelper.YesNo.Sim);
            ViewBag.TotalExameMedicoRecebido = totalExameMedicoRecebido.Count();

            //-------------------------------------------//

            var totalExamePsicoRealizado = lancamentos.Where(c => c.ProdutoId == 2 || c.ProdutoId == 3);
            ViewBag.TotalExamePsicoRealizado = totalExamePsicoRealizado.Count();

            var totalExamePsicoRecebido = totalExamePsicoRealizado.Where(c => c.StatusPgto == EnumHelper.YesNo.Sim);
            ViewBag.TotalExamePsicoRecebido = totalExamePsicoRecebido.Count();

            //-------------------------------------------//

            var totalLaudoRealizado = lancamentos.Where(c => c.ProdutoId == 4);
            ViewBag.TotalLaudoRealizado = totalLaudoRealizado.Count();

            var totalLaudoRecebido = totalLaudoRealizado.Where(c => c.StatusPgto == EnumHelper.YesNo.Sim);
            ViewBag.TotalLaudoRecebido = totalLaudoRecebido.Count();


            return View(lancamentos);
        }

        public async Task<IActionResult> Mensal()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var lancamentos = await relatorioDAL.ObterLancamentosClassificadosPorClinicaMensal().ToListAsync();

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lancamentos = lancamentos.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

            var dinheiro = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Dinheiro && c.Tipo == EnumHelper.Tipo.Credito).Sum(c => c.Valor);
            ViewBag.Dinheiro = dinheiro.ToString("0#.####");

            var cartao = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Cartao).Sum(c => c.Valor);
            ViewBag.Cartao = cartao.ToString("0#.####");

            var cheque = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Cheque).Sum(c => c.Valor);
            ViewBag.Cheque = cheque.ToString("0#.####");

            var transf = lancamentos.Where(c => c.FormaPgto == EnumHelper.FormaPgto.Transferencia).Sum(c => c.Valor);
            ViewBag.Transf = transf.ToString("0#.####");

            var credito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito && c.StatusPgto == EnumHelper.YesNo.Sim).Sum(c => c.Valor);
            ViewBag.Credito = credito.ToString("0#.####");

            var debito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Debito).Sum(c => c.Valor);
            ViewBag.Debito = debito.ToString("0#.####");

            var finalizados = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito && c.StatusPgto == EnumHelper.YesNo.Sim).Sum(c => c.Valor);
            ViewBag.Finalizados = finalizados.ToString("0#.####");

            var abertos = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito && c.StatusPgto == EnumHelper.YesNo.Não).Sum(c => c.Valor);
            ViewBag.Abertos = abertos.ToString("0#.####");

            var total = credito - debito;
            ViewBag.Total = total.ToString("0#.####");

            //-------------------------------------------//

            var totalExameMedicoRealizado = lancamentos.Where(c => c.ProdutoId == 1 || c.ProdutoId == 3 || c.ProdutoId == 5);
            ViewBag.TotalExameMedicoRealizado = totalExameMedicoRealizado.Count();

            var totalExameMedicoRecebido = totalExameMedicoRealizado.Where(c => c.StatusPgto == EnumHelper.YesNo.Sim);
            ViewBag.TotalExameMedicoRecebido = totalExameMedicoRecebido.Count();

            //-------------------------------------------//

            var totalExamePsicoRealizado = lancamentos.Where(c => c.ProdutoId == 2 || c.ProdutoId == 3);
            ViewBag.TotalExamePsicoRealizado = totalExamePsicoRealizado.Count();

            var totalExamePsicoRecebido = totalExamePsicoRealizado.Where(c => c.StatusPgto == EnumHelper.YesNo.Sim);
            ViewBag.TotalExamePsicoRecebido = totalExamePsicoRecebido.Count();

            //-------------------------------------------//

            var totalLaudoRealizado = lancamentos.Where(c => c.ProdutoId == 4);
            ViewBag.TotalLaudoRealizado = totalLaudoRealizado.Count();

            var totalLaudoRecebido = totalLaudoRealizado.Where(c => c.StatusPgto == EnumHelper.YesNo.Sim);
            ViewBag.TotalLaudoRecebido = totalLaudoRecebido.Count();

            return View(lancamentos);
        }

    }
}