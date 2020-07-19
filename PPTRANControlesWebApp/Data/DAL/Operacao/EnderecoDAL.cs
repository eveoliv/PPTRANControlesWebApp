using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL
{
    //REVISADO_20200715
    public class EnderecoDAL
    {
        private ApplicationContext context;

        public EnderecoDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Endereco> ObterEnderecosClassificadosPorCEP()
        {
            return context.Enderecos.OrderBy(c => c.Cep);
        }

        public async Task<Endereco> ObterEnderecoPorId(long id)
        {
            return await context.Enderecos
                .Include(c => c.Clinica)
                .Include(c => c.Cliente)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Endereco> GravarEndereco(Endereco endereco)
        {
            if (endereco.Id == null)
            {
                context.Add(endereco);
            }
            else
            {
                context.Enderecos.Update(endereco);
            }

            await context.SaveChangesAsync();
            return endereco;
        }
    }
}

