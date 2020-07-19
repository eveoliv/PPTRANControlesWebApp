using Models;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    //REVISADO_20200715
    public class ContatoDAL
    {
        private ApplicationContext context;

        public ContatoDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Contato> ObterContatosClassificadosPorNome()
        {
            return context.Contatos.OrderBy(c => c.Nome);
        }

        public async Task<Contato> ObterContatoPorId(long id)
        {
            return await context.Contatos
                .Include(e => e.Endereco)                
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Contato> GravarContato(Contato contato)
        {
            if (contato.Id == null)
            {
                context.Contatos.Add(contato);
            }
            else
            {
                context.Update(contato);
            }

            await context.SaveChangesAsync();
            return contato;
        }

        public async Task<Contato> EliminarContatoPorId(long id)
        {
            var contato = await ObterContatoPorId(id);
            context.Contatos.Remove(contato);
            await context.SaveChangesAsync();
            return contato;
        }      
    }
}
