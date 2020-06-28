using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Endereco
    {
        //pk
        [DisplayName("EndereçoId")]
        public long? EnderecoId { get; set; }

        public string CPF { get; set; }  

        [DisplayName("CEP")]
        public string Cep { get; set; }

        [DisplayName("Rua")]
        public string Rua { get; set; }

        [DisplayName("Bairro")]
        public string Bairro { get; set; }

        [DisplayName("Cidade")]
        public string Cidade { get; set; }

        [DisplayName("Estado")]
        public string Estado { get; set; }

        [DisplayName("Número")]
        public long? Numero { get; set; }

        [DisplayName("Complemento")]
        public string Complto { get; set; }

        //fk
        public virtual Cliente Cliente { get; set; }

    }
}
