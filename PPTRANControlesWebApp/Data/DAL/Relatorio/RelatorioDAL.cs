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

        public IQueryable<Caixa> ObterLancamentosClassificadosPorClinicaDiario(DateTime dateTime)
        {
            //return from a in context.Caixas
            //          join b in context.Clientes on a.ClienteId equals b.Id
            //          join c in context.Colaboradores on b.MedicoId equals c.Id
            //          join d in context.Colaboradores on b.PsicologoId equals d.Id
            //          join e in context.Clinicas on b.ClinicaId equals e.Id
            //          join f in context.Produtos on a.ProdutoId equals f.Id
            //          join g in context.Historicos on a.HistoricoId equals g.Id
            //          where b.Status == EnumHelper.Status.Ativo
            //          where a.Data == dateTime
            //          orderby a.Clinica
            //          select new
            //          {
            //              Data = a.Data,
            //              Debito = a.Tipo,
            //              Credito = a.Tipo,
            //              Valor = a.Valor,
            //              FormaPgto = a.FormaPgto,
            //              Status = a.Status,
            //              Produto = f.Nome,
            //              Historico = g.Nome,
            //              Nome = b.Nome,
            //              Medico = c.Nome,
            //              Psico = d.Nome
            //          };

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
