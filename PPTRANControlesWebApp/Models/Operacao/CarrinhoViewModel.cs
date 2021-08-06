using Models;
using System.ComponentModel;

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
