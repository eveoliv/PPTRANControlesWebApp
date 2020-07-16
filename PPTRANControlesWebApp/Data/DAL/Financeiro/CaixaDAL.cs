using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL
{
    //REVISADO_20200715
    public class CaixaDAL
    {
        private ApplicationContext context;

        public CaixaDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Caixa> ObterLancamentosClassificadosPorCliente()
        {
            return context.Caixas.OrderBy(c => c.Cliente);
        }

        public async Task<Caixa> ObterLancamentoPorId(long id)
        {
            return await context.Caixas
                .Include(c => c.Clinica)
                .Include(c => c.Cliente)                
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Caixa> GravarLancamento(Caixa caixa)
        {
            if (caixa.Id == null)
            {                
                context.Caixas.Add(caixa);
            }
            else
            {
                context.Update(caixa);
            }

            await context.SaveChangesAsync();
            return caixa;
        }

        public async Task<Caixa> EliminarLancamentoPorId(long id)
        {
            var caixa = await ObterLancamentoPorId(id);
            context.Caixas.Remove(caixa);
            await context.SaveChangesAsync();
            return caixa;
        }

        public async Task<Caixa> InativarLancamentoPorId(long id, string user)
        {
            var caixa = await ObterLancamentoPorId(id);
            caixa.IdUser = user;           
            context.Update(caixa);
            await context.SaveChangesAsync();
            return caixa;
        }
    }
}
