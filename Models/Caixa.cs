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
        public long? Id { get; set; }

        [DisplayName("Tipo")]
        public EnumHelper.Tipo Tipo { get; set; }

        [DisplayName("Data")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime Data { get; set; }

        [DisplayName("Saldo")]
        public decimal Saldo { get; set; }

        [DisplayName("Lançamento")]
        public EnumHelper.Lancamento Lancamento { get; set; }

    }
}
