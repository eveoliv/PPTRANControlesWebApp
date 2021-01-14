using System;
using Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Repasse> ObterRepassePorId(long id)
        {
            return await context.Repasses                
                .SingleOrDefaultAsync(c => c.Id == id);
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
