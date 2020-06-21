using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL
{
    public class EnderecoDAL
    {
        private Context _context;

        public EnderecoDAL(Context context)
        {
            _context = context;
        }

        public IQueryable<Endereco> ObterEnderecoPorId()
        {
            return null;
        }
    }
}
