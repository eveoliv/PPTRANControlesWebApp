﻿using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
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
            return _context.Clientes.OrderBy(c => c.Nome);
        }

        public async Task<Cliente> ObterClientePorId(long id)
        {
            return await _context.Clientes.Include(e => e.Endereco).
                SingleOrDefaultAsync(c => c.ClienteId == id);
        }



    }
}