using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class EntrevistaDAL
    {
        private readonly Context _context;

        public EntrevistaDAL(Context context)
        {
            _context = context;
        }
        
        public async Task<Entrevista> ObterEntrevistaPorId(long id)
        {
            return await _context.Entrevistas.Include(c => c.Cliente)
                .SingleOrDefaultAsync(e => e.EntrevistaId == id);
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {
            return await _context.Clientes.Include(e => e.Endereco).
                SingleOrDefaultAsync(c => c.ClienteId == id);
        }

        public async Task<Entrevista> GravaEntrevista(Entrevista entrevista)
        {
            if (entrevista.EntrevistaId == null)
            {
                _context.Add(entrevista);
            }
            else
            {
                _context.Update(entrevista);               
            }

            await _context.SaveChangesAsync();
            return entrevista;
        }
    }
}
