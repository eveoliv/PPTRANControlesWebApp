using Models;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

        public IQueryable<Caixa> ObterLancamentosClassificadosPorClinicaDiario()
        {
            return context.Caixas
              .Include(c => c.Cliente)
              .Include(c => c.Clinica)
              .Include(p => p.Produto)
              .Include(i => i.Historico)
              .Include(c => c.Colaborador)
              .Where(s => s.Status == EnumHelper.Status.Ativo)
              .Where(d => d.Data == DateTime.Today)
              .OrderBy(p => p.Clinica);
        }

        public IQueryable<Caixa> ObterLancamentosClassificadosPorClinicaMensal()
        {
            var ano = DateTime.Today.Year;
            var mes = DateTime.Today.Month;
            var dias = DateTime.DaysInMonth(ano, mes);

            DateTime dtInicio = new DateTime(ano,mes,01);
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
    }
}
