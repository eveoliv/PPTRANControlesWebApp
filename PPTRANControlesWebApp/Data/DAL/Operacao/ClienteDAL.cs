using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ClienteDAL
    {
        private ApplicationContext _context;

        public ClienteDAL(ApplicationContext context)
        {
            _context = context;
        }
    
        public IQueryable<Cliente> ObterClientesPorNome()
        {       
            return _context.Clientes
                .Include(i => i.Clinica)
                .Include(e => e.Endereco)
                .Include(h => h.Historico)
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome);
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {           

            return await _context.Clientes                
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
                cliente.StatusPgto = EnumHelper.OptForm.NAO;
                cliente.Status = EnumHelper.Status.Ativo;
                _context.Clientes.Add(cliente);
            }
            else
            {
                _context.Update(cliente);

            }

            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> ObterClientePorCpf(string cpf)
        {
            return await _context.Clientes
                 .Where(c => c.Status == EnumHelper.Status.Ativo)
                 .SingleOrDefaultAsync(c => c.CPF == cpf);
        }       
        
        public async Task<Cliente> ObterClienteIdPeloCpf(Cliente cliente)
        {
            var cpf = cliente.CPF;

            var idCli = _context.Clientes
                 .Where(c => c.Status == EnumHelper.Status.Ativo)
                 .SingleOrDefaultAsync(c => c.CPF == cpf);           

            return await idCli;
        }        
    }
}
