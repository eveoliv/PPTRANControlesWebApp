using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPTRANControlesWebApp.Data.DAL.Operacao
{
    public class CarrinhoDAL
    {
        private ApplicationContext context;

        public CarrinhoDAL(ApplicationContext context)
        {
            this.context = context;
        }

        public async Task<Carrinho> GravarCarrinho(Carrinho carrinho)
        {
            if (carrinho.Id == null)
            {
                context.Carrinhos.Add(carrinho);
            }
            else
            {
                context.Update(carrinho);
            }

            await context.SaveChangesAsync();
            return carrinho;
        }
    }
}
