using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Contato : BaseModel
    {
        [DisplayName("Nome")]
        public string Nome { get; set; }

        [DisplayName("Email")]
        public string Email { get; set; }

        [DisplayName("Telefone")]
        public string Tel1 { get; set; }

        [DisplayName("Telefone")]
        public string Tel2 { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }
     
        [DisplayName("Endereco")]
        public Endereco Endereco { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
