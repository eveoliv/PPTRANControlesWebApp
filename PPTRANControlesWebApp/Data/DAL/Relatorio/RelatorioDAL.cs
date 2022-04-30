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
        const string naoAtribuido = "NÃO ATRIBUIDO";

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

        public IQueryable<MensalViewModel> ObterLancamentosClassificadosPorClinicaMensal(DateTime dtInicio, DateTime dtFim)
        {
            return from a in context.Caixas
                   join b in context.Clinicas on a.ClinicaId equals b.Id
                   join c in context.Produtos on a.ProdutoId equals c.Id
                   join d in context.Historicos on a.HistoricoId equals d.Id
                   where a.Status == EnumHelper.Status.Ativo // 0
                   where a.StatusPgto == EnumHelper.YesNo.Sim // 1
                   where a.Data >= dtInicio && a.Data <= dtFim
                   orderby b.Nome
                   select new MensalViewModel
                   {
                       ClinicaId = b.Id,
                       Clinica = b.Nome,
                       HistoricoId = d.Id,
                       Historico = d.Nome,
                       ProdutoId = c.Id,
                       Produto = c.Nome,
                       Referencia = a.Ref,
                       Tipo = a.Tipo,
                       Valor = a.Valor
                   };
        }

        public IQueryable<DiarioMedicoViewModel> ObterExamePorMedicoDiario(DiarioMedicoViewModel model, string medico)
        {

            if (medico != null && medico != "Selecionar Medico")
            {
                var exame = from a in context.Caixas
                            join b in context.Clientes on a.ClienteId equals b.Id
                            join c in context.Colaboradores on b.MedicoId equals c.Id
                            where c.Nome == medico
                            where b.Status == EnumHelper.Status.Ativo
                            where a.Status == EnumHelper.Status.Ativo
                            where a.Tipo != EnumHelper.Tipo.Debito
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

                var avulso = from a in context.Caixas
                             join c in context.Colaboradores on a.ColaboradorId equals c.Id
                             where c.Nome == medico
                             where a.Status == EnumHelper.Status.Ativo
                             where a.Tipo != EnumHelper.Tipo.Debito
                             where a.ProdutoId == 1 || a.ProdutoId == 3 || a.ProdutoId == 5
                             where a.Data == model.Data
                             orderby c.Nome
                             select new DiarioMedicoViewModel
                             {
                                 Id = c.Id,
                                 ClinicaId = c.ClinicaId,
                                 Nome = c.Nome,
                                 Data = a.Data,
                                 Cliente = naoAtribuido
                             };

                return exame.Concat(avulso);
            }

            var exames = from a in context.Caixas
                         join b in context.Clientes on a.ClienteId equals b.Id
                         join c in context.Colaboradores on b.MedicoId equals c.Id
                         where b.Status == EnumHelper.Status.Ativo
                         where a.Status == EnumHelper.Status.Ativo
                         where a.Tipo != EnumHelper.Tipo.Debito
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

            var avulsos = from a in context.Caixas
                          join c in context.Colaboradores on a.ColaboradorId equals c.Id
                          where a.Status == EnumHelper.Status.Ativo
                          where a.Tipo != EnumHelper.Tipo.Debito
                          where a.ProdutoId == 1 || a.ProdutoId == 3 || a.ProdutoId == 5
                          where a.Data == model.Data
                          orderby c.Nome
                          select new DiarioMedicoViewModel
                          {
                              Id = c.Id,
                              ClinicaId = c.ClinicaId,
                              Nome = c.Nome,
                              Data = a.Data,
                              Cliente = naoAtribuido
                          };

            return exames.Concat(avulsos);
        }

        public IQueryable<DiarioPsicologoViewModel> ObterExamePorPsicologoDiario(DiarioPsicologoViewModel model, string psico)
        {
            if (psico != null && psico != "Selecionar Psicologo")
            {
                var exame = from a in context.Caixas
                            join b in context.Clientes on a.ClienteId equals b.Id
                            join c in context.Colaboradores on b.PsicologoId equals c.Id
                            where c.Nome == psico
                            where b.Status == EnumHelper.Status.Ativo
                            where a.Status == EnumHelper.Status.Ativo
                            where a.Tipo != EnumHelper.Tipo.Debito
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

                var avulso = from a in context.Caixas
                             join c in context.Colaboradores on a.ColaboradorId equals c.Id
                             where c.Nome == psico
                             where a.Status == EnumHelper.Status.Ativo
                             where a.Tipo != EnumHelper.Tipo.Debito
                             where a.ProdutoId == 2 || a.ProdutoId == 3
                             where a.Data == model.Data
                             orderby c.Nome
                             select new DiarioPsicologoViewModel
                             {
                                 Id = c.Id,
                                 ClinicaId = c.ClinicaId,
                                 Nome = c.Nome,
                                 Data = a.Data,
                                 Cliente = naoAtribuido
                             };

                return exame.Concat(avulso);
            }

            var exames = from a in context.Caixas
                         join b in context.Clientes on a.ClienteId equals b.Id
                         join c in context.Colaboradores on b.PsicologoId equals c.Id
                         where a.Tipo != EnumHelper.Tipo.Debito
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

            var avulsos = from a in context.Caixas
                          join c in context.Colaboradores on a.ColaboradorId equals c.Id
                          where a.Tipo != EnumHelper.Tipo.Debito
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
                              Cliente = naoAtribuido
                          };

            return exames.Concat(avulsos);
        }

        public IQueryable<SemanalPsicologoViewModel> ObterExamePorPsicologoSemanal(SemanalPsicologoViewModel model, string psico)
        {
            var exames = from a in context.Caixas
                         join b in context.Clientes on a.ClienteId equals b.Id
                         join c in context.Colaboradores on b.PsicologoId equals c.Id
                         where c.Nome == psico
                         where a.Data >= model.DataInicio && a.Data <= model.DataFim
                         where a.Tipo != EnumHelper.Tipo.Debito
                         where b.Status == EnumHelper.Status.Ativo
                         where a.Status == EnumHelper.Status.Ativo
                         where a.ProdutoId == 2 || a.ProdutoId == 3
                         orderby c.Nome
                         select new SemanalPsicologoViewModel
                         {
                             Id = c.Id,
                             ClinicaId = c.ClinicaId,
                             Nome = c.Nome,
                             DataCx = a.Data,
                             Cliente = b.Nome
                         };

            var avulsos = from a in context.Caixas
                          join c in context.Colaboradores on a.ColaboradorId equals c.Id
                          where c.Nome == psico
                          where a.Data >= model.DataInicio && a.Data <= model.DataFim
                          where a.Tipo != EnumHelper.Tipo.Debito
                          where a.Status == EnumHelper.Status.Ativo
                          where a.ProdutoId == 2 || a.ProdutoId == 3
                          orderby c.Nome
                          select new SemanalPsicologoViewModel
                          {
                              Id = c.Id,
                              ClinicaId = c.ClinicaId,
                              Nome = c.Nome,
                              DataCx = a.Data,
                              Cliente = naoAtribuido
                          };

            return exames.Concat(avulsos);
        }

        public IQueryable<SemanalMedicoViewModel> ObterExamePorMedicoSemanal(SemanalMedicoViewModel model, string medico)
        {
            var exames = from a in context.Caixas
                         join b in context.Clientes on a.ClienteId equals b.Id
                         join c in context.Colaboradores on b.MedicoId equals c.Id
                         where c.Nome == medico
                         where a.Data >= model.DataInicio && a.Data <= model.DataFim
                         where a.Tipo != EnumHelper.Tipo.Debito
                         where b.Status == EnumHelper.Status.Ativo
                         where a.Status == EnumHelper.Status.Ativo
                         where a.ProdutoId == 2 || a.ProdutoId == 3
                         orderby c.Nome
                         select new SemanalMedicoViewModel
                         {
                             Id = c.Id,
                             ClinicaId = c.ClinicaId,
                             Nome = c.Nome,
                             DataCx = a.Data,
                             Cliente = b.Nome
                         };

            var avulsos = from a in context.Caixas
                          join c in context.Colaboradores on a.ColaboradorId equals c.Id
                          where c.Nome == medico
                          where a.Data >= model.DataInicio && a.Data <= model.DataFim
                          where a.Tipo != EnumHelper.Tipo.Debito
                          where a.Status == EnumHelper.Status.Ativo
                          where a.ProdutoId == 2 || a.ProdutoId == 3
                          orderby c.Nome
                          select new SemanalMedicoViewModel
                          {
                              Id = c.Id,
                              ClinicaId = c.ClinicaId,
                              Nome = c.Nome,
                              DataCx = a.Data,
                              Cliente = naoAtribuido
                          };

            return exames.Concat(avulsos);
        }
    }
}    