using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPTRANControlesWebApp.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using PPTRANControlesWebApp.Data.DAL;
using Microsoft.AspNetCore.Authorization;
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

        public async Task<IActionResult> Diario()
        {
            var userId = userManager.GetUserAsync(User).Result.Id;
            var usuario = await userManager.FindByIdAsync(userId);
            var roleUser = await userManager.GetRolesAsync(usuario);

            var lancamentos = await relatorioDAL.ObterLancamentosClassificadosPorClinicaDiario().ToListAsync();

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

            var credito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Credito).Sum(c => c.Valor);
            ViewBag.Credito = credito.ToString("0#.####");

            var debito = lancamentos.Where(c => c.Tipo == EnumHelper.Tipo.Debito).Sum(c => c.Valor);
            ViewBag.Debito = debito.ToString("0#.####");

            var total = credito - debito;
            ViewBag.Total = total.ToString("0#.####");

            return View(lancamentos);
        }

    }
}