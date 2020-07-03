using Microsoft.EntityFrameworkCore;
using Models;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ColaboradorDAL
    {
        private Context _context;

        public ColaboradorDAL(Context context)
        {
            _context = context;
        }

        public IQueryable<Colaborador> ObterColaboradoresPorNome()
        {
            return _context.Colaboradores
                .Include(c => c.Clinica)
                .Include(e => e.Endereco)
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome);
        }

        public async Task<Colaborador> ObterColaboradorPorId(long id)
        {
            return await _context.Colaboradores
                .Include(c => c.Clinica)
                .Include(e => e.Endereco)
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.ColaboradorId == id);
        }

        public async Task<Colaborador> GravarColaborador(Colaborador colaborador)
        {
            if (colaborador.ColaboradorId == null)
            {
                colaborador.Status = EnumHelper.Status.Ativo;
                _context.Colaboradores.Add(colaborador);
            }
            else
            {
                _context.Update(colaborador);
            }

            await _context.SaveChangesAsync();
            return colaborador;
        }
    }
}
