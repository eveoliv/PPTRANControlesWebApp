using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL
{
    //REVISADO_20200715
    public class ColaboradorDAL
    {
        private ApplicationContext context;

        public ColaboradorDAL(ApplicationContext context)
        {
            this.context = context;
        }
        
        public IQueryable<Colaborador> ObterColaboradoresClassificadosPorNome()
        {
            return context.Colaboradores
                .Include(c => c.Clinica)
                .Where(s => s.Status == EnumHelper.Status.Ativo).OrderBy(c => c.Nome);
        }

        public IQueryable<Colaborador> ObterMedicosClassificadosPorNome()
        {
            return context.Colaboradores
                .Where(m => m.Funcao == EnumHelper.Funcao.Medico)
                .Where(m => m.Status == EnumHelper.Status.Ativo)
                .OrderBy(m => m.Nome);
        }

        public IQueryable<Colaborador> ObterPsicologosClassificadosPorNome()
        {
            return context.Colaboradores
               .Where(m => m.Funcao == EnumHelper.Funcao.Psicologo)
               .Where(m => m.Status == EnumHelper.Status.Ativo)
               .OrderBy(m => m.Nome);
        }

        public async Task<Colaborador> ObterColaboradorPorId(long id)
        {
            return await context.Colaboradores
                .Include(c => c.Clinica)
                .Include(e => e.Endereco)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Colaborador> GravarColaborador(Colaborador colaborador)
        {
            if (colaborador.Id == null)
            {               
                context.Colaboradores.Add(colaborador);
            }
            else
            {
                context.Update(colaborador);
            }

            await context.SaveChangesAsync();
            return colaborador;
        }

        public async Task<Colaborador> EliminarColaboradorPorId(long id)
        {
            var colaborador = await ObterColaboradorPorId(id);
            context.Colaboradores.Remove(colaborador);
            await context.SaveChangesAsync();
            return colaborador;
        }

        public async Task<Colaborador> InativarColaboradorPorId(long id, string user)
        {
            var colaborador = await ObterColaboradorPorId(id);
            colaborador.IdUser = user;
            colaborador.Status = EnumHelper.Status.Inativo;
            context.Update(colaborador);
            await context.SaveChangesAsync();
            return colaborador;
        }
    }
}
