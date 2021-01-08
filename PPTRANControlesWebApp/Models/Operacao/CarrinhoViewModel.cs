using System;
using Models;
using System.Linq;
using System.ComponentModel;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace PPTRANControlesWebApp.Models.Operacao
{
    public class CarrinhoViewModel
    {
        public long? Id { get; set; }
        public Carrinho Carrinho { get; set; }
        [DisplayName("Baixa automatica exame")]
        public string FormaPagamento { get; set; }
    }
}
