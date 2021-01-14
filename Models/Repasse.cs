using System;
using System.Text;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Models
{
    public class Repasse : BaseModel
    {
        public Repasse() { }

        [DisplayName("Profissional")]
        public string Profissional { get; set; }

        [DisplayName("Valor")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]
        [DataType(DataType.Currency, ErrorMessage = "Caracteres incorretos para o valor.")]
        public decimal Valor { get; set; }

        [DisplayName("Usuario")]
        public string IdUser { get; set; }
    }
}
