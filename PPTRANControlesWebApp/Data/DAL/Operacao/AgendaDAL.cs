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

        public IQueryable<Agenda> ObterAgendaClassificadaNomeCliente()
        {
            return context.Agendas               
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .OrderBy(c => c.Nome);           
        }

        public async Task<Agenda> ObterAgendaPorId(long id)
        {
            return await context.Agendas.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task GravarAgenda(Agenda agenda)
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

            return;
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
            agenda.Status = EnumHelper.Status.Inativo;
            context.Update(agenda);
            await context.SaveChangesAsync();
            return agenda;
        }
    }
}
