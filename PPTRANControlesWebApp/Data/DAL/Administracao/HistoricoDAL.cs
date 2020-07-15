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

        public IQueryable<Historico> ObterHistoricoPorNome()
        {
            return _context.Historicos.OrderBy(c => c.Nome);
        }

        public async Task<Historico> ObterHistoricoPorId(long id)
        {
            return await _context.Historicos
               .Where(c => c.Status == EnumHelper.Status.Ativo)
               .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Historico> GravarHitorico(Historico historico)
        {
            if (historico.Id == null)
            {
                historico.Status = EnumHelper.Status.Ativo;
                _context.Historicos.Add(historico);
            }
            else
            {
                _context.Update(historico);
            }

            await _context.SaveChangesAsync();
            return historico;
        }
    }
}
