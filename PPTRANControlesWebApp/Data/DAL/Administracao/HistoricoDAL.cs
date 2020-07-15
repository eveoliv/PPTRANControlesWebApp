using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    public class HistoricoDAL
    {
        private ApplicationContext context;

        public HistoricoDAL(ApplicationContext context)
        {
            this.context = context;
        }

        /*** Revisado ***/
        public IQueryable<Historico> ObterHistoricoPorNome()
        {
            return context.Historicos.Where(s => s.Status == EnumHelper.Status.Ativo).OrderBy(c => c.Nome);            
        }

        public async Task<Historico> ObterHistoricoPorId(long id)
        {
            return await context.Historicos
               .Where(c => c.Status == EnumHelper.Status.Ativo)
               .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Historico> GravarHitorico(Historico historico)
        {
            if (historico.Id == null)
            {
                historico.Status = EnumHelper.Status.Ativo;
                context.Historicos.Add(historico);
            }
            else
            {
                context.Update(historico);
            }

            await context.SaveChangesAsync();
            return historico;
        }
    }
}
