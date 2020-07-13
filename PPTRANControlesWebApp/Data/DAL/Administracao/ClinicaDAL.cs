using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ClinicaDAL
    {
        private ApplicationContext _context;

        public ClinicaDAL(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<Clinica> ObterClinicasPorNome()
        {
            return _context.Clinicas
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome);
        }

        public async Task<Clinica> ObterClinicaPorId(long id)
        {

            return await _context.Clinicas
                .Include(e => e.Endereco)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Clinica> GravarClinica(Clinica clinica)
        {
            if (clinica.Id == null)
            {
                clinica.Status = EnumHelper.Status.Ativo;
                _context.Clinicas.Add(clinica);
            }
            else
            {
                _context.Update(clinica);
            }

            await _context.SaveChangesAsync();
            return clinica;
        }
    }
}
