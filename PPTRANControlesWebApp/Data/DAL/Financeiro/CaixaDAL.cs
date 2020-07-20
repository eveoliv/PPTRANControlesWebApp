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

        public IQueryable<Caixa> ObterLancamentosClassificadosPorProduto()
        {
            return context.Caixas
                .Include(c => c.Cliente)
                .Include(p => p.Produto)
                .Include(i => i.Historico)
                .Include(c => c.Colaborador)
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(p => p.Produto);
        }

        public async Task<Caixa> ObterLancamentoPorId(long id)
        {
            return await context.Caixas
                .Include(c => c.Clinica)
                .Include(c => c.Cliente)
                .Include(p => p.Produto)
                .Include(h => h.Historico)
                .Include(c => c.Colaborador)
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

        public async Task<Caixa> GravarLancamentoPorCarrinhoAsync(Caixa caixa)
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
            return null;
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
            caixa.Status = EnumHelper.Status.Inativo;
            context.Update(caixa);
            await context.SaveChangesAsync();
            return caixa;         
        }
    }
}
