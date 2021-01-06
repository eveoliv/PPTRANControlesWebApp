using Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
using PPTRANControlesWebApp.Models.Relatorio;
using PPTRANControlesWebApp.Data.DAL.Relatorio;
using PPTRANControlesWebApp.Areas.Identity.Data;
using PPTRANControlesWebApp.Areas.Identity.Models;

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

        public async Task<IActionResult> DiarioCaixa(DateTime dateTime)
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            if (dateTime.Year == 1)
                dateTime = DateTime.Today;

            var lancamentos = await relatorioDAL.ObterLancamentosClassificadosPorClinicaDiario(dateTime).ToListAsync();

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lancamentos = lancamentos.Where(c => c.ClinicaId == userClinicaId).ToList();
            }

            ViewBag.DataAtual = dateTime.ToString("dd/MM/yyyy");

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

        public async Task<IActionResult> DiarioMedico(DiarioMedicoViewModel model, DateTime dateTime)
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var dtCabecalho = DateTime.Today;
            if (dateTime.Year == 1)
            {
                model.Data = DateTime.Today;
            }
            else
            {
                model.Data = dateTime;
                dtCabecalho = dateTime;
            }

            var lancamentos = relatorioDAL.ObterExamePorMedicoDiario(model);

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lancamentos = lancamentos.Where(l => l.ClinicaId == userClinicaId);
            }

            AgrupamentoDeExamesMedico(lancamentos);

            ViewBag.DataAtual = dtCabecalho.ToString("dd/MM/yyyy");

            return View(lancamentos);
        }

        public async Task<IActionResult> DiarioPsico(DiarioPsicologoViewModel model, DateTime dateTime)
        {

            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var dtCabecalho = DateTime.Today;
            if (dateTime.Year == 1)
            {
                model.Data = DateTime.Today;
            }
            else
            {
                model.Data = dateTime;
                dtCabecalho = dateTime;
            }

            var lancamentos = relatorioDAL.ObterExamePorPsicologoDiario(model);

            if (roleUser.FirstOrDefault() != RolesNomes.Administrador)
            {
                var colId = userManager.GetUserAsync(User).Result.ColaboradorId;
                var userClinicaId = colaboradorDAL.ObterColaboradorPorId(colId).Result.ClinicaId;
                lancamentos = lancamentos.Where(l => l.ClinicaId == userClinicaId);
            }

            AgrupamentoDeExamesPsico(lancamentos);

            ViewBag.DataAtual = dtCabecalho.ToString("dd/MM/yyyy");

            return View(lancamentos);
        }

        private void AgrupamentoDeExamesPsico(IQueryable<DiarioPsicologoViewModel> lancamentos)
        {
            var psico = (from l in lancamentos select new { l.Nome }).ToList();

            var groupped = 
                psico.GroupBy(x => x.Nome).Select(g => new {Chave = g.Key, Itens = g.ToList(), Total = g.Count()});

            string[] grupo = new string[10];
            int i = 0;
            foreach (var g in groupped)
            {
                grupo[i] = $"{g.Chave}     {g.Total}";
                i++;
            }

            ViewBag.Agrupar0 = grupo[0];
            ViewBag.Agrupar1 = grupo[1];
            ViewBag.Agrupar2 = grupo[2];
            ViewBag.Agrupar3 = grupo[3];
            ViewBag.Agrupar4 = grupo[4];
            ViewBag.Agrupar5 = grupo[5];
            ViewBag.Agrupar6 = grupo[6];
            ViewBag.Agrupar7 = grupo[7];
            ViewBag.Agrupar8 = grupo[8];
            ViewBag.Agrupar9 = grupo[9];
        }

        private void AgrupamentoDeExamesMedico(IQueryable<DiarioMedicoViewModel> lancamentos)
        {
            var psico = (from l in lancamentos select new { l.Nome }).ToList();

            var groupped = 
                psico.GroupBy(x => x.Nome).Select(g => new { Chave = g.Key, Itens = g.ToList(), Total = g.Count()});

            string[] grupo = new string[10];
            int i = 0;
            foreach (var g in groupped)
            {
                grupo[i] = $"{g.Chave}     {g.Total}";
                i++;
            }

            ViewBag.Agrupar0 = grupo[0];
            ViewBag.Agrupar1 = grupo[1];
            ViewBag.Agrupar2 = grupo[2];
            ViewBag.Agrupar3 = grupo[3];
            ViewBag.Agrupar4 = grupo[4];
            ViewBag.Agrupar5 = grupo[5];
            ViewBag.Agrupar6 = grupo[6];
            ViewBag.Agrupar7 = grupo[7];
            ViewBag.Agrupar8 = grupo[8];
            ViewBag.Agrupar9 = grupo[9];
        }
    }
}