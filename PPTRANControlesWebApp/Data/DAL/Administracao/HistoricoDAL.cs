using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    public class HistoricoDAL
    {
        private ApplicationContext _context;

        public HistoricoDAL(ApplicationContext context)
        {
            _context = context;
        }

        public async Task<Historico> ObterHistoricoPorId(long id)
        {
            return await _context.Historicos
               .SingleOrDefaultAsync(c => c.Id == id);
        }
    }
}
