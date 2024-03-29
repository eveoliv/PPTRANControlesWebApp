﻿using Models;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace PPTRANControlesWebApp.Data.DAL.Administracao
{
    //REVISADO_20200715
    public class ProdutoDAL
    {
        private ApplicationContext context;

        public ProdutoDAL(ApplicationContext context)
        {
            this.context = context;
        }        

        public IQueryable<Produto> ObterProdutosClassificadosPorNome()
        {
            //!6 e !7 valor fracionado e outros
            return context.Produtos.Where(s => s.Status == EnumHelper.Status.Ativo && s.Id != 7 && s.Id != 6 && s.Id != 3).OrderBy(c => c.Id);
        }

        public IQueryable<Produto> ObterProdutosClassificadosPorId()
        {
            return context.Produtos.Where(s => s.Status == EnumHelper.Status.Ativo).OrderBy(i => i.Id);
        }

        public async Task<Produto> ObterProdutoPorId(long id)
        {
            return await context.Produtos
                .Where(c => c.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Produto> ObterValorProdutoPorId(long id)
        {
            return await context.Produtos
                .Where(s => s.Status == EnumHelper.Status.Ativo)
                .SingleOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Produto> GravarProduto(Produto produto)
        {
            if (produto.Id == null)
            {
                context.Produtos.Add(produto);
            }
            else
            {
                context.Update(produto);
            }

            await context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto> EliminarProdutoPorId(long id)
        {
            var produto = await ObterProdutoPorId(id);
            context.Produtos.Remove(produto);
            await context.SaveChangesAsync();
            return produto;
        }

        public async Task<Produto> InativarProdutoPorId(long id, string user)
        {
            var produto = await ObterProdutoPorId(id);
            produto.IdUser = user;
            produto.Status = EnumHelper.Status.Inativo;
            context.Update(produto);
            await context.SaveChangesAsync();
            return produto;
        }
    }
}
