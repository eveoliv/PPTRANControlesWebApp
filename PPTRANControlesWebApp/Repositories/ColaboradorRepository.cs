using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Models;
using PPTRANControlesWebApp.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Repositories
{
    public interface IColaboradorRepository
    {
        Task SaveColaboradorAsync(List<Colaborador> colaborador);
        Task<IList<Colaborador>> GetColaboradorAsync();

    }

    public class ColaboradorRepository : BaseRepository<Colaborador>, IColaboradorRepository
    {
        public ColaboradorRepository(IConfiguration configuration, ApplicationContext context) : base(configuration, context) { }

        public async Task<IList<Colaborador>> GetColaboradorAsync()
        {
            return await dbSet
                .Include(c => c.Clinica)
                .Include(e => e.Endereco)
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome)
                .ToListAsync();                
        }

         //       .Include(c => c.Clinica)
         //       .Include(e => e.Endereco)
         //       .Where(s => s.Status == EnumHelper.Status.Ativo)
         //       .OrderBy(c => c.Nome);

        public Task SaveColaboradorAsync(List<Colaborador> colaborador)
        {
            throw new NotImplementedException();
        }
    }
}
