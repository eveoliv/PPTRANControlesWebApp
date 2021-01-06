using Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Models.Relatorio;

namespace PPTRANControlesWebApp.Data.DAL.Relatorio
{
    public class RelatorioDAL
    {
        private ApplicationContext context;

        public RelatorioDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Caixa> ObterLancamentosClassificadosPorClinica()
        {
            return context.Caixas
                .Include(c => c.Cliente)
                .Include(c => c.Clinica)
                .Include(p => p.Produto)
                .Include(i => i.Historico)
                .Include(c => c.Colaborador)
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(p => p.Clinica);
        }

        public IQueryable<Caixa> ObterLancamentosClassificadosPorClinicaDiario(DateTime dateTime)
        {
            return context.Caixas
              .Include(c => c.Cliente)
              .Include(c => c.Clinica)
              .Include(p => p.Produto)
              .Include(c => c.Colaborador)
              .Include(i => i.Historico)
              .Where(s => s.Status == EnumHelper.Status.Ativo)
              .Where(d => d.Data == dateTime)
              .OrderBy(p => p.Clinica);
        }

        public IQueryable<Caixa> ObterLancamentosClassificadosPorClinicaMensal()
        {
            var ano = DateTime.Today.Year;
            var mes = DateTime.Today.Month;
            var dias = DateTime.DaysInMonth(ano, mes);

            DateTime dtInicio = new DateTime(ano, mes, 01);
            DateTime dtFim = new DateTime(ano, mes, dias);

            return context.Caixas
              .Include(c => c.Cliente)
              .Include(c => c.Clinica)
              .Include(p => p.Produto)
              .Include(i => i.Historico)
              .Include(c => c.Colaborador)
              .Where(s => s.Status == EnumHelper.Status.Ativo)
              .Where(d => d.Data >= dtInicio && d.Data <= dtFim)
              .OrderBy(p => p.Clinica);
        }

        public IQueryable<DiarioMedicoViewModel> ObterExamePorMedicoDiario(DiarioMedicoViewModel model)
        {
            return from a in context.Caixas
                   join b in context.Clientes on a.ClienteId equals b.Id
                   join c in context.Colaboradores on b.MedicoId equals c.Id
                   where b.Status == EnumHelper.Status.Ativo
                   where a.Status == EnumHelper.Status.Ativo
                   where a.ProdutoId == 1 || a.ProdutoId == 3 || a.ProdutoId == 5
                   where a.Data == model.Data
                   orderby c.Nome
                   select new DiarioMedicoViewModel
                   {
                       Id = c.Id,
                       ClinicaId = c.ClinicaId,
                       Nome = c.Nome,
                       Data = a.Data,
                       Cliente = b.Nome
                   };
        }

        public IQueryable<DiarioPsicologoViewModel> ObterExamePorPsicologoDiario(DiarioPsicologoViewModel model)
        {
            return from a in context.Caixas
                   join b in context.Clientes on a.ClienteId equals b.Id
                   join c in context.Colaboradores on b.PsicologoId equals c.Id
                   where b.Status == EnumHelper.Status.Ativo
                   where a.Status == EnumHelper.Status.Ativo
                   where a.ProdutoId == 2 || a.ProdutoId == 3
                   where a.Data == model.Data
                   orderby c.Nome
                   select new DiarioPsicologoViewModel
                   {
                       Id = c.Id,
                       ClinicaId = c.ClinicaId,
                       Nome = c.Nome,
                       Data = a.Data,
                       Cliente = b.Nome
                   };
        }
    }
}
