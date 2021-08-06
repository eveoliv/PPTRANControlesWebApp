using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    //REVISADO_20200715
    public class HistoricoDAL
    {
        private ApplicationContext context;

        public HistoricoDAL(ApplicationContext context)
        {
            this.context = context;
        }
       
        public IQueryable<Historico> ObterHistoricosClassificadosPorNome()
        {
            //id historico 1 reservado para valor fracionado
            return context.Historicos.Where(s => s.Status == EnumHelper.Status.Ativo && s.Id != 1).OrderBy(c => c.Nome);            
        }

        public async Task<Historico> ObterHistoricoPorId(long id)
        {
            return await context.Historicos                
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Historico> GravarHistorico(Historico historico)
        {
            if (historico.Id == null)
            {
                context.Historicos.Add(historico);
            }
            else
            {
                context.Update(historico);
            }

            await context.SaveChangesAsync();
            return historico;
        }

        public async Task<Historico> EliminarHistoricoPorId(long id)
        {
            var historico = await ObterHistoricoPorId(id);
            context.Historicos.Remove(historico);
            await context.SaveChangesAsync();
            return historico;
        }

        public async Task<Historico> InativarHistoricoPorId(long id, string user)
        {
            var historico = await ObterHistoricoPorId(id);
            historico.IdUser = user;
            historico.Status = EnumHelper.Status.Inativo;
            context.Update(historico);
            await context.SaveChangesAsync();
            return historico;
        }
    }
}
