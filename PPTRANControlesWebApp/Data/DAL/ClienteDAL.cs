using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ClienteDAL
    {
        private Context _context;

        public ClienteDAL(Context context)
        {
            _context = context;
        }
    
        public IQueryable<Cliente> ObterClientesPorNome()
        {       
            return _context.Clientes
                .Include(i => i.Clinica)
                .Include(e => e.Endereco)
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome);
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {           

            return await _context.Clientes                
                .Include(c => c.Clinica)
                .Include(e => e.Endereco)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.ClienteId == id);
        }

        public async Task<Cliente> ObterClientePorCPF(string cpf)
        {
            return await _context.Clientes
                 .Where(c => c.Status == EnumHelper.Status.Ativo)
                 .SingleOrDefaultAsync(c => c.CPF == cpf);
        }

        public async Task<Cliente> GravarCliente(Cliente cliente)
        {
            if (cliente.ClienteId == null)
            {                
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
    }
}
