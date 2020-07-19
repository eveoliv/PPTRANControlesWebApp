using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;

namespace PPTRANControlesWebApp.Data.DAL
{
    //REVISADO_20200715
    public class ClienteDAL
    {       
        private ApplicationContext context;

        public ClienteDAL(ApplicationContext context)
        {
            this.context = context;           
        }
       
        public IQueryable<Cliente> ObterClientesClassificadosPorNome()
        {
            return context.Clientes.Where(s => s.Status == EnumHelper.Status.Ativo).OrderBy(c => c.Nome);
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
     
        public async Task<Cliente> EliminarClientePorId(long id)
        {
            var cliente = await ObterClientePorId(id);
            context.Clientes.Remove(cliente);
            await context.SaveChangesAsync();
            return cliente;
        }
        
        public async Task<Cliente> InativarClientePorId(long id, string user)
        {
            var cliente = await ObterClientePorId(id);
            cliente.IdUser = user;
            cliente.Status = EnumHelper.Status.Inativo;
            context.Update(cliente);
            await context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            return await context.Clientes
                .Where(s => s.Status == EnumHelper.Status.Ativo)                
                .SingleOrDefaultAsync(c => c.CPF == cpf);
        }
    }
}
