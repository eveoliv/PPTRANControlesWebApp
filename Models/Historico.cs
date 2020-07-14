using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Historico : BaseModel
    {
        public Historico() { }

        [DisplayName("Nome Historico")]
        public string Nome { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
       
    }
}
