using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Produto : BaseModel
    {
        public Produto() { }

        [DisplayName("Nome Exame")]
        public string Nome { get; set; }

        [DisplayName("Valor Exame")]
        public decimal Valor { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
