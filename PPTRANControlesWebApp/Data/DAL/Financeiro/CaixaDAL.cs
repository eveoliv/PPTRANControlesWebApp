using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Models;
using PPTRANControlesWebApp.Data;
using PPTRANControlesWebApp.Data.DAL;
using PPTRANControlesWebApp.Models;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class CaixaDAL
    {
        private Context _context;

        public CaixaDAL(Context context)
        {
            _context = context;
        }

        public IQueryable<Caixa> ObterLancamentos()
        {
            return _context.Caixas.OrderBy(c => c.CaixaId);
        }

        public async Task<Caixa> ObterLancamentoPorId(long id)
        {
            return await _context.Caixas
                .Include(c => c.Clinica)
                .Include(c => c.Cliente)
                .Include(c => c.Colaborador)
                .SingleOrDefaultAsync(c => c.CaixaId == id);
        }

        public async Task<Caixa> GravarLancamento(Caixa caixa)
        {
            if (caixa.CaixaId == null)
            {                
                _context.Caixas.Add(caixa);
            }
            else
            {
                _context.Update(caixa);
            }

            await _context.SaveChangesAsync();
            return caixa;
        }
    }
}
