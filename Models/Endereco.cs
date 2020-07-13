using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace Models
{
    public class Endereco : BaseModel
    {
        public Endereco() { }

        [DisplayName("CPF")]
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

        public string IdUser { get; set; }

        //fk
        public virtual Cliente Cliente { get; set; }
        public virtual Colaborador Colaborador { get; set; }

    }
}
