using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using PPTRANControlesWebApp.Models.Administracao;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    public class RepasseDAL
    {
        private ApplicationContext context;

        public RepasseDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Repasse> ObterRepassesClassificadosPorNome()
        {
            return context.Repasses.OrderBy(c => c.Profissional);
        }

        public IQueryable<Repasse> ObterClinicasComRepasse()
        {
            return context.Repasses.OrderBy(c => c.ClinicaId).Distinct();
        }

        public IQueryable<RepasseViewModel> ObterRepassesClassificadosPorClinica()
        {
            return from r in context.Repasses
                   join c in context.Clinicas on r.ClinicaId equals c.Id
                   select new RepasseViewModel
                   {
                       RepasseId = r.Id,
                       ClinicaId = r.ClinicaId,
                       ClinicaAlias = c.Alias,
                       Profissional = r.Profissional,
                       Valor = r.Valor
                   };
        }

        public async Task<Repasse> ObterRepassePorId(long id)
        {
            return await context.Repasses                
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Repasse> ObterRepassePorClinicaComProfissional(long? clinicaId, string profissional)
        {
            return await context.Repasses.OrderBy(r => r.Valor)
                .FirstOrDefaultAsync(r => r.ClinicaId == clinicaId && r.Profissional == profissional);
        }

        public async Task<Repasse> GravarRepasse(Repasse repasse)
        {
            if (repasse.Id == null)
            {
                context.Repasses.Add(repasse);
            }
            else
            {
                context.Update(repasse);
            }

            await context.SaveChangesAsync();
            return repasse;
        }
    }
}
