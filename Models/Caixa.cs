using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Caixa
    {
        [DisplayName("ID")]
        public long? CaixaId { get; set; }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Data { get; set; }

        [DisplayName("Histórico")]
        public string Historico { get; set; }

        //cartao, dinheiro
        [DisplayName("Tipo")]
        public EnumHelper.Tipo Tipo { get; set; }

        [DisplayName("Valor")]
        public decimal Valor { get; set; }

        //deb, cred
        [DisplayName("Pagamento")]
        public EnumHelper.FormaPgto FormaPgto { get; set; }

        //nome cliente
        [DisplayName("Referência")]
        public string Ref { get; set; }

        [DisplayName("Cliente")]
        public long? ClienteId { get; set; }
        public Cliente Cliente { get; set; }       

        [DisplayName("Clinica")]
        public long? ClinicaId { get; set; }
        public Clinica Clinica { get; set; }

        [DisplayName("Colaborador")]
        public long? ColaboradorId { get; set; }
        public Colaborador Colaborador { get; set; }

        public string CpfUser { get; set; }
    }
}
