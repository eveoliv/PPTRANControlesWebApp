using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL
{
    //REVISADO_20200715
    public class ClinicaDAL
    {
        private ApplicationContext context;

        public ClinicaDAL(ApplicationContext context)
        {
            this.context = context;
        }

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
                context.Clinicas.Add(clinica);
            }
            else
            {
                context.Update(clinica);
            }

            await context.SaveChangesAsync();
            return clinica;
        }

        public async Task<Clinica> EliminarClinicaPorId(long id)
        {
            var clinica = await ObterClinicaPorId(id);
            context.Clinicas.Remove(clinica);
            await context.SaveChangesAsync();
            return clinica;
        }

        public async Task<Clinica> InativarClinicaPorId(long id, string user)
        {
            var clinica = await ObterClinicaPorId(id);
            clinica.IdUser = user;
            clinica.Status = EnumHelper.Status.Inativo;
            context.Update(clinica);
            await context.SaveChangesAsync();
            return clinica;
        }
    }
}
