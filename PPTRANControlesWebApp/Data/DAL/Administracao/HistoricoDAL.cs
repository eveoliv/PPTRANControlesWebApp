using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    public class HistoricoDAL
    {
        private ApplicationContext _context;

        public HistoricoDAL(ApplicationContext context)
        {
            _context = context;
        }
    }
}
