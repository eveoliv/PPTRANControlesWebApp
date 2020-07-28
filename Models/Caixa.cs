using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Caixa : BaseModel
    {       
        public Caixa() { }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Data { get; set; }

        //cartao, dinheiro
        [DisplayName("Tipo")]
        public EnumHelper.Tipo Tipo { get; set; }

        [DisplayName("Status Lançamento")]
        public EnumHelper.Status Status { get; set; }

        [DisplayName("Valor")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        [DataType(DataType.Currency, ErrorMessage = "Caracteres incorretos para o valor.")]
        public decimal Valor { get; set; }

        //deb, cred
        [DisplayName("Pagamento")]
        public EnumHelper.FormaPgto FormaPgto { get; set; }

        [DisplayName("Pagto Realizado")]
        public EnumHelper.YesNo StatusPgto { get; set; }

        //nome cliente
        [DisplayName("Referência")]
        public string Ref { get; set; }

        [DisplayName("Histórico")]
        public long? HistoricoId { get; set; }
        public Historico Historico { get; set; }

        [DisplayName("Produto")]
        public long? ProdutoId { get; set; }
        public Produto Produto { get; set; }

        [DisplayName("Cliente")]
        public long? ClienteId { get; set; }
        public Cliente Cliente { get; set; }

        [DisplayName("Colaborador")]
        public long? ColaboradorId { get; set; }
        public Colaborador Colaborador { get; set; }

        [DisplayName("Clinica")]
        public long? ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
