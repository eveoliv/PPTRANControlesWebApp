using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    public class ProdutoDAL
    {
        private ApplicationContext _context;

        public ProdutoDAL(ApplicationContext context)
        {
            _context = context;
        }

        public IQueryable<Produto> ObterProdutos()
        {
            return _context.Produtos.OrderBy(p => p.Id);
        }

        public async Task<Produto> ObterProdutoPorId(long id)
        {
            return await _context.Produtos.SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Produto> GravarProduto(Produto produto)
        {
            if (produto.Id == null)
            {
                _context.Produtos.Add(produto);
            }
            else
            {
                _context.Update(produto);
            }

            await _context.SaveChangesAsync();
            return produto;
        }
    }
}
