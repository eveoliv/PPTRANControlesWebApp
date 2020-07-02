using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class ContextInitializer
    {
        public static void Initialize(Context context)
        {
            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            if (context.Enderecos.Any())
            {
                return;
            }
                        
            if (context.Clinicas.Any())
            {
                return;
            }
           
            if (context.Clientes.Any())
            {
                return;
            }

            if (context.Colaboradores.Any())
            {
                return;
            }

            if (context.Agendas.Any())
            {
                return;
            }

            if (context.Caixas.Any())
            {
                return;
            }          
        }
    }
}
