using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ColaboradorDAL
    {
        private ApplicationContext context;

        public ColaboradorDAL(ApplicationContext context)
        {
            this.context = context;
        }

        /*** Revisado ***/
        public IQueryable<Colaborador> ObterColaboradoresClassificadosPorNome()
        {
            return context.Colaboradores.Where(s => s.Status == EnumHelper.Status.Ativo).OrderBy(c => c.Nome);
        }

        /*** Revisado ***/
        public IQueryable<Colaborador> ObterMedicosClassificadosPorNome()
        {
            return context.Colaboradores
                .Where(m => m.Funcao == EnumHelper.Funcao.Medico)
                .Where(m => m.Status == EnumHelper.Status.Ativo)
                .OrderBy(m => m.Nome);
        }

        /*** Revisado ***/
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

        public async Task<Colaborador> ObterColaboradorPorCPF(string cpf)
        {
            return await context.Colaboradores
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.CPF == cpf);
        }

        public async Task<Colaborador> GravarColaborador(Colaborador colaborador)
        {
            if (colaborador.Id == null)
            {
                colaborador.Status = EnumHelper.Status.Ativo;
                context.Colaboradores.Add(colaborador);
            }
            else
            {
                context.Update(colaborador);
            }

            await context.SaveChangesAsync();
            return colaborador;
        }
    }
}
