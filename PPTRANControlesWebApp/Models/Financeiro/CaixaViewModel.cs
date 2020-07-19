using Models;

namespace PPTRANControlesWebApp.Models
{
    public class CaixaViewModel
    {
        public Caixa Caixa { get; set; }       
        public Cliente Cliente { get; set; }       
        public Clinica Clinica { get; set; }
        public Produto Produto { get; set; }
        public Historico Historico { get; set; }
        public Colaborador Colaborador { get; set; }
    }
}
