using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class EnderecoDAL
    {
        private ApplicationContext context;

        public EnderecoDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Endereco> ObterEnderecoPorId()
        {
            return null;
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

