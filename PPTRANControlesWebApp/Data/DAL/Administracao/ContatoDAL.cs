using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    public class ContatoDAL
    {
        private ApplicationContext _context;

        public ContatoDAL(ApplicationContext context)
        {
            _context = context;
        }
    }
}
