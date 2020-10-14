using System;

namespace PPTRANControlesWebApp.Models.Operacao
{
    public class PesquisaViewModel
    {
        public string Nome { get; set; }
        public string Cpf { get; set; }
        public DateTime DtInicio { get; set; }
        public DateTime DtFim { get; set; }
    }
}
