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

        [DisplayName("Tel. Celular")]
        public string Telefone1 { get; set; }

        [DisplayName("Tel. Fixo")]
        public string Telefone2 { get; set; }

        [DisplayName("Descrição")]
        public string Descricao { get; set; }

        [DisplayName("Observação")]
        public string Obs { get; set; }

        [DisplayName("Endereco")]
        public long? EnderecoId { get; set; }
        public Endereco Endereco { get; set; }

        [DisplayName("Usuário")]
        public string IdUser { get; set; }
    }
}
