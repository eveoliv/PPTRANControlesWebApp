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

        //public async Task<Clinica> ObterClinicaPorId(long id)
        //{

        //    return await _context.Clinicas
        //        .Include(e => e.Endereco)
        //        .Where(c => c.Status == EnumHelper.Status.Ativo)
        //        .SingleOrDefaultAsync(c => c.ClinicaId == id);
        //}

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
