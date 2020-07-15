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
        private ApplicationContext context;

        public ClinicaDAL(ApplicationContext context)
        {
            this.context = context;
        }

        /*** Revisado ***/
        public IQueryable<Clinica> ObterClinicasClassificadasPorNome()
        {
            return context.Clinicas.Where(s => s.Status == EnumHelper.Status.Ativo).OrderBy(c => c.Nome);
        }          

        public async Task<Clinica> ObterClinicaPorId(long id)
        {

            return await context.Clinicas
                .Include(e => e.Endereco)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Clinica> GravarClinica(Clinica clinica)
        {
            if (clinica.Id == null)
            {
                clinica.Status = EnumHelper.Status.Ativo;
                context.Clinicas.Add(clinica);
            }
            else
            {
                context.Update(clinica);
            }

            await context.SaveChangesAsync();
            return clinica;
        }
    }
}
