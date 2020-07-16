using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL.Operacao
{
    //REVISADO_20200715
    public class AgendaDAL
    {
        private ApplicationContext context;

        public AgendaDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public IQueryable<Agenda> ObterAgendaClassificadaIdCliente()
        {
            return context.Agendas.OrderBy(c => c.ClienteId);
        }

        public async Task<Agenda> ObterAgendaPorId(long id)
        {
            return await context.Agendas
                .Include(c => c.Colaborador)
                .Include(c => c.Clinica)                                       
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Agenda> GravarAgenda(Agenda agenda)
        {
            if (agenda.Id == null)
            {
                context.Agendas.Add(agenda);
            }
            else
            {
                context.Update(agenda);
            }

            await context.SaveChangesAsync();
            return agenda;
        }

        public async Task<Agenda> EliminarAgendaPorId(long id)
        {
            var agenda = await ObterAgendaPorId(id);
            context.Agendas.Remove(agenda);
            await context.SaveChangesAsync();
            return agenda;
        }

        public async Task<Agenda> InativarAgendaPorId(long id, string user)
        {
            var agenda = await ObterAgendaPorId(id);
            agenda.IdUser = user;            
            context.Update(agenda);
            await context.SaveChangesAsync();
            return agenda;
        }
    }
}
