using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ClienteDAL
    {       
        private ApplicationContext context;

        public ClienteDAL(ApplicationContext context)
        {
            this.context = context;           
        }
    
        public IQueryable<Cliente> ObterClientesPorNome()
        {       
            return context.Clientes
                .Include(i => i.Clinica)
                .Include(e => e.Endereco)
                .Include(h => h.Historico)
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome);
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {           
            return await context.Clientes                
                .Include(c => c.Clinica)
                .Include(e => e.Endereco)
                .Include(h => h.Historico)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.Id == id);
        }
       
        public async Task<Cliente> GravarCliente(Cliente cliente)
        {            
            if (cliente.Id == null)
            {                                          
                context.Clientes.Add(cliente);
            }
            else
            {
                context.Update(cliente);
            }

            await context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            return await context.Clientes
                 .Where(c => c.Status == EnumHelper.Status.Ativo)
                 .SingleOrDefaultAsync(c => c.CPF == cpf);
        }       
        
        public async Task<Cliente> ObterClienteIdPeloCpf(Cliente cliente)
        {
            var cpf = cliente.CPF;

            var idCli = context.Clientes
                 .Where(c => c.Status == EnumHelper.Status.Ativo)
                 .SingleOrDefaultAsync(c => c.CPF == cpf);           

            return await idCli;
        }        
    }
}
