using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Models
{
    public class Produto : BaseModel
    {
        public Produto() { }

        [DisplayName("Produto")]
        public string Nome { get; set; }

        //[RegularExpression("(\D)\s*([.\d,]+)")]
        [DisplayName("Valor Exame")]
        [DataType(DataType.Currency, ErrorMessage = "Formato inválido para valor!")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:#.####}")]        
        public decimal Valor { get; set; }

        [DisplayName("Status")]
        public EnumHelper.Status Status { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
