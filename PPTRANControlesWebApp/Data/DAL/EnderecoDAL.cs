﻿using Models;
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

        public async Task<Endereco> GravarEndereco(Endereco endereco)
        {
            if (endereco.EnderecoId == null)
            {
                _context.Add(endereco);
            }
            else
            {
                _context.Update(endereco);
            }

            await _context.SaveChangesAsync();
            return endereco;
        }

        public string BuscaEnderecoId(string cpf)
        {
            var idEndereco =
                       (from e in _context.Enderecos where e.CPF == cpf select e).Single();


            return null;
        }
    }
}
