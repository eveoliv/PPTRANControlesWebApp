using Models;
using System;

namespace PPTRANControlesWebApp.Models.Relatorio
{
    public class LancamentoViewModel
    {
        public long? ClinicaId { get; set; }
        public string Clinica { get; set; }

        public long? HistoricoId { get; set; }
        public string Historico { get; set; }

        public long? ProdutoId { get; set; }
        public string Produto { get; set; }

        public long? ClienteId { get; set; }
        public string ClienteNome { get; set; }

        public string Referencia { get; set; }
        public EnumHelper.Tipo Tipo { get; set; }
        public EnumHelper.FormaPgto FormaPgto { get; set; }
        public decimal Valor { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataFim { get; set; }
    }
}
