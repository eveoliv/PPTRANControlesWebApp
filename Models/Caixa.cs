using System;
using System.Collections.Generic;
using System.Text;

namespace Models
{
    public class Caixa
    {
        public long? Id { get; set; }
        public EnumHelper.Tipo Tipo { get; set; }
        public DateTime Data { get; set; }
        public decimal Saldo { get; set; }
        public EnumHelper.Lancamento Lancamento { get; set; }

    }
}
